using Building_Monitoring_WebApp.Model;

namespace Building_Monitoring_WebApp.Service
{
    public interface IRoomService
    {
        Task<List<Room>> GetRooms();
    }
}
