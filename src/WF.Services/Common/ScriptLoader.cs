using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WF.Service
{
    public class ScriptLoader
    {
        static Dictionary<string, string> QueryHashSet = new Dictionary<string, string>();
        static Dictionary<string, object> ObjectHashSet = new Dictionary<string, object>();
        public static string GetString(string name, Assembly assembly)
        {
            string data = null;
            if (!QueryHashSet.TryGetValue(name, out data))
            {
                string resName = assembly.GetManifestResourceNames().FirstOrDefault(mf => mf.EndsWith(name));
                if (resName != null)
                {
                    using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(resName)))
                    {
                        data = sr.ReadToEnd();
                        sr.Close();
                        QueryHashSet.Add(name, data);
                    }
                }
            }
            return data;
        }
        public static Stream GetStream(string name, Assembly assembly)
        {
            object data = null;
            if (!ObjectHashSet.TryGetValue(name, out data))
            {
                string resName = assembly.GetManifestResourceNames().FirstOrDefault(mf => mf.EndsWith(name));
                if (resName != null)
                {
                    using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(resName)))
                    {
                        data = sr.BaseStream;
                        sr.Close();
                        ObjectHashSet.Add(name, data);
                    }
                }
            }
            return (Stream)data;
        }
        public static string GetString(string name)
        {
            return ScriptLoader.GetString(name, typeof(ScriptLoader).Assembly);
        }
        public static Stream GetStream(string name)
        {
            return ScriptLoader.GetStream(name, typeof(ScriptLoader).Assembly);
        }
    }
}
