using WF.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Net.Http;

namespace WF.Service.Common
{
    public class UtilitySystem
    {
        public static void ConfigureDB()
        {
            UtilityDAO.ConfigureDB();
        }
    }
    public static class HttpRequestMessageExtensions
    {
        private const string HttpContext = "MS_HttpContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";

        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey(HttpContext))
            {
                dynamic ctx = request.Properties[HttpContext];
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }

            if (request.Properties.ContainsKey(RemoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }

            return null;
        }
        public static KeyValuePair<string, string>[] GetSOAHeaders(string username, string password)
        {
            string credential = $"{username}:{password}";
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(credential);
            var cipher = System.Convert.ToBase64String(plainTextBytes);
            KeyValuePair<string, string>[] headers = new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>("Authorization", $"Basic {cipher}"),
                    };
            return headers;
        }
    }
    public class MessagingFormatter
    {
        private static string CleanStringOfNonDigits(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            string cleaned = new string(s.Where(char.IsDigit).ToArray());
            return cleaned;
        }

        public static string formatPhonenumberToElevenDigit(string PhoneNumber)
        {

            if (PhoneNumber.Length > 11)
            {
                PhoneNumber = CleanStringOfNonDigits(PhoneNumber);
                if (PhoneNumber.StartsWith("234"))
                {
                    // PhoneNumber = PhoneNumber.Replace("234", string.Empty);
                    PhoneNumber = PhoneNumber.Substring(3);
                    if (!PhoneNumber.StartsWith("0"))
                    {
                        PhoneNumber = "0" + PhoneNumber;
                    }

                    if (PhoneNumber.Length > 11)
                    {
                        PhoneNumber = PhoneNumber.Substring(0, 11);
                    }

                }
                else
                {
                    if (!PhoneNumber.StartsWith("0"))
                    {
                        PhoneNumber = "0" + PhoneNumber;
                    }

                    if (PhoneNumber.Length > 11)
                    {
                        PhoneNumber = PhoneNumber.Substring(0, 11);
                    }
                }
            }
            else
            {
                PhoneNumber = CleanStringOfNonDigits(PhoneNumber);
                if (!string.IsNullOrEmpty(PhoneNumber))
                {
                    if (!PhoneNumber.StartsWith("0"))
                    {
                        PhoneNumber = "0" + PhoneNumber;
                    }

                    if (PhoneNumber.Length > 11)
                    {
                        PhoneNumber = PhoneNumber.Substring(0, 11);
                    }
                }
                else
                {
                    PhoneNumber = string.Empty;
                }
            }

            return PhoneNumber;
        }

    }
}
