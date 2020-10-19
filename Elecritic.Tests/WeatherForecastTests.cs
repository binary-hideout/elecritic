using Elecritic.Models;

using Xunit;

namespace Elecritic.UnitTests {

    public class WeatherForecastTests {

        [Theory]
        [InlineData(0, 32)]
        [InlineData(-40, -40)]
        public void GetFahrenheit_FromCelsius_ReturnFahrenheitConversion(int input, int expected) {
            // arrange
            var weatherForecast = new WeatherForecast {
                TemperatureCelsius = input
            };

            // act
            var getFahrenheit = weatherForecast.TemperatureFahrenheit;

            // assert
            Assert.Equal(expected, getFahrenheit);
        }
    }
}