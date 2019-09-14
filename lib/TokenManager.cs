using System;
using LMS_Portal.UI.Core.Utilities;
using Newtonsoft.Json;

namespace LMS_Portal.UI.Core.BusinessLogic
{
    public static class TokenManager
    {
        public static bool AuthenticateUserToken(string subsidiaryId, string username, string token,
            string callerFormName, string callerFormMethod, string callerIpAddress)
        {
            var result = false;

            try
            {
                if (ConfigurationUtility.GetAppSettingValue("EnableToken").ToUpper().Equals("Y"))
                {
                    var entrustServiceClient = new EntrustServiceClient.AuthWrapper();
                    var entrustServiceUrl = ConfigurationUtility.GetAppSettingValue("EntrustServiceUrl");
                    if (!string.IsNullOrEmpty(entrustServiceUrl))
                    {
                        entrustServiceClient.Url = entrustServiceUrl;
                    }

                    var response = entrustServiceClient.AuthMethod(new EntrustServiceClient.AuthRequest
                        {CustID = username, PassCode = token});

                    //
                    LogUtility.LogInfo(callerFormName, callerFormMethod, callerIpAddress,
                        $"Token Authentication for User >> {username} >> | Response >> {JsonConvert.SerializeObject(response)} | Result >> {result}");

                    if (response.Authenticated)
                    {
                        result = true;
                    }
                }
                else
                {
                    result = true;
                }

            }
            catch (Exception ex)
            {
                LogUtility.LogError(callerFormName, callerFormMethod, callerIpAddress, ex);
            }

            return result;
        }
    }
}