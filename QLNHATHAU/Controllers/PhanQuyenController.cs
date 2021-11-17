using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class PhanQuyenController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: PhanQuyen
        public ActionResult Index(int? page, int? IdQuyen, int? IdNguoiDung)
        {
            if (IdQuyen == null) IdQuyen = 0;
            List<Quyen> q = db_context.Quyens.ToList();
            ViewBag.QuyenList = new SelectList(q, "IDQuyen", "TenQuyen", IdQuyen);

            if (IdNguoiDung == null) IdNguoiDung = 0;

            if (page == null) page = 1;
            int pageSize = 50;
            int pageNumber = (page ?? 1);

            //var res = from a in db_context.
            return View();
        }
    }
}