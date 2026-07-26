using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows;
using TigerTrade.Chart.Base;
using TigerTrade.Chart.Objects.Common;
using TigerTrade.Chart.Objects.Enums;
using TigerTrade.Dx;
using TigerTrade.Dx.Enums;
using TigerTrade.Chart.Alerts;
using TigerTrade.Chart.Indicators.Common;
using TigerTrade.Core.Utils.Logging;

namespace TrendLineLibrary.Objects
{
    [DataContract(Name = "HorizontalSegmentObject", Namespace = "http://schemas.datacontract.org/2004/07/TrendLineLibrary.Objects")]
    [ChartObject("X_HorizontalSegment_001", "Горизонтальный отрезок", 3, Type = typeof(HorizontalSegmentObject))]
    public sealed class HorizontalSegmentObject : ObjectBase
    {
        // Поля
        private XColor _lineColor;
        private int _lineWidth = 1;
        private XDashStyle _lineStyle = XDashStyle.Solid;
        private string _text = string.Empty;
        private ObjectTextAlignment _textAlignment = ObjectTextAlignment.CenterTop;
        private int _fontSize = 14;
        private ChartAlertSettings _alert;
        private int _alertMinDistance = 3;
        private double _lastAlertValue;
        private int _lastAlertIndex;

        private Point _startScreen;
        private Point _endScreen;
        private Rect _lineBounds;

        // Свойства
        [DataMember(Name = "LineColor")]
        [Category("Линия"), DisplayName("Цвет линии")]
        public XColor LineColor
        {
            get => _lineColor;
            set
            {
                if (value == _lineColor) return;
                _lineColor = value;
                OnPropertyChanged();
            }
        }

        [DataMember(Name = "LineWidth")]
        [Category("Линия"), DisplayName("Толщина")]
        public int LineWidth
        {
            get => _lineWidth;
            set
            {
                value = Math.Max(1, Math.Min(10, value));
                if (value == _lineWidth) return;
                _lineWidth = value;
                OnPropertyChanged();
            }
        }

        [DataMember(Name = "LineStyle")]
        [Category("Линия"), DisplayName("Стиль")]
        public XDashStyle LineStyle
        {
            get => _lineStyle;
            set
            {
                if (value == _lineStyle) return;
                _lineStyle = value;
                OnPropertyChanged();
            }
        }

        [DataMember(Name = "Text")]
        [Category("Текст"), DisplayName("Текст")]
        public string Text
        {
            get => _text;
            set
            {
                if (value == _text) return;
                _text = value;
                OnPropertyChanged();
            }
        }

        [DataMember(Name = "TextAlignment")]
        [Category("Текст"), DisplayName("Расположение")]
        public ObjectTextAlignment TextAlignment
        {
            get => _textAlignment;
            set
            {
                if (value == _textAlignment) return;
                _textAlignment = value;
                OnPropertyChanged();
            }
        }

        [DataMember(Name = "FontSize")]
        [Category("Текст"), DisplayName("Размер шрифта")]
        public int FontSize
        {
            get => _fontSize;
            set
            {
                value = Math.Max(8, Math.Min(72, value));
                if (value == _fontSize) return;
                _fontSize = value;
                OnPropertyChanged();
            }
        }

        [DataMember(Name = "Alert")]
        [Category("Оповещение"), DisplayName("Оповещение")]
        public ChartAlertSettings Alert
        {
            get => _alert ?? (_alert = new ChartAlertSettings());
            set
            {
                if (Equals(value, _alert)) return;
                _alert = value;
                OnPropertyChanged();
            }
        }

        [DataMember(Name = "AlertMinDistance")]
        [Category("Оповещение"), DisplayName("Мин. расстояние")]
        public int AlertMinDistance
        {
            get => _alertMinDistance;
            set
            {
                if (value == _alertMinDistance) return;
                _alertMinDistance = value;
                OnPropertyChanged();
            }
        }

        protected override int PenWidth => LineWidth;

