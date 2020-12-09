using Elecritic.Models;

using Xunit;

namespace Elecritic.UnitTests {

    /// <summary>
    /// Unit tests for <see cref="Company"/> model class.
    /// </summary>
    public class CompanyTests {

        /// <summary>
        /// Test that method <see cref="Company.ToString"/> returns the same string as accessing property <see cref="Company.Name"/>.
        /// </summary>
        [Fact]
        public void ToString_MatchesNameProperty() {
            // arrange
            var aBrand = "A Brand";
            var company = new Company {
                Name = aBrand
            };

            // act
            var actual = company.ToString();

            // assert
            var expected = company.Name;
            Assert.Equal(expected, actual);
        }
    }
}