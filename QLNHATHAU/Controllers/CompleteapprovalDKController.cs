using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class CompleteapprovalDKController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: CompleteapprovalDK
        public ActionResult Index(int? page, int? NguoiDungID)
        {
            var res = (from a in db_context.PheDuyetBHLD_select(MyAuthentication.IDNguoidung).Where(x => x.TinhTrang == 1)
                       join th in db_context.DinhBienDKs on a.DBBHLDID equals th.IDDBDK
                       select new ApproveValidation()
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

            if (page == null) page = 1;
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(res.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Delete(int id)
        {
            db_context.PheDuyetBHLD_Delete(id);

            return RedirectToAction("Index", "CompleteapprovalDK");
        }
    }
}