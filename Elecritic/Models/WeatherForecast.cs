using System;

namespace Elecritic.Models {

    public class WeatherForecast {

        public DateTime Date { get; set; }

        public string Summary { get; set; }

        public int TemperatureCelsius { get; set; }

        public int TemperatureFahrenheit => (int)(TemperatureCelsius * (9.0 / 5.0)) + 32;
    }
}