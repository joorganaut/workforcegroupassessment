using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.Service
{
    public class ServiceLogger
    {
        static string FilePath = ConfigurationManager.AppSettings["FilePath"];
        public static async Task WriteLog(string log, string function = "CardManagement")
        {
            string dateStamp = DateTime.Now.ToString("ddMMyyyy");
            string fileName = $"{FilePath}\\{dateStamp}\\{function}_{dateStamp}.log";
            Directory.CreateDirectory(Path.GetDirectoryName(($"{fileName}")));
            if (!File.Exists(fileName))
            {
                File.AppendAllText(fileName, $"**** FSS Management Service ****{Environment.NewLine}");
            }
            string timeStamp = DateTime.Now.ToString("hh:mm:ss");
            File.AppendAllText(fileName, $"{timeStamp}:\t{log}{Environment.NewLine}");
        }
        static string dateStamp = DateTime.Now.ToString("ddMMyyyy");
        static string filePath = $"{ConfigurationManager.AppSettings["FilePath"]}/{dateStamp}.log";
        static ILogger log = new LoggerConfiguration().WriteTo.File(filePath).CreateLogger();
        public async static Task LogEvent(string function, string msg)
        {
            var task = Task.Factory.StartNew(() => {
                log.Information($"{function} Event: {msg}");
            });
            await Task.WhenAll(new Task[] { task });
        }
        public async static Task LogException(string function, Exception exception)
        {
            var task = Task.Factory.StartNew(() => {
                log.Information(exception, $"{function} Error: ");
            });
            await Task.WhenAll(new Task[] { task });
        }
        public static Exception GetInnerException(Exception ex)
        {
            return (ex.InnerException == null) ? ex : GetInnerException(ex.InnerException);
        }
    }
}
