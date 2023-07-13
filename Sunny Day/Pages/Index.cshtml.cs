using Microsoft.AspNetCore.Mvc.RazorPages;
using Sunny_Day.Models;
using Sunny_Day.Services;


namespace Sunny_Day.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public JsonFileRainfallService RainfallService;
        public IEnumerable<Rainfall> Rainfalls { get; set; }


        public IndexModel(ILogger<IndexModel> logger, JsonFileRainfallService rainfallService)
        {
            _logger = logger;
            RainfallService = rainfallService;

        }

        public void OnGet()
        {
            Rainfalls = RainfallService.GetDailyRainfall();
        }
    }
}