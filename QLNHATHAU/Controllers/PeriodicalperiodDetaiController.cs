using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class PeriodicalperiodDetaiController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: PeriodicalperiodDetai
        public ActionResult Index(int? page, int? id)
        {

            var res = (from a in db_context.ThoiHanBHLDDKCTs.Where(x => x.THDKID == id)
                       join dk in db_context.ThoiHanBHLDDKs on a.THDKID equals dk.IDTHDK
                       join bh in db_context.LoaiBHLDs on a.BHLDID equals bh.IDBHLD
                       select new PeriodicalperiodDetaiValidation()
                       {
                           IDTHDDKCT = a.IDTHDDKCT,
                           THDKID = (int)a.THDKID,
                           BHLDID = (int)a.BHLDID,
                           TenBHLD = bh.TenBHLD,
                           ThoiHan =(int)a.ThoiHan,
                           NgayNV = (DateTime)a.NgayNV
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
        public ActionResult Create(PeriodicalperiodDetaiValidation _DO, int id)
        {
            if (ModelState.IsValid)
            {
                var THDKID = id;
                try
                {

                    db_context.ThoiHanBHLDDKCT_insert(THDKID, _DO.BHLDID,_DO.ThoiHan,_DO.NgayNV);

                    TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới: " + e.Message + "');</script>";
                }
            }
            return RedirectToAction("Index", "PeriodicalperiodDetai", new { id = id });
        }
        public ActionResult Delete(int? id)
        {
            var idbhldx = db_context.ThoiHanBHLDDKCTs.Where(x => x.IDTHDDKCT == id).Select(g => g.THDKID).FirstOrDefault();
            try
            {
                db_context.ThoiHanBHLDDKCT_delete(id);
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "PeriodicalperiodDetai", new { id = idbhldx });
        }
        public ActionResult Edit(int? id)
        {
            var res = (from a in db_context.ThoiHanBHLDDKCT_searchByID(id)
                       select new PeriodicalperiodDetaiValidation
                       {
                           IDTHDDKCT = a.IDTHDDKCT,
                           THDKID = (int)a.THDKID,
                           BHLDID = (int)a.BHLDID,
                           ThoiHan = (int)a.ThoiHan,
                           NgayNV = (DateTime)a.NgayNV

                       }).ToList();
            PeriodicalperiodDetaiValidation DO = new PeriodicalperiodDetaiValidation();

            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDTHDDKCT = a.IDTHDDKCT;
                    DO.THDKID = (int)a.THDKID;
                    DO.BHLDID = (int)a.BHLDID;
                    DO.ThoiHan = (int)a.ThoiHan;
                    DO.NgayNV = (DateTime)a.NgayNV;

                }
                List<LoaiBHLD> bh = db_context.LoaiBHLDs.ToList();
                ViewBag.BHLDList = new SelectList(bh, "IDBHLD", "TenBHLD");

                ViewBag.NNV = DO.NgayNV.ToString("yyyy-MM-dd");
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }
        [HttpPost]
        public ActionResult Edit(PeriodicalperiodDetaiValidation _DO, int id)
        {
            var idbhldxedit = db_context.ThoiHanBHLDDKCTs.Where(x => x.IDTHDDKCT == id).Select(g => g.THDKID).FirstOrDefault();
            try
            {

                db_context.ThoiHanBHLDDKCT_update(_DO.IDTHDDKCT, _DO.BHLDID, _DO.ThoiHan, _DO.NgayNV);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại " + e.Message + " ');</script>";
            }

            return RedirectToAction("Index", "PeriodicalperiodDetai", new { id = idbhldxedit });
        }
    }
}