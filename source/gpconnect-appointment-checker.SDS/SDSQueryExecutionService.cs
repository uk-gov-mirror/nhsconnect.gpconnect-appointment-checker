﻿using gpconnect_appointment_checker.DAL.Interfaces;
using gpconnect_appointment_checker.DTO.Request.Logging;
using gpconnect_appointment_checker.SDS.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace gpconnect_appointment_checker.SDS
{
    public class SDSQueryExecutionService : ISDSQueryExecutionService
    {
        private readonly ILogger<SDSQueryExecutionService> _logger;
        private readonly ILogService _logService;
        private readonly IConfiguration _configuration;
        private readonly LdapConnection _connection;

        public SDSQueryExecutionService(ILogger<SDSQueryExecutionService> logger, ILogService logService, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _logService = logService;
        }

        public async Task<T> ExecuteLdapQuery<T>(string searchBase, string filter) where T : class
        {
            return await ExecuteLdapQuery<T>(searchBase, filter, null);
        }

        public async Task<T> ExecuteLdapQuery<T>(string searchBase, string filter, string[] attributes) where T : class
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                var logMessage = new SpineMessage
                {
                    RequestPayload = $"{searchBase} {filter} {attributes}",
                    SpineMessageTypeId = (int)GPConnect.Constants.SpineMessageTypes.SpineLdapQuery
                };
                var ldapConnection = await GetConnection();
                var results = new Dictionary<string, object>();
                var searchResults = ldapConnection.Search(searchBase, LdapConnection.ScopeSub, filter, attributes, false);

                while (searchResults.HasMore())
                {
                    var nextEntry = searchResults.Next();
                    var attributeSet = nextEntry.GetAttributeSet();

                    foreach (var attribute in attributeSet)
                    {
                        results.Add(attribute.Name, attribute.StringValue);
                    }
                }

                if (results.Count > 0)
                {
                    string jsonDictionary = JsonConvert.SerializeObject(results);
                    logMessage.ResponsePayload = jsonDictionary;
                    logMessage.RoundTripTimeMs = sw.ElapsedMilliseconds;
                    _logService.AddSpineMessageLog(logMessage);

                    var result = JsonConvert.DeserializeObject<T>(jsonDictionary);
                    return result;
                }
                return null;
            }
            catch (Exception exc)
            {
                _logger.LogError("An error has occurred while attempting to execute an LDAP query", exc);
                throw;
            }
        }

        private async Task<ILdapConnection> GetConnection()
        {
            try
            {
                var ldapConn = _connection;

                if (ldapConn == null)
                {
                    string userName = string.Empty;
                    string password = string.Empty;

                    ldapConn = new LdapConnection
                    {
                        SecureSocketLayer = bool.TryParse(_configuration.GetSection("Spine:sds_use_ldaps").Value, out _),
                        ConnectionTimeout = int.Parse(_configuration.GetSection("Spine:timeout_seconds").Value) * 1000
                    };
                    ldapConn.Connect(_configuration.GetSection("Spine:sds_hostname").Value, int.Parse(_configuration.GetSection("Spine:sds_port").Value));
                    ldapConn.Bind(userName, password);
                }

                return ldapConn;
            }
            catch (TimeoutException timeoutException)
            {
                _logger.LogError("A timeout error has occurred", timeoutException);
                throw;
            }
            catch (Exception exc)
            {
                _logger.LogError("An error has occurred while attempting to establish a connection to the LDAP server", exc);
                throw;
            }
        }
    }
}
