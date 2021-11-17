using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class PeriodicwarehouseController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Periodicwarehouse
        public ActionResult Index(int? page, DateTime? begind, DateTime? endd)
        {
            var res = (from a in db_context.DinhBienDKs/*.Where(x => x.PheDuyetID == 2)*/
                       join pb in db_context.PhongBans on a.PhongBanID equals pb.IDPhongBan
                       join ns in db_context.NhanViens on a.NhanSuID equals ns.ID
                       join vt in db_context.Vitris on a.VTLVID equals vt.IDViTri
                       join db in db_context.DinhBienBHLDs on a.DBID equals db.IDDB
                       join tt in db_context.TinhTrangNBHLDs on a.TinhTrangID equals tt.IDNBHLD
                       where (a.NgayXuat >= begind && a.NgayXuat <= endd )
                       select new DemarcationValidation()
                       {
                           IDDBDK = a.IDDBDK,
                           NhanSuID = (int)a.NhanSuID,
                           MaNV = ns.MaNV,
                           HoTen = ns.HoTen,
                           DBID = (int)a.DBID,
                           TenVTBHLD = db.TenVTBHLD,
                           PhongBanID = (int)a.PhongBanID,
                           TenPhongBan = pb.TenPhongBan,
                           VTLVID = (int)a.VTLVID,
                           TenVTLV = vt.TenViTri,
                           NgayXuat = (DateTime)a.NgayXuat,
                           TinhTrangID = (int)a.TinhTrangID,
                           TenTT = tt.TenTT
                       }).OrderByDescending(x => x.NgayXuat).ToList();

            if (page == null) page = 1;
            int pageSize = 10;
            int pageNumber = ((int)(page ?? 1));
            return View(res.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult _ToolSearch(DemarcationValidation DO)
        {
            return PartialView(DO);
        }
        [HttpPost]
        public ActionResult Save(List<DemarcationValidation> data)
        {
            try
            {
                foreach (var i in data)
                {
                    db_context.DinhBienDK_save(i.IDDBDK, i.TinhTrangID);

                    TempData["msgSuccess"] = "<script>alert('Cập nhật thành công');</script>";
                } 
            }
            catch (Exception e)
            {
                TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới hợp đồng');</script>";
            }
            return RedirectToAction("Index", "Periodicwarehouse");
        }
        public ActionResult UpdateBulk(string data)
        {

            return PartialView();
        }
        [HttpPost]
        public ActionResult UpdateBulk(List<DemarcationValidation> data, DateTime NgayXuat)
        {
            var NgayX = db_context.DinhBienDKs.Where(x => x.NgayXuat == NgayXuat).ToList();
            try
            {
                foreach (var i in NgayX)

                    db_context.DinhBienDK_saveALL(i.IDDBDK, 1);

                TempData["msgSuccess"] = "<script>alert('Cập nhật thành công');</script>";
            }
            catch
            {
                TempData["msgError"] = "<script>alert('Có lỗi khi cập nhật');</script>";
            }
            return RedirectToAction("Index", "Periodicwarehouse");
        }
    }
}