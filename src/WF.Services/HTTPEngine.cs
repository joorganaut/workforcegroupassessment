using WF.Core;
using WF.Core.Contract;
using WF.Core.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WF.Service
{
    public class HTTPEngine<T> : IDisposable where T : IHTTPObject
    {
        public async static Task<string> PostMessage(T obj, HTTPConfig config)
        {
            string result = string.Empty;
            try
            {
                var json = await obj.AsJson();
                var content = new StringContent(json, Encoding.UTF8, config.DataType);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(config.IP);
                    client.DefaultRequestHeaders
                        .Accept.Add(new MediaTypeWithQualityHeaderValue(config.DataType));//ACCEPT header
                    if (config.Headers != null && config.Headers.Length > 0)
                    {
                        for (int i = 0; i < config.Headers.Length; i++)
                        {
                            client.DefaultRequestHeaders
                                .Add(config.Headers[i].Key, config.Headers[i].Value);//ACCEPT header
                        }
                    }
                    var response = await client.PostAsync(config.Route, content);
                    if (response != null && response.Content != null)
                    {
                        obj.Error = result = await response.Content.ReadAsStringAsync();
                        //await obj.LoadModel(obj.Error);
                    }
                    else
                    {
                        obj.Error = $"503 - Invalid Response from Service";
                    }
                }
            }
            catch (WebException we)
            {
                obj.Error = $"{((HttpWebResponse)we.Response).StatusCode.ToString()} - {((HttpWebResponse)we.Response).StatusDescription}";
            }
            catch (TimeoutException te)
            {
                obj.Error = "Service Timed Out";
            }
            catch (Exception e)
            {
                obj.Error = e.Message;
            }
            return result;
        }

        public async static Task<string> PostMessage(T obj, HTTPConfig config, bool isXml = true)
        {
            string result = string.Empty;
            try
            {
                var json = await obj.AsXml();
                HttpRequestMessage request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(config.IP + config.Route),
                    Method = HttpMethod.Post
                };
                request.Content = new StringContent(json, Encoding.UTF8, config.DataType);
                //var content = new StringContent(json, Encoding.UTF8, config.DataType);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(config.IP);
                    client.DefaultRequestHeaders
                        .Accept.Add(new MediaTypeWithQualityHeaderValue(config.DataType));//ACCEPT header
                    //request.Content.Headers.ContentType = new MediaTypeHeaderValue(config.DataType);
                    if (config.Headers != null && config.Headers.Length > 0)
                    {
                        for (int i = 0; i < config.Headers.Length; i++)
                        {
                            client.DefaultRequestHeaders
                                .Add(config.Headers[i].Key, config.Headers[i].Value);//ACCEPT header
                        }
                    }
                    var response = client.SendAsync(request).Result;
                    if (response != null && response.Content != null)
                    {
                        obj.Error = result = await response.Content.ReadAsStringAsync();
                        //await obj.LoadModel(obj.Error);
                    }
                    else
                    {
                        obj.Error = $"503 - Invalid Response from Service";
                    }
                }
            }
            catch (AggregateException ae)
            {
                obj.Error = ae.Message;
            }
            catch (WebException we)
            {
                obj.Error = $"{((HttpWebResponse)we.Response).StatusCode.ToString()} - {((HttpWebResponse)we.Response).StatusDescription}";
            }
            catch (TimeoutException te)
            {
                obj.Error = "Service Timed Out";
            }
            catch (Exception e)
            {
                obj.Error = e.Message;
            }
            return result;
        }

        public async static Task<string> PostMessageFromTemplate(T obj, string message, HTTPConfig config, bool isXml = true)
        {
            string result = string.Empty;
            try
            {
                var json = message;
                HttpRequestMessage request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(config.IP + config.Route),
                    Method = HttpMethod.Post
                };
                //request.Content = new StringContent(json.ToString(), Encoding.UTF8);
                var content = new StringContent(json, Encoding.UTF8, config.DataType);
                request.Content = content;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(config.IP);
                    client.DefaultRequestHeaders
                        .Accept.Add(new MediaTypeWithQualityHeaderValue(config.DataType));//ACCEPT header
                    //request.Content.Headers.ContentType = new MediaTypeHeaderValue(config.DataType);
                    if (config.Headers != null && config.Headers.Length > 0)
                    {
                        for (int i = 0; i < config.Headers.Length; i++)
                        {
                            client.DefaultRequestHeaders
                                .Add(config.Headers[i].Key, config.Headers[i].Value);//ACCEPT header
                        }
                    }
                    var response = client.SendAsync(request).Result;
                    if (response != null && response.Content != null)
                    {
                        obj.Error = result = await response.Content.ReadAsStringAsync();
                        //await obj.LoadModel(obj.Error);
                    }
                    else
                    {
                        obj.Error = $"503 - Invalid Response from Service";
                    }
                }
            }
            catch (AggregateException ae)
            {
                obj.Error = ae.Message;
            }
            catch (WebException we)
            {
                obj.Error = $"{((HttpWebResponse)we.Response).StatusCode.ToString()} - {((HttpWebResponse)we.Response).StatusDescription}";
            }
            catch (TimeoutException te)
            {
                obj.Error = "Service Timed Out";
            }
            catch (Exception e)
            {
                obj.Error = e.Message;
            }
            return result;
        }

        public void Dispose()
        {
        }
    }

    public class HTTPSOAPEngine<T> : IDisposable where T : IHTTPObject
    {
        public async static Task<string> PostMessage(T obj, HTTPConfig config)
        {
            string result = string.Empty;
            try
            {
                var json = await obj.AsXml();
                var content = new StringContent(json, Encoding.UTF8, config.DataType);
                using (var client = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(config.IP + config.Route),
                        Method = HttpMethod.Post
                    };
                    request.Content = new StringContent(json.ToString(), Encoding.UTF8, "text/xml");

                    request.Headers.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
                    request.Headers.Add("SOAPAction", config.SOAPAction);

                    HttpResponseMessage response = client.SendAsync(request).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }

                    obj.Error = result = await response.Content.ReadAsStringAsync();
                    var soapResponse = XDocument.Parse(result);
                }
            }
            catch (WebException we)
            {
                obj.Error = $"{((HttpWebResponse)we.Response).StatusCode.ToString()} - {((HttpWebResponse)we.Response).StatusDescription}";
            }
            catch (TimeoutException te)
            {
                obj.Error = "Service Timed Out";
            }
            catch (Exception e)
            {
                obj.Error = e.Message;
            }
            return result;
        }

        public void Dispose()
        {
        }
    }
}
