using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class QuyenController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Quyen
        public ActionResult Index(int? page, int? id)
        {
            var res = from a in db_context.Quyen_select()
                select new QuyenValidation
                {
                    IDQuyen = a.IDQuyen,
                    TenQuyen = a.TenQuyen
                };

            if (page == null) page = 1;
            int pageSize = Common.ConfigStatic.pageSize;
            int pageNumber = (page ?? 1);

            return View(res.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: Create Quyen
        public ActionResult Create()
        {

            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(QuyenValidation _DO)
        {
           
                try
                {
                    var data = db_context.Quyen_insert(_DO.TenQuyen);
                    TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";

                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới quyền');</script>";
                }
     

            return RedirectToAction("Index", "Quyen");
        }

        public ActionResult Edit(int id)
        {
            var res = (from q in db_context.Quyen_SearchByID(id)
                        select new QuyenValidation
                        {
                            IDQuyen=q.IDQuyen,
                            TenQuyen = q.TenQuyen,
                        }).ToList();

            QuyenValidation DO = new QuyenValidation();
            if (res.Count > 0)
            {
                foreach (var nv in res)
                {
                    DO.IDQuyen = nv.IDQuyen;
                    DO.TenQuyen = nv.TenQuyen;
                }
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }

        [HttpPost]
        public ActionResult Edit(QuyenValidation _DO)
        {
            try
            {
                db_context.Quyen_update(_DO.IDQuyen, _DO.TenQuyen);
                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại');</script>";
            }

            return RedirectToAction("Index", "Quyen");
        }

        public ActionResult Delete(int id)
        {
            try
            {
                db_context.Quyen_delete(id);
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Quyen");
        }
    }
}