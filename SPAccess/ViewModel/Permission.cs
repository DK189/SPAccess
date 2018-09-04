using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccess.ViewModel
{
    public class Permission : SPModel
    {
        private PermissionIdentity permissionIdentity;

        internal Permission(SPAccess SPAccess, PermissionIdentity perm)
            : base(SPAccess)
        {
            permissionIdentity = perm;
        }

        public int PermissionID
        {
            get
            {
                return permissionIdentity.PermissionID;
            }
        }

        public string Name
        {
            get
            {
                return permissionIdentity.Name;
            }
        }
        public string Title
        {
            get
            {
                return permissionIdentity.Title;
            }
        }
        public string Require
        {
            get
            {
                return permissionIdentity.Require;
            }
        }

        internal override string GetMeta(string Name)
        {
            if (SPAccess.DB.PermissionInformation.Any(inf => inf.PermissionID == permissionIdentity.PermissionID && inf.Name == Name))
            {
                return SPAccess.DB.PermissionInformation.Where(inf => inf.PermissionID == permissionIdentity.PermissionID && inf.Name == Name).Select(infor => infor.Value).FirstOrDefault();
            }
            return null;
        }

        internal override void SetMeta(string Name, string rawValue)
        {
            if (SPAccess.DB.PermissionInformation.Any(inf => inf.PermissionID == permissionIdentity.PermissionID && inf.Name == Name))
            {
                var infor = SPAccess.DB.PermissionInformation.First(inf => inf.PermissionID == permissionIdentity.PermissionID && inf.Name == Name);
                infor.Value = rawValue;
                infor.UpdateTime = DateTime.Now;

                SPAccess.DB.SaveChanges();
            }
            else
            {
                var infor = SPAccess.DB.PermissionInformation.Create();
                infor.PermissionID = permissionIdentity.PermissionID;
                infor.Name = Name;
                infor.Value = rawValue;
                infor.UpdateTime = DateTime.Now;

                SPAccess.DB.PermissionInformation.Add(infor);
                SPAccess.DB.SaveChanges();
            }
        }
    }
}
