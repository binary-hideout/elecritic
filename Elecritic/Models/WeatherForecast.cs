using System;

namespace Elecritic.Models {

    /// <summary>
    /// Sample class. It's only purpose is to demonstrate the use of a sample unit test (see Elecritic.UnitTests).
    /// If there is no test using this, remove the class.
    /// </summary>
    public class WeatherForecast {

        public DateTime Date { get; set; }

        public string Summary { get; set; }

        public int TemperatureCelsius { get; set; }

        public int TemperatureFahrenheit => (int)(TemperatureCelsius * (9.0 / 5.0)) + 32;
    }
}