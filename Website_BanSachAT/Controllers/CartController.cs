using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_BanSachAT.Models;
namespace Website_BanSachAT.Controllers
{
    public class CartController : Controller
    {
        SachOnlineDataContext db = new SachOnlineDataContext();
        // GET: Cart
        public List<GioHang> LayGioHang()
        {
            List<GioHang> listGH = Session["GioHang"] as List<GioHang>;
            // nếu giỏ hàng chưa có thì khởi tạo giỏ hàng
            if(listGH == null)
            {
                listGH = new List<GioHang>();
                Session["GioHang"] = listGH;                  
            }
            return listGH;
         
        }
        public ActionResult ThemGioHang(int ms, string url)
        {
            // lấy giỏ hàng hiện tại
            List<GioHang> lstGH = LayGioHang();
            // Kiểm tra nếu sản phẩm chưa có trong giỏ hàng thì thêm sản phẩm, ngược lại nếu đã tồn tại thì tăng số lượng
            GioHang sp = lstGH.Find(n => n.iMasach == ms);
            if(sp == null)
            {
                sp = new GioHang(ms);
                lstGH.Add(sp);
            }
            else
            {
                sp.iSoLuong++;
            }
            return Redirect(url);
     
        }
        private int TongSoLuong()
        {
            int iTongSL = 0;
            List<GioHang> listGH = Session["GioHang"] as List<GioHang>;
            if(listGH != null)
            {
                iTongSL = listGH.Sum(n => n.iSoLuong);
            }
            return iTongSL;
        }
        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> listGH = Session["GioHang"] as List<GioHang>;
            if(listGH != null)
            {
                dTongTien = listGH.Sum(n => n.dThanhTien);
            }
            return dTongTien;
        }
        public ActionResult Cart()
        {
            List<GioHang> listGH = LayGioHang();
            if(listGH.Count == 0)
            {
                return RedirectToAction("Index", "Book");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(listGH);
        }
        // View hiện thị trong cửa hàng
        public ActionResult CartPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        public ActionResult XoaSPKhoiGioHang(int iMaSach)
        {
            List<GioHang> listGH = LayGioHang();
            GioHang sp = listGH.SingleOrDefault(n => n.iMasach == iMaSach);
            if(sp != null)
            {
                listGH.RemoveAll(n => n.iMasach == iMaSach);
                if(listGH.Count == 0)
                {
                    return RedirectToAction("Index", "Book");
                }
            }
            return RedirectToAction("Cart");
        }
        public ActionResult CapNhatGioHang(int iMaSach,FormCollection f)
        {
            List<GioHang> listGH = LayGioHang();
            GioHang sp = listGH.SingleOrDefault(n => n.iMasach == iMaSach);
            if(sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("Cart");
        }
        public ActionResult XoaGioHang()
        {
            List<GioHang> listGH = LayGioHang();
            listGH.Clear();
            return RedirectToAction("Index", "Book");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if(Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == " ")
            {
                return Redirect("~/User/DangNhap?id=2");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Book");
            }
            List<GioHang> listGH = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(listGH);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
           
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<GioHang> listGH = LayGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            var NgayGiao = String.Format("{0:MM/dd/yyyy}", f["NgayGiao"]);
            ddh.TinhTrangGiaoHang = 1;
            ddh.DaThanhToan = false;
            db.DONDATHANGs.InsertOnSubmit(ddh);
            db.SubmitChanges();
            foreach(var item in listGH)
            {
                CHITIETDATHANG ctdh = new CHITIETDATHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaSach = item.iMasach;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.sDonGia;
                db.CHITIETDATHANGs.InsertOnSubmit(ctdh);
            }
            db.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDatHang", "Cart");

        }
        public ActionResult XacNhanDatHang()
        {
            return View();
        }
    }
}