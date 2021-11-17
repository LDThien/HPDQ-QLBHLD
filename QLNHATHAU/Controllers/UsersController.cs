//using PagedList;
//using QLNHATHAU.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;
//using System.Data;
//using System.Data.Entity.Core.Objects;

//namespace QLNHATHAU.Controllers
//{
//    public class UsersController : Controller
//    {
//        GET: Users
//       QLNhaThauEntities db_context = new QLNhaThauEntities();
//        GET: NguoiDung
//        public ActionResult Index(int? page)
//        {
//            var dataList = (from a in db_context.NguoiDungs
//                            join d in db_context.NhanViens on a.NhanVienID equals d.IDNhanVien
//                            join b in db_context.PhongBans on a.PhongBanID equals b.IDPhongBan
//                            select new NguoiDungValidation()
//                            {
//                                IDNguoiDung = a.IDNguoiDung,
//                                TenDangNhap = a.TenDangNhap,
//                                Email = a.Email,
//                                TenNV = d.TenNV,
//                                MaNV = d.MaNV,
//                                Ten = b.TenDai
//                            }).ToList();

//            if (page == null) page = 1;
//            int pageSize = Common.ConfigStatic.pageSize;
//            int pageNumber = (page ?? 1);

//            return View(dataList.ToPagedList(pageNumber, pageSize));
//        }

//        GET: Create Quyen
//        public ActionResult Create()
//        {
//            List<Quyen> pq = db_context.Quyens.ToList();
//            ViewBag.PQList = new SelectList(pq, "IDQuyen", "TenQuyen");

//            List<NhanVien> nv = db_context.NhanViens.ToList();
//            ViewBag.NVList = new SelectList(nv, "IDNhanVien", "MaNV");

//            List<PhongBan> pb = db_context.PhongBans.ToList();
//            ViewBag.PBList = new SelectList(pb, "IDPhongBan", "TenDai");

//            return PartialView();
//        }

//        [HttpPost]
//        public ActionResult Create(NguoiDungValidation _DO)
//        {
//            bool flag = true;
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    if (flag)
//                    {
//                        ObjectParameter returnId = new ObjectParameter("IdNguoiDung", typeof(int));
//                        db_context.NguoiDung_insert(_DO.TenDangNhap, _DO.MatKhau, _DO.Email, _DO.PhongBanID, _DO.NhanVienID, returnId);
//                        List<Quyen> pq = db_context.Quyens.ToList();
//                        int id = Convert.ToInt32(returnId.Value);
//                        foreach (var q in pq)
//                        {
//                            db_context.PhanQuyen_insert(id, q.IDQuyen);
//                        }
//                        flag = false;
//                    }

//                    TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";

//                }
//                catch (Exception e)
//                {
//                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới quyền: " + e.Message + "');</script>";
//                }
//            }

//            return RedirectToAction("Index", "Users");
//        }

//        public ActionResult Edit(int id)
//        {
//            List<Quyen> pq = db_context.Quyens.ToList();
//            ViewBag.PList = new SelectList(pq, "IDQuyen", "TenQuyen");

//            List<NhanVien> emp = db_context.NhanViens.ToList();
//            ViewBag.NVList = new SelectList(emp, "IDNhanVien", "MaNV");

//            List<PhongBan> pb = db_context.PhongBans.ToList();
//            ViewBag.PBList = new SelectList(pb, "IDPhongBan", "TenDai");

//            var res = (from q in db_context.NguoiDung_SearchByID(id)
//                       select new NguoiDungValidation
//                       {
//                           IDNguoiDung = q.IDNguoiDung,
//                           TenDangNhap = q.TenDangNhap,
//                           MatKhau = q.MatKhau,
//                           Email = q.Email,
//                           PhongBanID = (int)q.PhongBanID,
//                           NhanVienID = (int)q.NhanVienID
//                       }).ToList();
//            NguoiDungValidation DO = new NguoiDungValidation();
//            int idNguoiDung = res[0].IDNguoiDung;
//            ViewBag.PQList = db_context.NguoiDung_Get_PhanQuyen(idNguoiDung).ToList();
//            if (res.Count > 0)
//            {
//                foreach (var nv in res)
//                {
//                    DO.IDNguoiDung = nv.IDNguoiDung;
//                    DO.TenDangNhap = nv.TenDangNhap;
//                    DO.MatKhau = nv.MatKhau;
//                    DO.Email = nv.Email;
//                    DO.PhongBanID = nv.PhongBanID;
//                    DO.NhanVienID = nv.NhanVienID;
//                }
//            }
//            else
//            {
//                return HttpNotFound();
//            }
//            return PartialView(DO);
//        }

