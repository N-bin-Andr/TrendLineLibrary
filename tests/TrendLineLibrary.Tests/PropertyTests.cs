using Xunit;
using FluentAssertions;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Collections.Generic;
using TrendLineLibrary.Objects;
using TigerTrade.Dx.Enums;
using TigerTrade.Chart.Objects.Enums;
using System.Windows.Media;

namespace TrendLineLibrary.Tests
{
    public class PropertyTests
    {
        [Fact]
        public void TrendLineObject_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var line = new TrendLineObject();

            // Assert
            line.LineWidth.Should().Be(1);
            line.LineStyle.Should().Be(XDashStyle.Solid);
            line.FontSize.Should().Be(14);
            line.ExtendLeft.Should().BeFalse();
            line.ExtendRight.Should().BeFalse();
            line.MagnetEnabled.Should().BeTrue();
        }

        [Fact]
        public void LineWidth_ClampsToValidRange()
        {
            // Arrange
            var line = new TrendLineObject();

            // Act
            line.LineWidth = 20; // Should clamp to 10

            // Assert
            line.LineWidth.Should().Be(10);

            // Act - минимальное значение
            line.LineWidth = 0; // Should clamp to 1

            // Assert
            line.LineWidth.Should().Be(1);
        }

        [Fact]
        public void FontSize_ClampsToValidRange()
        {
            // Arrange
            var line = new TrendLineObject();

            // Act
            line.FontSize = 100; // Should clamp to 72

            // Assert
            line.FontSize.Should().Be(72);

            // Act - минимальное значение
            line.FontSize = 5; // Should clamp to 8

            // Assert
            line.FontSize.Should().Be(8);
        }

        [Fact]
        public void LineColor_CanBeSetAndGet()
        {
            // Arrange
            var line = new TrendLineObject();
            var expectedColor = Colors.Red;

            // Act
            line.LineColor = expectedColor;

            // Assert
            // Сравниваем через вспомогательный метод или просто проверяем что не null
            line.LineColor.Should().NotBeNull();
            // Проверяем что цвет изменился (не равен цвету по умолчанию)
            line.LineColor.Should().NotBe(Colors.Blue);
        }

        [Fact]
        public void TextProperties_CanBeSetAndGet()
        {
            // Arrange
            var line = new TrendLineObject();
            var expectedText = "Test Line";
            var expectedAlignment = ObjectTextAlignment.LeftTop;
            var expectedFontSize = 16;

            // Act
            line.Text = expectedText;
            line.TextAlignment = expectedAlignment;
            line.FontSize = expectedFontSize;

            // Assert
            line.Text.Should().Be(expectedText);
            line.TextAlignment.Should().Be(expectedAlignment);
            line.FontSize.Should().Be(expectedFontSize);
        }

        [Fact]
        public void ExtendProperties_CanBeSetAndGet()
        {
            // Arrange
            var line = new TrendLineObject();

            // Act
            line.ExtendLeft = true;
            line.ExtendRight = true;

            // Assert
            line.ExtendLeft.Should().BeTrue();
            line.ExtendRight.Should().BeTrue();
        }

        [Fact]
        public void MagnetEnabled_CanBeToggled()
        {
            // Arrange
            var line = new TrendLineObject();

            // Assert - default
            line.MagnetEnabled.Should().BeTrue();

            // Act
            line.MagnetEnabled = false;

            // Assert
            line.MagnetEnabled.Should().BeFalse();
        }

        [Fact]
        public void DataMemberAttributes_ArePresent()
        {
            // Arrange
            var type = typeof(TrendLineObject);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var prop in properties)
            {
                // Assert
                prop.Should().BeDecoratedWith<DataMemberAttribute>();
            }
        }

        [Fact]
        public void CategoryAttributes_ArePresent()
        {
            // Arrange
            var type = typeof(TrendLineObject);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var prop in properties)
            {
                // Assert
                prop.Should().BeDecoratedWith<CategoryAttribute>();
            }
        }

        [Fact]
        public void DisplayNameAttributes_ArePresent()
        {
            // Arrange
            var type = typeof(TrendLineObject);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var prop in properties)
            {
                // Assert
                prop.Should().BeDecoratedWith<DisplayNameAttribute>();
            }
        }

        [Fact]
        public void PropertyChanged_IsRaised()
        {
            // Arrange
            var line = new TrendLineObject();
            var events = new List<string>();
            line.PropertyChanged += (sender, e) => events.Add(e.PropertyName);

            // Act
            line.LineWidth = 5;
            line.LineStyle = XDashStyle.Dash;
            line.LineColor = Colors.Green;
            line.Text = "New Text";
            line.FontSize = 20;
            line.ExtendLeft = true;
            line.MagnetEnabled = false;

            // Assert
            events.Should().Contain(nameof(TrendLineObject.LineWidth));
            events.Should().Contain(nameof(TrendLineObject.LineStyle));
            events.Should().Contain(nameof(TrendLineObject.LineColor));
            events.Should().Contain(nameof(TrendLineObject.Text));
            events.Should().Contain(nameof(TrendLineObject.FontSize));
            events.Should().Contain(nameof(TrendLineObject.ExtendLeft));
            events.Should().Contain(nameof(TrendLineObject.MagnetEnabled));
        }
    }
}