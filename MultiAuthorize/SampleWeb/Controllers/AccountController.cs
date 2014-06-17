using MultiAuthorize;
using SampleWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleWeb.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// 登入畫面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            ViewBag.Title = "使用者登入";

            if (Request.IsAuthenticated("User"))
                return RedirectToAction("Index", "Home");
            else
                return View();
        }

        /// <summary>
        /// 登入驗證
        /// </summary>
        /// <param name="model">使用者輸入的登入參數</param>
        /// <param name="returnUrl">登入成功後要轉回的網址</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (model.Account.Equals("User") && model.Password.Equals("Pass"))
            {
                AuthenticationHelper.SignIn("User", model.Account, "User");

                return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? Url.Action("Index", "Home") : returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "帳號密碼錯誤！");
            }

            return View(model);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            AuthenticationHelper.SignOut("User");
            return RedirectToAction("Login");
        }
    }
}