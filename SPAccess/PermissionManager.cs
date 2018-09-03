using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccess
{
    public class PermissionManager : _SPManager
    {
        internal PermissionManager(SPAccess SPAccess) : base(SPAccess)
        {
        }

        public bool CheckPermissionExists(string Name)
        {
            return DB.PermissionIdentity.Any(p => p.Name == Name);
        }

        public bool CreatePermission(string Name, string Title, string Require = null)
        {
            try
            {
                if (!CheckPermissionExists(Name))
                {
                    var perm = DB.PermissionIdentity.Create();
                    perm.Name = Name;
                    perm.Title = Title;
                    perm.Require = Require;
                    DB.PermissionIdentity.Add(perm);
                    DB.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
#if DEBUG

                throw ex;
#endif
            }
            return false;
        }

        public bool UpdatePermission(string Name, string Title, string Require = null)
        {
            if (!CheckPermissionExists(Name))
            {
                return CreatePermission(Name, Title, Require);
            }
            else
            {
                UpdatePermission(DB.PermissionIdentity.First(p => p.Name == Name).PermissionID, Name, Title, Require);
            }
            return true;
        }

        private bool UpdatePermission(int ID, string Name, string Title, string Require = null)
        {
            try
            {
                var perm = DB.PermissionIdentity.First(p => p.PermissionID == ID);
                perm.Name = Name;
                perm.Title = Title;
                perm.Require = Require;

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

        public ViewModel.Permission GetPermission(int ID)
        {
            try
            {
                return new ViewModel.Permission(SPAccess, DB.PermissionIdentity.First(p => p.PermissionID == ID));
            }
            catch (Exception ex)
            {
#if DEBUG

                throw ex;
#endif
            }

            return null;
        }

        public List<ViewModel.Permission> GetPermissions(int? limit = null, int? page = null)
        {
            try
            {
                var query = DB.PermissionIdentity.AsQueryable();

                if (limit.HasValue)
                {
                    if (!page.HasValue)
                    {
                        page = 1;
                    }
                    page -= 1;

                    query = query.Skip(page.Value * limit.Value).Take(limit.Value);
                }

                return query.ToList().Select(g => new ViewModel.Permission(SPAccess, g)).ToList();
            }
            catch (Exception ex)
            {
#if DEBUG

                throw ex;
#endif
            }
            return new List<ViewModel.Permission>();
        }

        public ViewModel.Permission GetPermission(string Name)
        {
            try
            {
                return new ViewModel.Permission(SPAccess, DB.PermissionIdentity.First(p => p.Name == Name));
            }
            catch (Exception ex)
            {
#if DEBUG

                throw ex;
#endif
            }

            return null;
        }

        public bool AllowGroup(int PermissionID, int GroupID, bool allow = true)
        {
            try
            {
                var group = DB.GroupIdentity.First(g => g.GroupID == GroupID);
                if (allow && !group.PermissionIdentity.Any(p => p.PermissionID == PermissionID))
                {
                    group.PermissionIdentity.Add(DB.PermissionIdentity.First(p => p.PermissionID == PermissionID));
                    DB.SaveChanges();
                }
                if (!allow && group.PermissionIdentity.Any(p => p.PermissionID == PermissionID))
                {
                    group.PermissionIdentity.Remove(DB.PermissionIdentity.First(p => p.PermissionID == PermissionID));
                    DB.SaveChanges();
                }

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

        public bool DenyUser(int PermissionID, int UserID, bool deny = true)
        {
            try
            {
                var user = DB.UserIdentity.First(u => u.UserID == UserID);
                if (deny && !user.PermissionIdentity.Any(p => p.PermissionID == PermissionID))
                {
                    user.PermissionIdentity.Add(DB.PermissionIdentity.First(p => p.PermissionID == PermissionID));
                    DB.SaveChanges();
                }
                if (!deny && user.PermissionIdentity.Any(p => p.PermissionID == PermissionID))
                {
                    user.PermissionIdentity.Remove(DB.PermissionIdentity.First(p => p.PermissionID == PermissionID));
                    DB.SaveChanges();
                }

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
