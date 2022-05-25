
using Building_Monitoring_WebApp.Model;
using System.Text.Json;

namespace Building_Monitoring_WebApp.Service
{
    public class RoomService : IRoomService
    {
        private readonly HttpClient client;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public RoomService(HttpClient client)
        {
            this.client = client;
            this.jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        public async Task<List<Room>> GetRooms()
        {
            var response = await client.GetAsync("https://building-monitoring.azurewebsites.net/api/rooms");

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var rooms = JsonSerializer.Deserialize<List<Room>>(content, jsonSerializerOptions);
            return rooms;
        }

        public async Task<Room> GetRoom(int id)
        {
            var response = await client.GetAsync("https://building-monitoring.azurewebsites.net/api/rooms/" + id.ToString());

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var room = JsonSerializer.Deserialize<Room>(content, jsonSerializerOptions);
            return room;
        }
    }
}
