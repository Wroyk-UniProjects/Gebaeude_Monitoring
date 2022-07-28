﻿using Building_Monitoring_WebApp.Model;

namespace Building_Monitoring_WebApp.Service
{
    public interface IConfigService
    {
        // Gets Global config
        Task<Config> GetConfig();

        // Gets config for one room
        Task<Config> GetConfigByID(int id);
        
        // Sets global-config
        Task<bool> PatchConfig(Config newConfig);

        // Sets individual config for on room
        Task<bool> putIndividualConfigByID(int id, Config config);
        
        // Sets Room to use global-config
        Task<bool> deleteIndvidualConfigByID(int id);
    }
}
