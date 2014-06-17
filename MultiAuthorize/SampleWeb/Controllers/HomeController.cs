using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MultiAuthorize;

namespace SampleWeb.Controllers
{
    [MultiAuthorizeAttribute]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "使用者頁面";
            return View();
        }

    }
}