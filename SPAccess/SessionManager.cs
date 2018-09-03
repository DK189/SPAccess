using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccess
{
    public class SessionManager : _SPManager
    {
        internal SessionManager(SPAccess SPAccess) : base(SPAccess)
        {
        }

        public ViewModel.Session CreateSessionForUser(int UserID)
        {
            return CreateSessionForUser(DB.UserIdentity.First(u => u.UserID == UserID));
        }

        public ViewModel.Session CreateSessionForUser(UserIdentity User)
        {
            try
            {
                if (User != null && DB.UserIdentity.Contains(User))
                {
                    var now = DateTime.UtcNow;
                    var session = DB.Session.Create();
                    session.UserID = User.UserID;
                    session.Token = "access-token-v1";
                    session.UpdateTime = now;
                    session.ExpiredTime = session.UpdateTime.AddDays(30.0);
                    DB.SaveChanges();
                    session.Token += "|" + now + "|" + session.SessionID;
                    DB.SaveChanges();

                    return new ViewModel.Session(SPAccess, session);
                }
            }
            catch (Exception ex)
            {
#if DEBUG

                throw ex;
#endif
            }

            return null;
        }

        public ViewModel.Session OpenSessionByToken(string Token)
        {
            try
            {
                return new ViewModel.Session(SPAccess, DB.Session.First(s => s.Token == Token && s.ExpiredTime > DateTime.Now));
            }
            catch (Exception ex)
            {
#if DEBUG

                throw ex;
#endif
            }
            return null;
        }

        public bool RevokeSession(int SessionID)
        {
            try
            {
                var now = DateTime.Now;
                DB.Session.Where(s => s.SessionID == SessionID && s.ExpiredTime > now).ToList().ForEach(s => s.ExpiredTime = now);
                DB.SaveChanges();
                DB.Session.Where(s => s.SessionID == SessionID && s.ExpiredTime > now).ToList().ForEach(s => DB.Reload(s));
                return false;
            }
            catch (Exception ex)
            {
#if DEBUG

                throw ex;
#endif
            }
            return false;
        }

        public bool RevokeAllSessionsOfUser(int UserID)
        {
            try
            {
                var now = DateTime.Now;
                DB.Session.Where(s => s.UserID == UserID && s.ExpiredTime > now).ToList().ForEach(s => s.ExpiredTime = now);
                DB.SaveChanges();
                DB.Session.Where(s => s.UserID == UserID && s.ExpiredTime > now).ToList().ForEach(s => DB.Reload(s));
                return false;
            }
            catch (Exception ex)
            {
#if DEBUG

                throw ex;
#endif
            }
            return false;
        }

        public object test()
        {
            return DB.Session.Where(s => "test-token|07/14/2018 20:08:48|9991" == s.Token).AsQueryable().ToString();
            return null;
        }
    }
}
