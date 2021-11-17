using ClosedXML.Excel;
using ExcelDataReader;
using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class DemarcationController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Demarcation
        public ActionResult Index(int? page, int? IDPhongBan, int? ID, int? IDViTri, int? IDNguoiDung, int? id)
        {
            List<PhongBan> pb1 = db_context.PhongBans.ToList();
            if (IDPhongBan == null) IDPhongBan = 0;
            ViewBag.PBList = new SelectList(pb1, "IDPhongBan", "TenPhongBan", IDPhongBan);

            List<NhanVien> nv1 = db_context.NhanViens.ToList();
            if (ID == null) ID = 0;
            ViewBag.NVList = new SelectList(nv1, "ID", "HoTen", ID);

            List<Vitri> vt1 = db_context.Vitris.ToList();
            if (IDViTri == null) IDViTri = 0;
            ViewBag.VTList = new SelectList(vt1, "IDViTri", "TenViTri", IDViTri);

            //var nd = db_context.NguoiDungs.Where(x => x.IDNguoiDung == MyAuthentication.IDNguoidung).SingleOrDefault();

            //var pbt = db_context.PhongBans.Where(x => x.IDPhongBan == nd.NhanVien.IDPhongBan).SingleOrDefault();

            var res = (from a in db_context.DinhBienDKs/*.Where(x => x.PhongBanID == pbt.IDPhongBan)*/
                       join pb in db_context.PhongBans on a.PhongBanID equals pb.IDPhongBan
                       join ns in db_context.NhanViens on a.NhanSuID equals ns.ID
                       join vt in db_context.Vitris on a.VTLVID equals vt.IDViTri
                       join db in db_context.DinhBienBHLDs on a.DBID equals db.IDDB
                       join tt in db_context.TinhTrangNBHLDs on a.TinhTrangID equals tt.IDNBHLD
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
                           TenTT = tt.TenTT,
                           PheDuyetID = (int)a.PheDuyetID
                       });

            if (IDPhongBan != 0)
                res = res.Where(x => x.PhongBanID == IDPhongBan);

            if (ID != 0)
                res = res.Where(x => x.NhanSuID == ID);

            if (IDViTri != 0)
                res = res.Where(x => x.VTLVID == IDViTri);

            if (page == null) page = 1;
            int pageSize = 10;
            int pageNumber = ((int)(page ?? 1));
            return View(res.ToList().ToPagedList(pageNumber, pageSize));
        }

        // get phòng ban vị trí làm việc theo nhân viên
        public JsonResult GetNhanvien(string search)
        {
            var nd = db_context.NguoiDungs.Where(x => x.IDNguoiDung == MyAuthentication.IDNguoidung).SingleOrDefault();

            var pbt = db_context.PhongBans.Where(x => x.IDPhongBan == nd.NhanVien.IDPhongBan).SingleOrDefault();

            var result = (from a in db_context.NhanVien_search_DK(search).Where(x => x.IDPhongBan == pbt.IDPhongBan)
                          select new DemarcationValidation
                          {
                              NhanSuID = a.ID,
                              MaNV = a.MaNV,
                              HoTen = a.HoTen,
                              PhongBanID = (int)a.IDPhongBan,
                              VTLVID = (int)a.IDViTri,
                          }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            List<PhongBan> pb = db_context.PhongBans.ToList();
            ViewBag.PBList = new SelectList(pb, "IDPhongBan", "TenPhongBan");

            List<NhanVien> ns = db_context.NhanViens.ToList();
            ViewBag.NSList = new SelectList(ns, "ID", "HoTen");

            List<Vitri> vt = db_context.Vitris.ToList();
            ViewBag.VTList = new SelectList(vt, "IDViTri", "TenViTri");

            List<DinhBienBHLD> db = db_context.DinhBienBHLDs.ToList();
            ViewBag.DBList = new SelectList(db, "IDDB", "TenVTBHLD");

            List<TinhTrangNBHLD> tt = db_context.TinhTrangNBHLDs.ToList();
            ViewBag.TTList = new SelectList(tt, "IDNBHLD", "TenTT");

            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(DemarcationValidation _DO)
        {
            try
            {
                var nv = db_context.NhanViens.Where(x => x.MaNV == _DO.MaNV).SingleOrDefault();
                var idnv = db_context.NhanViens.Where(x => x.ID == nv.ID).SingleOrDefault();
                if (nv != null)
                {
                    var vt = db_context.Vitris.Where(x => x.IDViTri == nv.IDViTri).SingleOrDefault();
                    if (vt != null)
                    {
                        var pb = db_context.PhongBans.Where(x => x.IDPhongBan == nv.IDPhongBan).SingleOrDefault();
                        if (pb != null)
                        {
                            var bhld = db_context.DinhBienBHLDs.Where(x => x.VTLVID == vt.IDViTri).SingleOrDefault();
                            if (bhld != null)
                            {
                                db_context.DinhBienDK_insert(idnv.ID, bhld.IDDB, pb.IDPhongBan, vt.IDViTri, _DO.NgayXuat, 0, 0);
                                TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
                            }

                        }
                    }
                }

            }
            catch (Exception e)
            {
                TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới: " + e.Message + "');</script>";
            }

            return RedirectToAction("Index", "Demarcation");
        }

        public ActionResult Delete(int id)
        {
            try
            {
                db_context.DinhBienDK_delete(id);
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Demarcation");
        }

        public ActionResult Edit(int id)
        {
            var res = (from a in db_context.DinhBienDK_searchByID(id)
                       join pb in db_context.PhongBans on a.PhongBanID equals pb.IDPhongBan
                       join ns in db_context.NhanViens on a.NhanSuID equals ns.ID
                       join vt in db_context.Vitris on a.VTLVID equals vt.IDViTri
                       join db in db_context.DinhBienBHLDs on a.DBID equals db.IDDB
                       join tt in db_context.TinhTrangNBHLDs on a.TinhTrangID equals tt.IDNBHLD
                       select new DemarcationValidation
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
                       }).ToList();
            DemarcationValidation DO = new DemarcationValidation();

            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDDBDK = a.IDDBDK;
                    DO.NhanSuID = (int)a.NhanSuID;
                    DO.DBID = (int)a.DBID;
                    DO.PhongBanID = (int)a.PhongBanID;
                    DO.VTLVID = (int)a.VTLVID;
                    DO.NgayXuat = (DateTime)a.NgayXuat;
                    DO.TinhTrangID = (int)a.TinhTrangID;
                }

                List<PhongBan> pb = db_context.PhongBans.ToList();
                ViewBag.PhongBanID = new SelectList(pb, "IDPhongBan", "TenPhongBan", DO.PhongBanID);

                List<NhanVien> ns = db_context.NhanViens.Where(x => x.IDPhongBan == DO.PhongBanID).ToList();
                ViewBag.NhanSuID = new SelectList(ns, "ID", "HoTen", DO.NhanSuID);

                List<Vitri> vt = db_context.Vitris.ToList();
                ViewBag.VTLVID = new SelectList(vt, "IDViTri", "TenViTri", DO.VTLVID);

                List<DinhBienBHLD> db = db_context.DinhBienBHLDs.Where(x => x.VTLVID == DO.VTLVID).ToList();
                ViewBag.DBID = new SelectList(db, "IDDB", "TenVTBHLD");

                List<TinhTrangNBHLD> tt = db_context.TinhTrangNBHLDs.ToList();
                ViewBag.TinhTrangID = new SelectList(tt, "IDNBHLD", "TenTT", DO.TinhTrangID);

                ViewBag.NXBHLD = DO.NgayXuat.ToString("yyyy-MM-dd");

            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }
        [HttpPost]
        public ActionResult Edit(DemarcationValidation _DO)
        {
            try
            {

                db_context.DinhBienDK_update(_DO.IDDBDK, _DO.NhanSuID, _DO.DBID, _DO.PhongBanID, _DO.VTLVID, _DO.NgayXuat, _DO.TinhTrangID);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {

                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại " + e.Message + " ');</script>";
            }

            return RedirectToAction("Index", "Demarcation");
        }
        public ActionResult CheckInformation(int id)
        {
            CheckInforProtection _DO = new CheckInforProtection();

            var res = (from a in db_context.DinhBienDK_searchByID(id)
                       join pb in db_context.PhongBans on a.PhongBanID equals pb.IDPhongBan
                       join ns in db_context.NhanViens on a.NhanSuID equals ns.ID
                       join vt in db_context.Vitris on a.VTLVID equals vt.IDViTri
                       join db in db_context.DinhBienBHLDs on a.DBID equals db.IDDB
                       join tt in db_context.TinhTrangNBHLDs on a.TinhTrangID equals tt.IDNBHLD

                       select new DemarcationValidation
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
                       }).ToList();
            DemarcationValidation DO = new DemarcationValidation();

            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDDBDK = a.IDDBDK;
                    DO.NhanSuID = (int)a.NhanSuID;
                    DO.DBID = (int)a.DBID;
                    DO.PhongBanID = (int)a.PhongBanID;
                    DO.VTLVID = (int)a.VTLVID;
                    DO.NgayXuat = (DateTime)a.NgayXuat;
                    DO.TinhTrangID = (int)a.TinhTrangID;
                }

                //ID Trưởng phó bộ phận
                var TPBP = (from ql in db_context.NguoiDungs
                            join a in db_context.NhanViens on ql.NhanVienID equals a.ID
                            select new CheckInforUser
                            {
                                IDNguoiDung = ql.IDNguoiDung,
                                HoTen = a.HoTen + " : " + ql.TenDangNhap,
                            }).ToList();
                //ID nhân viên P.ATMT
                var NVANNT = (from ql in db_context.NguoiDungs
                              join a in db_context.NhanViens on ql.NhanVienID equals a.ID
                              select new CheckInforUser
                              {
                                  IDNguoiDung = ql.IDNguoiDung,
                                  HoTen = a.HoTen + " : " + ql.TenDangNhap,
                              }).ToList();
                //ID Trưởng phó P.ATMT
                var TPATMT = (from ql in db_context.NguoiDungs
                              join a in db_context.NhanViens on ql.NhanVienID equals a.ID
                              select new CheckInforUser
                              {
                                  IDNguoiDung = ql.IDNguoiDung,
                                  HoTen = a.HoTen + " : " + ql.TenDangNhap,
                              }).ToList();

                ViewBag.TPBPList = new SelectList(TPBP, "IDNguoiDung", "HoTen");
                ViewBag.NVANTMList = new SelectList(NVANNT, "IDNguoiDung", "HoTen");
                ViewBag.TPATMTList = new SelectList(TPATMT, "IDNguoiDung", "HoTen");

                _DO.DBBHLDID = id;

            }
            return PartialView(_DO);
        }
        [HttpPost]
        public ActionResult CheckInformation(CheckInforProtection _DO, int? id)
        {
            try
            {


                //insert Trưởng phó bộ phận
                db_context.PheDuyetBHLD_Insert(_DO.IDTPBP, 5, _DO.DBBHLDID, 0, 1, null);
                //insert nhân viên P.ATMT
                db_context.PheDuyetBHLD_Insert(_DO.IDNVATMT, 5, _DO.DBBHLDID, 0, 2, null);
                //insert Trưởng phó P.ATMT
                db_context.PheDuyetBHLD_Insert(_DO.IDTPATMT, 5, _DO.DBBHLDID, 0, 3, null);

                var IDDK = id;
                db_context.DinhBienDK_UpdateTK(IDDK, 1);

                TempData["msgSuccess"] = "<script>alert('Trình ký thành công');</script>";

            }

            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Trình ký thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Demarcation");
        }
        public ActionResult ImportExcel()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult ImportExcel(DemarcationValidation _DO)
        {
            string filePath = string.Empty;
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["FileUpload"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string path = Server.MapPath("~/UploadedFiles/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    filePath = path + Path.GetFileName(DateTime.Now.ToString("yyyyMMddHHmm") + "-" + file.FileName);

                    file.SaveAs(filePath);
                    Stream stream = file.InputStream;

                    IExcelDataReader reader = null;
                    if (file.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (file.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        TempData["msg"] = "<script>alert('Vui lòng chọn đúng định dạng file Excel');</script>";
                        return View();
                    }
                    DataSet result = reader.AsDataSet();
                    DataTable dt = result.Tables[0];
                    reader.Close();
                    int dtc = 0;
                    string S_SL = "";
                    string S_se = "";
                    string ID = "";
                        try
                        {
                            for(int i = 2; i < dt.Rows.Count; i++) 
                             {
                                if (dt.Rows[i][0].ToString() != "")
                                {
                                string NhanVien = dt.Rows[i][0].ToString();
                                var CheckNV = db_context.NhanViens.Where(x => x.MaNV == NhanVien).ToList();

                                string PhongBan = dt.Rows[i][2].ToString();
                                var CheckPB = db_context.PhongBans.Where(x => x.TenPhongBan == PhongBan).ToList();

                                string ViTri = dt.Rows[i][3].ToString();
                                var CheckVT = db_context.Vitris.Where(x => x.TenViTri == ViTri).ToList();

                                string BHLD = dt.Rows[i][4].ToString();
                                var CheckBHLD = db_context.DinhBienBHLDs.Where(x => x.TenVTBHLD == BHLD).ToList();

                                string NgayX = dt.Rows[i][5].ToString();

                                string TinhTrang = dt.Rows[i][6].ToString();
                                var CheckTT = db_context.TinhTrangNBHLDs.Where(x => x.TenTT == TinhTrang).ToList();

                                NhanVienValidation _DONV = new NhanVienValidation();
                                PhongBanValidation _DOPB = new PhongBanValidation();
                                ViTriValidation _DOVT = new ViTriValidation();
                                ProtectionmarginValidation _DOBHLD = new ProtectionmarginValidation();

                                foreach (var nv in CheckNV)
                                {
                                    _DONV.ID = nv.ID;
                                    _DONV.MaNV = nv.MaNV;
                                    _DONV.HoTen = nv.HoTen;
                                }
                                foreach (var pb in CheckPB)
                                {
                                    _DOPB.IDPhongBan = pb.IDPhongBan;
                                    _DOPB.TenPhongBan = pb.TenPhongBan;

                                }
                                foreach (var vt in CheckVT)
                                {
                                    _DOVT.IDViTri = vt.IDViTri;
                                    _DOVT.TenViTri = vt.TenViTri;
                                }
                                foreach (var bh in CheckBHLD)
                                {
                                    _DOBHLD.IDDB = bh.IDDB;
                                    _DOBHLD.TenVTBHLD = bh.TenVTBHLD;
                                }
                                var NX = DateTime.ParseExact(NgayX,"dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                                System.Data.Entity.Core.Objects.ObjectParameter returnIDDBDK = new System.Data.Entity.Core.Objects.ObjectParameter("IDDBDK", typeof(int));
                                db_context.DinhBienDK_insertDS(_DONV.ID, _DOBHLD.IDDB, _DOPB.IDPhongBan, _DOVT.IDViTri, NX, 0, 0, returnIDDBDK);
                                int IDDBDK = Convert.ToInt32(returnIDDBDK.Value);
                                ID = returnIDDBDK.Value.ToString();

                                string LoaiBH = dt.Rows[i][7].ToString();
                                var CheckLBH = db_context.LoaiBHLDs.Where(x => x.TenBHLD == LoaiBH).ToList();

                                ProtectiontypeValidation _DOL = new ProtectiontypeValidation();
                                foreach (var lbh in CheckLBH)
                                {
                                    _DOL.IDBHLD = lbh.IDBHLD;
                                    _DOL.TenBHLD = lbh.TenBHLD;
                                }

                                
                                    S_SL = dt.Rows[i][8].ToString();
                                    S_se = dt.Rows[i][9].ToString();

                                    db_context.BHLDDK_insertDS(IDDBDK, _DOL.IDBHLD, Convert.ToInt32(S_SL), S_se);

                                

                            } 
                            else
                            {
                                string LoaiBH = dt.Rows[i][7].ToString();
                                var CheckLBH = db_context.LoaiBHLDs.Where(x => x.TenBHLD == LoaiBH).ToList();

                                ProtectiontypeValidation _DOL = new ProtectiontypeValidation();
                                foreach (var lbh in CheckLBH)
                                {
                                    _DOL.IDBHLD = lbh.IDBHLD;
                                    _DOL.TenBHLD = lbh.TenBHLD;
                                }
                                    S_SL = dt.Rows[i][8].ToString();
                                    S_se = dt.Rows[i][9].ToString();

                                    db_context.BHLDDK_insertDS(Convert.ToInt32(ID), _DOL.IDBHLD, Convert.ToInt32(S_SL), S_se);
                                
                            }    
                            
                            }  
                            

                        }
                        catch (Exception ex)
                        {
                            TempData["msgSuccess"] = "<script>alert('File import không đúng định dạng. Vui lòng tải biểu mẫu file import');</script>";
                        }
                }
                else
                {
                    TempData["msgSuccess"] = "<script>alert('Vui lòng nhập file Import');</script>";
                }
            }
            else
            {
                TempData["msgSuccess"] = "<script>alert('Vui lòng nhập file Import');</script>";
            }

            return RedirectToAction("Index", "Demarcation");
        }
        private bool SL(string Soluong)
        {
            if (Soluong != "")
                return true;
            else
                return false;
        }
        public ActionResult DownExcelFile()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult DownExcelFile(object sender, EventArgs e)
        {

            string filePath = "C:\\Users\\HPDQ\\Excel BHLĐ\\Biểu mẫu BHLĐ định kỳ.xlsx";
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                // Clear Rsponse reference  
                Response.Clear();
                // Add header by specifying file name  
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                // Add header for content length  
                Response.AddHeader("Content-Length", file.Length.ToString());
                // Specify content type  
                Response.ContentType = "text/plain";
                // Clearing flush  
                Response.Flush();
                // Transimiting file  
                Response.TransmitFile(file.FullName);
                Response.End();
            }
            else
            {
             
            }
            return RedirectToAction("Index", "Demarcation");
        }

    }
}