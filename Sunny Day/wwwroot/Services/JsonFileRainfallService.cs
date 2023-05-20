using System.Collections.Generic;
using System.IO;

namespace Sunny_Day.wwwroot.Services
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
