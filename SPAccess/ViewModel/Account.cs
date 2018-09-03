using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccess.ViewModel
{
    public class Account : SPModel
    {
        internal UserIdentity userIdentity;

        internal Group group;

        internal Account(SPAccess SPAccess, global::SPAccess.UserIdentity userIdentity)
            : base(SPAccess)
        {
            this.userIdentity = userIdentity;

            group = new Group(this.SPAccess, userIdentity.GroupIdentity.First());
        }

        public int UserID
        {
            get
            {
                return userIdentity.UserID;
            }
        }

        public bool IsSuperAdmin
        {
            get
            {
                return UserID == SPAccess.ConfigurationManager.SAID;
            }
        }

        public string Username
        {
            get
            {
                return userIdentity.Username;
            }
        }

        public IList<ViewModel.Permission> PermissionsDenied
        {
            get
            {
                return userIdentity.PermissionIdentity.Select(p => new ViewModel.Permission(this.SPAccess, p)).ToList().AsReadOnly();
            }
        }

        public bool Can(string PermissionName)
        {
            return IsSuperAdmin || (group.Can(PermissionName) && !userIdentity.PermissionIdentity.Any(p => p.Name == PermissionName));
        }
    }
}
