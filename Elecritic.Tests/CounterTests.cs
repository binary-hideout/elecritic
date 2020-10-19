using Bunit;

using Elecritic.Pages;

using Xunit;

namespace Elecritic.UnitTests {
    public class CounterTests {

        [Fact]
        public void IncrementCount_FirstClick_CurrentCountIsOne() {
            // arrange
            using var context = new TestContext();
            var counter = context.RenderComponent<Counter>();

            // act
            counter.Find("button").Click();
            var actual = 1;

            // assert
            counter.Find("p").MarkupMatches($"<p>Current count: {actual}</p>");
        }
    }
}
