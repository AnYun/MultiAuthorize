using MultiAuthorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleWeb.Areas.Manage.Controllers
{
    [MultiAuthorizeAttribute(AuthorizeName = "Admin", AuthorizeArea = "Manage", AuthorizeController = "ManageAccount", Roles = "Admin")]
    public class ManageHomeController : Controller
    {
        /// <summary>
        /// 管理者首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Title = "管理者頁面";
            return View();
        }
    }
}