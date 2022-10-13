using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_BanSachAT.Models;

namespace Website_BanSachAT.Controllers
{
    public class SearchController : Controller
    {
        SachOnlineDataContext db = new SachOnlineDataContext();
        // GET: Search
        public ActionResult Search(string strSearch)
        {
            if (!string.IsNullOrEmpty(strSearch))
            {
                var kq = from s in db.SACHes where s.TenSach.Contains(strSearch) select s;
                //var kq = db.SACHes.Where(s => s.MaCD == int.Parse(strSearch)).OrderByDescending(s => s.SoLuongBan); phương thức 

                ViewBag.KQ = kq.Count();
                ViewBag.Search = strSearch;
                return View(kq);
            }
         return View();
        }
        public ActionResult ThongKe()
        {
            var kq = from s in db.SACHes
                     group s by s.MaCD into g
                     select new ReportInfo
                     {
                         Id = g.Key.ToString(),
                         Count = g.Count(),
                         Sum = g.Sum(n => n.SoLuongBan),
                         MAX = g.Max(n => n.SoLuongBan),
                         MIN = g.Min(n => n.SoLuongBan),
                         AVG = Convert.ToDecimal(g.Average(n => n.SoLuongBan))
                     };
            return View(kq);
        }
    }
}