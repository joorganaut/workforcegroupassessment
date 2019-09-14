using WF.Core.Data;
using WF.Service;
using WF.Service.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WF.Api.Handlers
{
    public class LogRequestAndResponseHandler : DelegatingHandler
    {
        //public log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //Get Calling IP
            //var IpAddress = request.GetClientIpAddress();
            // log request body
            var routeData = request.RequestUri.AbsolutePath.ToLower();
            if (!routeData.StartsWith("/currencyexcore/api") && !routeData.Contains("api"))
            {
                return await base.SendAsync(request, cancellationToken);
            }
            if (routeData.Contains("token"))
            {
                return await base.SendAsync(request, cancellationToken);
            }
            DateTime requestDate = DateTime.Now;
            string requestBody = await request.Content.ReadAsStringAsync();
            string responseBody = string.Empty;
            //log.Info("<<<<<<<<<<<< Start Of Request >>>>>>>>>>");
            

            //log.Info(requestBody);

            //log.Info("<<<<<<<<<<<< End Of Request >>>>>>>>>");

            // let other handlers process the request

            var result = await base.SendAsync(request, cancellationToken);

            if (result.Content != null)
            {
                // once response body is ready, log it

                responseBody = await result.Content.ReadAsStringAsync();
                if (String.IsNullOrEmpty(responseBody))
                {
                    responseBody = string.Empty;
                }
                //log.Info("<<<<<<<<<<<< Start Of Response >>>>>>>>>>");

                //log.Info(responseBody);

                //log.Info("<<<<<<<<<<<< End Of Response >>>>>>>>>");


            }

            string ipAddress = string.Empty;
            string method = string.Empty;
            string uri = string.Empty;

            //var rrs = new RequestResponseEntryService();
            var ctx = request.GetOwinContext();

            try
            {
                if (ctx != null)
                {
                    if (ctx.Request != null)
                    {
                        if (ctx.Request.Path.Value != "/")
                        {
                            AuditTrail trail = new AuditTrail()
                            {
                                Action = ctx.Request.Path.Value,
                                IP = ctx.Request.RemoteIpAddress,
                                Request = requestBody,
                                RequestTime = requestDate,
                                Response = responseBody,
                                ResponseTime = DateTime.Now,
                                Name = ctx.Request.User.Identity.Name
                            };
                            await new AuditTrailSystem().LogAuditTrail(trail);
                        }
                        //rrs.Save(requestBody, responseBody, requestDate, ctx.Request.Uri.AbsoluteUri, ctx.Request.Method, ctx.Request.RemoteIpAddress,ctx.Request.User.Identity.Name);
                    }
                    else
                    {
                        //rrs.Save(requestBody, responseBody, requestDate, string.Empty, string.Empty, string.Empty,string.Empty);
                    }

                }
            }
            catch (AggregateException ae)
            {

                AuditTrail trail = new AuditTrail()
                {
                    Action = ctx.Request.Path.Value,
                    IP = ctx.Request.RemoteIpAddress,
                    Request = requestBody,
                    RequestTime = requestDate,
                    Response = responseBody,
                    ResponseTime = DateTime.Now,
                    Name = ctx.Request.User.Identity.Name,
                    Error = ae.Message
                };
                await new AuditTrailSystem().LogAuditTrail(trail);
            }
            catch (Exception badex)
            {
                AuditTrail trail = new AuditTrail()
                {
                    Action = ctx.Request.Path.Value,
                    IP = ctx.Request.RemoteIpAddress,
                    Request = requestBody,
                    RequestTime = requestDate,
                    Response = responseBody,
                    ResponseTime = DateTime.Now,
                    Name = ctx.Request.User.Identity.Name,
                    Error = badex.Message
                };
                await new AuditTrailSystem().LogAuditTrail(trail);
                //rrs.logevent("Memory Exception Occured--------", MethodBase.GetCurrentMethod().Name, "info");
                //rrs.Save(requestBody, responseBody, requestDate, string.Empty, string.Empty, string.Empty,string.Empty);
            }
            return result;
        }
    }
}