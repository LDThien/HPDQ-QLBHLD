using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class ConsumabledetailsController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Consumabledetails
        public ActionResult Index(int? page, int? id)
        {

            var res = (from a in db_context.BHLDTHs.Where(x => x.DBTHID == id)
                       join db in db_context.DinhBienTHs on a.DBTHID equals db.IDDBTH
                       join bh in db_context.LoaiBHLDs on a.BHLDID equals bh.IDBHLD
                       select new ConsumabledetailsValidation()
                       {
                           IDBHLDTH = a.IDBHLDTH,
                           DBTHID = (int)a.DBTHID,
                           BHLDID = (int)a.BHLDID,
                           TenBHLD = bh.TenBHLD,
                           Soluong = (int)a.Soluong
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
        public ActionResult Create(ConsumabledetailsValidation _DO, int id)
        {
            if (ModelState.IsValid)
            {
                var DBTHID = id;
                try
                {

                    db_context.BHLDTH_insert(DBTHID, _DO.BHLDID, _DO.Soluong);

                    TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới: " + e.Message + "');</script>";
                }
            }
            return RedirectToAction("Index", "Consumabledetails", new { id = id });
        }
        public ActionResult Delete(int? id)
        {
            var idbhldx = db_context.BHLDTHs.Where(x => x.IDBHLDTH == id).Select(g => g.DBTHID).FirstOrDefault();
            try
            {
                db_context.BHLDTH_delete(id);
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Consumabledetails", new { id = idbhldx });
        }
        public ActionResult Edit(int? id)
        {
            var res = (from a in db_context.BHLDTH_searchByID(id)
                       select new ConsumabledetailsValidation
                       {
                           IDBHLDTH = a.IDBHLDTH,
                           DBTHID = (int)a.DBTHID,
                           BHLDID = (int)a.BHLDID,
                           Soluong = (int)a.Soluong

                       }).ToList();
            ConsumabledetailsValidation DO = new ConsumabledetailsValidation();

            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDBHLDTH = a.IDBHLDTH;
                    DO.DBTHID = (int)a.DBTHID;
                    DO.BHLDID = (int)a.BHLDID;
                    DO.Soluong = (int)a.Soluong;

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
        public ActionResult Edit(ConsumabledetailsValidation _DO, int id)
        {
            var idbhldxedit = db_context.BHLDTHs.Where(x => x.IDBHLDTH == id).Select(g => g.DBTHID).FirstOrDefault();
            try
            {

                db_context.BHLDTH_update(_DO.IDBHLDTH, _DO.BHLDID, _DO.Soluong);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại " + e.Message + " ');</script>";
            }

            return RedirectToAction("Index", "Consumabledetails", new { id = idbhldxedit });
        }

    }
}