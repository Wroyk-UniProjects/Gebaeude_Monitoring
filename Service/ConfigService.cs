using Building_Monitoring_WebApp.Model;
using System.Net.Http.Json;
using System.Text.Json;

namespace Building_Monitoring_WebApp.Service
{
    public class ConfigService : IConfigService
    {
        private readonly string url = "https://building-monitoring.azurewebsites.net/api/rooms/config";
        private HttpClient client;
        private JsonSerializerOptions jsonSerializerOptions;

        public ConfigService(HttpClient client)
        {
            this.client = client;
            this.jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive  = true };
        }
        public async Task<Config> GetConfig()
        {
            var response = await client.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var config = JsonSerializer.Deserialize<Config>(content, jsonSerializerOptions);
            return config;
        }
        public async Task PutConfig(Config newConfig)
        {
            var response = await client.PutAsJsonAsync(url, newConfig);
            var content = await response.Content.ReadFromJsonAsync<Config>();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content.ToString());
            }
        }
    }
}
