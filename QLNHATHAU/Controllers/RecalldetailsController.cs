using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class RecalldetailsController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Recalldetails
        public ActionResult Index(int? page, int? id)
        {

            var res = (from a in db_context.ThuHoiBHLDCTs.Where(x => x.ThuHoiID == id)
                       join db in db_context.ThuHoiBHLDs on a.ThuHoiID equals db.IDThuHoi
                       join bh in db_context.LoaiBHLDs on a.BHLDID equals bh.IDBHLD
                       join tt in db_context.TinhTrangTHs on a.TinhTrangID equals tt.IDTTThuHoi
                       select new RecalldetailsValidation()
                       {
                           IDTHCT = a.IDTHCT,
                           ThuHoiID = (int)a.ThuHoiID,
                           BHLDID = (int)a.BHLDID,
                           TenBHLD = bh.TenBHLD,
                           SoLuong = (int)a.SoLuong,
                           Size = a.Size,
                           TinhTrangID = (int)a.TinhTrangID,
                           TenTinhTrang = tt.TenTTThuHoi
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

            List<TinhTrangTH> tt = db_context.TinhTrangTHs.ToList();
            ViewBag.TTHTList = new SelectList(tt, "IDTTThuHoi", "TenTTThuHoi");

            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(RecalldetailsValidation _DO, int id)
        {
            if (ModelState.IsValid)
            {
                var ThuHoiID = id;
                try
                {

                    db_context.ThuHoiBHLDCT_insert(ThuHoiID, _DO.BHLDID, _DO.SoLuong, _DO.Size, _DO.TinhTrangID);

                    TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới: " + e.Message + "');</script>";
                }
            }
            return RedirectToAction("Index", "Recalldetails", new { id = id });
        }
        public ActionResult Delete(int? id)
        {
            var idbhldx = db_context.ThuHoiBHLDCTs.Where(x => x.IDTHCT == id).Select(g => g.ThuHoiID).FirstOrDefault();
            try
            {
                db_context.ThuHoiBHLDCT_delete(id);
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Recalldetails", new { id = idbhldx });
        }
        public ActionResult Edit(int? id)
        {
            var res = (from a in db_context.ThuHoiBHLDCT_searchByID(id)
                       select new RecalldetailsValidation
                       {
                           IDTHCT = a.IDTHCT,
                           ThuHoiID = (int)a.ThuHoiID,
                           BHLDID = (int)a.BHLDID,
                           SoLuong = (int)a.SoLuong,
                           Size = a.Size,
                           TinhTrangID = (int)a.TinhTrangID

                       }).ToList();
            RecalldetailsValidation DO = new RecalldetailsValidation();

            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDTHCT = a.IDTHCT;
                    DO.ThuHoiID = (int)a.ThuHoiID;
                    DO.BHLDID = (int)a.BHLDID;
                    DO.SoLuong = (int)a.SoLuong;
                    DO.Size = a.Size;
                    DO.TinhTrangID = (int)a.TinhTrangID;

                }
                List<LoaiBHLD> bh = db_context.LoaiBHLDs.ToList();
                ViewBag.BHLDList = new SelectList(bh, "IDBHLD", "TenBHLD");

                List<TinhTrangTH> tt = db_context.TinhTrangTHs.ToList();
                ViewBag.TTHTList = new SelectList(tt, "IDTTThuHoi", "TenTTThuHoi");
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }
        [HttpPost]
        public ActionResult Edit(RecalldetailsValidation _DO, int id)
        {
            var idbhldxedit = db_context.ThuHoiBHLDCTs.Where(x => x.IDTHCT == id).Select(g => g.ThuHoiID).FirstOrDefault();
            try
            {

                db_context.ThuHoiBHLDCT_update(_DO.IDTHCT, _DO.BHLDID, _DO.SoLuong, _DO.Size, _DO.TinhTrangID);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại " + e.Message + " ');</script>";
            }

            return RedirectToAction("Index", "Recalldetails", new { id = idbhldxedit });
        }
    }
}