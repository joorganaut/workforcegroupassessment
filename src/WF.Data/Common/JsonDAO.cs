using WF.Core.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.DAO.Common
{
    public class JsonDAO<T> where T : IJsonObject
    {
        public async static Task<List<T>> RetrieveAll(string dataSourceName)
        {
            var jsonString = ScriptLoader.GetString(dataSourceName);
            return await Task.FromResult(JsonConvert.DeserializeObject<List<T>>(jsonString));
        }
    }
}
