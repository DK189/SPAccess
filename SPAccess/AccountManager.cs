using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccess
{
    public class AccountManager : _SPManager
    {
        internal AccountManager(SPAccess SPAccess) : base(SPAccess)
        {
            try
            {
                if (!DB.UserIdentity.Any())
                {
                    int GID;
                    if (DB.GroupIdentity.Any())
                    {
                        var group = DB.GroupIdentity.First();
                        GID = group.GroupID;
                    }
                    else
                    {
                        var group = CreateGroup("Administrators", "[BASE] Administrators");
                        GID = group.GroupID;
                    }
                    var sa = CreateAccount("admin", "SonPhat.net", GID);
                    SPAccess.ConfigurationManager.SAID = sa.UserID;
                }
            }
            catch (Exception ex)
            {
#if DEBUG

                throw ex;
#endif
            }
        }

        public ViewModel.Group GetGroup (int GroupID)
        {
            try
            {
                return new ViewModel.Group(SPAccess, DB.GroupIdentity.First(g => g.GroupID == GroupID));
            }
            catch (Exception ex)
            {
#if DEBUG

                throw ex;
#endif
            }
            return null;
        }

        public List<ViewModel.Group> GetGroups(int? limit = null, int? page = null)
        {
            try
            {
                var query = DB.GroupIdentity.AsQueryable();

                if (limit.HasValue)
                {
                    if (!page.HasValue)
                    {
                        page = 1;
                    }
                    page -= 1;

                    query = query.Skip(page.Value * limit.Value).Take(limit.Value);
                }

                return query.ToList().Select(g => new ViewModel.Group(SPAccess, g)).ToList();
            }
            catch (Exception ex)
            {
#if DEBUG

                throw ex;
#endif
            }
            return new List<ViewModel.Group>();
        }

        public ViewModel.Group CreateGroup(string Name, string Description)
        {
            if (DB.GroupIdentity.Any(g => g.Name == Name))
            {
                throw new ArgumentException($"The group [{Name}] is already exists!");
            }
            var group = DB.GroupIdentity.Create();
            group.Name = Name;
            group.Description = Description;
            group.UpdateTime = DateTime.Now;
            DB.GroupIdentity.Add(group);
            DB.SaveChanges();

            return new ViewModel.Group(SPAccess, group);
        }

        public bool ChangeGroupName(int GroupID, string Name)
        {
            if (!DB.GroupIdentity.Any(g => g.GroupID == GroupID))
            {
                throw new KeyNotFoundException($"Cannot find group by id = {GroupID}!");
            }
            var group = DB.GroupIdentity.First(g => g.GroupID == GroupID);
            group.Name = Name;
            group.UpdateTime = DateTime.Now;
            try
            {
                DB.SaveChanges();
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

        public ViewModel.Account GetAccount(int UserID)
        {
            try
            {
                var user = DB.UserIdentity.First(u => u.UserID == UserID);
                DB.Reload(user);
                return new ViewModel.Account(SPAccess, user);
            }
            catch (Exception ex)
            {
#if DEBUG

                throw ex;
#endif
            }
            return null;
        }

        public List<ViewModel.Account> GetAccounts(int? limit = null, int? page = null)
        {
            try
            {
                DB.Reload(DB.UserIdentity);
                
                var query = DB.UserIdentity.AsQueryable();

                if (limit.HasValue)
                {
                    if (!page.HasValue)
                    {
                        page = 1;
                    }
                    page -= 1;

                    query = query.Skip(page.Value * limit.Value).Take(limit.Value);
                }

                return query.ToList().Select(g => new ViewModel.Account(SPAccess, g)).ToList();
            }
            catch (Exception ex)
            {
#if DEBUG

                throw ex;
#endif
            }
            return new List<ViewModel.Account>();
        }

        public ViewModel.Account CreateAccount(string Username, string Password, int GroupID)
        {
            if (DB.UserIdentity.Any(g => g.Username == Username))
            {
                throw new ArgumentException($"The user [{Username}] is already exists!");
            }
            if (!DB.GroupIdentity.Any(g => g.GroupID == GroupID))
            {
                throw new KeyNotFoundException($"Cannot find group by id = {GroupID}!");
            }
            var user = DB.UserIdentity.Create();
            user.Username = Username;
            user.Password = HashPassword(Password);
            user.GroupIdentity = DB.GroupIdentity.Where(g => g.GroupID == GroupID).ToList();
            user.UpdateTime = DateTime.Now;
            DB.UserIdentity.Add(user);
            DB.SaveChanges();

            return new ViewModel.Account(SPAccess, user);
        }

        public bool ChangeGroupOfAccount(int UserID, int GroupID)
        {
            if (!DB.UserIdentity.Any(g => g.UserID == UserID))
            {
                throw new KeyNotFoundException($"Cannot find user by id = {UserID}!");
            }
            if (!DB.GroupIdentity.Any(g => g.GroupID == GroupID))
            {
                throw new KeyNotFoundException($"Cannot find group by id = {GroupID}!");
            }
            var user = DB.UserIdentity.First(u => u.UserID == UserID);
            user.GroupIdentity = DB.GroupIdentity.Where(g => g.GroupID == GroupID).ToList();
            user.UpdateTime = DateTime.Now;
            try
            {
                DB.SaveChanges();
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

        public bool ChangeAccountPassword(int UserID, string Password)
        {
            if (!DB.UserIdentity.Any(g => g.UserID == UserID))
            {
                throw new KeyNotFoundException($"Cannot find user by id = {UserID}!");
            }
            var user = DB.UserIdentity.First(u => u.UserID == UserID);
            user.Password = HashPassword(Password);
            user.UpdateTime = DateTime.Now;
            try
            {
                DB.SaveChanges();
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

        public UserIdentity CheckUsernameAndPassword(string Username, string Password)
        {
            return DB.UserIdentity.FirstOrDefault(u => u.Username == Username && u.Password == HashPassword(Password));
        }

        protected string HashPassword (string RawPassword)
        {
            string Hash = string.Empty;
            Hash = "***".Replace("***", RawPassword);
            return Hash;
        }
    }
}
