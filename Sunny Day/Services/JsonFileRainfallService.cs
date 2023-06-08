using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using Sunny_Day.Models;
using Microsoft.Extensions.Configuration;
using System.Text;
using Newtonsoft.Json;

namespace Sunny_Day.Services
{
    public class JsonFileRainfallService
    {
        public JsonFileRainfallService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "mockdata.json"); }
        }

        public IEnumerable<Rainfall> GetDailyRainfall()
        {
            string url = $"http://forecast-v2.metoceanapi.com/point/time";

            var iso8601StringStart = "2023-06-05T00:00:00Z";
            DateTime dateISO8602Start = DateTime.ParseExact(iso8601StringStart, "yyyy-MM-ddTHH:mm:ssZ",
                                            System.Globalization.CultureInfo.InvariantCulture);

            var iso8601StringEnd = "2023-06-06T00:00:00Z";
            DateTime dateISO8602End = DateTime.ParseExact(iso8601StringEnd, "yyyy-MM-ddTHH:mm:ssZ",
                                            System.Globalization.CultureInfo.InvariantCulture);

            MetserviceRequestTemplate requestData = new MetserviceRequestTemplate();

            LatLngArray latLngArray = new LatLngArray();
            latLngArray.lat = -41.2;
            latLngArray.lon = 174.9;

            var latLngList = new List<LatLngArray>{
                latLngArray
            };

            TimeIntervals time = new TimeIntervals();
            time.from = dateISO8602Start;
            time.to = dateISO8602End;
            time.interval = "1h";

            requestData.points = latLngList;
            requestData.time = time;
            requestData.variables = new string[] { "precipitation.rate" };

            using (HttpClient client = new HttpClient())
            {

                var apiKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["MetserviceApiKey"];

                client.BaseAddress = new Uri(url);

                client.DefaultRequestHeaders
                 .Add("x-api-key", apiKey);

                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var requestJson = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);

                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url)
                };
                request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var response = client.SendAsync(request).Result;
                var deserialiserSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                var json = response.Content.ReadAsStringAsync().Result;
                Rainfall metserviceResponse = JsonConvert.DeserializeObject<Rainfall>(json, deserialiserSettings);
                yield return metserviceResponse;
            }
        }
    }
}
