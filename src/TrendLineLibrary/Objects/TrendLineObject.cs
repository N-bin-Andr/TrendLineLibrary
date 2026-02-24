using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using TigerTrade.Chart.Base;
using TigerTrade.Chart.Objects.Common;
using TigerTrade.Chart.Objects.Enums;
using TigerTrade.Dx;
using TigerTrade.Dx.Enums;
using TigerTrade.Chart.Alerts;
using TigerTrade.Chart.Indicators.Common;
using TigerTrade.Core.Utils.Logging;
using System.Windows.Input;

namespace TrendLineLibrary.Objects
{
    [DataContract(Name = "TrendLineObject", Namespace = "http://schemas.datacontract.org/2004/07/TrendLineLibrary.Objects")]
    [ChartObject("X_TrendLine", "Трендовая линия", 2, Type = typeof(TrendLineObject))]
    public sealed class TrendLineObject : ObjectBase
    {
        // Поля для свойств
        private XColor _lineColor;
        private int _lineWidth = 1;
        private XDashStyle _lineStyle = XDashStyle.Solid;
        private string _text = string.Empty;
        private ObjectTextAlignment _textAlignment = ObjectTextAlignment.CenterTop;
        private int _fontSize = 14;
        private bool _extendLeft = false;
        private bool _extendRight = false;
        private bool _magnetEnabled = true;
        private ChartAlertSettings _alert;
        private int _alertMinDistance = 3;
        private double _lastAlertValue;
        private int _lastAlertIndex;
        private bool _isAdjusting;

        // Свойства линии
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

        // Свойства текста
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

        [DataMember(Name = "ExtendLeft")]
        [Category("Поведение"), DisplayName("Удлинить влево")]
        public bool ExtendLeft
        {
            get => _extendLeft;
            set
            {
                if (value == _extendLeft) return;
                _extendLeft = value;
                OnPropertyChanged();
            }
        }

        [DataMember(Name = "ExtendRight")]
        [Category("Поведение"), DisplayName("Удлинить вправо")]
        public bool ExtendRight
        {
            get => _extendRight;
            set
            {
                if (value == _extendRight) return;
                _extendRight = value;
                OnPropertyChanged();
            }
        }

