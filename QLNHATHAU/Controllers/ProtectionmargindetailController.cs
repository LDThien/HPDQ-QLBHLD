using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class ProtectionmargindetailController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Protectionmargindetail
        public ActionResult Index(int? page, int? id)
        {

            var res = (from a in db_context.DinhBienChiTiets.Where(x => x.DBID == id)
                      join bhld in db_context.LoaiBHLDs on a.BHLDID equals bhld.IDBHLD
                      join db in db_context.DinhBienBHLDs on a.DBID equals db.IDDB
                       select new ProtectionmargindetailValidation()
                       {
                         IDDBCT = a.IDDBCT,
                         BHLDID =(int)a.BHLDID,
                         TenBHLD = bhld.TenBHLD,
                         DBID =(int)a.DBID,
                         TenVTBHLD = db.TenVTBHLD,
                         DVT = a.DVT,
                         SoLuong =(int)a.SoLuong,
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
            List<DinhBienBHLD> db = db_context.DinhBienBHLDs.Where(x => x.IDDB == id).ToList();
            ViewBag.DBList = new SelectList(db, "IDDB", "TenVTBHLD");

            List<LoaiBHLD> bh = db_context.LoaiBHLDs.ToList();
            ViewBag.BHLDList = new SelectList(bh, "IDBHLD", "TenBHLD");

            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(ProtectionmargindetailValidation _DO)
        {
            if (ModelState.IsValid)
            {
  
                try
                {

                    db_context.DinhBienChiTiet_insert(_DO.BHLDID, _DO.DBID, _DO.DVT, _DO.SoLuong, _DO.ThoiHan, _DO.GhiChu);

                    TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới: " + e.Message + "');</script>";
                }
            }
            return RedirectToAction("Index", "Protectionmargindetail", new { id = _DO.DBID });
        }
        public ActionResult Delete(int? id)
        {
            var idbhldx = db_context.DinhBienChiTiets.Where(x => x.IDDBCT == id).Select(g => g.DBID).FirstOrDefault();
            try
            {
                db_context.DinhBienChiTiet_delete(id);
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Protectionmargindetail", new { id = idbhldx });
        }
        public ActionResult Edit(int? id)
        {
            var res = (from a in db_context.DinhBienChiTiet_searchByID(id)
                       select new ProtectionmargindetailValidation
                       {
                           IDDBCT = a.IDDBCT,
                           BHLDID = (int)a.BHLDID,
                           DBID = (int)a.DBID,
                           DVT = a.DVT,
                           SoLuong = (int)a.SoLuong,
                           ThoiHan = (int)a.ThoiHan,
                           GhiChu = a.GhiChu

                       }).ToList();
            ProtectionmargindetailValidation DO = new ProtectionmargindetailValidation();

            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDDBCT = a.IDDBCT;
                    DO.BHLDID = (int)a.BHLDID;
                    DO.DBID = (int)a.DBID;
                    DO.DVT = a.DVT;
                    DO.SoLuong = (int)a.SoLuong;
                    DO.ThoiHan = (int)a.ThoiHan;
                    DO.GhiChu = a.GhiChu;

                }

                db_context.Configuration.ProxyCreationEnabled = false;
                List<DinhBienBHLD> db = db_context.DinhBienBHLDs.Where(x => x.IDDB == id).ToList();
                ViewBag.DBList = new SelectList(db, "IDDB", "TenVTBHLD");

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
        public ActionResult Edit(ProtectionmargindetailValidation _DO, int id)
        {
            var idbhldxedit = db_context.DinhBienChiTiets.Where(x => x.IDDBCT == id).Select(g => g.DBID).FirstOrDefault();
            try
            {

                db_context.DinhBienChiTiet_update(_DO.IDDBCT, _DO.BHLDID, _DO.DVT,_DO.SoLuong,_DO.ThoiHan, _DO.GhiChu);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại " + e.Message + " ');</script>";
            }

            return RedirectToAction("Index", "Protectionmargindetail", new { id = idbhldxedit });
        }
    }
}