//        [HttpPost]
//        public ActionResult Edit(NguoiDungValidation _DO)
//        {
//            try
//            {

//                db_context.NguoiDung_update(_DO.IDNguoiDung, _DO.TenDangNhap, _DO.MatKhau, _DO.Email, _DO.PhongBanID, _DO.NhanVienID);
//                var dsQuyenMoi = _DO.IDQuyen[0].Remove(_DO.IDQuyen[0].Length - 1, 1).Split(',');
//                db_context.Delete_PhanQyen(_DO.IDNguoiDung);
//                foreach (string q in dsQuyenMoi)
//                {
//                    db_context.PhanQuyen_insert(_DO.IDNguoiDung, Int32.Parse(q));
//                }
//                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
//            }
//            catch (Exception e)
//            {
//                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại:" + e.Message + "');</script>";
//            }

//            return RedirectToAction("Index", "Users");
//        }

//        public ActionResult Delete(int id)
//        {
//            var NguoiDung = from a in db_context.NguoiDungs
//                            join x in db_context.PhanQuyens
//                             on a.IDNguoiDung equals x.NguoiDungID
//                            where id == a.IDNguoiDung
//                            select a;
//            db_context.NguoiDung_delete(id);

//            return RedirectToAction("Index", "Users");
//        }

//        public ActionResult InsertPermission(int id)
//        {
//            List<Quyen> pq = db_context.Quyens.ToList();
//            ViewBag.PList = new SelectList(pq, "IDQuyen", "TenQuyen");

//            List<NhanVien> emp = db_context.NhanViens.ToList();
//            ViewBag.NVList = new SelectList(emp, "IDNhanVien", "MaNV");

//            List<PhongBan> pb = db_context.PhongBans.ToList();
//            ViewBag.PBList = new SelectList(pb, "IDPhongBan", "Ten");

//            var res = (from q in db_context.NguoiDung_SearchByID(id)
//                       select new NguoiDungValidation
//                       {
//                           IDNguoiDung = q.IDNguoiDung,
//                           TenDangNhap = q.TenDangNhap,
//                           Email = q.Email,
//                           PhongBanID = (int)q.PhongBanID,
//                           NhanVienID = (int)q.NhanVienID
//                           DanhSachQuyen = q.DaSachQuyen
//                       }).ToList();

//            int idNguoiDung = res[0].IDNguoiDung;
//            NguoiDungValidation DO = new NguoiDungValidation();
//            ViewBag.PQList = db_context.NguoiDung_Get_PhanQuyen(idNguoiDung).ToList();
//            if (res.Count > 0)
//            {
//                foreach (var nv in res)
//                {
//                    DO.IDNguoiDung = nv.IDNguoiDung;
//                    DO.TenDangNhap = nv.TenDangNhap;
//                    DO.Email = nv.Email;
//                    DO.PhongBanID = nv.PhongBanID;
//                    DO.NhanVienID = nv.NhanVienID;
//                }
//                DO.QuyenList = pq;
//            }
//            else
//            {
//                return HttpNotFound();
//            }

//            return PartialView(DO);
//        }

//        [HttpPost]
//        public ActionResult InsertPermission(FormCollection formCollection, NguoiDungValidation _DO)
//        {
//            var dsQuyenMoi = _DO.IDQuyen[0].Remove(_DO.IDQuyen[0].Length - 1, 1).Split(',');
//            db_context.Delete_PhanQyen(_DO.IDNguoiDung);
//            foreach (string q in dsQuyenMoi)
//            {
//                db_context.PhanQuyen_insert(_DO.IDNguoiDung, Int32.Parse(q));
//            }

//            return RedirectToAction("Index", "Users");
//        }
//    }
//}