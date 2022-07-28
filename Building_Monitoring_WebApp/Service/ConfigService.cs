using Building_Monitoring_WebApp.Model;
using System.Net.Http.Json;
using System.Text.Json;

namespace Building_Monitoring_WebApp.Service
{
    public class ConfigService : IConfigService
    {
        private readonly string urlBase = "https://building-monitoring.azurewebsites.net/api/rooms";
        private HttpClient client;
        private JsonSerializerOptions jsonSerializerOptions;

        public ConfigService(HttpClient client)
        {
            this.client = client;
            this.jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive  = true };
        }

		public async Task<bool> deleteIndvidualConfigByID(int id)
		{
            var response = await client.DeleteAsync(urlBase + $"/{id}/config/");

            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }

		public async Task<Config> GetConfig()
        {
            var response = await client.GetAsync(urlBase + "/config/");

            var content = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            var config = JsonSerializer.Deserialize<Config>(content, jsonSerializerOptions);
            return config;
        }

		public async Task<Config> GetConfigByID(int id)
		{
            var response = await client.GetAsync(urlBase + $"/{id}/config/");

            var content = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            var config = JsonSerializer.Deserialize<Config>(content, jsonSerializerOptions);
            return config;
        }

		public async Task<bool> PatchConfig(Config newConfig)
        {
            var jsonContent = JsonContent.Create(newConfig);
            var response = await client.PatchAsync(urlBase + "/config/", jsonContent);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }

		public async Task<bool> putIndividualConfigByID(int id, Config config)
		{
            var jsonContent = JsonContent.Create(config);
            var response = await client.PutAsync(urlBase + $"/{id}/config/", jsonContent);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }
	}
}
