using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_BanSachAT.Models;
using PagedList;
using PagedList.Mvc;

namespace Website_BanSachAT.Controllers
{
    public class BookController : Controller
    {
        SachOnlineDataContext data = new SachOnlineDataContext();
        private List<SACH> LaySachMoi(int count)
        {

            return data.SACHes.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        private List<SACH> SachBanNhieu(int count)
        {
            return data.SACHes.OrderByDescending(a => a.SoLuongBan).Take(count).ToList();
        }
        // GET: Book
        public ActionResult Index (int ? page)
        {
            int iSize = 6;
            var iPageNum = (page ?? 1);
            var listSachMoi = LaySachMoi(20);
            return View(listSachMoi.ToPagedList(iPageNum, iSize));
        }
        public ActionResult SachBanNhieu()
        {
            var listSachBn = SachBanNhieu(6);
            return PartialView(listSachBn);
        }
        public ActionResult NavPartial()
        {
            return PartialView();
        }
        public ActionResult TopicPartial()
        {
            return PartialView();
        }
        public ActionResult TopicPartial_New()
        {
            return PartialView();
        }
        public ActionResult SliderPartial()
        {
            return PartialView();
        }
        public ActionResult ChuDepartial()
        {
            var listChuDe = from cd in data.CHUDEs select cd;
            return PartialView(listChuDe);
        }
        public ActionResult SachTheoChuDe(int id, int ? page)
        {
            ViewBag.MaCD = id;
            //tạo biến quy định số sản phẩm trên mỗi trang
            int iSize = 3;
            //tạo biến số trang
            int iPageNum = (page ?? 1);
            var Sach = from s in data.SACHes where s.MaCD == id select s;
            return View(Sach.ToPagedList(iPageNum, iSize));
        }
        public ActionResult NhaXuatBan()
        {
            var listNXB = from xb in data.NHAXUATBANs select xb;
            return PartialView(listNXB);
        }
        public ActionResult SachTheoNXB(int id, int ? page)
        {
            ViewBag.MaNXB = id;
            int iSize = 3;
            int iPageNum = (page ?? 1);
            var Sach = from s in data.SACHes where s.MaNXB == id select s;
            return View(Sach.ToPagedList(iPageNum, iSize));
        }
        public ActionResult ChiTietSach(int id)
        {
            var Sach = from s in data.SACHes where s.MaSach == id select s;
            return View(Sach.Single());
        }
        

    }
}