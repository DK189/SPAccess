using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SPAccess.ForWebAppMVC
{
    public class CookieManager
    {
        public SPContext SPContext { get; private set; }

        public TimeSpan Expires { get; set; }

        internal CookieManager (SPContext SPContext)
        {
            this.SPContext = SPContext;

            Expires = new TimeSpan(7, 0, 0, 0);
        }

        public string this[string Name]
        {
            get
            {
                if (Name == null)
                {
                    return null;
                }
                var _Name = SPCore.Crypt.Md5.Encrypt(Name);
                return SPCore.Crypt.Rc4.Decrypt(Name, HttpContext.Current.Request.Cookies[_Name] != null ? HttpContext.Current.Request.Cookies[_Name].Value : "");
            }
            set
            {
                if (Name == null)
                {
                    return;
                }
                if (value == null)
                {
                    return;
                }

                var _Name = SPCore.Crypt.Md5.Encrypt(Name);
                var cookie = new HttpCookie(_Name, SPCore.Crypt.Rc4.Encrypt(Name, value))
                {
                    Expires = DateTime.Now + Expires
                };

                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public string Token
        {
            get
            {
                return this["SPAccess-Token"];
            }
            set
            {
                this["SPAccess-Token"] = value;
            }
        }
    }
}
