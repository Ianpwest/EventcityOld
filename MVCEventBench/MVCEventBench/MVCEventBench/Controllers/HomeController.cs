using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCEventBench.Classes;

namespace MVCEventBench.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Create an instance of the data object I want to use as the model for my control
            NailVent myTestNailVent = new NailVent();

            NailVent myTO = new NailVent();
            myTO.Title = "This is the other title";

            //Put the NailVent object in the viewbag so I can reference it in the view
            ViewBag.myTestNailVent = myTestNailVent;
            ViewBag.myTO = myTO;

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
