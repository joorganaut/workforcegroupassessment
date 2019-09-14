using WF.Core.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.Core.Processors
{
    public class BusinessObjectTransformer
    {
    }
    public static class BusinessObjectExtension
    {
        public async static Task<IBusinessObject> Load<T>(this T obj, string json) where T : IBusinessObject
        {
            var task = Task.Factory.StartNew(() => { obj = (T)JsonConvert.DeserializeObject(json, obj.GetType()); });
            Task.WaitAll(new Task[] { task });
            return await Task.FromResult(obj);
        }

        public async static Task<IHTTPObject> LoadModel<T>(this T obj, string json) where T : IHTTPObject
        {
            var task = Task.Factory.StartNew(() => { obj = (T)JsonConvert.DeserializeObject(json, obj.GetType()); });
            Task.WaitAll(new Task[] { task });
            return await Task.FromResult(obj);
        }
        //public async static Task<string> AsJson<T>(this T obj) where T : IHTTPObject
        //{
        //    return await Task.FromResult(JsonConvert.SerializeObject(obj));
        //}
    }
}
