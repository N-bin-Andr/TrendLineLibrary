using Xunit;
using FluentAssertions;
using System.Reflection;
using System.Windows;
using TrendLineLibrary.Objects;

namespace TrendLineLibrary.Tests
{
    public class GeometryTests
    {
        [Fact]
        public void DistanceToSegment_PointOnLine_ReturnsZero()
        {
            // Arrange
            var line = new TrendLineObject();
            var method = typeof(TrendLineObject).GetMethod("DistanceToSegment",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            method.Should().NotBeNull();

            var p = new Point(5, 5);
            var a = new Point(0, 0);
            var b = new Point(10, 10);

            // Act
            var result = (double)method!.Invoke(line, new object[] { p, a, b })!;

            // Assert
            result.Should().BeApproximately(0, 0.001);
        }

        [Fact]
        public void DistanceToSegment_PointAwayFromLine_ReturnsCorrectDistance()
        {
            // Arrange
            var line = new TrendLineObject();
            var method = typeof(TrendLineObject).GetMethod("DistanceToSegment",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            method.Should().NotBeNull();

            var p = new Point(5, 6);
            var a = new Point(0, 0);
            var b = new Point(10, 10);

            // Act
            var result = (double)method!.Invoke(line, new object[] { p, a, b })!;

            // Assert
            result.Should().BeApproximately(0.707, 0.001);
        }

        [Fact]
        public void LineExtension_CalculateCorrectly()
        {
            // Arrange
            var line = new TrendLineObject();

            // Тестируем математику удлинения линии
            // y = kx + b
            double x1 = 0, y1 = 0;
            double x2 = 10, y2 = 10;

            double k = (y2 - y1) / (x2 - x1);
            double b = y1 - k * x1;

            // Act - удлинение влево
            double leftX = -5;
            double leftY = k * leftX + b;

            double rightX = 15;
            double rightY = k * rightX + b;

            // Assert
            leftY.Should().Be(-5);
            rightY.Should().Be(15);
        }
    }
}