using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SPAccess.ForWebAppMVC.API
{
    public class ApiPermissionAttribute : AuthorizationFilterAttribute
    {
        private bool IsCheckLogin;

        public string Name { get; private set; }
        public string Title { get; private set; }
        public string Require { get; private set; }

        /// <summary>
        /// Yêu cầu đăng nhập không cần quyền
        /// </summary>
        public ApiPermissionAttribute()
        {
            IsCheckLogin = true;
        }

        /// <summary>
        /// Yêu cầu đăng nhập và có quyền quy định trong Name
        /// </summary>
        /// <param name="Name">Tên định danh quyền</param>
        public ApiPermissionAttribute(string Name)
            : this(Name, Name)
        {

        }

        /// <summary>
        /// Yêu cầu đăng nhập và có quyền quy định trong Name
        /// </summary>
        /// <param name="Name">Tên định danh quyền</param>
        /// <param name="Title">Tiêu đề hiển thị trong khu vực quản lý</param>
        /// <param name="Require">Quyền yêu cầu liên quan. ( Ví dụ trong mô hình quản lý có chức năng danh sách, thêm, xem, sửa, xoá. Về mặt logic để dùng chức năng sửa, cần phải có thêm quyền danh sách để có thể xem và xác định đối tượng trong danh sách. Vì vậy người lập trình cần định nghĩa tại đây để gợi ý cho cấp quản lý. )</param>
        public ApiPermissionAttribute(string Name, string Title, string Require = null)
        {
            this.Name = Name;
            this.Title = Title;
            this.Require = Require;

            SPContext.Current.SPAccess.PermissionManager.UpdatePermission(this.Name, this.Title, this.Require);
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (SPContext.Current.Session == null)
            {
                actionContext.Response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                using(var obj = actionContext.Response.Content as ObjectContent)
                {
                    obj.Value = new
                    {
                        Status = false,
                        Error = "Require login."
                    };
                }
            }
            else
            {
                if (IsCheckLogin || SPContext.Current.Session.Account.Can(Name))
                {

                }
                else
                {
                    actionContext.Response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    using (var obj = actionContext.Response.Content as ObjectContent)
                    {
                        obj.Value = new
                        {
                            Status = false,
                            Error = "Access denied."
                        };
                    }
                }
            }
        }
    }
}
