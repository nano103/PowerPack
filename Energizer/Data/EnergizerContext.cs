using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Energizer.Models
{
    public class EnergizerContext
    {
        private static EnergizerContext instance;
        public static EnergizerContext getInstance()
        {
            if (instance == null)
                instance = new EnergizerContext();
            return instance;
        }

        static HttpClient client = new HttpClient();
        public EnergizerContext ()         
        {
            client.BaseAddress = new Uri("http://localhost:8050/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Uri> CreateMeasurePointAsync(MeasurePoint MeasurePoint)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/MeasurePoints", MeasurePoint);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        public async Task<ICollection<CalcUnit>> GetCalcUnitsAsync(string filter)
        {
            ICollection<CalcUnit> CalcUnits = null;
            HttpResponseMessage response = await client.GetAsync("api/CalcUnits/"+ filter);
            if (response.IsSuccessStatusCode)
            {
                CalcUnits = await response.Content.ReadAsAsync<ICollection<CalcUnit>>();
            }
            return CalcUnits;
        }

        public async Task<ICollection<MeasurePoint>> GetMeasurePointsAsync()
        {
            ICollection<MeasurePoint> MeasurePoints = null;
            HttpResponseMessage response = await client.GetAsync("api/MeasurePoints");
            if (response.IsSuccessStatusCode)
            {
                MeasurePoints = await response.Content.ReadAsAsync<ICollection<MeasurePoint>>();
            }
            return MeasurePoints;
        }

        public async Task<ICollection<Consumer>> GetConsumersAsync()
        {
            ICollection<Consumer> Consumers = null;
            HttpResponseMessage response = await client.GetAsync("api/Consumers");
            if (response.IsSuccessStatusCode)
            {
                Consumers = await response.Content.ReadAsAsync<ICollection<Consumer>>();
            }
            return Consumers;
        }
        public async Task<ICollection<EntityType>> GetOutdatedAsync<EntityType>(string filter)
        {
            ICollection<EntityType> ElectricMeters = null;
            HttpResponseMessage response = await client.GetAsync("api/outdated/" + filter);
            if (response.IsSuccessStatusCode)
            {
                ElectricMeters = await response.Content.ReadAsAsync<ICollection<EntityType>>();
            }
            return ElectricMeters;
        }
    }
}
