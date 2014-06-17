using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MultiAuthorize
{
    /// <summary>
    /// Request 擴充方法
    /// </summary>
    public static class RequestExtension
    {
        /// <summary>
        /// 判斷是否有登入且有權限
        /// Notice：不包含判斷角色和使用者
        /// </summary>
        /// <param name="request"></param>
        /// <param name="authorizeName">用來分別不同登入驗證的名稱</param>
        /// <returns></returns>
        public static bool IsAuthenticated(this HttpRequestBase request, string authorizeName)
        {
            HttpCookie authCookie = AuthenticationHelper.GetAuthCookie(authorizeName);

            return AuthenticationHelper.CheckAuthorization(authCookie);
        }

        /// <summary>
        /// 判斷是否有登入且有權限
        /// Notice：不包含判斷角色和使用者
        /// </summary>
        /// <param name="request"></param>
        /// <param name="authorizeName">用來分別不同登入驗證的名稱</param>
        /// <param name="roles">角色</param>
        /// <param name="users">使用者</param>
        /// <returns></returns>
        public static bool IsAuthenticated(this HttpRequestBase request, string authorizeName, string roles, string users)
        {
            HttpCookie authCookie = AuthenticationHelper.GetAuthCookie(authorizeName);

            return AuthenticationHelper.CheckAuthorization(authCookie, roles, users);
        }

        /// <summary>
        /// 判斷指定角色在指定的驗證名稱下是否存在且有權限
        /// </summary>
        /// <param name="request"></param>
        /// <param name="authorizeName">用來分別不同登入驗證的名稱</param>
        /// <param name="roles">角色</param>
        /// <returns></returns>
        public static bool IsAuthenticatedRole(this HttpRequestBase request, string authorizeName, string roles)
        {
            HttpCookie authCookie = AuthenticationHelper.GetAuthCookie(authorizeName);

            return AuthenticationHelper.CheckAuthorization(authCookie, roles);
        }

        /// <summary>
        /// 判斷指定使用者在指定的驗證名稱下是否存在且有權限
        /// </summary>
        /// <param name="request"></param>
        /// <param name="authorizeName">用來分別不同登入驗證的名稱</param>
        /// <param name="users">使用者</param>
        /// <returns></returns>
        public static bool IsAuthenticatedUser(this HttpRequestBase request, string authorizeName, string users)
        {
            HttpCookie authCookie = AuthenticationHelper.GetAuthCookie(authorizeName);

            return AuthenticationHelper.CheckAuthorization(users, authCookie);
        }
    }
}
