using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class ApproveTHController : Controller
    { 
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: ApproveTH
        public ActionResult Index(int? page, int? NguoiDungID)
        {
            var res = (from a in db_context.PheDuyetTH_select(MyAuthentication.IDNguoidung).Where(x => x.TinhTrang == 0)
                       join th in db_context.DinhBienTHs on a.BHLDTHID equals th.IDDBTH
                       select new ApproveTHValidation()
                       {
                           IDPDTH = a.IDPDTH,
                           NguoiDungID = (int)a.NguoiDungID,
                           BHLDTHID = th.IDDBTH,
                           MaNV = th.NhanVien.MaNV,
                           HoTen = th.NhanVien.HoTen,
                           TenVT = th.NhanVien.Vitri.TenViTri,
                           TenBHLDTVT = th.DinhBienBHLDTH.TenVTBHLD,
                           TenPB = th.NhanVien.PhongBan.TenPhongBan

                       }).ToList();

            if (page == null) page = 1;
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(res.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Delete(int id)
        {
            db_context.PheDuyetTH_Delete(id);

            return RedirectToAction("Index", "ApproveTH");
        }
        public ActionResult Edit(int id)
        {
            var res = (from a in db_context.PheDuyetTH_select(MyAuthentication.IDNguoidung).Where(x => x.TinhTrang == 0)
                       join th in db_context.DinhBienTHs on a.BHLDTHID equals th.IDDBTH
                       select new ApproveTHValidation()
                       {
                           IDPDTH = a.IDPDTH,
                           NguoiDungID = (int)a.NguoiDungID,
                           BHLDTHID = th.IDDBTH,
                           MaNV = th.NhanVien.MaNV,
                           HoTen = th.NhanVien.HoTen,
                           TenVT = th.NhanVien.Vitri.TenViTri,
                           TenBHLDTVT = th.DinhBienBHLDTH.TenVTBHLD,
                           TenPB = th.NhanVien.PhongBan.TenPhongBan,
                           TinhTrang = (int)a.TinhTrang,
                           GhiChu = a.GhiChu,
                       }).ToList();

            ApproveTHValidation DO = new ApproveTHValidation();
            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDPDTH = a.IDPDTH;
                    DO.NguoiDungID = (int)a.NguoiDungID;
                    DO.BHLDTHID = (int)a.BHLDTHID;
                    DO.GhiChu = a.GhiChu;
                    DO.TinhTrang = a.TinhTrang;


                }
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }
        [HttpPost]
        public ActionResult Edit(ApproveTHValidation _DO, int? id)
        {
            try
            {
                db_context.PheDuyetTH_Updata(_DO.IDPDTH, 1, _DO.GhiChu);

                TempData["msgSuccess"] = "<script>alert('Phê Duyệt Thành Công');</script>";

            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Phê Duyệt Thất Bại: " + e.Message + "');</script>";
            }
            var pd = db_context.PheDuyetTHs.Where(x => x.IDPDTH == id).SingleOrDefault();

            var BHLD = db_context.PheDuyetTHs.Where(x => x.BHLDTHID == pd.BHLDTHID).ToList();

            foreach (var i in BHLD)

                if (i.CapPD == i.CapPD && i.TinhTrang == 1)
                {
                    db_context.DinhBienTH_UpdateTK(i.BHLDTHID, 2);
                }
                else
                {
                    db_context.DinhBienTH_UpdateTK(i.BHLDTHID, 1);

                }
            return RedirectToAction("Index", "ApproveTH");
        }
    }
}