using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class CompleteapprovalTHController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: CompleteapprovalTH
        public ActionResult Index(int? page, int? NguoiDungID)
        {
            var res = (from a in db_context.PheDuyetTH_select(MyAuthentication.IDNguoidung).Where(x => x.TinhTrang == 1)
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
                           GhiChu = a.GhiChu

                       }).ToList();

            if (page == null) page = 1;
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(res.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Delete(int id)
        {
            db_context.PheDuyetTH_Delete(id);

            return RedirectToAction("Index", "CompleteapprovalTH");
        }
    }
}