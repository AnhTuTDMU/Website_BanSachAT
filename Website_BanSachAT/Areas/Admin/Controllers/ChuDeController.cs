using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_BanSachAT.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace Website_BanSachAT.Areas.Admin.Controllers
{
    public class ChuDeController : Controller
    {
        SachOnlineDataContext data = new SachOnlineDataContext();
        // GET: Admin/ChuDe
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult DsChuDe()
        {
            try
            {
                var dsCD = (from cd in data.CHUDEs select new { MaCD = cd.MaCD, TenCD = cd.TenChuDe }).ToList();
                return Json(new { code = 200, dsCD = dsCD, msg = "Lấy danh sách chủ đề thành công" }, JsonRequestBehavior.AllowGet);
                    
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách chủ đề thất bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Details(int MaCD)
        {
            try
            {
                var cd = (from s in data.CHUDEs
                          where (s.MaCD == MaCD) select new { MaCD = s.MaCD, TenChuDe = s.TenChuDe }).SingleOrDefault();
                return Json(new { code = 200, cd = cd, msg = "Lấy thông tin chủ đề thành công" }, JsonRequestBehavior.AllowGet);
            }
           catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách chủ đề thất bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult AddChuDe(string strTenCD)
        {
            try
            {
                var cd = new CHUDE();
                cd.TenChuDe = strTenCD;
                data.CHUDEs.InsertOnSubmit(cd);
                data.SubmitChanges();
                return Json(new { code = 200, msg = "Thêm chủ đề thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm chủ đề thất bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
          
        }
        public JsonResult Update(int MaCD, string strTenCD)
        {
            try
            {
                var cd = data.CHUDEs.SingleOrDefault(c => c.MaCD == MaCD);
                cd.TenChuDe = strTenCD;
                data.SubmitChanges();

                return Json(new { code = 200, msg = "Sửa chủ đề thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Sửa chủ đề thất bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Delete(int MaCD)
        {
            try
            {
                var cd = data.CHUDEs.SingleOrDefault(c => c.MaCD == MaCD);
                data.CHUDEs.DeleteOnSubmit(cd);
                data.SubmitChanges();

                return Json(new { code = 200, msg = "Xóa chủ đề thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa chủ đề thất bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
   
}