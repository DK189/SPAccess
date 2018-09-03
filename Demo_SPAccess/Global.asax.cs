// Global.asax.cs => Khởi tạo môi trường
namespace Demo_SPAccess
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            System.Web.Mvc.AreaRegistration.RegisterAllAreas();
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(System.Web.Mvc.GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(System.Web.Routing.RouteTable.Routes);
            BundleConfig.RegisterBundles(System.Web.Optimization.BundleTable.Bundles);


            // Khởi tạo SPContext với tham số kết nối mặc định
            var SPContext = new SPAccess.ForWebAppMVC.SPContext();

            // Khai báo trang đăng nhập.
            SPContext.LoginPage = "~/Account/";
        }
    }
}
