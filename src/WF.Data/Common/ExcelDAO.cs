using WF.Core.Contract;
using CsvHelper;
using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF.DAO.Common
{
    public class ExcelDAO<T> where T : IExcelObject
    {
        public async static Task<List<T>> RetrieveObjectsFromCSV(string filePath)
        {
            List<T> result = new List<T>();
            var task = Task.Factory.StartNew(() =>
            {
                using (var data = new StreamReader(filePath))
                {
                    using (var csv = new CsvReader(data))
                    {
                        csv.Configuration.HeaderValidated = null;
                        csv.Configuration.MissingFieldFound = null;
                        result = csv.GetRecords<T>().ToList();                        
                    }
                }
            });
            Task.WaitAll(new Task[] { task });
            return await Task.FromResult(result);
        }
        public static async Task<DataTable> ConvertListToDataTable(List<T> transactions)
        {
            DataTable table = new DataTable();
            try
            {
                var task = Task.Factory.StartNew(() =>
                {
                    using (var reader = ObjectReader.Create(transactions))
                    {
                        table.Load(reader);
                    }
                });
                Task.WaitAll(new Task[] { task });
            }
            catch (Exception e)
            {
                //Error = e.Message;
            }
            return await Task.FromResult(table);
        }
    }   
}
