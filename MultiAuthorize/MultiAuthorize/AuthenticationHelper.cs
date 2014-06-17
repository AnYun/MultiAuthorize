using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace MultiAuthorize
{
    /// <summary>
    /// 登入驗證相關處理
    /// </summary>
    public class AuthenticationHelper
    {
        /// <summary>
        /// 驗證登入
        /// </summary>
        /// <param name="authorizeName">用來分別不同登入驗證的名稱</param>
        /// <param name="name">登入名稱</param>
        /// <param name="roles">角色（多個角色用逗號分隔）</param>
        /// <param name="Timeout">設定 Timeout 的時間（預設為 20 分鐘）</param>
        public static void SignIn(string authorizeName, string name, string roles, int Timeout = 20)
        {
            var authTicket = new FormsAuthenticationTicket(1, name, DateTime.Now, DateTime.Now.AddMinutes(Timeout), false, roles);
            WriteAuthentication(authTicket, authorizeName);
        }

        /// <summary>
        /// 驗證登出
        /// </summary>
        /// <param name="authorizeName">用來分別不同登入驗證的名稱</param>
        public static void SignOut(string authorizeName)
        {
            // 寫入過期的 Cookie
            HttpContext.Current.Response.Headers.Add("Set-Cookie", Uri.EscapeDataString(authorizeName) + "=; path=/; expires=Thu, 01-Jan-1970 00:00:00 GMT");
        }

        /// <summary>
        /// 寫入驗證票券到 Cookie
        /// </summary>
        /// <param name="authTicket">驗證票券</param>
        /// <param name="authorizeName">用來分別不同登入驗證的名稱</param>
        private static void WriteAuthentication(FormsAuthenticationTicket authTicket, string authorizeName)
        {
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(authorizeName, encryptedTicket);
            authCookie.HttpOnly = true;
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }

        /// <summary>
        /// 取得指定名稱的 AuthCookie
        /// </summary>
        /// <param name="authorizeName">用來分別不同登入驗證的名稱</param>
        /// <returns></returns>
        public static HttpCookie GetAuthCookie(string authorizeName)
        {
            return HttpContext.Current.Request.Cookies[authorizeName];
        }

        /// <summary>
        /// 從 Cookie 取驗證票券
        /// </summary>
        /// <param name="authCookie">Cookie</param>
        /// <returns></returns>
        public static FormsAuthenticationTicket GetAuthTicket(HttpCookie authCookie)
        {
            try
            {
                return FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch
            {
                return new FormsAuthenticationTicket(1, "", DateTime.Now, DateTime.Now.AddMinutes(-20), false, "");
            }
        }

        /// <summary>
        /// 直接取得驗證票券，在登入狀態下可以用來取得登入者資訊
        /// </summary>
        /// <param name="authorizeName">用來分別不同登入驗證的名稱</param>
        /// <returns></returns>
        public static FormsAuthenticationTicket GetAuthTicket(string authorizeName)
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[authorizeName];
            FormsAuthenticationTicket authTicket = GetAuthTicket(authCookie);

            return authTicket;
        }

        /// <summary>
        /// 判斷角色是否存在
        /// </summary>
        /// <param name="Roles">系統角色</param>
        /// <param name="UserRoles">使用者角色</param>
        /// <returns></returns>
        public static bool CheckRoleExists(string Roles, string UserRoles)
        {
            List<string> rolesList = rolesList = Roles.ToLower().Split(',').ToList();
            List<string> userRolesList = UserRoles.ToLower().Split(',').ToList();

            // 取交集，個數為 0 代表沒有符合的角色
            var intersectRoles = rolesList.Intersect(userRolesList);

            if (intersectRoles.Count() == 0)
            {
                return false;
            }

            return true;
        }

        #region 確認驗證
        /// <summary>
        /// 驗證 authCookie 是否有效
        /// </summary>
        /// <param name="authCookie">儲存授權資訊的 Cookie</param>
        /// <returns></returns>
        public static bool CheckAuthorization(HttpCookie authCookie)
        {
            return CheckAuthorization(authCookie, null, null);
        }

        /// <summary>
        /// 驗證 authCookie 是否有效且包含指定的使用者
        /// </summary>
        /// <param name="Users">使用者</param>
        /// <param name="authCookie">儲存授權資訊的 Cookie</param>
        /// <returns></returns>
        public static bool CheckAuthorization(string Users, HttpCookie authCookie)
        {
            return CheckAuthorization(authCookie, null, Users);
        }

        /// <summary>
        /// 驗證 authCookie 是否有效且包含指定的角色
        /// </summary>
        /// <param name="authCookie">儲存授權資訊的 Cookie</param>
        /// <param name="Roles">角色</param>
        /// <returns></returns>
        public static bool CheckAuthorization(HttpCookie authCookie, string Roles)
        {
            return CheckAuthorization(authCookie, Roles, null);
        }

        /// <summary>
        /// 驗證 authCookie 是否有效且包含指定的角色或使用者
        /// </summary>
        /// <param name="authCookie">儲存授權資訊的 Cookie</param>
        /// <param name="Roles">角色</param>
        /// <param name="Users">使用者</param>
        /// <returns></returns>
        public static bool CheckAuthorization(HttpCookie authCookie, string Roles, string Users)
        {
            // 驗證的 Cookie 不存在就驗證失敗
            if (authCookie == null || authCookie.Value == "")
            {
                return false;
            }

            FormsAuthenticationTicket authTicket = GetAuthTicket(authCookie);

            // 驗證票券過期也失敗
            if (authTicket.Expired || authTicket.Expiration <= DateTime.Now)
            {

                return false;
            }

            // 有使用者資訊就驗證使用者是否符合
            if (!string.IsNullOrEmpty(Users))
            {
                // 驗證使用者是否符合，不符合也失敗
                List<string> usersList = Users.ToLower().Split(',').ToList();
                if (!usersList.Contains(authTicket.Name.ToLower()))
                {
                    return false;
                }
            }

            // 有角色資訊就驗證角色是否符合
            if (!string.IsNullOrEmpty(Roles))
            {
                // 沒有符合的角色也失敗
                if (!CheckRoleExists(Roles, authTicket.UserData))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}
