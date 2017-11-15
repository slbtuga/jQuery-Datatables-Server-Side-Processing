﻿namespace Tests.Integration.Mvc.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using JQDT;
    using TestData.Data.Models;

    public class HomeController : Controller
    {
        public static IQueryable<SimpleDataModel> SimpleDataBig { get; set; }
        public static IQueryable<ComplexDataModel> ComplexDataBig { get; internal set; }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult SimpleDataTestPage()
        {
            return View();
        }

        public ActionResult SimpleDataNoPagingTestPage()
        {
            return this.View();
        }

        [JQDataTable]
        public ActionResult GetSimpleData(int take = int.MaxValue)
        {
            return this.View(HomeController.SimpleDataBig.Take(take));
        }

        public ActionResult GetSimpleDataFull()
        {
            return this.Json(HomeController.SimpleDataBig, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ComplexDataTestPage(bool isPaged = true)
        {
            this.ViewBag.IsPaged = isPaged.ToString().ToLower();

            return View();
        }

        [JQDataTable]
        public ActionResult GetComplexData(int take = int.MaxValue)
        {
            return this.View(HomeController.ComplexDataBig.Take(take));
        }

        public ActionResult GetComplexDataFull()
        {
            return this.Json(HomeController.ComplexDataBig, JsonRequestBehavior.AllowGet);
        }
    }
}