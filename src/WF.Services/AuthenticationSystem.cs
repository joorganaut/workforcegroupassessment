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
using EntrustServiceClient = WF.Service.EntrustServiceRef;

namespace WF.Service
{
    public class AuthenticationSystem
    {
        HTTPConfig postingConfig;
        public AuthenticationSystem()
        {
            postingConfig = new HTTPConfig();
            postingConfig.DataType = "application/json";
            postingConfig.Method = HTTPMethod.POST;
        }
        public async static Task<bool> AuthenticateUserToken(string subsidiaryId, string username, string token,
            string callerFormName, string callerFormMethod, string callerIpAddress)
        {
            var result = false;

            try
            {
                using (EntrustServiceClient.AuthWrapperClient client = new EntrustServiceClient.AuthWrapperClient())
                {
                    var response = await client.AuthMethodAsync(new EntrustServiceClient.AuthRequest
                    { CustID = username, PassCode = token });
                    await ServiceLogger.WriteLog($"{callerFormName}, {callerFormMethod}, {callerIpAddress}, Token Authentication for User >> {username} >> | Response >> {JsonConvert.SerializeObject(response)} | Result >> {result}");
                    if (response.Authenticated)
                    {
                        result = true;
                    }
                } 
            }
            catch (Exception ex)
            {
                await ServiceLogger.WriteLog($"This error for {callerFormName}, {callerFormMethod}, {callerIpAddress} => {ex}");
            }

            return result;
        }
        public async Task<AuthenticationResponse> Authenticate2FA(AuthenticationRequest request)
        {
            AuthenticationResponse result = new AuthenticationResponse();
            try
            {
                using (EntrustServiceClient.AuthWrapperClient client = new EntrustServiceClient.AuthWrapperClient())
                {
                    var response = await client.AuthMethodAsync(new EntrustServiceClient.AuthRequest
                    { CustID = request.Username, PassCode = request.Token });
                    if (response != null)
                    {
                        if (response.Authenticated)
                        {
                            result.IsAuthenticated = response.Authenticated;
                            result.ResponseCode = "000";
                            result.ResponseMessage = $"Successful";
                        }
                        else
                        {
                            result.ResponseCode = "013";
                            result.ResponseMessage = !string.IsNullOrWhiteSpace(response.Message) ? response.Message : $"Unable to parse response message";
                        }
                    }
                    else
                    {
                        result.ResponseCode = "013";
                        result.ResponseMessage =  $"No response";

                    }
                }
            }
            catch (AggregateException ae)
            {
                result.ResponseCode = "096";
                result.ResponseMessage = $"2FA failed: {ae.Message}";
            }
            catch (Exception e)
            {
                result.ResponseCode = "096";
                result.ResponseMessage = $"2FA failed: {e.Message}";
            }
            return result;
        }

        public async Task<AuthenticationResponse> AuthenticateActiveDirectory(AuthenticationRequest request)
        {
            AuthenticationResponse result = new AuthenticationResponse();
            try
            {
                using (ADServiceRef.ServiceSoapClient client = new ADServiceRef.ServiceSoapClient())
                {
                    var response = await client.ADValidateUserAsync(request.Username, request.Password);
                    if (response != null && response.Body != null)
                    {
                        var responseBody = response.Body;
                        string authenticateResponse = responseBody.ADValidateUserResult;
                        bool boolResp = false;
                        if (bool.TryParse(authenticateResponse.Split('|')[0], out boolResp))
                        {
                            result.IsAuthenticated = boolResp;
                            result.ResponseCode = "000";
                            result.ResponseMessage = $"Successful";
                        }
                        else
                        {
                            result.ResponseCode = "013";
                            result.ResponseMessage = $"Unable to parse response message";
                        }
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

        public async Task<AuthenticationResponse> Authenticate2FAAsync(AuthenticationRequest request)
        {
            AuthenticationResponse result = new AuthenticationResponse();
            try
            {
                if (string.IsNullOrWhiteSpace(request.Username))
                {
                    result.ResponseCode = "013";
                    result.ResponseMessage = "Invalid Username";
                    return result;
                }
                if (string.IsNullOrWhiteSpace(request.Token))
                {
                    result.ResponseCode = "013";
                    result.ResponseMessage = "Invalid Token";
                    return result;
                }
                var apiConfig = await new ApiConfigurationSystem().RetrieveByName("2FA");
                postingConfig.IP = apiConfig != null ? apiConfig.UrlPrefix : ConfigurationManager.AppSettings["api"];
                postingConfig.Route = "api/Authenticate2FA";
                postingConfig.Headers = HttpRequestMessageExtensions.GetSOAHeaders("Megatron", "decepticons attack");
                var httpResponse = await HTTPEngine<AuthenticationRequest>.PostMessage(request, postingConfig);
                result = await result.LoadModel(httpResponse) as AuthenticationResponse;
                if (result != null)
                {
                    if (result.IsAuthenticated)
                    {
                        result.IsAuthenticated = result.IsAuthenticated;
                        result.ResponseCode = "000";
                        result.ResponseMessage = $"Successful";
                    }
                    else
                    {
                        result.ResponseCode = "013";
                        var error = await new ErrorMessage().LoadModel(httpResponse);
                        result.ResponseMessage = !string.IsNullOrWhiteSpace(result.ResponseMessage) 
                            ? result.ResponseMessage : error != null ? error.Error : $"Unable to parse response message";
                    }
                }
                else
                {
                    result.ResponseCode = "013";
                    result.ResponseMessage = $"No response";

                }
            }
            catch (Exception e)
            {
                result.ResponseCode = "097";
                result.ResponseMessage = $"2FA Failed for User {request.Username}: {e.Message}";
            }
            return result;
        }
        public async Task<AuthenticationResponse> AuthenticateActiveDirectoryAsync(AuthenticationRequest request)
        {
            AuthenticationResponse result = new AuthenticationResponse();
            try
            {
                if (string.IsNullOrWhiteSpace(request.Username))
                {
                    result.ResponseCode = "013";
                    result.ResponseMessage = "Invalid Username";
                    return result;
                }
                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    result.ResponseCode = "013";
                    result.ResponseMessage = "Invalid Password";
                    return result;
                }
                var apiConfig = await new ApiConfigurationSystem().RetrieveByName("2FA");
                postingConfig.IP = apiConfig != null ? apiConfig.UrlPrefix : ConfigurationManager.AppSettings["api"];
                postingConfig.Route = "api/AuthenticateActiveDirectory";
                postingConfig.Headers = HttpRequestMessageExtensions.GetSOAHeaders("Megatron", "decepticons attack");
                var httpResponse = await HTTPEngine<AuthenticationRequest>.PostMessage(request, postingConfig);
                result = await result.LoadModel(httpResponse) as AuthenticationResponse;
                if (result != null && result.IsAuthenticated)
                {
                    result.ResponseCode = "000";
                    result.ResponseMessage = "Successful";
                }
                else
                {
                    result.ResponseCode = "091";
                    var error = await new ErrorMessage().LoadModel(httpResponse);
                    result.ResponseMessage = error != null ? error.Error : "Undefined";
                }
            }
            catch (Exception e)
            {
                result.ResponseCode = "097";
                result.ResponseMessage = $"AD Authentication Failed for User {request.Username}: {e.Message}";
            }
            return result;
        }
    }
}
