using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website_BanSachAT.Models;

namespace Website_BanSachAT.Areas.Admin.Controllers
{
    public class SliderController : Controller
    {
        SachOnlineDataContext data = new SachOnlineDataContext();
        // GET: Admin/Slider
        public ActionResult Index()
        {
            var list = from sl in data.Slides select sl;
            return PartialView(list);
        }
        [HttpPost]
        [ValidateInput(true)]
        public ActionResult Create(Slide sli, FormCollection f, HttpPostedFileBase fFileUpload)
        {
                if (fFileUpload == null)
                {
                    ViewBag.ThongBao = "Hãy chọn Ảnh";

                    ViewBag.TenSlider = f["sTenSlider"];
                    ViewBag.MoTa = f["sMoTa"];
                    ViewBag.LoaiSlider = f["sLoaiSlider"];
                    ViewBag.HienThi = f["bHienThi"];
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        var fFileName = Path.GetFileName(fFileUpload.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images/Slider"), fFileName);
                        if (!System.IO.File.Exists(path))
                        {
                            fFileUpload.SaveAs(path);
                        }
                        sli.TieuDe = f["sTenSlider"];
                        sli.Anh = fFileName;
                        sli.MoTa = f["sMoTa"];
                        sli.HienThi = bool.Parse(f["bHienThi"]);
                        sli.LoaiSlide = f["sLoaiSlider"];
                        data.Slides.InsertOnSubmit(sli);
                        data.SubmitChanges();
    
                        return RedirectToAction("Index");
                    }
                }
                return View();
            }

        }
}