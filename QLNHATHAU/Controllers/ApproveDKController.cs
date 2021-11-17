using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class ApproveDKController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: ApproveDK
        public ActionResult NotApproved(int? page, int? IDNguoidung)
        {
            var res = (from a in db_context.PheDuyetBHLD_select(MyAuthentication.IDNguoidung).Where(x => x.TinhTrang == 0)
                       join th in db_context.DinhBienDKs on a.DBBHLDID equals th.IDDBDK 
                       select new ApproveValidation()
                       {
                           IDPD = a.IDPD,
                           NguoiDungID =(int)a.NguoiDungID,
                           DBBHLDID = th.IDDBDK,
                           MaNV = th.NhanVien.MaNV,
                           HoTen = th.NhanVien.HoTen,
                           TenVT = th.NhanVien.Vitri.TenViTri,
                           TenBHLDTVT = th.DinhBienBHLD.TenVTBHLD,
                           TenPB = th.NhanVien.PhongBan.TenPhongBan

                       }).ToList();

            if (page == null) page = 1;
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(res.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Delete(int id)
        {
            db_context.PheDuyetBHLD_Delete(id);

            return RedirectToAction("NotApproved", "ApproveDK");
        }
        public ActionResult Edit(int id)
        {
            var res = (from a in db_context.PheDuyetBHLD_searchByID(id)
                       join th in db_context.DinhBienDKs on a.DBBHLDID equals th.IDDBDK
                       select new ApproveValidation
                       {
                           IDPD = a.IDPD,
                           NguoiDungID = (int)a.NguoiDungID,
                           DBBHLDID = th.IDDBDK,
                           MaNV = th.NhanVien.MaNV,
                           HoTen = th.NhanVien.HoTen,
                           TenVT = th.NhanVien.Vitri.TenViTri,
                           TenBHLDTVT = th.DinhBienBHLD.TenVTBHLD,
                           TenPB = th.NhanVien.PhongBan.TenPhongBan,
                           TinhTrang = (int)a.TinhTrang,
                           GhiChu = a.GhiChu
                       }).ToList();

            ApproveValidation DO = new ApproveValidation();


            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDPD = a.IDPD;
                    DO.NguoiDungID = (int)a.NguoiDungID;
                    DO.DBBHLDID = (int)a.DBBHLDID;
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
        public ActionResult Edit(ApproveValidation _DO,int? id)
        {
           

            try
            {

                db_context.PheDuyetBHLD_Updata(_DO.IDPD, 1, _DO.GhiChu);

              
                TempData["msgSuccess"] = "<script>alert('Phê Duyệt Thành Công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Phê Duyệt Thất Bại: " + e.Message + "');</script>";
            }
            var pd = db_context.PheDuyetBHLDs.Where(x => x.IDPD == id).SingleOrDefault();

            var BHLD = db_context.PheDuyetBHLDs.Where(x => x.DBBHLDID == pd.DBBHLDID).ToList();

            foreach (var i in BHLD)

                if (i.CapPD == i.CapPD && i.TinhTrang == 1)
                {
                    db_context.DinhBienDK_UpdateTK(i.DBBHLDID, 2);
                }
                else
                {
                        db_context.DinhBienDK_UpdateTK(i.DBBHLDID, 1);

                }
            return RedirectToAction("NotApproved", "ApproveDK");
        }
    }
}