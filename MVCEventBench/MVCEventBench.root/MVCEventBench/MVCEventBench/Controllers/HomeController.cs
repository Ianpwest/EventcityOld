using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCEventBench.Classes;
using System.Data;

namespace MVCEventBench.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Local Instance of the database model
        /// </summary>
        private MVCEventBench.Models.dbEventModel m_db = new MVCEventBench.Models.dbEventModel();

        /// <summary>
        /// Local Instance of the build controls manager
        /// </summary>
        private BuildControls m_BuildControlsMgr = new BuildControls();

        public ActionResult Index()
        {
            //Qry to pull all events and information about those events from the database
            var eventsQRY = from r in m_db.Event
                            where r.bIsDeleted == false
                            select r;

            //Lists to hold the results
            List<NailVent> listNailVentToday = new List<NailVent>();
            List<NailVent> listNailVentWeek = new List<NailVent>();
            List<NailVent> listNailVentMonth = new List<NailVent>();

            m_BuildControlsMgr.QryResultsToColumnControls((System.Data.Objects.ObjectQuery)eventsQRY,listNailVentToday,listNailVentWeek,listNailVentMonth);

            //Put the lists of controls into the viewbag
            ViewBag.listNailVentToday = listNailVentToday;
            ViewBag.listNailVentWeek = listNailVentWeek;
            ViewBag.listNailVentMonth = listNailVentMonth;
           
            return View();
        }

        public ActionResult About()
        {
            
            return View();
        }


        public ActionResult EventClicked(Guid gEventGUID)
        {
            NailVent nv = m_BuildControlsMgr.BuildEventControl(gEventGUID);

            //Might need to add a view here that returns a event not found page if nv is null
            return View("~/Views/Event/Index.cshtml",nv);
        }

        #region Utility

        
        
        #endregion
    }
}
