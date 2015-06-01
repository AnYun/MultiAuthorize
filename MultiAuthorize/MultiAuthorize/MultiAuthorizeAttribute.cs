using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace MultiAuthorize
{
    /// <summary>
    /// 自定義登入驗證，可適用多個驗證登入
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class MultiAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 驗證識別用的名稱
        /// </summary>
        public string AuthorizeName { set; get; }
        /// <summary>
        /// 驗證失敗要轉換到的驗證 Controller
        /// </summary>
        public string AuthorizeController { set; get; }
        /// <summary>
        /// 驗證失敗要轉換到的驗證 Action
        /// </summary>
        public string AuthorizeAction { set; get; }
        /// <summary>
        /// 驗證失敗要轉換到的驗證 Area
        /// </summary>
        public string AuthorizeArea { set; get; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var skipAuthorization =
                filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute),
                    inherit: true);

            if (skipAuthorization)
            {
                return;
            }

            var authCookie = AuthenticationHelper.GetAuthCookie(AuthorizeName ?? "User");

            // 驗證失敗要轉換的網址
            RedirectToRouteResult authorizeUrl = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = AuthorizeController ?? "Account",
                            action = AuthorizeAction ?? "Login",
                            area = AuthorizeArea,
                            returnUrl = filterContext.HttpContext.Request.RawUrl
                        }));

            if (!AuthenticationHelper.CheckAuthorization(authCookie, Roles, Users))
                filterContext.Result = authorizeUrl;
        }
    }
}
