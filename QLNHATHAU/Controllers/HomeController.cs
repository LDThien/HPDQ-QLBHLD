using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class HomeController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
         
        public ActionResult GetDataDK()
        {
            var query = db_context.DinhBienDKs.Include("NhanVien")
                .GroupBy(p => p.NhanVien.PhongBan.TenPhongBan)
                .Select(x => new { name = x.Key, count = x.Count()}).ToList();

            return Json(query,JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataTH()
        {
            var query = db_context.DinhBienTHs.Include("NhanVien")
                .GroupBy(p => p.NhanVien.PhongBan.TenPhongBan)
                .Select(x => new { name = x.Key, count = x.Count()}).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDataNVM()
        {
            var query = db_context.DinhBienNVs.Include("NhanVien")
                .GroupBy(p => p.NhanVien.PhongBan.TenPhongBan)
                .Select(x => new { name = x.Key, count = x.Count() }).ToList();

            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDataTHoi()
        {
            var query = db_context.ThuHoiBHLDCTs.Include("LoaiBHLD")
                .GroupBy(p => p.LoaiBHLD.TenBHLD)
                .Select(x => new { name = x.Key, count = x.Sum(w => w.SoLuong)}).ToList();

            return Json(query, JsonRequestBehavior.AllowGet);
        }
    }
}