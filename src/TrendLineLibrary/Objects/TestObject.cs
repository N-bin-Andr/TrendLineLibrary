using System.Runtime.Serialization;
using TigerTrade.Chart.Base;
using TigerTrade.Chart.Objects.Common;
using TigerTrade.Dx;
using System.Collections.Generic;

namespace TrendLineLibrary.Objects
{
    [DataContract(Name = "TestObject")]
    [ChartObject("X_Test", "Тестовый объект", 1, Type = typeof(TestObject))]
    public sealed class TestObject : ObjectBase
    {
        public TestObject()
        {
            ControlPoints = new ObjectPoint[1];
            ControlPoints[0] = new ObjectPoint(10, 100.0);
        }

        protected override void Draw(DxVisualQueue visual, ref List<ObjectLabelInfo> labels)
        {
        }

        protected override bool InObject(int x, int y)
        {
            return false;
        }

        protected override int PenWidth => 1;
    }
}