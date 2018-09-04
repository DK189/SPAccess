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

        internal override string GetMeta(string Name)
        {
            if (SPAccess.DB.GroupInformation.Any(inf => inf.GroupID == groupIdentity.GroupID && inf.Name == Name))
            {
                return SPAccess.DB.GroupInformation.Where(inf => inf.GroupID == groupIdentity.GroupID && inf.Name == Name).Select(infor => infor.Value).FirstOrDefault();
            }
            return null;
        }

        internal override void SetMeta(string Name, string rawValue)
        {
            if (SPAccess.DB.GroupInformation.Any(inf => inf.GroupID == groupIdentity.GroupID && inf.Name == Name))
            {
                var infor = SPAccess.DB.GroupInformation.First(inf => inf.GroupID == groupIdentity.GroupID && inf.Name == Name);
                infor.Value = rawValue;
                infor.UpdateTime = DateTime.Now;

                SPAccess.DB.SaveChanges();
            }
            else
            {
                var infor = SPAccess.DB.GroupInformation.Create();
                infor.GroupID = groupIdentity.GroupID;
                infor.Name = Name;
                infor.Value = rawValue;
                infor.UpdateTime = DateTime.Now;

                SPAccess.DB.GroupInformation.Add(infor);
                SPAccess.DB.SaveChanges();
            }
        }
    }
}
