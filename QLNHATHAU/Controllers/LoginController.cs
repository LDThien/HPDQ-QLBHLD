using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using QLNHATHAU.Common;
using QLNHATHAU.Models;

namespace QLNHATHAU.Controllers
{
    public class LoginController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(UserLogin u)
        {
            string mk = Encryptor.MD5Hash(u.Password);
            NguoiDung user = db_context.NguoiDungs.Where(x => x.TenDangNhap == u.Username && x.MatKhau == mk && x.IsLock == 0).FirstOrDefault();
            if (user != null)
            {
                string Cookie = string.Format("{0};{1};{2};{3};{4};{5}", user.IDNguoiDung, user.TenDangNhap, user.NhanVienID, user.IsLock, true,"");
                FormsAuthentication.SetAuthCookie(Cookie, u.RememberMe);

                Session["TK"] = user;
                Session["HoTen"] = user.NhanVien.HoTen;
                Session["TenDangNhap"] = user.TenDangNhap;
         
                return RedirectToAction("Index", "Home");

            }
            else
            {
                return View();
            }


        }
        public ActionResult Logout()
        { 
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Login");

        }
        //public ActionResult ChangePassword()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult ChangePassword(UseraccountValidation model)
        //{
        //    if (Session["TK"] != null)
        //    {
        //        string mk = Encryptor.MD5Hash(model.MatKhauCu);
        //        NguoiDung Suser = (NguoiDung)Session["TK"];
        //        NguoiDung user = db_context.NguoiDungs.SingleOrDefault(x => x.TenDangNhap == model.TenDangNhap && x.MatKhau == mk);
        //        if (user != null)
        //        {
        //            //user.MatKhau = model.MatKhau;
        //            mk = Encryptor.MD5Hash(model.MatKhau);
        //            user.MatKhau = mk;
        //            db_context.SaveChanges();
        //            Session.Clear();
        //            Session.Abandon();
        //            //TempData["msg"] = "<script>alert('Cập nhập thành công')</script>";
        //            ViewBag.Message = "<script>alert('Thay đổi mật khẩu thành công');window.location.href = '/Login</script>";
        //            //Page.ClientScript.RegisterClientScriptBlock(GetType(), "alerta", "alert('Save records with success')", true);
        //            //return RedirectToAction("Index", "Login");
        //            return View();
        //        }
        //        else
        //        {
        //            ViewBag.Message = "<script>alert('Mật khẩu cũ không đúng, vui lòng nhập lại')</script>";
        //            //TempData["msg"] = "<script>alert('Mật khẩu cũ không đúng, vui lòng nhập lại')</script>";
        //            return View();
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.Message = "<script>alert('Lỗi thay đổi mật khẩu')</script>";
        //        //TempData["msg"] = "<script>alert('Lỗi thay đổi mật khẩu')</script>";
        //        return View();
        //    }

        //}
    }
}