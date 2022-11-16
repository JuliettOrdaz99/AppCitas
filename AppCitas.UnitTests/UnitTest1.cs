namespace AppCitas.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var x = 1;

            // Act
            var y = 2 * x;

            // Assert
            Assert.Equal(2, y);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 4)]
        public void Test2(int x, int resp)
        {
            // Arrange

            // Act
            int y = 2 * x;

            // Assert
            Assert.Equal(resp, y);
        }
    }
}