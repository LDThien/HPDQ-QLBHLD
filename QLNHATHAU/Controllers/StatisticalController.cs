using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class StatisticalController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Statistical
        public ActionResult Index(int? page)
        {
            var dataList = (from pb in db_context.PhongBans 
                            join dk in db_context.DinhBienDKs on pb.IDPhongBan equals dk.PhongBanID into SLDK
                            join th in db_context.DinhBienTHs on pb.IDPhongBan equals th.PhongBanID into SLTH
                            join nv in db_context.DinhBienNVs on pb.IDPhongBan equals nv.PhongBanID into SLNVM
                            select new StatisticalValidation()
                            {
                                IDPhongBan = pb.IDPhongBan,
                                TenPhongBan = pb.TenPhongBan,
                                SLDK = SLDK.Count(),
                                SLTH = SLTH.Count(),
                                SLNVM = SLNVM.Count(),

                            }).OrderBy(x => x.IDPhongBan).ToList();

            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(dataList.ToPagedList(pageNumber, pageSize));

        }
    }
}