        public HorizontalSegmentObject()
        {
            /*

            try
            {
                System.IO.File.AppendAllText(@"C:\Temp\HorizontalSegment_debug.txt",
                    $"{DateTime.Now}: Constructor called. ControlPoints length = {ControlPoints.Length}\n");
            }
            catch { }
            ControlPoints = new ObjectPoint[2];
            ControlPoints[0] = new ObjectPoint(10, 100.0);
            ControlPoints[1] = new ObjectPoint(100, 100.0);
            */
        }

        protected override void Prepare()
        {
            base.Prepare();

            if (Canvas == null || ControlPoints == null || ControlPoints.Length < 2) return;

            // БЕРЁМ ПЕРВЫЕ ДВЕ ТОЧКИ (платформа может дать 3)
            Point p1 = ToPoint(ControlPoints[0]);
            Point p2 = ToPoint(ControlPoints[1]);

            // Принудительно выравниваем Y
            p2.Y = p1.Y;

            _startScreen = p1;
            _endScreen = p2;

            // Вычисляем ограничивающий прямоугольник
            double minX = Math.Min(_startScreen.X, _endScreen.X) - 5;
            double minY = Math.Min(_startScreen.Y, _endScreen.Y) - 5;
            double maxX = Math.Max(_startScreen.X, _endScreen.X) + 5;
            double maxY = Math.Max(_startScreen.Y, _endScreen.Y) + 5;

            _lineBounds = new Rect(minX, minY, maxX - minX, maxY - minY);
        }

        protected override void Draw(DxVisualQueue visual, ref List<ObjectLabelInfo> labels)
        {

            if (Canvas == null || ControlPoints == null || ControlPoints.Length < 2) return;

            // ИСПОЛЬЗУЕМ _startScreen и _endScreen из Prepare()
            var brush = new XBrush(_lineColor);
            var pen = new XPen(brush, _lineWidth, _lineStyle);

            visual.DrawLine(pen, _startScreen, _endScreen);

            if (DataProvider != null)
            {
                labels.Add(new ObjectLabelInfo(ControlPoints[0].Y, _lineColor));
                labels.Add(new ObjectLabelInfo(ControlPoints[1].Y, _lineColor));
            }

            DrawText(visual);
        }

        private void DrawText(DxVisualQueue visual)
        {
            if (string.IsNullOrEmpty(_text) || TextAlignment == ObjectTextAlignment.Hide || Canvas == null)
                return;

            var font = new XFont(Canvas.ChartFont.Name, _fontSize);
            var textSize = font.GetSize(_text);

            double centerX = (_startScreen.X + _endScreen.X) / 2.0;
            double centerY = (_startScreen.Y + _endScreen.Y) / 2.0;

            double x = centerX - textSize.Width / 2.0;
            double y = centerY - textSize.Height / 2.0;

            switch (TextAlignment)
            {
                case ObjectTextAlignment.LeftTop:
                    x = _startScreen.X;
                    y = _startScreen.Y - textSize.Height - 5;
                    break;
                case ObjectTextAlignment.CenterTop:
                    x = centerX - textSize.Width / 2.0;
                    y = _startScreen.Y - textSize.Height - 5;
                    break;
                case ObjectTextAlignment.RightTop:
                    x = _endScreen.X - textSize.Width;
                    y = _startScreen.Y - textSize.Height - 5;
                    break;
                case ObjectTextAlignment.LeftMiddle:
                    x = _startScreen.X;
                    y = centerY - textSize.Height / 2.0;
                    break;
                case ObjectTextAlignment.CenterMiddle:
                    x = centerX - textSize.Width / 2.0;
                    y = centerY - textSize.Height / 2.0;
                    break;
                case ObjectTextAlignment.RightMiddle:
                    x = _endScreen.X - textSize.Width;
                    y = centerY - textSize.Height / 2.0;
                    break;
                case ObjectTextAlignment.LeftBottom:
                    x = _startScreen.X;
                    y = _endScreen.Y + 5;
                    break;
                case ObjectTextAlignment.CenterBottom:
                    x = centerX - textSize.Width / 2.0;
                    y = _endScreen.Y + 5;
                    break;
                case ObjectTextAlignment.RightBottom:
                    x = _endScreen.X - textSize.Width;
                    y = _endScreen.Y + 5;
                    break;
            }

            var textRect = new Rect(x, y, textSize.Width, textSize.Height);

            if (Theme != null)
            {
                var bgBrush = new XBrush(Theme.ChartBackColor);
                visual.FillRectangle(bgBrush, textRect);
            }

            var textBrush = new XBrush(_lineColor);
            visual.DrawString(_text, font, textBrush, textRect);
        }

