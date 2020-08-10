using System;

namespace ReadSwap.Api
{
    /// <summary>
    /// The forcast of the day
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// The date for the forcast
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Tmp in c
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// Tmp in F
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// Small Desciption
        /// </summary>
        public string Summary { get; set; }
    }
}
