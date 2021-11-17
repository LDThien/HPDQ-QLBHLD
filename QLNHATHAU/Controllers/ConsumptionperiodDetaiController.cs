using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class ConsumptionperiodDetaiController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: ConsumptionperiodDetai
        public ActionResult Index(int? page, int? id)
        {

            var res = (from a in db_context.ThoiHanBHLDTHCTs.Where(x => x.THTHID == id)
                       join th in db_context.ThoiHanBHLDTHs on a.THTHID equals th.IDTHTH
                       join bh in db_context.LoaiBHLDs on a.BHLDID equals bh.IDBHLD
                       select new ConsumptionperiodDetaiValidation()
                       {
                           IDTHTHCT = a.IDTHTHCT,
                           THTHID = (int)a.THTHID,
                           BHLDID = (int)a.BHLDID,
                           TenBHLD = bh.TenBHLD,
                           ThoiHan = (int)a.ThoiHan,
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
        public ActionResult Create(ConsumptionperiodDetaiValidation _DO, int id)
        {
            if (ModelState.IsValid)
            {
                var THTHID = id;
                try
                {

                    db_context.ThoiHanBHLDTHCT_insert(THTHID, _DO.BHLDID, _DO.ThoiHan, _DO.NgayNV);

                    TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới: " + e.Message + "');</script>";
                }
            }
            return RedirectToAction("Index", "ConsumptionperiodDetai", new { id = id });
        }
        public ActionResult Delete(int? id)
        {
            var idbhldx = db_context.ThoiHanBHLDTHCTs.Where(x => x.IDTHTHCT == id).Select(g => g.THTHID).FirstOrDefault();
            try
            {
                db_context.ThoiHanBHLDTHCT_delete(id);
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "ConsumptionperiodDetai", new { id = idbhldx });
        }
        public ActionResult Edit(int? id)
        {
            var res = (from a in db_context.ThoiHanBHLDTHCT_searchByID(id)
                       select new ConsumptionperiodDetaiValidation
                       {
                           IDTHTHCT = a.IDTHTHCT,
                           THTHID = (int)a.THTHID,
                           BHLDID = (int)a.BHLDID,
                           ThoiHan = (int)a.ThoiHan,
                           NgayNV = (DateTime)a.NgayNV

                       }).ToList();
            ConsumptionperiodDetaiValidation DO = new ConsumptionperiodDetaiValidation();

            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDTHTHCT = a.IDTHTHCT;
                    DO.THTHID = (int)a.THTHID;
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
        public ActionResult Edit(ConsumptionperiodDetaiValidation _DO, int id)
        {
            var idbhldxedit = db_context.ThoiHanBHLDTHCTs.Where(x => x.IDTHTHCT == id).Select(g => g.THTHID).FirstOrDefault();
            try
            {

                db_context.ThoiHanBHLDTHCT_update(_DO.IDTHTHCT, _DO.BHLDID, _DO.ThoiHan, _DO.NgayNV);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại " + e.Message + " ');</script>";
            }

            return RedirectToAction("Index", "ConsumptionperiodDetai", new { id = idbhldxedit });
        }
    }
}