        [DataMember(Name = "MagnetEnabled")]
        [Category("Поведение"), DisplayName("Примагничивание к свечам")]
        public bool MagnetEnabled
        {
            get => _magnetEnabled;
            set
            {
                if (value == _magnetEnabled) return;
                _magnetEnabled = value;
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

        public TrendLineObject()
        {
            // Инициализация двух контрольных точек
            ControlPoints = new ObjectPoint[2];
            ControlPoints[0] = new ObjectPoint(10, 100.0);
            ControlPoints[1] = new ObjectPoint(20, 110.0);

            // Инициализация свойств
            _lineColor = Colors.Blue;
            _lineWidth = 1;
            _lineStyle = XDashStyle.Solid;
            _text = string.Empty;
            _textAlignment = ObjectTextAlignment.CenterTop;
            _fontSize = 14;
        }

        private Point _startScreen;
        private Point _endScreen;
        private Rect _lineBounds;

        public override void CopyTemplate(ObjectBase objectBase, bool style)
        {
            System.Diagnostics.Debug.WriteLine("=== CopyTemplate called ===");
            System.Diagnostics.Debug.WriteLine($"objectBase type: {objectBase?.GetType()}");
            System.Diagnostics.Debug.WriteLine($"style: {style}");

            if (objectBase is TrendLineObject obj)
            {
                System.Diagnostics.Debug.WriteLine("Target is TrendLineObject");
                System.Diagnostics.Debug.WriteLine($"Source LineColor: {obj.LineColor}");
                System.Diagnostics.Debug.WriteLine($"Source LineWidth: {obj.LineWidth}");
                System.Diagnostics.Debug.WriteLine($"Source Text: {obj.Text}");

                // Копируем алерты
                Alert.Copy(obj.Alert, !style);
                OnPropertyChanged(nameof(Alert));
                AlertMinDistance = obj.AlertMinDistance;

                // Копируем настройки линии (через свойства!)
                LineColor = obj.LineColor;
                LineWidth = obj.LineWidth;
                LineStyle = obj.LineStyle;

                // Копируем настройки текста (через свойства!)
                Text = obj.Text;
                TextAlignment = obj.TextAlignment;
                FontSize = obj.FontSize;

                // Копируем настройки поведения (через свойства!)
                ExtendLeft = obj.ExtendLeft;
                ExtendRight = obj.ExtendRight;
                MagnetEnabled = obj.MagnetEnabled;

                System.Diagnostics.Debug.WriteLine("Copy completed");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Target is NOT TrendLineObject");
            }

            // ВАЖНО: base.CopyTemplate в конце!
            base.CopyTemplate(objectBase, style);
        }

        protected override void Draw(DxVisualQueue visual, ref List<ObjectLabelInfo> labels)
        {
            if (Canvas == null) return;

            // Используем сохраненные координаты из Prepare()
            var brush = new XBrush(_lineColor);
            var pen = new XPen(brush, _lineWidth, _lineStyle);

            // Рисуем линию
            visual.DrawLine(pen, _startScreen, _endScreen);

            // Добавляем информацию для подписей (цены)
            if (DataProvider != null)
            {
                labels.Add(new ObjectLabelInfo(ControlPoints[0].Y, _lineColor));
                labels.Add(new ObjectLabelInfo(ControlPoints[1].Y, _lineColor));
            }

            // Рисуем текст поверх линии с фоном
            DrawText(visual);
        }

        protected override bool InObject(int x, int y)
        {
            if (Canvas == null || ControlPoints.Length < 2) return false;

            // Быстрая проверка по ограничивающему прямоугольнику
            if (!_lineBounds.Contains(x, y)) return false;

            // Точная проверка расстояния до линии
            double distance = DistanceToSegment(new Point(x, y), _startScreen, _endScreen);
            return distance <= (LineWidth / 2.0) + 2;
        }

        public override void DrawControlPoints(DxVisualQueue visual)
        {
            if (Canvas == null || ControlPoints.Length < 2 || Theme == null) return;

            // Рисуем контрольные точки для каждого конца линии
            foreach (var cp in ControlPoints)
            {
                Point pt = ToPoint(cp);
                double size = 6; // Размер точки
                Rect rect = new Rect(pt.X - size, pt.Y - size, size * 2, size * 2);

                // Используем тему для цветов
                visual.FillRectangle(Theme.ChartCpFillBrush, rect);
                visual.DrawRectangle(Theme.ChartCpLinePen, rect);
            }
        }

        public override int GetControlPoint(int x, int y)
        {
            if (Canvas == null || ControlPoints.Length < 2) return -1;

            double threshold = 10; // Радиус захвата точки

            for (int i = 0; i < ControlPoints.Length; i++)
            {
                Point pt = ToPoint(ControlPoints[i]);
                double distance = Math.Sqrt((x - pt.X) * (x - pt.X) + (y - pt.Y) * (y - pt.Y));

                if (distance <= threshold)
                    return i;
            }

            return -1;
        }


        public override void ExtraPointChanged(int index, ObjectPoint op)
        {
            base.ExtraPointChanged(index, op);

            if (_isAdjusting) return;
            _isAdjusting = true;

            try
            {
                // Ваш существующий код для примагничивания и отладки
                if (_magnetEnabled && DataProvider != null && Canvas != null)
                {
                    // ... существующий код примагничивания ...
                }

                // НОВОЕ: Выравнивание по горизонтали при нажатом Shift
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    int otherIndex = (index == 0) ? 1 : 0;
                    // Устанавливаем Y другой точки равной Y изменённой
                    ControlPoints[otherIndex].Y = op.Y;
                }
            }
            finally
            {
                _isAdjusting = false;
            }
        }
        /*

        public override void ControlPointChanged(int index)
        {
            base.ControlPointChanged(index);

            if (_isAdjusting) return;
            _isAdjusting = true;

            try
            {
                // Выравнивание по горизонтали при нажатом Shift
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    int otherIndex = (index == 0) ? 1 : 0;
                    // Устанавливаем Y другой точки равной Y изменённой
                    ControlPoints[otherIndex].Y = ControlPoints[index].Y;
                }
            }
            finally
            {
                _isAdjusting = false;
            }
        } */

        public override void CheckAlert(List<IndicatorBase> indicators)
        {
            if (!Alert.IsActive || DataProvider == null || ControlPoints.Length < 2) return;

            try
            {
                // Уравнение линии: y = a * x + b, где x - индекс свечи
                double x1 = ControlPoints[0].X;
                double y1 = ControlPoints[0].Y;
                double x2 = ControlPoints[1].X;
                double y2 = ControlPoints[1].Y;
                double a = (y2 - y1) / (x2 - x1);
                double b = y1 - a * x1;

                foreach (var indicator in indicators)
                {
                    // Проверяем каждую свечу на графике
                    if (indicator.CheckAlert(ControlPoints[0].Y, AlertMinDistance, ref _lastAlertIndex, ref _lastAlertValue) ||
                        indicator.CheckAlert(ControlPoints[1].Y, AlertMinDistance, ref _lastAlertIndex, ref _lastAlertValue))
                    {
                        string message = $"{indicator.Name}: пересечение трендовой линии на уровне {DataProvider.Symbol.FormatPrice((decimal)ControlPoints[0].Y)}.";
                        AddAlert(Alert, message);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteError(ex);
            }
        }

        // Вспомогательный метод для расчета расстояния от точки до отрезка
        private double DistanceToSegment(Point p, Point a, Point b)
        {
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;

            // Если отрезок вырожден в точку
            if (dx == 0 && dy == 0)
                return Math.Sqrt((p.X - a.X) * (p.X - a.X) + (p.Y - a.Y) * (p.Y - a.Y));

            // Вычисляем проекцию точки на отрезок
            double t = ((p.X - a.X) * dx + (p.Y - a.Y) * dy) / (dx * dx + dy * dy);
            t = Math.Max(0, Math.Min(1, t));

            // Находим ближайшую точку на отрезке
            double projX = a.X + t * dx;
            double projY = a.Y + t * dy;

            // Возвращаем расстояние
            return Math.Sqrt((p.X - projX) * (p.X - projX) + (p.Y - projY) * (p.Y - projY));
        }

        private void DrawText(DxVisualQueue visual)
        {
            if (string.IsNullOrEmpty(_text) || TextAlignment == ObjectTextAlignment.Hide || Canvas == null)
                return;

            var font = new XFont(Canvas.ChartFont.Name, _fontSize);
            var textSize = font.GetSize(_text);

            // Определяем позицию текста (по центру линии)
            double centerX = (_startScreen.X + _endScreen.X) / 2.0;
            double centerY = (_startScreen.Y + _endScreen.Y) / 2.0;

            double x = centerX - textSize.Width / 2.0;
            double y = centerY - textSize.Height / 2.0;

            // Корректировка позиции в зависимости от выравнивания
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

            // Рисуем фон (цвет фона графика)
            if (Theme != null)
            {
                var bgBrush = new XBrush(Theme.ChartBackColor); // Изменено с ChartBackground на ChartBackColor
                visual.FillRectangle(bgBrush, textRect);
            }

            // Рисуем текст
            var textBrush = new XBrush(_lineColor);
            visual.DrawString(_text, font, textBrush, textRect);
        }


        protected override void Prepare()
        {
            base.Prepare();

            if (Canvas == null || ControlPoints.Length < 2) return;

            // Преобразуем координаты точек в экранные
            Point p1 = ToPoint(ControlPoints[0]);
            Point p2 = ToPoint(ControlPoints[1]);

            // Исследуем DataProvider при каждом Prepare (для отладки)
            if (DataProvider != null)
            {
                System.Diagnostics.Debug.WriteLine("=== DataProvider Properties ===");
                var properties = DataProvider.GetType().GetProperties();
                foreach (var prop in properties)
                {
                    try
                    {
                        var value = prop.GetValue(DataProvider);
                        System.Diagnostics.Debug.WriteLine($"Property: {prop.Name} = {value}");
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine($"Property: {prop.Name} = [Cannot read]");
                    }
                }

                System.Diagnostics.Debug.WriteLine("=== DataProvider Methods ===");
                var methods = DataProvider.GetType().GetMethods();
                foreach (var method in methods)
                {
                    if (method.Name.Contains("Candle") || method.Name.Contains("Get") || method.Name.Contains("Item") || method.Name.Contains("Data"))
                    {
                        System.Diagnostics.Debug.WriteLine($"Method: {method.Name}");
                    }
                }
            }

            // Если включено удлинение, продлеваем линию до границ холста
            if (_extendLeft || _extendRight)
            {
                double k = (p2.Y - p1.Y) / (p2.X - p1.X);
                double b = p1.Y - k * p1.X;

                if (_extendLeft)
                {
                    double leftX = Canvas.Rect.Left;
                    double leftY = k * leftX + b;
                    p1 = new Point(leftX, leftY);
                }

                if (_extendRight)
                {
                    double rightX = Canvas.Rect.Right;
                    double rightY = k * rightX + b;
                    p2 = new Point(rightX, rightY);
                }
            }

            _startScreen = p1;
            _endScreen = p2;

            // Вычисляем ограничивающий прямоугольник для хит-теста
            double minX = Math.Min(_startScreen.X, _endScreen.X) - 5;
            double minY = Math.Min(_startScreen.Y, _endScreen.Y) - 5;
            double maxX = Math.Max(_startScreen.X, _endScreen.X) + 5;
            double maxY = Math.Max(_startScreen.Y, _endScreen.Y) + 5;

            _lineBounds = new Rect(minX, minY, maxX - minX, maxY - minY);
        }





    }
}