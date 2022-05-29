using Building_Monitoring_WebApp.Model;
using System.Text.Json;

namespace Building_Monitoring_WebApp.Service
{
    public class ConfigService
    {
        private HttpClient client;
        private JsonSerializerOptions jsonSerializerOptions;

        public ConfigService(HttpClient client)
        {
            this.client = client;
            this.jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive  = true };
        }
        public async Task<Config> GetConfig()
        {
            var response = await client.GetAsync("https://building-monitoring.azurewebsites.net/api/rooms/config'");

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var config = JsonSerializer.Deserialize<Config>(content, jsonSerializerOptions);
            return config;
        }
    }
}
