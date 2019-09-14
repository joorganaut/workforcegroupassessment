using WF.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using System.Reflection;

namespace WF.Service
{
    public class ApiConfigurationSystem
    {
        public static string Error;
        StandardKernel kernel = new StandardKernel(new NinjectSettings() { LoadExtensions = false });
        public ApiConfigurationSystem()
        {
            kernel.Load(Assembly.GetExecutingAssembly());
        }
        public async Task<bool> SaveConfigurationAsync(ApiConfiguration user)
        {
            bool result = false;
            Error = string.Empty;
            user.DateCreated = DateTime.Now;
            user.DateLastModified = DateTime.Now;
            var Implementation = kernel.Get<IDIDAOImplementation<ApiConfiguration>>();
            Implementation._obj = user;
            var response = await Implementation.ExecuteAsync("Save");
            if (bool.TryParse(response.ToString(), out result))
            {
                Error = Implementation._errMsg;
            }
            return result;
        }

        public async Task<bool> UpdateApiConfigurationAsync(ApiConfiguration user)
        {
            bool result = false;
            Error = string.Empty;
            user.DateLastModified = DateTime.Now;
            var Implementation = kernel.Get<IDIDAOImplementation<ApiConfiguration>>();
            Implementation._obj = user;
            var response = await Implementation.ExecuteAsync("Update");
            if (bool.TryParse(response.ToString(), out result))
            {
                Error = Implementation._errMsg;
            }
            return result;
        }
        public async Task<ApiConfiguration> RetrieveByName(string name)
        {
            KeyValuePair<string, object>[] kvp = new KeyValuePair<string, object>[] { new KeyValuePair<string, object>("Name", name) };
            var Implementation = kernel.Get<IDIDAOImplementation<ApiConfiguration>>();
            Implementation._hasParameters = true;
            Implementation._parameters = kvp;
            var response = await Implementation.ExecuteAsync("Retrieve");
            return response as ApiConfiguration;
        }
    }
}
