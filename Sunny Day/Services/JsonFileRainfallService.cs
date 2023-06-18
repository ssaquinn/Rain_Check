using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using Sunny_Day.Models;
using Microsoft.Extensions.Configuration;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Intrinsics.X86;
using System;

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
            Rainfall metserviceResponse;
            for (int i = 0; i < 10; i++)
            {

                var startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + i, 0, 0, 0);
                var endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + i, 23, 59, 59);
                var zone = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");

                string url = $"http://forecast-v2.metoceanapi.com/point/time";

                DateTime dateISO8602Start = DateTime.ParseExact(TimeZoneInfo.ConvertTimeFromUtc(startTime, zone).ToString("yyyy-MM-ddTHH:mm:ssZ"), "yyyy-MM-ddTHH:mm:ssZ",
                                                System.Globalization.CultureInfo.InvariantCulture);


                DateTime dateISO8602End = DateTime.ParseExact(TimeZoneInfo.ConvertTimeFromUtc(endTime, zone).ToString("yyyy-MM-ddTHH:mm:ssZ"), "yyyy-MM-ddTHH:mm:ssZ",
                                                System.Globalization.CultureInfo.InvariantCulture);


                MetserviceRequestTemplate requestData = new MetserviceRequestTemplate();

                LatLngArray latLngArray = new LatLngArray();
                latLngArray.lat = -41.2;
                latLngArray.lon = 174.9;

                var latLngList = new List<LatLngArray>{latLngArray};

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
                   yield return metserviceResponse = (JsonConvert.DeserializeObject<Rainfall>(json, deserialiserSettings));

                }

            }
           
        }
    }
}
