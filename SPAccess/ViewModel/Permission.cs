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
    }
}
