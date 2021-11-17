using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class AccountController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Account
        public ActionResult Index( int? page, int? ID, int? IDNguoiDung)
        {
            List<NhanVien> nv1 = db_context.NhanViens.ToList();
            if (ID == null) ID = 0;
            ViewBag.NVList = new SelectList(nv1, "ID", "HoTen", ID);


            List<NguoiDung> nd = db_context.NguoiDungs.ToList();
            if (IDNguoiDung == null) IDNguoiDung = 0;
            ViewBag.NDList = new SelectList(nd, "IDNguoiDung", "TenDangNhap", IDNguoiDung);

            var res = (from a in db_context.NguoiDungs
                       join nv in db_context.NhanViens on a.NhanVienID equals nv.ID
                       join q in db_context.Quyens on a.IDQuyen equals q.IDQuyen
                       select new NguoiDungValidation()
                       {
                           IDNguoiDung = a.IDNguoiDung,
                           TenDangNhap = a.TenDangNhap,
                           NhanVienID =(int)a.NhanVienID,
                           MaNV = nv.MaNV,
                           HoTen = nv.HoTen,
                           TenPhongBan = a.NhanVien.PhongBan.TenPhongBan,
                           TenViTri = a.NhanVien.Vitri.TenViTri,
                           IDQuyen = (int)a.IDQuyen,
                           TenQuyen = q.TenQuyen
                       });
            if (ID != 0)
                res = res.Where(x => x.NhanVienID == ID);

            if (IDNguoiDung != 0)
                res = res.Where(x => x.IDNguoiDung == IDNguoiDung);
            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = ((int)(page ?? 1));
            return View(res.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Edit(int id)
        {
            var res = (from a in db_context.NguoiDung_SearchByID(id)
                       select new NguoiDungValidation
                       {
                           IDNguoiDung = a.IDNguoiDung,
                           TenDangNhap = a.TenDangNhap,
                           NhanVienID =(int)a.NhanVienID,
                           IDQuyen = (int)a.IDQuyen
                       }).ToList();
            NguoiDungValidation DO = new NguoiDungValidation();

            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDNguoiDung = a.IDNguoiDung;
                    DO.TenDangNhap = a.TenDangNhap;
                    DO.NhanVienID = (int)a.NhanVienID;
                    DO.IDQuyen = (int)a.IDQuyen;
                }

                List<NhanVien> nv = db_context.NhanViens.ToList();
                ViewBag.NVList = new SelectList(nv, "ID", "HoTen", DO.NhanVienID);

                List<Quyen> q = db_context.Quyens.ToList();
                ViewBag.QList = new SelectList(q, "IDQuyen", "TenQuyen", DO.IDQuyen);
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }
        [HttpPost]
        public ActionResult Edit(NguoiDungValidation _DO)
        {
            try
            {

                db_context.NguoiDung_updateTK(_DO.IDNguoiDung, _DO.TenDangNhap, _DO.NhanVienID,_DO.IDQuyen);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {

                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại " + e.Message + " ');</script>";
            }

            return RedirectToAction("Index", "Account");
        }
    }
}