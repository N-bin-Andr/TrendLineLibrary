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

        protected override void Prepare()
        {
            base.Prepare();

            if (Canvas == null || ControlPoints.Length < 2) return;

            // Преобразуем координаты точек в экранные
            _startScreen = ToPoint(ControlPoints[0]);
            _endScreen = ToPoint(ControlPoints[1]);

            // Вычисляем ограничивающий прямоугольник для хит-теста
            double minX = Math.Min(_startScreen.X, _endScreen.X) - 5;
            double minY = Math.Min(_startScreen.Y, _endScreen.Y) - 5;
            double maxX = Math.Max(_startScreen.X, _endScreen.X) + 5;
            double maxY = Math.Max(_startScreen.Y, _endScreen.Y) + 5;

            _lineBounds = new Rect(minX, minY, maxX - minX, maxY - minY);
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
    }
}