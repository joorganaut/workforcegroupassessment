using AXAMansard.Framework.DTO;
using WF.Core.Contract;
using Newtonsoft.Json;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WF.Core
{
    public interface IBusinessObject : IDataObject
    {
        bool IsEnabled { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateLastModified { get; set; }
        string CreatedBy { get; set; }
        string LastModifiedBy { get; set; }
        string Error { get; set; }
        string ToString();
    }
    [PrimaryKey("ID", AutoIncrement = true)]
    public abstract class BusinessObject : IBusinessObject, IBulkTransaction
    {
        [Newtonsoft.Json.JsonIgnore]
        public virtual long ID { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual bool IsEnabled { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual DateTime DateCreated { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual DateTime DateLastModified { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual string Error { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual string CreatedBy { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual string LastModifiedBy { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual string MFBCode { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual string Name { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class Variance
    {
        public string Prop { get; set; }
        public object valA { get; set; }
        public object valB { get; set; }
    }

    public static class BusinessObjectExtension
    {
        public async static Task<List<Variance>> DetailedCompare<T>(this T val1, T val2) where T : IBusinessObject
        {
            List<Variance> variances = new List<Variance>();
            var task = Task.Factory.StartNew(() => {
                try {
                    PropertyInfo[] fi = val1.GetType().GetProperties();
                    foreach (PropertyInfo f in fi)
                    {
                        Variance v = new Variance();
                        v.Prop = f.Name;
                        v.valA = f.GetValue(val1, null);
                        v.valB = f.GetValue(val2, null);
                        try
                        {
                            if (!v.valA.Equals(v.valB))
                                variances.Add(v);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                catch
                {
                }
            });
            return await Task.FromResult(variances);
        }
        public async static Task<T> Clone<T>(this T obj, T s_obj, Expression<Func<T, object>> selector = null) where T : IBusinessObject
        {
            T result = default(T);
            var task = Task.Factory.StartNew(() =>
            {
                AutoMapper.Mapper.Initialize(cfg =>
                {
                    if (selector != null)
                    {
                        cfg.CreateMap<T, T>().ForMember(selector, x => x.Ignore());
                    }
                    else
                    {
                        cfg.CreateMap<T, T>();
                    }
                });
            });
            Task.WaitAll(new Task[] {task });
            return await Task.FromResult(result);
        }
    }
}
