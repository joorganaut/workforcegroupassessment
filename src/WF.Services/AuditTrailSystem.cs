using WF.Core.Contract;
using WF.Core.Data;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WF.Service
{
    public class AuditTrailSystem
    {
        public static string Error;
        StandardKernel kernel = new StandardKernel(new NinjectSettings() { LoadExtensions = false });
        public AuditTrailSystem()
        {
            kernel.Load(Assembly.GetExecutingAssembly());
        }
        public async Task<bool> SaveAuditTrailAsync(AuditTrail user)
        {
            bool result = false;
            Error = string.Empty;
            user.DateCreated = DateTime.Now;
            user.DateLastModified = DateTime.Now;
            var Implementation = kernel.Get<IDIDAOImplementation<AuditTrail>>();
            Implementation._obj = user;
            var response = await Implementation.ExecuteAsync("Save");
            if (bool.TryParse(response.ToString(), out result))
            {
                Error = Implementation._errMsg;
            }
            return result;
        }

        public async Task<bool> UpdateAuditTrailAsync(AuditTrail user)
        {
            bool result = false;
            Error = string.Empty;
            user.DateLastModified = DateTime.Now;
            var Implementation = kernel.Get<IDIDAOImplementation<AuditTrail>>();
            Implementation._obj = user;
            var response = await Implementation.ExecuteAsync("Update");
            if (bool.TryParse(response.ToString(), out result))
            {
                Error = Implementation._errMsg;
            }
            return result;
        }
        public async Task<AuditTrail> RetrieveByAction(string action)
        {
            KeyValuePair<string, object>[] kvp = new KeyValuePair<string, object>[] { new KeyValuePair<string, object>("Action", action) };
            var Implementation = kernel.Get<IDIDAOImplementation<AuditTrail>>();
            Implementation._hasParameters = true;
            Implementation._parameters = kvp;
            var response = await Implementation.ExecuteAsync("Retrieve");
            return response as AuditTrail;
        }

        public async Task<AuditTrail> LogRequestAuditTrail(string action, string request, string response, DateTime requestTime, DateTime responseTime)
        {
            AuditTrail trail = new AuditTrail();
            trail.Action = action;
            trail.DateCreated = DateTime.Now;
            trail.DateLastModified = DateTime.Now;
            trail.RequestTime = requestTime;
            trail.ResponseTime = responseTime;
            trail.Request = request;
            trail.Response = response;
            return trail;
        }
        public async Task<AuditTrail> LogAuditTrail(AuditTrail trail)
        {
            trail.DateCreated = DateTime.Now;
            trail.DateLastModified = DateTime.Now;
            if (await SaveAuditTrailAsync(trail))
            {
                trail.Error = $"Successfully Saved for User: {trail.Name}";
            }
            else
            {
                trail.Error = $"Failed to Save Trail for User: {trail.Name}";
            }
            return trail;
        }
    }
}
