using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class PeriodicdetailsController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Periodicdetails
        public ActionResult Index(int? page, int? id)
        {

            var res = (from a in db_context.BHLDDKs.Where(x => x.DBDKID == id)
                       join db in db_context.DinhBienDKs on a.DBDKID equals db.IDDBDK
                       join bh in db_context.LoaiBHLDs on a.BHLDID equals bh.IDBHLD
                       select new PeriodicdetailsValidation()
                       {
                           IDBHLDDK = a.IDBHLDDK,
                           DBDKID = (int)a.DBDKID,
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
        public ActionResult Create(PeriodicdetailsValidation _DO, int id)
        {
            if (ModelState.IsValid)
            {
                var DBDKID = id;
                try
                {

                    db_context.BHLDDK_insert(DBDKID, _DO.BHLDID, _DO.Soluong, _DO.Size);

                    TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới: " + e.Message + "');</script>";
                }
            }
            return RedirectToAction("Index", "Periodicdetails", new { id = id });
        }
        public ActionResult Delete(int? id)
        {
            var idbhlddk = db_context.BHLDDKs.Where(x => x.IDBHLDDK == id).Select(g => g.DBDKID).FirstOrDefault();
            try
            {
                db_context.BHLDDK_delete(id);
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Periodicdetails", new { id = idbhlddk });
        }

        public ActionResult Edit(int? id)
        {
            var res = (from a in db_context.BHLDDK_searchByID(id)
                       select new PeriodicdetailsValidation
                       {
                           IDBHLDDK = a.IDBHLDDK,
                           DBDKID = (int)a.DBDKID,
                           BHLDID = (int)a.BHLDID,
                           Soluong = (int)a.Soluong,
                           Size = a.Size

                       }).ToList();
            PeriodicdetailsValidation DO = new PeriodicdetailsValidation();

            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDBHLDDK = a.IDBHLDDK;
                    DO.DBDKID = (int)a.DBDKID;
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
        public ActionResult Edit(PeriodicdetailsValidation _DO, int id)
        {
            var idbhldedit = db_context.BHLDDKs.Where(x => x.IDBHLDDK == id).Select(g => g.DBDKID).FirstOrDefault();
            try
            {

                db_context.BHLDDK_update(_DO.IDBHLDDK, _DO.BHLDID, _DO.Soluong, _DO.Size);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại " + e.Message + " ');</script>";
            }

            return RedirectToAction("Index", "Periodicdetails", new { id = idbhldedit });
        }
    }
}