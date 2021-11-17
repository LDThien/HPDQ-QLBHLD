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
    public class ConsumedregularlyController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Consumedregularly
        public ActionResult Index(int? page, int? IDViTri)
        {
            List<Vitri> vt1 = db_context.Vitris.ToList();
            if (IDViTri == null) IDViTri = 0;
            ViewBag.VTList = new SelectList(vt1, "IDViTri", "TenViTri", IDViTri);

            var res = (from a in db_context.DinhBienBHLDTHs
                       join vt in db_context.Vitris on a.VTLVID equals vt.IDViTri
                       select new ConsumedregularlyValidation()
                       {
                           IDDBTH = a.IDDBTH,
                           TenVTBHLD = a.TenVTBHLD,
                           VTLVID = (int)a.VTLVID,
                           TenVTLV = vt.TenViTri
                       });
            if (IDViTri != 0)
                res = res.Where(x => x.VTLVID == IDViTri);

            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = ((int)(page ?? 1));
            return View(res.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create()
        {
            List<Vitri> vt = db_context.Vitris.ToList();
            ViewBag.VTList = new SelectList(vt, "IDViTri", "TenViTri");

            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(ConsumedregularlyValidation _DO)
        {


            try
            {

                db_context.DinhBienBHLDTH_insert(_DO.TenVTBHLD, _DO.VTLVID);

                TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới: " + e.Message + "');</script>";
            }

            return RedirectToAction("Index", "Consumedregularly");
        }
        public ActionResult Delete(int id)
        {
            try
            {
                db_context.DinhBienBHLDTH_delete(id);
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Consumedregularly");
        }
        public ActionResult Edit(int id)
        {
            var res = (from a in db_context.DinhBienBHLDTH_searchByID(id)
                       select new ConsumedregularlyValidation
                       {
                           IDDBTH = a.IDDBTH,
                           TenVTBHLD = a.TenVTBHLD,
                           VTLVID = (int)a.VTLVID

                       }).ToList();
            ConsumedregularlyValidation DO = new ConsumedregularlyValidation();

            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDDBTH = a.IDDBTH;
                    DO.TenVTBHLD = a.TenVTBHLD;
                    DO.VTLVID = (int)a.VTLVID;
                }

                List<Vitri> vt = db_context.Vitris.ToList();
                ViewBag.VTList = new SelectList(vt, "IDViTri", "TenViTri");
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }
        [HttpPost]
        public ActionResult Edit(ConsumedregularlyValidation _DO)
        {
            try
            {

                db_context.DinhBienBHLDTH_update(_DO.IDDBTH, _DO.TenVTBHLD, _DO.VTLVID);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại " + e.Message + " ');</script>";
            }

            return RedirectToAction("Index", "Consumedregularly");
        }
        public ActionResult ImportExcel()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult ImportExcel(ConsumedregularlyValidation _DO)
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

                    string ĐVT = "";
                    string S_SL = "";
                    string T_TH = "";
                    string GhiChu = "";
                    string ID = "";

                        try
                        {
                            for (int i = 2; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i][0].ToString() != "")
                                {
                                    string BHLĐTVT = dt.Rows[i][0].ToString();

                                    string ViTri = dt.Rows[i][1].ToString();
                                    var CheckVT = db_context.Vitris.Where(x => x.TenViTri == ViTri).ToList();

                                    ViTriValidation _DOVT = new ViTriValidation();


                                    foreach (var vt in CheckVT)
                                    {
                                        _DOVT.IDViTri = vt.IDViTri;
                                        _DOVT.TenViTri = vt.TenViTri;
                                    }

                                    System.Data.Entity.Core.Objects.ObjectParameter returnIDDBTH = new System.Data.Entity.Core.Objects.ObjectParameter("IDDBTH", typeof(int));
                                    db_context.DinhBienBHLDTH_insertDS(BHLĐTVT, _DOVT.IDViTri, returnIDDBTH);
                                    int IDDBTH = Convert.ToInt32(returnIDDBTH.Value);
                                    ID = returnIDDBTH.Value.ToString();

                                    string LoaiBH = dt.Rows[i][2].ToString();
                                    var CheckLBH = db_context.LoaiBHLDs.Where(x => x.TenBHLD == LoaiBH).ToList();

                                    ProtectiontypeValidation _DOL = new ProtectiontypeValidation();
                                    foreach (var lbh in CheckLBH)
                                    {
                                        _DOL.IDBHLD = lbh.IDBHLD;
                                        _DOL.TenBHLD = lbh.TenBHLD;
                                    }

                                    ĐVT = dt.Rows[i][3].ToString();
                                    S_SL = dt.Rows[i][4].ToString();
                                    T_TH = dt.Rows[i][5].ToString();
                                    GhiChu = dt.Rows[i][6].ToString();

                                    db_context.DinhBienBHLDTHCT_insertDS(_DOL.IDBHLD, IDDBTH, ĐVT, Convert.ToInt32(S_SL), Convert.ToInt32(T_TH), GhiChu);

                                } 
                                else
                                {
                                    string LoaiBH = dt.Rows[i][2].ToString();
                                    var CheckLBH = db_context.LoaiBHLDs.Where(x => x.TenBHLD == LoaiBH).ToList();

                                    ProtectiontypeValidation _DOL = new ProtectiontypeValidation();
                                    foreach (var lbh in CheckLBH)
                                    {
                                        _DOL.IDBHLD = lbh.IDBHLD;
                                        _DOL.TenBHLD = lbh.TenBHLD;
                                    }

                                    ĐVT = dt.Rows[i][3].ToString();
                                    S_SL = dt.Rows[i][4].ToString();
                                    T_TH = dt.Rows[i][5].ToString();
                                    GhiChu = dt.Rows[i][6].ToString();

                                    db_context.DinhBienBHLDTHCT_insertDS(_DOL.IDBHLD, Convert.ToInt32(ID), ĐVT, Convert.ToInt32(S_SL), Convert.ToInt32(T_TH), GhiChu);

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

            return RedirectToAction("Index", "Consumedregularly");
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

            string filePath = "C:\\Users\\HPDQ\\Excel BHLĐ\\Biểu mẫu ĐBCP tiêu hao.xlsx";
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
            return RedirectToAction("Index", "Consumedregularly");
        }
    }
}