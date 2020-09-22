﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace gpconnect_appointment_checker.DAL.Interfaces
{
    public interface IConfigurationService
    {
        Task<List<DTO.Response.Configuration.General>> GetGeneralConfigurations();
        Task<DTO.Response.Configuration.Spine> GetSpineConfiguration();
        Task<List<DTO.Response.Configuration.SdsQuery>> GetSdsQueryConfiguration();
        Task<DTO.Response.Configuration.Sso> GetSsoConfiguration();
    }
}
