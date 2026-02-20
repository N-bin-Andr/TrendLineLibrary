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
        public TrendLineObject()
        {
            // Инициализация двух контрольных точек
            ControlPoints = new ObjectPoint[2];
            ControlPoints[0] = new ObjectPoint(10, 100.0);
            ControlPoints[1] = new ObjectPoint(20, 110.0);
        }

        protected override void Draw(DxVisualQueue visual, ref List<ObjectLabelInfo> labels)
        {
            // Базовая отрисовка будет добавлена позже
        }

        protected override bool InObject(int x, int y)
        {
            return false;
        }

        protected override int PenWidth => 1;
    }
}