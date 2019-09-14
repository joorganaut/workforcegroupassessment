using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace WF.Core.Contract
{
    public interface IHTTPObject
    {
        string Error { get; set; }
        
    }
    public static class HTTPObjectTransformer
    {
        public async static Task<T> LoadJson<T>(this T obj, string json) where T : IHTTPObject
        {
            var task = Task.Factory.StartNew(() => { obj = (T)JsonConvert.DeserializeObject(json, obj.GetType()); });
            Task.WaitAll(new Task[] { task });
            return await Task.FromResult(obj);
        }

        public async static Task<T> LoadJsonModel<T>(this T obj, string json) where T : IHTTPObject
        {
            var task = Task.Factory.StartNew(() => { obj = (T)JsonConvert.DeserializeObject(json, obj.GetType()); });
            Task.WaitAll(new Task[] { task });
            return await Task.FromResult(obj);
        }
        public async static Task<string> AsJson<T>(this T obj) where T : IHTTPObject
        {
            return await Task.FromResult(JsonConvert.SerializeObject(obj));
        }

        public async static Task<T> LoadXml<T>(this T obj, string xml) where T : IHTTPObject
        {
            var task = Task.Factory.StartNew(() => {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                using (var reader = new StringReader(xml))
                {
                    reader.ReadToEnd();
                    obj = (T)serializer.Deserialize(reader);
                    reader.Close();
                }
            });
            Task.WaitAll(new Task[] { task });
            return await Task.FromResult(obj);
        }
        public async static Task<T> LoadXmlFromStream<T>(this T obj, Stream xml) where T : IHTTPObject
        {
            var task = Task.Factory.StartNew(() => {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                using (var reader = new StreamReader(xml))
                {
                    reader.ReadToEnd();
                    obj = (T)serializer.Deserialize(reader);
                    reader.Close();
                }
            });
            Task.WaitAll(new Task[] { task });
            return await Task.FromResult(obj);
        }

        public async static Task<T> LoadXmlModel<T>(this T obj, string xml) where T : IHTTPObject
        {
            var task = Task.Factory.StartNew(() => {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());

                using (var reader = new StringReader(xml))
                {
                    reader.ReadToEnd();
                    obj = (T)serializer.Deserialize(reader);
                    reader.Close();
                }
            });
            Task.WaitAll(new Task[] { task });
            return await Task.FromResult(obj);
        }
        public async static Task<string> AsXml<T>(this T obj) where T : IHTTPObject
        {
            //return await Task.Factory.StartNew(() =>
            //{
            if (obj == null)
            {
                return string.Empty;
            }
            try
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                var stringWriter = new StringWriter();
                using (var writer = XmlWriter.Create(stringWriter))
                {
                    xmlserializer.Serialize(writer, obj);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
            //});
            //JsonConvert.SerializeObject(obj));
        }
    }
}
