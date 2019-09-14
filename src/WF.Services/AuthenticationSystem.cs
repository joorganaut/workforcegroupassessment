using WF.Core.Models;
using WF.Core.Processors;
using WF.Service.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.DirectoryServices;

namespace WF.Service
{
    public class AuthenticationSystem
    {
        HTTPConfig postingConfig;
        string LDAPHost = ConfigurationManager.AppSettings["LDAPHost"]; //LDAP://ldap.forumsys.com:389/ou=scientists,dc=example,dc=com
        public AuthenticationSystem()
        {
            postingConfig = new HTTPConfig();
            postingConfig.DataType = "application/json";
            postingConfig.Method = HTTPMethod.POST;
        }
        
        

        public async Task<AuthenticationResponse> AuthenticateActiveDirectory(AuthenticationRequest request)
        {
            AuthenticationResponse result = new AuthenticationResponse();
            try
            {
                string ldapServer = LDAPHost;
                string userName = request.Username;
                string password = request.Password;
                using (var dirctoryEntry = new DirectoryEntry(ldapServer, userName, password, AuthenticationTypes.ServerBind))
                {
                    object nativeObject = dirctoryEntry.NativeObject;
                    if (nativeObject != null)
                    {
                        result.IsAuthenticated = true;
                    }
                }
            }
            catch (AggregateException ae)
            {
                result.ResponseCode = "096";
                result.ResponseMessage = $"Unable to parse response message: {ae.Message}";
            }
            catch (Exception e)
            {
                result.ResponseCode = "096";
                result.ResponseMessage = $"Unable to parse response message: {e.Message}";
            }
            return result;
        }
    }
}
