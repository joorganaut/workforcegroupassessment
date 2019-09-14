using AXAMansard.Framework.DAO;
using AXAMansard.Framework.DTO;
using AXAMansard.Framework.Utility;
using FluentNHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WF.DAO
{
    public class UtilityDAO : CoreDAO<IDataObject, long>
    {
        static string MappingAssemblyName = ConfigurationManager.AppSettings["MappingAssembly"];
        public static void ConfigureDB()
        {
            var mappingAssembly = Assembly.Load(MappingAssemblyName);
            NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration();
            var f_cfg = Fluently
                .Configure(cfg)
                .Mappings(m => m.FluentMappings
                .AddFromAssembly(mappingAssembly)).Diagnostics(d=>d.Enable().OutputToFile(@"C:\Logs\fluent.txt"));
            cfg = f_cfg.BuildConfiguration();
            DBSchemaUpdate.GenerateSchema(cfg);
            //DBSchemaUpdate.GenerateSchema(f_cfg);
        }
    }
}
