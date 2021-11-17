using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class NewstaffController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Newstaff
        public ActionResult Index(int? page, int? id)
        {

            var res = (from a in db_context.BHLĐXuat.Where( x => x.BDNVID == id)
                       join db in db_context.DinhBienNVs on a.BDNVID equals db.IDBDNV
                       join bh in db_context.LoaiBHLDs on a.BHLDID equals bh.IDBHLD
                       select new ExportprotectionValidation()
                       {
                           IDBHLDX = a.IDBHLDX,
                           BDNVID = (int)a.BHLDID,
                           BHLDID = (int)a.BHLDID,
                           TenBHLD = bh.TenBHLD,
                           Soluong = (int)a.Soluong,
                           Size = a.Size
                       });

            if (page == null) page = 1;
            int pageSize = 10;
            int pageNumber = ((int)(page ?? 1));
            return View(res.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create(int? id)
        {
            List<LoaiBHLD> bh = db_context.LoaiBHLDs.ToList();
            ViewBag.BHLDList = new SelectList(bh, "IDBHLD", "TenBHLD");

            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(ExportprotectionValidation _DO, int id)
        {
            if (ModelState.IsValid)
            {
                var BDNVID = id;
                try
                {

                    db_context.BHLĐXuat_insert( BDNVID , _DO.BHLDID, _DO.Soluong, _DO.Size);

                    TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới: " + e.Message + "');</script>";
                }
            }
            return RedirectToAction("Index", "Newstaff", new { id = id });
        }

        public ActionResult Delete(int? id)
        {
            var idbhldx = db_context.BHLĐXuat.Where(x => x.IDBHLDX == id).Select(g => g.BDNVID).FirstOrDefault();
            try
            {
                db_context.BHLĐXuat_delete(id);
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Newstaff", new { id = idbhldx });
        }

        public ActionResult Edit(int? id)
        {
            var res = (from a in db_context.BHLĐXuat_searchByID(id)
                       select new ExportprotectionValidation
                       {
                           IDBHLDX = a.IDBHLDX,
                           BDNVID = (int)a.BHLDID,
                           BHLDID = (int)a.BHLDID,
                           Soluong = (int)a.Soluong,
                           Size = a.Size

                       }).ToList();
            ExportprotectionValidation DO = new ExportprotectionValidation();

            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDBHLDX = a.IDBHLDX;
                    DO.BDNVID = (int)a.BHLDID;
                    DO.BHLDID = (int)a.BHLDID;
                    DO.Soluong = (int)a.Soluong;
                    DO.Size = a.Size;

                }
                List<LoaiBHLD> bh = db_context.LoaiBHLDs.ToList();
                ViewBag.BHLDList = new SelectList(bh, "IDBHLD", "TenBHLD");
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }
        [HttpPost]
        public ActionResult Edit(ExportprotectionValidation _DO, int id)
        {
            var idbhldxedit = db_context.BHLĐXuat.Where(x => x.IDBHLDX == id).Select(g => g.BDNVID).FirstOrDefault();
            try
            {

                db_context.BHLĐXuat_update(_DO.IDBHLDX, _DO.BHLDID, _DO.Soluong, _DO.Size);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại " + e.Message + " ');</script>";
            }

            return RedirectToAction("Index", "Newstaff", new { id = idbhldxedit });
        }
    }
}