using Xunit;
using TrendLineLibrary.Objects;

namespace TrendLineLibrary.Tests;

public class TrendLineObjectTests
{
    [Fact]
    public void TrendLineObject_DefaultConstructor_ShouldInitializeTwoPoints()
    {
        // Arrange & Act
        var line = new TrendLineObject();

        // Assert
        Assert.NotNull(line);
        Assert.Equal(2, line.ControlPoints.Length);
    }
}