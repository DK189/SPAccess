using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccess.ViewModel
{
    public class Group : SPModel
    {
        private GroupIdentity groupIdentity;

        internal Group(SPAccess SPAccess, global::SPAccess.GroupIdentity groupIdentity)
            : base(SPAccess)
        {
            this.groupIdentity = groupIdentity;
        }

        public int GroupID
        {
            get
            {
                return groupIdentity.GroupID;
            }
        }

        public string Name
        {
            get
            {
                return groupIdentity.Name;
            }
        }

        public string Description
        {
            get
            {
                return groupIdentity.Description;
            }
        }

        public IList<ViewModel.Permission> PermissionsAllowed
        {
            get
            {
                return groupIdentity.PermissionIdentity.Select(p => new ViewModel.Permission(SPAccess, p)).ToList().AsReadOnly();
            }
        }

        public bool Can(string permissionName)
        {
            return groupIdentity.PermissionIdentity.Any(p => p.Name == permissionName);
        }
    }
}
