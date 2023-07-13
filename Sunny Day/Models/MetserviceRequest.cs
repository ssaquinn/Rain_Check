namespace Sunny_Day.Models
{

    public class MetserviceRequestTemplate
    {

        public List<LatLngArray> points { get; set; }
        public string[] variables { get; set; }
        public TimeIntervals time { get; set; }
    }

    public class LatLngArray
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    public class TimeIntervals
    {
        public DateTime from { get; set; }
        public string interval { get; set; }
        public DateTime to { get; set; }
    }
}