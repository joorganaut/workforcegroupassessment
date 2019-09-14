using WF.Core;
using WF.Core.Contract;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace WF.DAO.Common
{
    public class NPCoreDAO<T, idT> where T : class
    {
        IDatabase database;
        public NPCoreDAO(IDatabase database)
        {
            this.database = database;
        }
        public NPCoreDAO(string connection)
        {
            this.database = NPDatabaseManager.GetDatabaseByConnectionName(connection);
        }

        public async Task<T> SaveAsync(T obj)
        {
            try
            {
                await database.InsertAsync<T>(obj);                
            }
            catch (Exception e)
            { }
            return obj;
        }
        public async Task SaveAsync(T obj, bool nonStatic)
        {
            await database.InsertAsync<T>(obj);
        }
        public void Save(T obj, bool nonStatic = true)
        {
            database.InsertAsync<T>(obj);
            //return obj;
        }

        public async Task<T> Update(T obj, params string[] args)
        {
            foreach (string s in args)
            {
                await database.UpdateAsync<T>(obj, CreateGetterExpression(s));
            }
            return obj;
        }
        public async Task UpdateAsync(T obj, bool nonStatic, params string[] args)
        {
            foreach (string s in args)
            {
                await database.UpdateAsync<T>(obj, CreateGetterExpression(s));
            }
        }
        public async Task UpdateAsync(T obj, List<string> columns)
        {
            //var task = Task.Factory.StartNew(() =>
            //{
                await database.UpdateAsync(obj, columns);
            //});
            //Task.WaitAll(new Task[] { task});
        }
        public async Task<T> UpdateAsyncWithGetter(T obj, params string[] args)
        {
            foreach (string s in args)
            {
                await database.UpdateAsync<T>(obj, CreateGetterExpression(s));
            }
            return obj;
        }
       
        public async Task UpdateAsync(T obj, params string[] args)
        {
            await database.UpdateAsync(obj, args);
        }
        public async Task<T> Retrieve(idT id)
        {
            return await database.SingleByIdAsync<T>(id);
        }

        public async Task<T> Retrieve(long id)
        {
            return await database.SingleByIdAsync<T>(id);
        }
        public async Task<List<T>> RetrieveAll()
        {
            return await database.FetchAsync<T>();
        }
        public void Commit()
        {
            database.CompleteTransaction();
        }
        public async Task<T> RetrieveDataObjectByParameterAsync(string query, params object[] args)
        {
            Sql sql = new Sql(query, args);
            return await database.SingleAsync<T>(sql);
        }
        public async Task<List<T>> RetrieveDataObjectsByParametersAsync(string query, params object[] args)
        {
            Sql sql = new Sql(query, args);
            return await database.FetchAsync<T>(sql);
        }
        public async Task<List<T>> RetrieveDataObjectsByParametersAsync(Expression<Func<T, bool>> selector = null)
        {
            return await database.Query<T>().Where(selector).ToListAsync();
        }
        public async Task<T> RetrieveDataObjectByParametersAsync(Expression<Func<T, bool>> selector = null)
        {
            return await database.Query<T>().Where(selector).SingleOrDefaultAsync();
        }
        public async Task<Page<T>> RetrievePagedDataObjectsByParametersAsync(Expression<Func<T, bool>> selector = null, int batchSize = 500)
        {
            return await database.Query<T>().Where(selector).ToPageAsync(1, batchSize);
        }
        public async Task<Page<T>> RetrievePagedDataObjectsByParametersAsync(int page = 1, int batchSize = 500, Expression<Func<T, bool>> selector = null)
        {
            return await database.Query<T>().Where(selector).ToPageAsync(page, batchSize);
        }

        public void InsertList(List<T> items, string constringName)
        {
            this.database = NPDatabaseManager.GetDatabaseByConnectionName(constringName);
            var task = Task.Factory.StartNew(() => { database.InsertBulk<T>(items); });
            task.Wait();
        }
        public async Task<List<T>> RetrieveDataObjectsByParametersAsync(string query, string sort, string direction, int page, int pageSize = 10, params object[] args)
        {
            Sql sql = new Sql(query);
            return await database.FetchAsync<T>(page, pageSize, sql);
        }
        public async Task<Page<T>> RetrievePagedDataObjectsByParametersAsync(string constringName, int page, int pageSize = 10, Expression<Func<T, bool>> selector = null)
        {
            this.database = NPDatabaseManager.GetDatabaseByConnectionName(constringName);
            return await database.Query<T>().Where(selector).ToPageAsync(page, pageSize);
        }
        public async Task<IPagedList<T>> RetrieveIPagedDataObjectsByParametersAsync(string query, int page, int pageSize)
        {
            return await Task.FromResult(database.Query<T>(query).ToPagedList(page <= 0 ? 1 : page, pageSize <= 0 ? 10 : pageSize));
        }
        public async Task<Page<T>> RetrieveIPageDataObjectsByParametersAsync(string query, int page, int pageSize)
        {
            var rez = await database.PageAsync<T>(page <= 0 ? 1 : page, pageSize <= 0 ? 10 : pageSize, query);            
            return rez;
            //return await Task.FromResult(database.Query<T>(query).ToPagedList(page <= 0 ? 1 : page, pageSize <= 0 ? 10 : pageSize));
        }
        public async Task<List<T>> RetrievePagedDataObjectsByParametersAsync(string constringName, string query, int page, int pageSize = 10, Expression<Func<T, bool>> selector = null)
        {
            this.database = NPDatabaseManager.GetDatabaseByConnectionName(constringName);
            Sql sql = new Sql(query);
            return await database.FetchAsync<T>(page, pageSize, sql);
        }
        private Expression<Func<T, object>> CreateGetterExpression(string propertyName)
        {
            var getter = typeof(T).GetProperty(propertyName).GetGetMethod(); //propertyInfo.GetGetMethod();

            Expression<Func<T, object>> expression = m => getter.Invoke(m, new object[] { });
            return expression;
        }
    }
    public class NPDatabaseManager
    {
        static Dictionary<string, IDatabase> DataBaseHashSet = new Dictionary<string, IDatabase>();
        public static IDatabase GetDatabaseByConnectionName(string connectionName)
        {
            IDatabase result = null;
            if (!DataBaseHashSet.TryGetValue(connectionName, out result))
            {
                try
                {
                    //var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
                    //OracleConnection sql = new OracleConnection(connectionString);
                    //SqlConnection sql = new SqlConnection(connectionString.ConnectionString);
                    result = new Database(connectionName);
                    DataBaseHashSet.Add(connectionName, result);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            return result;
        }
    }
}
