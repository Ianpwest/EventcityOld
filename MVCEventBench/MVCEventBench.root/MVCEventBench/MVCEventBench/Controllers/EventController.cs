using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCEventBench.Classes;
using System.Web.Helpers;

namespace MVCEventBench.Controllers
{
    public class EventController : Controller
    {
        /// <summary>
        /// Local instance of the dbEventModel
        /// </summary>
        private MVCEventBench.Models.dbEventModel m_db = new Models.dbEventModel();

        public ActionResult DetailsAjax(string strEventGUID)
        {
            string strDescription = string.Empty;

            //Parse the string into a guid
            Guid gEventGUID = new Guid();
            Guid.TryParse(strEventGUID, out gEventGUID);

            var results = from r in m_db.Event
                          where r.gEventGUID == gEventGUID
                          select r;
            
            foreach (MVCEventBench.Models.Event result in results)
            {
                strDescription = "<b>Description:</b>" + result.strDescription + "\r\n" + "<a href=" + result.strWebpage + ">" + result.strWebpage + "</a>";
            }

            return Json(new {success = true, message = strDescription});
        }

        // GET: /Event/
        public ActionResult Index(NailVent nv)
        {
            
            return View();
        }

        //
        // GET: /Event/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Event/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Event/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Event/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Event/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Event/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Event/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
