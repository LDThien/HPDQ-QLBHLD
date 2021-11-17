using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class PermissionController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Permission
        public ActionResult Index(int? page)
        {
            ViewBag.PermissionList = db_context.PhanQuyen_NDSelectAll().ToList()
                .GroupBy(x => x.TenDangNhap)
                .ToDictionary(x => x.Key, x => x.Select(e => e.IDQuyen).ToList());
            List<Quyen> q = db_context.Quyens.ToList();
            List<int> IdQuyens = new List<int>();
            foreach(var i in q)
            {
                IdQuyens.Add(i.IDQuyen);
            }
            ViewBag.QuyenList = IdQuyens;
            ViewBag.QuyenListFull = new SelectList(q, "IDQuyen", "TenQuyen");

            return View();
        }

        //[ActionName("SaveData")]
        [HttpPost]
        public ActionResult SaveData(List<ModelT> arrPerr)
        {
            try
            {
                if (arrPerr != null)
                {
                    foreach (var i in arrPerr)
                    {
                        if (i.IdQuyen != null)
                        {
                            var convertToArr = i.IdQuyen.Split(',');
                            ObjectParameter returnId = new ObjectParameter("IDNguoiDung", typeof(int));
                            db_context.NguoiDung_DeletePermission(i.userName, returnId);
                            int IDNguoiDung = Convert.ToInt32(returnId.Value);
                            foreach (var x in convertToArr)
                            {
                                db_context.PhanQuyen_insert(IDNguoiDung, Int32.Parse(x));
                            }
                        }
                    }
                    TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
                }
            }
            catch(Exception e)
            {
                TempData["msgError"] = "<script>alert('Có lỗi : "+e.Message+"');</script>";
            }

            return RedirectToAction("Index", "Permission");
        }

        //[HttpPost]
        //public ActionResult Create(int? page)
        //{
        //    return View();
        //}
    }
}