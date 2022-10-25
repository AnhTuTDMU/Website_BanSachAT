using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_BanSachAT.Models;
using PagedList.Mvc;
using System.IO;
namespace Website_BanSachAT.Areas.Admin.Controllers
{
    public class BookController : Controller
    {
        SachOnlineDataContext data = new SachOnlineDataContext();
        // GET: Admin/Book
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int PageSize = 7;
            return View(data.SACHes.ToList().OrderBy(n => n.MaSach).ToPagedList(iPageNum, PageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SACH sach, FormCollection f, HttpPostedFileBase fFileUpLoad)
        {
            //Dua du lieu vao dropdown
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            if (fFileUpLoad == null)
            {
                //Noi dung thong bao yeu cau chon anh bia
                ViewBag.ThongBao = "Hay chon anh bia";
                //luu thong tin de khi load lai trang do yeu cau chon anh bia se hien thi cac thong tin len trang
                ViewBag.TenSach = f["sTenSach"];
                ViewBag.MoTa = f["sMoTa"];
                ViewBag.SoLuong = int.Parse(f["iSoLuong"]);
                ViewBag.GiaBan = decimal.Parse(f["mGiaBan"]);
                ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", int.Parse(f["MaCD"]));
                ViewBag.MaCD = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", int.Parse(f["MaNXB"]));
                return View();

            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Lay ten file( khai bao thu vien : System IO)
                    var sFileName = Path.GetFileName(fFileUpLoad.FileName);
                    //Lay duong dan luu file
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                    //Kiem tra anh bia da ton tai chua de luu len thu muc
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpLoad.SaveAs(path);
                    }
                    //luu SACH vao CSDL
                    sach.TenSach = f["sTenSach"];
                    sach.MoTa = f["sMoTa"].Replace("<p>", "").Replace("</p>", "\n");
                    sach.AnhBia = sFileName;
                    sach.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
                    sach.SoLuongBan = int.Parse(f["iSoLuong"]);
                    sach.GiaBan = decimal.Parse(f["mGiaBan"]);
                    sach.MaCD = int.Parse(f["MaCD"]);
                    sach.MaNXB = int.Parse(f["MaNXB"]);
                    data.SACHes.InsertOnSubmit(sach);
                    data.SubmitChanges();
                    return RedirectToAction("Index", "Book");
                }
                return View();
            }
        }
        public ActionResult Details(int id)
        {
            var sach = data.SACHes.SingleOrDefault(a => a.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        public ActionResult Delete(int id)
        {
            var sach = data.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View();
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteComform(int id, FormCollection f)
        {
            var sach = data.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = data.CHITIETDATHANGs.Where(ct => ct.MaSach == id);
            if (ctdh.Count() > 0)
            {
                // noi dung hien thi khi sach can xoa da co trong table ChitietDonHang
                ViewBag.ThongBao = "Sách này đang có trong bảng Chi Tiết Đặt Hàng <br>" +
                    "Nếu muốn xóa thì phải xóa hết mã sách này trong bảng Chi tiết đặt hàng";
                return View(sach);
            }
            //Xóa hết thông tin của cuốn sách trong table VietSach trước khi xóa sach này 
            var vietsach = data.VIETSACHes.Where(vs => vs.MaSach == id).ToList();
            if (vietsach != null)
            {
                data.VIETSACHes.DeleteAllOnSubmit(vietsach);
                data.SubmitChanges();
            }
            // xoa sach
            data.SACHes.DeleteOnSubmit(sach);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sach = data.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var sach = data.SACHes.SingleOrDefault(n => n.MaSach == int.Parse(f["iMaSach"]));
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            if (ModelState.IsValid)
            {
                if (fFileUpload != null)
                {
                    //lay ten file( khai bao thu vien: system: IO)
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    //lay duong dan file
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                    //kiem tra file da ton tai chua
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    sach.AnhBia = sFileName;
                }
                //Luu sach vao csdl
                sach.TenSach = f["sTenSach"];
                sach.MoTa = f["sMoTa"].Replace("<p>", "").Replace("</p>", "\n");
                sach.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
                sach.SoLuongBan = int.Parse(f["iSoLuong"]);
                sach.GiaBan = decimal.Parse(f["mGiaBan"]);
                sach.MaCD = int.Parse(f["MaCD"]);
                sach.MaNXB = int.Parse(f["MaNXB"]);
                data.SubmitChanges();
                //ve lai trang quan ly sach 
                return RedirectToAction("Index");
            }
            return View(sach);
        }
    }
}