using System.Linq;
using Xunit;

namespace TransportTycoon.Tests
{
    public class SolutionTests
    {
        [Theory]
        [InlineData("A", 5)]
        [InlineData("B", 5)]
        [InlineData("AB", 5)]
        [InlineData("BB", 5)]
        [InlineData("ABB", 7)]
        [InlineData("ABBA", 15)]
        [InlineData("AAAABBBB", 29)]
        public void ShouldDeliver(string destinations, int durationTimeInHours)
        {
            // Arrange
            var solution = new World(destinations.Select(x => x.ToString()));

            // Act
            solution.Deliver();

            // Assert
            Assert.Equal(durationTimeInHours, solution.CurrentTime);
        }
    }
}
