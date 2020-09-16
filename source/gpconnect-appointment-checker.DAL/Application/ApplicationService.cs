﻿using Dapper;
using gpconnect_appointment_checker.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace gpconnect_appointment_checker.DAL.Application
{
    public class ApplicationService : IApplicationService
    {
        private readonly ILogger<ApplicationService> _logger;
        private readonly IDataService _dataService;
        private readonly IAuditService _auditService;

        public ApplicationService(IConfiguration configuration, ILogger<ApplicationService> logger, IDataService dataService, IAuditService auditService)
        {
            _logger = logger;
            _dataService = dataService;
            _auditService = auditService;
        }

        public async Task<DTO.Response.Application.Organisation> GetOrganisation(string odsCode)
        {
            var functionName = "application.get_organisation";
            var parameters = new DynamicParameters();
            parameters.Add("_ods_code", odsCode, DbType.String, ParameterDirection.Input);
            var result = await _dataService.ExecuteFunction<DTO.Response.Application.Organisation>(functionName, parameters);
            return result.FirstOrDefault();
        }

        public async void SynchroniseOrganisation(DTO.Request.Application.Organisation organisation)
        {
            var functionName = "application.synchronise_organisation";
            var parameters = new DynamicParameters();
            parameters.Add("_ods_code", organisation.ODSCode);
            parameters.Add("_organisation_type_name", organisation.OrganisationTypeName);
            parameters.Add("_organisation_name", organisation.OrganisationName);
            parameters.Add("_address_line_1", organisation.AddressLine1);
            parameters.Add("_address_line_2", organisation.AddressLine2);
            parameters.Add("_locality", organisation.Locality);
            parameters.Add("_city", organisation.City);
            parameters.Add("_county", organisation.County);
            parameters.Add("_postcode", organisation.Postcode);
            parameters.Add("_is_gpconnect_consumer", organisation.IsGPConnectConsumer);
            parameters.Add("_is_gpconnect_provider", organisation.IsGPConnectProvider);
            await _dataService.ExecuteFunction(functionName, parameters);
        }
        public async void LogonUser(DTO.Request.Application.User user)
        {
            var functionName = "application.logon_user";
            var parameters = new DynamicParameters();
            parameters.Add("_email_address", user.EmailAddress);
            parameters.Add("_display_name", user.DisplayName);
            parameters.Add("_organisation_id", user.OrganisationId);
            await _dataService.ExecuteFunction(functionName, parameters);
        }

        public async void LogoffUser(DTO.Request.Application.User user)
        {
            var functionName = "application.logoff_user";
            var parameters = new DynamicParameters();
            parameters.Add("_email_address", user.EmailAddress);
            parameters.Add("_user_session_id", user.UserSessionId);
            await _dataService.ExecuteFunction(functionName, parameters);
        }

        public async void SetUserAuthorised(DTO.Request.Application.User user)
        {
            var functionName = "application.set_user_isauthorised";
            var parameters = new DynamicParameters();
            parameters.Add("_email_address", user.EmailAddress);
            parameters.Add("_is_authorised", user.IsAuthorised);
            await _dataService.ExecuteFunction(functionName, parameters);
        }
    }
}