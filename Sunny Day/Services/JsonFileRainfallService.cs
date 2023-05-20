using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Sunny_Day.Models;

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

        public IEnumerable<Rainfall> GetRainfall()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<Rainfall[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }
    }
}
