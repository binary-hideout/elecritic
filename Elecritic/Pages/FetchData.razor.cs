using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Elecritic.Models;

using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {

    public partial class FetchData {

        [Inject]
        private HttpClient Http { get; set; }

        private WeatherForecast[] Forecasts { get; set; }

        protected override async Task OnInitializedAsync() {
            Forecasts = await Http.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json");
        }
    }
}