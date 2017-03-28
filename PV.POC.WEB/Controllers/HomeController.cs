using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PV.POC.WEB.Models;
using ClaimsMapping;

namespace PV.POC.WEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View(new IndexModel());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [HttpPost]
        public ActionResult SubmitText(IndexModel model)
        {
            var obj = new PVpoc.Controllers.NaturalLanguageController();
            model.Claim = obj.GetAnnotatedText(model.InputText);

            return View("Index",new IndexModel());
        }
    }
}