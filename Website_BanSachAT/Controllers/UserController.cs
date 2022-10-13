using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Website_BanSachAT.Models;

namespace Website_BanSachAT.Controllers
{
    public class UserController : Controller
    {
        SachOnlineDataContext db = new SachOnlineDataContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DangKy()
        {
            return View();
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            var sHoten = collection["HoTen"];
            var sTenDn = collection["TaiKhoan"];
            var sMatkhau = collection["MatKhau"];
            var sMatkhaunl = collection["MatKhauNL"];
            var sDiaChi = collection["DiaChi"];
            var sEmail = collection["Email"];
            var sDienThoai = collection["DienThoai"];
            var sNgaySinh = string.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);
            
             if (String.IsNullOrEmpty(sMatkhaunl))
            {
                ViewData["err4"] = "Nhập lại mật khẩu";
            }
            else if (sMatkhau != sMatkhaunl)
            {
                ViewData["err4"] = "Mật khẩu nhập lại không khớp";
            }
            else if(db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDn) != null){
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
            }
            else if (db.KHACHHANGs.SingleOrDefault(n => n.Email == sEmail) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else if(ModelState.IsValid)
            {
                kh.HoTen = sHoten;
                kh.TaiKhoan = sTenDn;
                kh.MatKhau = sMatkhau;
                kh.Email = sEmail;
                kh.DiaChi = sDiaChi;
                kh.DienThoai = sDienThoai;
                kh.NgaySinh = DateTime.Parse(sNgaySinh);
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("DangNhap");
            }
            return this.DangKy();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            int state = int.Parse(Request.QueryString["id"]);
            ViewBag.TenDN = "Người dùng";
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == sMatKhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoan"] = kh;
                    if(state == 1)
                    {
                        return RedirectToAction("Index", "Book");
                    }
                    else 
                    {
                        return RedirectToAction("DatHang", "Cart");
                    }
                }

                else
                {
                    ViewBag.ThongBao = "Sai mật khẩu hoặc tài khoản";
                }
            }
            return View();
        }
        public ActionResult DangXuat()
        {
            FormsAuthentication.SignOut();
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index", "Book");
        }
    }
}