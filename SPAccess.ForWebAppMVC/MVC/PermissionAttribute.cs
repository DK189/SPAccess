using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SPAccess.ForWebAppMVC.MVC
{
    public class PermissionAttribute : FilterAttribute, IAuthorizationFilter
    {
        private bool IsCheckLogin;

        public string Name { get; private set; }
        public string Title { get; private set; }
        public string Require { get; private set; }

        /// <summary>
        /// Yêu cầu đăng nhập không cần quyền
        /// </summary>
        public PermissionAttribute ()
        {
            IsCheckLogin = true;
        }

        /// <summary>
        /// Yêu cầu đăng nhập và có quyền quy định trong Name
        /// </summary>
        /// <param name="Name">Tên định danh quyền</param>
        public PermissionAttribute(string Name)
            : this(Name, Name)
        {

        }

        /// <summary>
        /// Yêu cầu đăng nhập và có quyền quy định trong Name
        /// </summary>
        /// <param name="Name">Tên định danh quyền</param>
        /// <param name="Title">Tiêu đề hiển thị trong khu vực quản lý</param>
        /// <param name="Require">Quyền yêu cầu liên quan. ( Ví dụ trong mô hình quản lý có chức năng danh sách, thêm, xem, sửa, xoá. Về mặt logic để dùng chức năng sửa, cần phải có thêm quyền danh sách để có thể xem và xác định đối tượng trong danh sách. Vì vậy người lập trình cần định nghĩa tại đây để gợi ý cho cấp quản lý. )</param>
        public PermissionAttribute(string Name, string Title, string Require = null)
        {
            IsCheckLogin = false;

            this.Name = Name;
            this.Title = Title;
            this.Require = Require;

            SPContext.Current.SPAccess.PermissionManager.UpdatePermission(this.Name, this.Title, this.Require);
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SPContext.Current.Session == null)
            {
                if (SPContext.Current.LoginPage != null)
                {
                    filterContext.Result = new RedirectResult(SPContext.Current.LoginPage);
                }
                else
                {
                    filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "We are missing Login page?.");
                }
            }
            else
            {
                //var anyPerms = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType
                //    .GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.InvokeMethod)
                //    .Where(
                //        m =>
                //            m.CustomAttributes
                //                .Any(
                //                    a =>
                //                        a.AttributeType == typeof(PermissionAttribute)
                //                )
                //    )
                //    .Select(
                //        m =>
                //            m.GetCustomAttributes(typeof(PermissionAttribute), true)
                //                .FirstOrDefault()
                //    )
                //    .Where(
                //        o =>
                //            o != null
                //    )
                //    .Select(
                //        o =>
                //            (o as PermissionAttribute).Name
                //    )
                //    .ToList();
                if (IsCheckLogin || SPContext.Current.Session.Account.Can(Name))
                {

                }
                else
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
        }
    }
}
