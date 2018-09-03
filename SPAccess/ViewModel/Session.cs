using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccess.ViewModel
{
    public class Session : SPModel
    {
        private global::SPAccess.Session session;

        private Account account;

        internal Session(SPAccess SPAccess, global::SPAccess.Session session)
            : base(SPAccess)
        {
            this.session = session;
            this.account = new Account(this.SPAccess, session.UserIdentity);
        }

        public int SessionID
        {
            get
            {
                return session.SessionID;
            }
        }

        public string Token
        {
            get
            {
                return session.Token;
            }
        }

        public Account Account
        {
            get
            {
                return account;
            }
        }
    }
}
