using System.Windows.Controls;

namespace Wpf.NotificationCenter.Tests.Converters
{
    public class ExpanderRotateAngleConverterTests : MockerTestBase<ExpanderRotateAngleConverter>
    {

        [Fact]
        public void Create_ShouldCreateSuccessfully()
        {
            Component.Should().NotBeNull();
        }

        [Theory]
        [InlineData(ExpandDirection.Left, 90)]
        [InlineData(ExpandDirection.Right, -90)]
        [InlineData(ExpandDirection.Up, 0)]
        [InlineData(ExpandDirection.Down, 0)]
        [InlineData(null, 0)]
        [InlineData("foo", 0)]
        public void Convert_Correct_ShouldWork(object direction, object expected)
        {
            Component.Convert(direction, null, null, null).Should().Be(expected);
        }

    }
}
