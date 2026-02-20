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

        protected override void Draw(DxVisualQueue visual, ref List<ObjectLabelInfo> labels)
        {
            if (Canvas == null) return;

            // Получаем экранные координаты точек
            var startPoint = ToPoint(ControlPoints[0]);
            var endPoint = ToPoint(ControlPoints[1]);

            // Создаем перо для рисования
            var brush = new XBrush(_lineColor);
            var pen = new XPen(brush, _lineWidth, _lineStyle);

            // Рисуем линию
            visual.DrawLine(pen, startPoint, endPoint);
        }

        protected override bool InObject(int x, int y)
        {
            return false;
        }
    }
}