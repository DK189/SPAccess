using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

        internal override string GetMeta(string Name)
        {
            if (SPAccess.DB.UserInformation.Any(inf => inf.UserID == userIdentity.UserID && inf.Name == Name))
            {
                return SPAccess.DB.UserInformation.Where(inf => inf.UserID == userIdentity.UserID && inf.Name == Name).Select(infor => infor.Value).FirstOrDefault();
            }
            return null;
        }

        internal override void SetMeta(string Name, string rawValue)
        {
            if (SPAccess.DB.UserInformation.Any(inf => inf.UserID == userIdentity.UserID && inf.Name == Name))
            {
                var infor = SPAccess.DB.UserInformation.First(inf => inf.UserID == userIdentity.UserID && inf.Name == Name);
                infor.Value = rawValue;
                infor.UpdateTime = DateTime.Now;

                SPAccess.DB.SaveChanges();
            }
            else
            {
                var infor = SPAccess.DB.UserInformation.Create();
                infor.UserID = userIdentity.UserID;
                infor.Name = Name;
                infor.Value = rawValue;
                infor.UpdateTime = DateTime.Now;

                SPAccess.DB.UserInformation.Add(infor);
                SPAccess.DB.SaveChanges();
            }
        }
    }
}
