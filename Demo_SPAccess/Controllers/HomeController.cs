// HomeController.cs => Ví dụ trong file Controller
namespace Demo_SPAccess.Controllers
{
    // Các Action thuộc HomeController đều sẽ chịu ảnh hưởng bởi quyền Home
    [SPAccess.ForWebAppMVC.MVC.Permission("Home", "Cụm chức năng trang chủ.")]
    public class HomeController : System.Web.Mvc.Controller
    {
        public System.Web.Mvc.ActionResult Index()
        {
            return View();
        }

        // About action sẽ chỉ chịu ảnh hưởng bởi quyền About
        [SPAccess.ForWebAppMVC.MVC.Permission("About", "Giới thiệu")]
        public System.Web.Mvc.ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public System.Web.Mvc.ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}