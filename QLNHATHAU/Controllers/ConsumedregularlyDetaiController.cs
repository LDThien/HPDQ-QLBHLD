using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class ConsumedregularlyDetaiController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: ConsumedregularlyDetai
        public ActionResult Index(int? page, int? id)
        {

            var res = (from a in db_context.DinhBienBHLDTHCTs.Where(x => x.DBTHID == id)
                       join bhld in db_context.LoaiBHLDs on a.BHLDID equals bhld.IDBHLD
                       join db in db_context.DinhBienBHLDTHs on a.DBTHID equals db.IDDBTH
                       select new ConsumedregularlyDetaiValidation()
                       {
                           IDDBTHCT = a.IDDBTHCT,
                           BHLDID = (int)a.BHLDID,
                           TenBHLD = bhld.TenBHLD,
                           DBTHID = (int)a.DBTHID,
                           TenVTBHLD = db.TenVTBHLD,
                           DVT = a.DVT,
                           SoLuong = (int)a.SoLuong,
                           ThoiHan = (int)a.ThoiHan,
                           GhiChu = a.GhiChu
                       });

            if (page == null) page = 1;
            int pageSize = 10;
            int pageNumber = (int)(page ?? 1);
            return View(res.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create(int? id)
        {
            db_context.Configuration.ProxyCreationEnabled = false;
            List<DinhBienBHLDTH> db = db_context.DinhBienBHLDTHs.Where(x => x.IDDBTH == id).ToList();
            ViewBag.DBList = new SelectList(db, "IDDBTH", "TenVTBHLD");

            List<LoaiBHLD> bh = db_context.LoaiBHLDs.ToList();
            ViewBag.BHLDList = new SelectList(bh, "IDBHLD", "TenBHLD");

            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(ConsumedregularlyDetaiValidation _DO)
        {
            if (ModelState.IsValid)
            {

                try
                {

                    db_context.DinhBienBHLDTHCT_insert(_DO.BHLDID, _DO.DBTHID, _DO.DVT, _DO.SoLuong, _DO.ThoiHan, _DO.GhiChu);

                    TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới: " + e.Message + "');</script>";
                }
            }
            return RedirectToAction("Index", "ConsumedregularlyDetai", new { id = _DO.DBTHID });
        }
        public ActionResult Delete(int? id)
        {
            var idbhldth = db_context.DinhBienBHLDTHCTs.Where(x => x.IDDBTHCT == id).Select(g => g.DBTHID).FirstOrDefault();
            try
            {
                db_context.DinhBienBHLDTHCT_delete(id);
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "ConsumedregularlyDetai", new { id = idbhldth });
        }
        public ActionResult Edit(int? id)
        {
            var res = (from a in db_context.DinhBienBHLDTHCT_searchByID(id)
                       select new ConsumedregularlyDetaiValidation
                       {
                           IDDBTHCT = a.IDDBTHCT,
                           BHLDID = (int)a.BHLDID,
                           DBTHID = (int)a.DBTHID,
                           DVT = a.DVT,
                           SoLuong = (int)a.SoLuong,
                           ThoiHan = (int)a.ThoiHan,
                           GhiChu = a.GhiChu

                       }).ToList();
            ConsumedregularlyDetaiValidation DO = new ConsumedregularlyDetaiValidation();

            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDDBTHCT = a.IDDBTHCT;
                    DO.BHLDID = (int)a.BHLDID;
                    DO.DBTHID = (int)a.DBTHID;
                    DO.DVT = a.DVT;
                    DO.SoLuong = (int)a.SoLuong;
                    DO.ThoiHan = (int)a.ThoiHan;
                    DO.GhiChu = a.GhiChu;

                }

                db_context.Configuration.ProxyCreationEnabled = false;
                List<DinhBienBHLDTH> db = db_context.DinhBienBHLDTHs.Where(x => x.IDDBTH == id).ToList();
                ViewBag.DBList = new SelectList(db, "IDDBTH", "TenVTBHLD");

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
        public ActionResult Edit(ConsumedregularlyDetaiValidation _DO, int id)
        {
            var idbhldthedit = db_context.DinhBienBHLDTHCTs.Where(x => x.IDDBTHCT == id).Select(g => g.DBTHID).FirstOrDefault();
            try
            {

                db_context.DinhBienBHLDTHCT_update(_DO.IDDBTHCT, _DO.BHLDID, _DO.DVT, _DO.SoLuong, _DO.ThoiHan, _DO.GhiChu);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại " + e.Message + " ');</script>";
            }

            return RedirectToAction("Index", "ConsumedregularlyDetai", new { id = idbhldthedit });
        }
    }
}