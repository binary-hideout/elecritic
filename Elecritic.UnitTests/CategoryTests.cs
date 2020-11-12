
using Elecritic.Models;

using Xunit;

namespace Elecritic.UnitTests {
    public class CategoryTests {

        /// <summary>
        /// Test that method <see cref="Category.ToString"/> returns the same string as accessing property <see cref="Category.Name"/>.
        /// </summary>
        [Fact]
        public void ToString_MatchesNameProperty() {
            // arrange
            var cellphone = "Cellphone";
            var category = new Category {
                Name = cellphone
            };

            // act
            var actual = category.ToString();

            // assert
            var expected = category.Name;
            Assert.Equal(expected, actual);
        }
    }
}
