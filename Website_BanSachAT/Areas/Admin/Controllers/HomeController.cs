using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_BanSachAT.Models;
using PagedList.Mvc;
using PagedList;
using System.IO;
namespace Website_BanSachAT.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        SachOnlineDataContext data = new SachOnlineDataContext();
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
          
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var sTenDN = f["UserName"];
            var sMatKhau = f["Password"];
            ADMIN ad = data.ADMINs.SingleOrDefault(n => n.TenDN == sTenDN && n.MatKhau == sMatKhau);
            if (ad != null)
            {
                Session["Admin"] = ad;
                return RedirectToAction("Index", "Home");
                
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

    }
}