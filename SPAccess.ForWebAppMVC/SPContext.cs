using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPAccess.ForWebAppMVC
{
    public class SPContext
    {
        private static SPContext _Current;
        public static SPContext Current
        {
            get
            {
                if (_Current == null)
                {
                    _Current = new SPContext();
                }
                return _Current;
            }
            private set
            {
                _Current = value;
            }
        }

        public SPAccess SPAccess { get; private set; }

        public CookieManager CookieManager { get; private set; }

        private string _LoginPage = null;

        public string LoginPage
        {
            get
            {
                return _LoginPage;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _LoginPage = null;
                    return;
                }
                if (!value.StartsWith("~/"))
                {
                    throw new ArgumentException("LoginPage is absolute path to Login action, and it is must start with '~/'!");
                }

                _LoginPage = value;
            }
        }

        private Dictionary<string, ViewModel.Session> SessionCache = new Dictionary<string, ViewModel.Session>();

        /// <summary>
        /// Khởi tạo ngữ cảnh xử lý truy cập cho hệ thống theo cấu hình mặc định
        /// </summary>
        public SPContext()
            : this(new SPAccess())
        {

        }

        /// <summary>
        /// Khởi tạo ngữ cảnh xử lý truy cập cho hệ thống theo chuỗi kết nối tuỳ biến
        /// </summary>
        /// <param name="ConnectionString">Chuỗi kết nối ở dạng SQLConnectionString hoặc EntityConnectionString</param>
        /// <param name="IsEntityConnectionString">Cho hệ thống biết chuỗi này là chuỗi kết nối cho SQL hay Entity (Default: true)</param>
        public SPContext(string ConnectionString, bool IsEntityConnectionString = true)
            : this(new SPAccess(ConnectionString, IsEntityConnectionString))
        {

        }

        private SPContext(SPAccess SPAccess)
        {
            _Current = this;

            this.SPAccess = SPAccess;
            this.CookieManager = new CookieManager(this);

            LoadAllPermission();
        }

        private void LoadAllPermission()
        {
            List<object> ls = new List<object>();
            var Assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(
                    a =>
                        a.GetReferencedAssemblies().Select(ra => ra.FullName).Contains(this.GetType().Assembly.GetName().FullName)
                )
                .ToList();
            foreach (var _assembly in Assemblies.Select((a, i) => new {a,i }))
            {
                var assembly = _assembly.a;
                try
                {
                    var types = assembly.GetExportedTypes()
                    .Where(
                        t =>
                            t.Name.EndsWith("Controller")
                            &&
                            t.IsClass
                            &&
                            t.IsPublic
                            &&
                            !t.IsAbstract
                            &&
                            (
                                t.BaseType == typeof(System.Web.Mvc.Controller)
                                ||
                                t.BaseType == typeof(System.Web.Http.ApiController)
                            )
                    )
                    .ToList();
                    foreach (var type in types)
                    {
                        List<object> ListControllerPerm = new List<object>();
                        List<object> ListActionPerm = new List<object>();

                        Type PermType = null;

                        if (type.BaseType == typeof(System.Web.Mvc.Controller))
                        {
                            PermType = typeof(MVC.PermissionAttribute);
                        }
                        else if (type.BaseType == typeof(System.Web.Http.ApiController))
                        {
                            PermType = typeof(API.ApiPermissionAttribute);
                        }

                        if (PermType != null)
                        {
                            ListControllerPerm.AddRange(type.GetCustomAttributes(PermType, true));
                            foreach (var action in type.GetMethods())
                            {
                                ListActionPerm.AddRange(action.GetCustomAttributes(PermType, true));
                            }
                        }

                        ls.AddRange(ListControllerPerm);
                        ls.AddRange(ListActionPerm);
                    }
                }
                catch (Exception)
                {

                }
            }
            ls.Count.Equals(0);
        }

        public ViewModel.Session Session
        {
            get
            {
                var token = CookieManager.Token;
                if (!SessionCache.ContainsKey(token))
                {
                    try
                    {
                        var session = SPAccess.LoginByToken(token);
                        SessionCache[session.Token] = session;
                    }
                    catch
                    {
                        return null;
                    }
                }
                return SessionCache[token];
            }
            set
            {
                SessionCache[value.Token] = value;
            }
        }

        public bool Login(string Username, string Password)
        {
            try
            {
                var session = SPAccess.LoginByUser(Username, Password);
                CookieManager.Token = session.Token;
            }
            catch (Exception ex)
            {
#if DEBUG

                throw ex;
#endif
            }
            return false;
        }

        /// <summary>
        /// Hàm xử lý đăng xuất phiên làm việc hiện tại
        /// </summary>
        /// <returns></returns>
        public bool Logout ()
        {
            try
            {
                if (SPAccess.Logout(Session))
                {
                    SessionCache.Remove(CookieManager.Token);
                    CookieManager.Token = "Việt Nam Vô Địch!";
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

        public void Test ()
        {
            CookieManager.Token = @"test-token|07/14/2018 20:08:48|9991";
        }
    }
}
