using Building_Monitoring_WebApp.Model;

namespace Building_Monitoring_WebApp.Service
{
    public interface IConfigService
    {
        Task<Config> GetConfig();
        Task PutConfig(Config newConfig);
    }
}
