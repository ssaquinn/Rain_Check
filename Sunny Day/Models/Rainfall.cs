using Newtonsoft.Json;
using System;

namespace Sunny_Day.Models
{
    public class Rainfall
    {
        public string? title { get; set; }

        public Dimensions? dimensions { get; set; }
        public Variables? variables { get; set; }

    }

    public class Dimensions {
        public Time? time { get; set; }
      
    }

    public class Time
    {
        public DateTime[]? data { get; set; }
    }

    public class Variables
    {
        [JsonProperty("precipitation.rate")]
        public Precipitation? precipitationRate { get; set; }
    }

    public class Precipitation
    {

        
        public double[]? data { get; set; }
    }
}
