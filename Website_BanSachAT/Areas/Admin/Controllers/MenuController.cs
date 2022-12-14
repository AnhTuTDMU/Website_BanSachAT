using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_BanSachAT.Models;

namespace Website_BanSachAT.Areas.Admin.Controllers
{
    public class MenuController : Controller
    {
        SachOnlineDataContext data = new SachOnlineDataContext();
        // GET: Admin/Menu
        public ActionResult Index()
        {
            var listMenu = data.MENUs.Where(m => m.ParentId == null).OrderBy(m => m.OrderNumber).ToList();
            int[] a = new int[listMenu.Count()];
            for (int i = 0; i < listMenu.Count; i++)
            {
                var l = data.MENUs.Where(m => m.ParentId == listMenu[i].Id);
                a[i] = l.Count();
            }
            ViewBag.lst = a;
            List<CHUDE> cd = data.CHUDEs.ToList();
            ViewBag.ChuDe = cd;
            List<TRANGTIN> tt = data.TRANGTINs.ToList();
            ViewBag.TrangTin = tt;
            return View(listMenu);
        }
        [ChildActionOnly]
        public ActionResult ChildMenu(int parentId)
        {
            List<MENU> lst = new List<MENU>();
            lst = data.MENUs.Where(m => m.ParentId == parentId).OrderBy(m => m.OrderNumber).ToList();
            ViewBag.Count = lst.Count();
            int[] a = new int[lst.Count()];
            for (int i = 0; i < lst.Count; i++)
            {
                var l = data.MENUs.Where(m => m.ParentId == lst[i].Id);
                a[i] = l.Count();
            }
            ViewBag.lst = a;
            return PartialView("ChildMenu", lst);
        }
        [ChildActionOnly]
        public ActionResult ChildMenu1(int parentId)
        {
            List<MENU> lst = new List<MENU>();
            lst = data.MENUs.Where(m => m.ParentId == parentId).OrderBy(m => m.OrderNumber).ToList();
            ViewBag.Count = lst.Count();
            int[] a = new int[lst.Count()];
            for (int i = 0; i < lst.Count; i++)
            {
                var l = data.MENUs.Where(m => m.ParentId == lst[i].Id);
                a[i] = l.Count();
            }
            ViewBag.lst = a;
            return PartialView("ChildMenu1", lst);
        }
        [HttpPost]
        public ActionResult AddMenu(FormCollection f)
        {
            if (!string.IsNullOrEmpty(f["ThemChuDe"]))
            {
                MENU m = new MENU();
                var cd = data.CHUDEs.Where(c => c.MaCD == int.Parse(f["MaCD"].ToString())).SingleOrDefault();
                m.MenuName = cd.TenChuDe;
                m.MenuLink = "sach-theo-chu-de" + cd.MaCD;
                if (!string.IsNullOrEmpty(f["ParentID"]))
                {
                    m.ParentId = int.Parse(f["ParentID"]);
                }
                else
                {
                    m.ParentId = null;
                }
                m.OrderNumber = int.Parse(f["Number"]);
                data.MENUs.InsertOnSubmit(m);
                data.SubmitChanges();
            }
            else if (!string.IsNullOrEmpty(f["ThemTrang"]))
            {
                MENU m = new MENU();
                var trang = data.TRANGTINs.Where(t => t.MaTT == int.Parse(f["MaTT"].ToString())).SingleOrDefault();
                m.MenuName = trang.TenTrang;
                m.MenuLink = trang.MetaTitle;
                if (!string.IsNullOrEmpty(f["ParentID"]))
                {
                    m.ParentId = int.Parse(f["ParentID"]);
                }
                else
                {
                    m.ParentId = null;
                }
                m.OrderNumber = int.Parse(f["Number1"]);
                data.MENUs.InsertOnSubmit(m);
                data.SubmitChanges();
            }
            else if (!string.IsNullOrEmpty(f["ThemLink"]))
            {
                MENU m = new MENU();
                m.MenuName = f["TenMenu"];
                m.MenuLink = f["Link"];
                if (!string.IsNullOrEmpty(f["ParentID"]))
                {
                    m.ParentId = int.Parse(f["ParentID"]);
                }
                else
                {
                    m.ParentId = null;
                }
                m.OrderNumber = int.Parse(f["Number2"]);
                data.MENUs.InsertOnSubmit(m);
                data.SubmitChanges();
            }
            return Redirect("~/Admin/Menu/Index");
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            List<MENU> submn = data.MENUs.Where(m => m.ParentId == id).ToList();
            if (submn.Count() > 0)
            {
                return Json(new { code = 500, msg = "Còn menu con, không xóa được" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var mn = data.MENUs.SingleOrDefault(m => m.Id == id);
                data.MENUs.DeleteOnSubmit(mn);
                data.SubmitChanges();
                return Json(new { code = 200, msg = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Update(int id)
        {
            try
            {
                var mn = (from m in data.MENUs
                          where (m.Id == id)
                          select new
                          {
                              Id = m.Id,
                              MenuName = m.MenuName,
                              MenuLink = m.MenuLink,
                              OrderNumber = m.OrderNumber
                          }).SingleOrDefault();
                return Json(new { code = 200, mn = mn, msg = "Lấy thông tin thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin thất bại." + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Update(int id, string strTenMenu, string strLink, int STT)
        {
            try
            {
                var mn = data.MENUs.SingleOrDefault(m => m.Id == id);
                mn.MenuName = strTenMenu;
                mn.MenuLink = strLink;
                mn.OrderNumber = STT;
                data.SubmitChanges();
                return Json(new { code = 200, msg = "Sửa Menu thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Sửa menu thất bại. Lỗi" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}