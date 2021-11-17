using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class RecallController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Recall
        public ActionResult Index(int? page, int? IDPhongBan, int? ID, int? IDViTri)
        {
            List<PhongBan> pb1 = db_context.PhongBans.ToList();
            if (IDPhongBan == null) IDPhongBan = 0;
            ViewBag.PBList = new SelectList(pb1, "IDPhongBan", "TenPhongBan", IDPhongBan);

            List<NhanVien> nv1 = db_context.NhanViens.ToList();
            if (ID == null) ID = 0;
            ViewBag.NVList = new SelectList(nv1, "ID", "HoTen", ID);

            List<Vitri> vt1 = db_context.Vitris.ToList();
            if (IDViTri == null) IDViTri = 0;
            ViewBag.VTList = new SelectList(vt1, "IDViTri", "TenViTri", IDViTri);

            var res = (from a in db_context.ThuHoiBHLDs
                       join pb in db_context.PhongBans on a.PhongBanID equals pb.IDPhongBan
                       join ns in db_context.NhanViens on a.NhanSuID equals ns.ID
                       join vt in db_context.Vitris on a.VTLVID equals vt.IDViTri
                       select new RecallValidation()
                       {
                           IDThuHoi = a.IDThuHoi,
                           NhanSuID = (int)a.NhanSuID,
                           MaNV = ns.MaNV,
                           HoTen = ns.HoTen,
                           PhongBanID = (int)a.PhongBanID,
                           TenPhongBan = pb.TenPhongBan,
                           VTLVID = (int)a.VTLVID,
                           TenVTLV = vt.TenViTri,
                           NgayTH = (DateTime)a.NgayTH
                       });

            if (IDPhongBan != 0)
                res = res.Where(x => x.PhongBanID == IDPhongBan);

            if (ID != 0)
                res = res.Where(x => x.NhanSuID == ID);

            if (IDViTri != 0)
                res = res.Where(x => x.VTLVID == IDViTri);

            if (page == null) page = 1;
            int pageSize = 10;
            int pageNumber = ((int)(page ?? 1));
            return View(res.ToList().ToPagedList(pageNumber, pageSize));
        }
        // get phòng ban vị trí theo nhân viên
        public JsonResult GetNhanvien(string search)
        {
            var result = (from a in db_context.NhanVien_search_thuhoi(search)
                          select new RecallValidation
                          {
                              NhanSuID = a.ID,
                              MaNV = a.MaNV,
                              HoTen = a.HoTen,
                              PhongBanID = (int)a.IDPhongBan,
                              VTLVID = (int)a.IDViTri,
                          }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create()
        {
            List<PhongBan> pb = db_context.PhongBans.ToList();
            ViewBag.PBList = new SelectList(pb, "IDPhongBan", "TenPhongBan");

            List<NhanVien> ns = db_context.NhanViens.ToList();
            ViewBag.NSList = new SelectList(ns, "ID", "HoTen");

            List<Vitri> vt = db_context.Vitris.ToList();
            ViewBag.VTList = new SelectList(vt, "IDViTri", "TenViTri");

            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(RecallValidation _DO)
        {
            try
            {
                var nv = db_context.NhanViens.Where(x => x.MaNV == _DO.MaNV).SingleOrDefault();
                var idnv = db_context.NhanViens.Where(x => x.ID == nv.ID).SingleOrDefault();
                if (nv != null)
                {
                    var vt = db_context.Vitris.Where(x => x.IDViTri == nv.IDViTri).SingleOrDefault();
                    if (vt != null)
                    {
                        var pb = db_context.PhongBans.Where(x => x.IDPhongBan == nv.IDPhongBan).SingleOrDefault();
                        if (pb != null)
                        {
                            db_context.ThuHoiBHLD_insert(nv.ID, nv.IDPhongBan, nv.IDViTri, _DO.NgayTH);

                            TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
                        }
                    }
                }

            }
            catch (Exception e)
            {
                TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới: " + e.Message + "');</script>";
            }

            return RedirectToAction("Index", "Recall");
        }
        public JsonResult GetNhanSu(int id)
        {
            db_context.Configuration.ProxyCreationEnabled = false;
            List<NhanVien> gns = db_context.NhanViens.Where(x => x.IDPhongBan == id && x.IDTinhTrangLV == 0).ToList();
            return Json(gns, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                db_context.ThuHoiBHLD_delete(id);
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Recall");
        }
        public ActionResult Edit(int id)
        {
            var res = (from a in db_context.ThuHoiBHLD_searchByID(id)
                       select new RecallValidation
                       {
                           IDThuHoi = a.IDThuHoi,
                           NhanSuID = (int)a.NhanSuID,
                           PhongBanID = (int)a.PhongBanID,
                           VTLVID = (int)a.VTLVID,
                           NgayTH = (DateTime)a.NgayTH
                       }).ToList();
            RecallValidation DO = new RecallValidation();

            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDThuHoi = a.IDThuHoi;
                    DO.NhanSuID = (int)a.NhanSuID;
                    DO.PhongBanID = (int)a.PhongBanID;
                    DO.VTLVID = (int)a.VTLVID;
                    DO.NgayTH = (DateTime)a.NgayTH;
                }

                List<PhongBan> pb = db_context.PhongBans.ToList();
                ViewBag.PhongBanID = new SelectList(pb, "IDPhongBan", "TenPhongBan", DO.PhongBanID);

                List<NhanVien> ns = db_context.NhanViens.Where(x => x.IDPhongBan == DO.PhongBanID).ToList();
                ViewBag.NhanSuID = new SelectList(ns, "ID", "HoTen", DO.NhanSuID);

                List<Vitri> vt = db_context.Vitris.ToList();
                ViewBag.VTLVID = new SelectList(vt, "IDViTri", "TenViTri", DO.VTLVID);

                ViewBag.NTHBHLD = DO.NgayTH.ToString("yyyy-MM-dd");

            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }
        [HttpPost]
        public ActionResult Edit(RecallValidation _DO)
        {
            try
            {

                db_context.ThuHoiBHLD_update(_DO.IDThuHoi, _DO.NhanSuID, _DO.PhongBanID, _DO.VTLVID, _DO.NgayTH);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {

                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại " + e.Message + " ');</script>";
            }

            return RedirectToAction("Index", "Recall");
        }
    }
}