        public override void DrawControlPoints(DxVisualQueue visual)
        {
            if (Canvas == null || ControlPoints == null || ControlPoints.Length < 2 || Theme == null) return;

            for (int i = 0; i < ControlPoints.Length; i++)
            {
                // ПРОПУСКАЕМ СРЕДНЮЮ ТОЧКУ (индекс 2)
                if (i == 2) continue;

                Point pt = ToPoint(ControlPoints[i]);
                double size = 6;
                Rect rect = new Rect(pt.X - size, pt.Y - size, size * 2, size * 2);

                visual.FillRectangle(Theme.ChartCpFillBrush, rect);
                visual.DrawRectangle(Theme.ChartCpLinePen, rect);
            }
        }

        public override int GetControlPoint(int x, int y)
        {
            try
            {
                System.IO.File.AppendAllText(@"C:\Temp\HorizontalSegment_debug.txt",
                    $"{DateTime.Now}: GetControlPoint called. ControlPoints length = {ControlPoints?.Length}\n");
            }
            catch { }

            if (Canvas == null || ControlPoints == null || ControlPoints.Length < 2) return -1;

            double threshold = 10;

            for (int i = 0; i < ControlPoints.Length; i++)
            {
                Point pt = ToPoint(ControlPoints[i]);
                double distance = Math.Sqrt((x - pt.X) * (x - pt.X) + (y - pt.Y) * (y - pt.Y));

                if (distance <= threshold)
                    return i;
            }

            return -1;
        }

        public override void CheckAlert(List<IndicatorBase> indicators)
        {
            if (!Alert.IsActive || DataProvider == null || ControlPoints.Length < 2) return;

            try
            {
                foreach (var indicator in indicators)
                {
                    if (indicator.CheckAlert(ControlPoints[0].Y, AlertMinDistance, ref _lastAlertIndex, ref _lastAlertValue) ||
                        indicator.CheckAlert(ControlPoints[1].Y, AlertMinDistance, ref _lastAlertIndex, ref _lastAlertValue))
                    {
                        string message = $"{indicator.Name}: пересечение уровня {DataProvider.Symbol.FormatPrice((decimal)ControlPoints[0].Y)}.";
                        AddAlert(Alert, message);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteError(ex);
            }
        }

        public override void CopyTemplate(ObjectBase objectBase, bool style)
        {
            base.CopyTemplate(objectBase, style);

            if (objectBase is HorizontalSegmentObject obj)
            {
                Alert.Copy(obj.Alert, !style);
                AlertMinDistance = obj.AlertMinDistance;
                _lineColor = obj._lineColor;
                _lineWidth = obj._lineWidth;
                _lineStyle = obj._lineStyle;
                _text = obj._text;
                _textAlignment = obj._textAlignment;
                _fontSize = obj._fontSize;
            }
        }

        protected override bool InObject(int x, int y)
        {
            if (Canvas == null || ControlPoints.Length < 2) return false;

            if (!_lineBounds.Contains(x, y)) return false;

            double distance = DistanceToSegment(new Point(x, y), _startScreen, _endScreen);
            return distance <= (LineWidth / 2.0) + 2;
        }

        private double DistanceToSegment(Point p, Point a, Point b)
        {
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;

            if (dx == 0 && dy == 0)
                return Math.Sqrt((p.X - a.X) * (p.X - a.X) + (p.Y - a.Y) * (p.Y - a.Y));

            double t = ((p.X - a.X) * dx + (p.Y - a.Y) * dy) / (dx * dx + dy * dy);
            t = Math.Max(0, Math.Min(1, t));

            double projX = a.X + t * dx;
            double projY = a.Y + t * dy;

            return Math.Sqrt((p.X - projX) * (p.X - projX) + (p.Y - projY) * (p.Y - projY));
        }
    }
}