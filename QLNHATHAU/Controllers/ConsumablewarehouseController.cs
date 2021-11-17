using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class ConsumablewarehouseController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Consumablewarehouse
        public ActionResult Index(int? page, DateTime? begind, DateTime? endd)
        {
            var res = (from a in db_context.DinhBienTHs
                       join pb in db_context.PhongBans on a.PhongBanID equals pb.IDPhongBan
                       join ns in db_context.NhanViens on a.NhanSuID equals ns.ID
                       join vt in db_context.Vitris on a.VTLVID equals vt.IDViTri
                       join db in db_context.DinhBienBHLDTHs on a.DBTHID equals db.IDDBTH
                       join tt in db_context.TinhTrangNBHLDs on a.TinhTrangID equals tt.IDNBHLD
                       where (a.NgayXuat >= begind && a.NgayXuat <= endd)
                       select new ConsumptionValidation()
                       {
                           IDDBTH = a.IDDBTH,
                           NhanSuID = (int)a.NhanSuID,
                           MaNV = ns.MaNV,
                           HoTen = ns.HoTen,
                           DBTHID = (int)a.DBTHID,
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
            int pageSize = 20;
            int pageNumber = ((int)(page ?? 1));
            return View(res.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult _ToolSearch(ConsumptionValidation DO)
        {
            return PartialView(DO);
        }
        [HttpPost]
        public ActionResult Save(List<ConsumptionValidation> data)
        {
            try
            {
                foreach (var i in data)
                {
                    db_context.DinhBienTH_save(i.IDDBTH, i.TinhTrangID);

                    TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
                }

              
            }
            catch (Exception e)
            {
                TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới hợp đồng');</script>";
            }
            return RedirectToAction("Index", "Consumablewarehouse");
        }

        public ActionResult UpdateBulk(string data)
        {

            return PartialView();
        }
        [HttpPost]
        public ActionResult UpdateBulk(List<ConsumptionValidation> data, DateTime NgayXuat)
        {
            var NgayX = db_context.DinhBienTHs.Where(x => x.NgayXuat == NgayXuat).ToList();
            try
            {
                foreach (var i in NgayX)

                    db_context.DinhBienTH_saveAll(i.IDDBTH, 1);

                TempData["msgSuccess"] = "<script>alert('Cập nhật thành công');</script>";
            }
            catch
            {
                TempData["msgError"] = "<script>alert('Có lỗi khi cập nhật');</script>";
            }
            return RedirectToAction("Index", "Consumablewarehouse");
        }
    }
}