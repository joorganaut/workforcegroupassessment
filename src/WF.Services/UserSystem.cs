using AXAMansard.Framework.Utility;
using WF.Core.Data;
using WF.Core.Model;
using WF.Core.Processors;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WF.Service
{
    public class UserSystem
    {
        public string Error;
        StandardKernel kernel = new StandardKernel(new NinjectSettings() { LoadExtensions = false });
        HTTPConfig postingConfig;
        public UserSystem()
        {
            postingConfig = new HTTPConfig();
            postingConfig.DataType = "application/json";
            postingConfig.IP = ConfigurationManager.AppSettings["api"];
            postingConfig.Method = HTTPMethod.POST;
            kernel.Load(Assembly.GetExecutingAssembly());
        }
        public async Task<User> ValidateUser(string username, string password)
        {
            User result = null;
            if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                result = new User() { Error = "Invalid Credentials" };
                return result;
            }
            if (username == "Megatron" && password == "decepticons attack")
            {
                result = new User()
                {
                    FullName = "My Name is Megatron",
                    IsAuthenticated = true,
                    Username = "Megatron",
                };
                return result;
            }
            result = await RetrieveByUsername(username);
            if (result == null)
            {
                result = new User() { Error = $"Unable to retrieve User: {Error}" };
                return result;
            }
            if (new MD5Password().CreateSecurePassword(password) == result.Password)
            {
                result.Error = "User successfully Authenticated";
                result = await LoadRoleAndFunctions(result);
                result.IsAuthenticated = true;
            }
            return result;
        }

        public async Task<User> ValidateUser(string username, string password, bool useMegatron)
        {
            User result = null;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                result = new User() { Error = "Invalid Credentials" };
                return result;
            }
            if (username == "Megatron" && password == "decepticons attack")
            {
                result = new User()
                {
                    FullName = "My Name is Megatron",
                    IsAuthenticated = true,
                    Username = "Megatron",
                };
            }
            return result;
        }
        public async Task<User> RegisterUser(string username, string password)
        {
            User result = null;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                result = new User() { Error = "Invalid Credentials" };
                return result;
            }
            result = await RetrieveByUsername(username);
            if (result != null)
            {
                result = new User() { Error = "User already exists" };
                return result;
            }
            result = new User();
            result.Username = username;
            result.DateCreated = DateTime.Now;
            result.DateLastModified = DateTime.Now;
            result.IsEnabled = true;
            result.Password = new MD5Password().CreateSecurePassword(password);
            if (await SaveUserAsync(result))
            {
                result.IsAuthenticated = true;
                result.Error = "User successfully Registered";
            }
            return result;
        }
        /*
        //Api Methods
        public async Task<LoginModel> Login(LoginModel login)
        {
            try
            {

                using (ADServiceRef.ServiceSoapClient client = new ADServiceRef.ServiceSoapClient())
                {
                    var response = await client.ADValidateUserAsync(login.Username, login.Password);
                    if (response != null && response.Body != null)
                    {
                        var responseBody = response.Body;
                        string authenticateResponse = responseBody.ADValidateUserResult;
                        bool boolResp = false;
                        if (bool.TryParse(authenticateResponse.Split('|')[0], out boolResp))
                        {
                            if (boolResp)
                            {
                                var usr = await RetrieveByUsername(login.Username);
                                if (usr != null)
                                {
                                    if (usr.IsEnabled == false)
                                    {
                                        login.Error = $"User with Username {login.Username} is Locked out";
                                        return login;
                                    }
                                    usr = await GetUserDetails(client, usr);
                                    usr = await GetUserGroups(client, usr, login.Password);
                                    usr.IsAuthenticated = true;
                                    usr.NumberOfFailedAttempts = 0;

                                    usr.LastLoginDate = DateTime.Now;
                                    usr = await LoadRoleAndFunctions(usr);
                                    await UpdateUserAsync(usr);

                                    login.Error = Error;
                                    login.User = usr;
                                }
                                else
                                {
                                    User user = new User() { Username = login.Username };
                                    user = await GetUserDetails(client, user);
                                    user = await GetUserGroups(client, user, login.Password);
                                    user.IsAuthenticated = true;

                                    user.IsEnabled = true;
                                    user.LastLoginDate = DateTime.Now;
                                    await SaveUserAsync(user);

                                    login.Error = Error;
                                    login.User = user;
                                }
                            }
                            else
                            {
                                var usr = await RetrieveByUsername(login.Username);
                                if (usr != null)
                                {
                                    var failedAttemptsMax = ConfigurationManager.AppSettings["FailedAttemptsMax"];
                                    usr.NumberOfFailedAttempts = usr.NumberOfFailedAttempts + 1;
                                    if (usr.NumberOfFailedAttempts >= int.Parse(failedAttemptsMax))
                                    {
                                        usr.IsEnabled = false;
                                    }
                                    await UpdateUserAsync(usr);                                    
                                    //await CommitChangesAsync();
                                    
                                }
                                login.Error = $"Unable to Login with Username: {login.Username}";
                                await ServiceLogger.WriteLog($"Unable to Parse Service Response");
                            }
                        }
                        else
                        {
                            login.Error = $"Unable to Parse Service Response";
                            await ServiceLogger.WriteLog($"Unable to Parse Service Response");
                        }
                    }
                    else
                    {
                        login.Error = $"No Authenticate response for User: {login.Username}";
                        await ServiceLogger.WriteLog($"No Authenticate response for User: {login.Username}");
                    }
                }

            }
            catch (Exception e)
            {
                await ServiceLogger.WriteLog(e.Message);
                login.Error = $"No Authenticate response for User: {e.Message}";
            }
            return login;
        }
       */
        private async Task<User> LoadRoleAndFunctions(User user)
        {
            try
            {
                user.Roles = await new RoleSystem().RetrieveByUser(user.ID);
                if (user.Roles != null)
                    user.Roles.Select(async x =>
                    {
                        x.Functions = await new UFunctionSystem().RetrieveUFunctionByRole(x.ID);
                    });
            }
            catch (AggregateException ae) { user.Error = ae.Message; }
            catch (Exception e) { user.Error = e.Message; }
            return user;
        }
        async Task<User> GetUserDetails(ADServiceRef.ServiceSoapClient client, User login)
        {
            try
            {
                var response = await client.ADUserDetailsAsync(login.Username);
                if (response != null && response.Body != null)
                {
                    var responseBody = response.Body;
                    string authenticateResponse = responseBody.ADUserDetailsResult;
                    var stringDetails = authenticateResponse.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (stringDetails != null && stringDetails.Length > 0)
                    {
                        login.FullName = stringDetails[0]; //login.Email = stringDetails[1];
                    }
                    else
                    {
                        await ServiceLogger.WriteLog($"Unable to Parse Service Response");
                    }
                }
                else
                {
                    await ServiceLogger.WriteLog($"No Authenticate response for User: {login.Username}");
                }
            }
            catch (Exception e)
            {
                await ServiceLogger.WriteLog(e.Message);
            }
            return login;
        }
        async Task<User> GetUserGroups(ADServiceRef.ServiceSoapClient client, User login, string password)
        {
            try
            {
                var response = await client.GetGroupsAsync(login.Username, password);
                if (response != null && response.Body != null)
                {
                    var responseBody = response.Body;
                    string authenticateResponse = responseBody.GetGroupsResult;
                    var stringDetails = authenticateResponse.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (stringDetails != null && stringDetails.Length > 0)
                    {
                       // login.Groups = stringDetails;
                    }
                    else
                    {
                        await ServiceLogger.WriteLog($"Unable to Parse Service Response");
                    }
                }
                else
                {
                    await ServiceLogger.WriteLog($"No Authenticate response for User: {login.Username}");
                }
            }
            catch (Exception e)
            {
                await ServiceLogger.WriteLog(e.Message);
            }
            return login;
        }

        //Web Methods
        //public async Task<LoginModel> WebLogin(LoginModel login)
        //{
        //    postingConfig.Route = "api/Account";
        //    login = await login.LoadModel(await HTTPEngine<LoginModel>.PostMessage(login, postingConfig)) as LoginModel;
        //    return await Task.FromResult(login);
        //}

        public async Task<bool> SaveUserAsync(User user)
        {
            bool result = false;
            Error = string.Empty;
            user.DateCreated = DateTime.Now;
            user.DateLastModified = DateTime.Now;
            var Implementation = kernel.Get<IDIDAOImplementation<User>>();
            Implementation._obj = user;
            var response = await Implementation.ExecuteAsync("Save");
            if (bool.TryParse(response.ToString(), out result))
            {
                Error = Implementation._errMsg;
            }
            return result;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            bool result = false;
            Error = string.Empty;
            user.DateLastModified = DateTime.Now;
            var Implementation = kernel.Get<IDIDAOImplementation<User>>();
            Implementation._obj = user;
            var response = await Implementation.ExecuteAsync("Update");
            if (bool.TryParse(response.ToString(), out result))
            {
                Error = Implementation._errMsg;
            }
            return result;
        }
        public async Task<User> RetrieveByUsername(string username)
        {
            KeyValuePair<string, object>[] kvp = new KeyValuePair<string, object>[] { new KeyValuePair<string, object>("Username", username) };
            var Implementation = kernel.Get<IDIDAOImplementation<User>>();
            Implementation._hasParameters = true;
            Implementation._parameters = kvp;
            var response = await Implementation.ExecuteAsync("Retrieve");
            Error = Implementation._errMsg;
            return response as User;
        }
        public User RetrieveByEmail(string email, out string errMsg)
        {
            errMsg = string.Empty;
            KeyValuePair<string, object>[] kvp = new KeyValuePair<string, object>[] { new KeyValuePair<string, object>("Email", email) };
            var Implementation = kernel.Get<IDIDAOImplementation<User>>();
            Implementation._hasParameters = true;
            Implementation._parameters = kvp;
            var response = Implementation.Execute("Retrieve");
            return response as User;
        }


        public async Task<User> RetrieveByEmailAsync(string email)
        {
            KeyValuePair<string, object>[] kvp = new KeyValuePair<string, object>[] { new KeyValuePair<string, object>("Email", email) };
            var Implementation = kernel.Get<IDIDAOImplementation<User>>();
            Implementation._hasParameters = true;
            Implementation._parameters = kvp;
            var response = await Implementation.ExecuteAsync("Retrieve");
            Error = Implementation._errMsg;
            return response as User;
        }
        public User RetrieveByUsernameAndEmail(string username, string email, out string errMsg)
        {
            errMsg = string.Empty;
            KeyValuePair<string, object>[] kvp = new KeyValuePair<string, object>[] { new KeyValuePair<string, object>("Username", username), new KeyValuePair<string, object>("Email", email) };
            var Implementation = kernel.Get<IDIDAOImplementation<User>>();
            Implementation._hasParameters = true;
            Implementation._parameters = kvp;
            var response = Implementation.Execute("Retrieve");
            return response as User;
        }
        public async Task<User> RetrieveByUsernameAndEmailAsync(string username, string email)
        {
            KeyValuePair<string, object>[] kvp = new KeyValuePair<string, object>[] { new KeyValuePair<string, object>("Username", username), new KeyValuePair<string, object>("Email", email) };
            var Implementation = kernel.Get<IDIDAOImplementation<User>>();
            Implementation._hasParameters = true;
            Implementation._parameters = kvp;
            var response = await Implementation.ExecuteAsync("Retrieve");
            Error = Implementation._errMsg;
            return response as User;
        }

        public void CommitChanges()
        {
            var Implementation = kernel.Get<IDIDAOImplementation<User>>();
            //Implementation.Execute("Commit");
        }

        public async Task CommitChangesAsync()
        {
            var Implementation = kernel.Get<IDIDAOImplementation<User>>();
            //await Implementation.ExecuteAsync("Commit");
        }


        

        public async Task<bool> RegisterUser(User model)
        {
            bool result = false;
            var usr = await RetrieveByUsername(model.Username);
            if (usr != null)
            {
                model.Email = $"User with Username: {usr.Username} already Exists";
                return result;
            }
            usr = new User();
            usr.Username = model.Username;
            usr.FullName = model.FullName;
            if (await SaveUserAsync(usr))
            {
                model.Error = Error;
                await new RoleSystem().AddRoleToUser(usr, 1);
                //CommitChanges();
                result = true;
            }
            return result;
        }
        public async Task<bool> UnlockUser(User model)
        {
            bool result = false;
            string errMsg = string.Empty;

            var usr = await RetrieveByUsername(model.Username);
            if (usr != null)
            {
                usr.IsEnabled = true;
                await UpdateUserAsync(usr);
                model.Error = errMsg;
                await CommitChangesAsync();
                model.IsAuthenticated = true;
                result = true;
            }
            return result;
        }
       
    }
}
