using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccess
{
    public class SPAccess
    {
        public AccountManager AccountManager { get; private set; }
        public SessionManager SessionManager { get; private set; }
        public PermissionManager PermissionManager { get; private set; }
        public ConfigurationManager ConfigurationManager { get; private set; }

        internal SPAccessDB DB;

        /// <summary>
        /// Khởi tạo SPAccess với chuỗi kết nối trong tệp .config
        /// </summary>
        public SPAccess()
        {
            this.DB = new SPAccessDB();
            Constructor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConnectionString">Chuỗi kết nối ở dạng SQLConnectionString hoặc EntityConnectionString</param>
        /// <param name="IsEntityConnectionString">Cho hệ thống biết chuỗi này là chuỗi kết nối cho SQL hay Entity (Default: true)</param>
        public SPAccess(string ConnectionString, bool IsEntityConnectionString = true)
        {
            string RealConnectionString = string.Empty;
            if (IsEntityConnectionString)
            {
                RealConnectionString = ConnectionString;
            }
            else
            {
                RealConnectionString = new EntityConnectionStringBuilder()
                {
                    Name = "SPAccessDB",
                    Provider = "System.Data.SqlClient",
                    ProviderConnectionString = ConnectionString,
                    
                }.ConnectionString;
            }
            this.DB = new SPAccessDB(RealConnectionString);
            Constructor();
        }

        internal SPAccess(SPAccessDB DB)
        {
            this.DB = DB;
            Constructor();
        }

        private void Constructor()
        {
            try
            {
                var CreateIfNotExists = DB.Database.CreateIfNotExists();
            }
            catch (Exception ex)
            {
#if DEBUG

                throw ex;
#endif
            }

            DB.Database.Log = Query_Log;

            DB.Set<UserIdentity>().AsNoTracking();
            DB.Set<GroupIdentity>().AsNoTracking();
            DB.Set<PermissionIdentity>().AsNoTracking();
            DB.Set<Session>().AsNoTracking();

            AccountManager = new AccountManager(this);
            SessionManager = new SessionManager(this);
            PermissionManager = new PermissionManager(this);
            ConfigurationManager = new ConfigurationManager(this);

            ConfigurationManager.StartTime = DateTime.Now;
        }

        public List<string> Queries = new List<string>();

        private void Query_Log(string query)
        {
            Queries.Add(query);
        }

        public ViewModel.Session LoginByUser(string Username, string Password)
        {
            return SessionManager.CreateSessionForUser(AccountManager.CheckUsernameAndPassword(Username, Password));
        }

        public ViewModel.Session LoginByToken(string Token)
        {
            return SessionManager.OpenSessionByToken(Token);
        }

        public bool Logout (ViewModel.Session Session)
        {
            try
            {
                SessionManager.RevokeSession(Session.SessionID);
                return true;
            }
            catch (Exception ex)
            {
#if DEBUG

                throw ex;
#endif
            }
            return false;
        }
    }
}
