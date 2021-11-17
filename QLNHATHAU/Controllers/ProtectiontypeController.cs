using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class ProtectiontypeController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Protectiontype
        public ActionResult Index(int? page)
        {
            var res = (from a in db_context.LoaiBHLDs
                       select new ProtectiontypeValidation()
                       {
                           IDBHLD = a.IDBHLD,
                           TenBHLD = a.TenBHLD
                       }).ToList();

            if (page == null) page = 1;
            int pageSize = 10;
            int pageNumber = ((int)(page ?? 1));
            return View(res.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(ProtectiontypeValidation _DO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var data = db_context.LoaiBHLD_insert(_DO.TenBHLD);
                    TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";

                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới công');</script>";
                }
            }

            return RedirectToAction("Index", "Protectiontype");
        }
        public ActionResult Delete(int id)
        {
            try
            {
                db_context.LoaiBHLD_delete(id);
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Protectiontype");
        }
        public ActionResult Edit(int id)
        {
            var res = (from a in db_context.LoaiBHLD_SearchByID(id)
                       select new ProtectiontypeValidation
                       {
                           IDBHLD = a.IDBHLD,
                           TenBHLD = a.TenBHLD
                       }).ToList();

            ProtectiontypeValidation DO = new ProtectiontypeValidation();
            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDBHLD = a.IDBHLD;
                    DO.TenBHLD = a.TenBHLD;
                }
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }
        [HttpPost]
        public ActionResult Edit(ProtectiontypeValidation _DO)
        {
            try
            {
                db_context.LoaiBHLD_update(_DO.IDBHLD, _DO.TenBHLD);
                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại');</script>";
            }

            return RedirectToAction("Index", "Protectiontype");
        }
    }
}