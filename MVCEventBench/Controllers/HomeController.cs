using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCEventBench.Classes;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq.Dynamic;
using System.Web.Security;


namespace MVCEventBench.Controllers
{
    public class HomeController : Controller
    {
        //Lists to hold the category results
        private List<NailVent> m_listNailVentToday = new List<NailVent>();
        private List<NailVent> m_listNailVentWeek = new List<NailVent>();
        private List<NailVent> m_listNailVentMonth = new List<NailVent>();

        /// <summary>
        /// Local Instance of the database model
        /// </summary>
        private MVCEventBench.Models.dbEventModel m_db = new MVCEventBench.Models.dbEventModel();

        /// <summary>
        /// Local Instance of the build controls manager
        /// </summary>
        private BuildControls m_BuildControlsMgr = new BuildControls();

        /// <summary>
        /// Home page controller
        /// </summary>
        /// <returns>Home page view</returns>
        public ActionResult Index()
        {
            //Put the lists of controls into the viewbag
            ViewBag.listNailVentToday = m_listNailVentToday;
            ViewBag.listNailVentWeek = m_listNailVentWeek;
            ViewBag.listNailVentMonth = m_listNailVentMonth;

            //Get the data for the state cb
            GetStateCB();
            GetCitiesCB(string.Empty);
            GetTagsCB();
            GetRadiusSelection();

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Takes a set of filter paramaters, queries the database and then returns a partial view to render
        /// </summary>
        /// <param name="strCity">City to filter on</param>
        /// <param name="strState">State to filter on</param>
        /// <returns>Partial View NailVentContainer</returns>
        public ActionResult FilteredPartialContainer(string strCityLat, string strCityLong, string strCity, string strState, string strTag, string strDate, string strRadius)
        {
            string strQRY = string.Empty;
            Dictionary<string, string> strCityReturn = new Dictionary<string, string>();

            //Qry to pull events and information about those events from the database
            var eventsQRY = (IQueryable)null;

            if ((!string.IsNullOrEmpty(strCity)) && (!string.IsNullOrEmpty(strState)))
            {
                eventsQRY = GetFilteredQuery(strCity, strState, strTag, strDate, strRadius);
            }
            else
            {
                //Only do this if we don't have a manual city and state...This will always be the case for the initial loading of the page. We
                //also need to handle populating the city state combobox's with the city and state that we find for them. What radius should we add here?
                strCityReturn = GetClosestCity(strCityLat, strCityLong);

                string strCityName = strCityReturn.FirstOrDefault().Key;
                string strStateName = strCityReturn.FirstOrDefault().Value;

                eventsQRY = GetFilteredQuery(strCityName, strStateName, strTag, strDate, strRadius);
            }

            //Gets the events for their category day,week,month
            m_BuildControlsMgr.QryResultsToColumnControls((System.Data.Objects.ObjectQuery)eventsQRY, m_listNailVentToday, m_listNailVentWeek, m_listNailVentMonth);

            //Put the lists of controls into the viewbag
            ViewBag.listNailVentToday = m_listNailVentToday;
            ViewBag.listNailVentWeek = m_listNailVentWeek;
            ViewBag.listNailVentMonth = m_listNailVentMonth;

            return PartialView("NailVentContainerPartialView");
        }

        public ActionResult Portal()
        {
            return View();
        }

        #region Events

        /// <summary>
        /// Returns a details page if the nailpartialview is clicked. TODO: needs to be more dynamic 
        /// </summary>
        /// <param name="gEventGUID">Guid of the event</param>
        /// <returns>Details page of the selected event</returns>
        public ActionResult EventClicked(Guid gEventGUID)
        {
            var eventsQRY = from r in m_db.Event
                            where r.bIsDeleted == false && r.gEventGUID == gEventGUID
                            select r;

            Random random = new Random();
            var returnedQryEventClicked = (IQueryable)null;
            string strTagToUse = string.Empty;
            string strCityEventClicked = string.Empty;
            string strStateEventClicked = string.Empty;
            string strDateEventClicked = string.Empty;
            bool tag1 = false;
            bool tag2 = false;
            bool tag3 = false;
            
            switch (random.Next(1, 4))      // randomly selects which tag will be used to find a similar event
            {
                case 1:
                    tag1 = true;
                    break;
                case 2:
                    tag2 = true;
                    break;
                case 3:
                    tag3 = true;
                    break;
                default:
                    tag1 = true;
                    break;
            }

            foreach (MVCEventBench.Models.Event vent in eventsQRY)              // collects the information so we can use this to find similar events
            {
                if (tag1)
                {
                    strTagToUse = vent.strTag1;
                }
                if (tag2)
                {
                    strTagToUse = vent.strTag2;
                }
                if (tag3)
                {
                    strTagToUse = vent.strTag3;
                }
                strCityEventClicked = vent.strEventCity;
                strStateEventClicked = vent.strEventState;
                strDateEventClicked = vent.dEventDateStart.ToString();
            }

            // This will return us a query to use to build the nailvents
            returnedQryEventClicked = GetFilteredQuery(strCityEventClicked, strStateEventClicked, strTagToUse, strDateEventClicked, "25");            
            int intQryLength = returnedQryEventClicked.Count();         // length of the returned query - need to know if there is less than 4 total including original
            List<Guid> listSimilarEvents = new List<Guid>();
            List<Guid> listFinalSelection = new List<Guid>();

            // Goes through the query of similar events and extracts all of them except for the one we started with            
            foreach(MVCEventBench.Models.Event similarEvent in returnedQryEventClicked)
            {
                    if (!similarEvent.gEventGUID.Equals(gEventGUID))
                    {
                        listSimilarEvents.Add(similarEvent.gEventGUID);
                    }
            }

            NailVent nvSimilar0;
            NailVent nvSimilar1;
            NailVent nvSimilar2;           

            /**********************************************************************
             * This next piece will fill the list of selected events from the entire list of similar events
             * It goes through the loop and selects a random event from the similar event list and then assigns it to the final selection as long as it hasn't already been assigned to it            
            ***********************************************************************/
            if (listSimilarEvents.Count > 3)
            {
                while (listFinalSelection.Count < 3)
                {
                    int i = random.Next(0, listSimilarEvents.Count);
                    if (!listFinalSelection.Contains(listSimilarEvents[i]))
                    {
                        listFinalSelection.Add(listSimilarEvents[i]);
                    }
                }
            }
            else
            {        
                    listFinalSelection = listSimilarEvents;                
            }

            int countSimilar = 0;

            if (listFinalSelection.Count == 1)
            {
                countSimilar = 1;

                nvSimilar0 = m_BuildControlsMgr.BuildEventControl(listSimilarEvents[0]);
                ViewBag.nvSimilar0 = nvSimilar0;

                nvSimilar1 = m_BuildControlsMgr.BuildEventControl(listSimilarEvents[0]);
                ViewBag.nvSimilar1 = nvSimilar1;

                nvSimilar2 = m_BuildControlsMgr.BuildEventControl(listSimilarEvents[0]);
                ViewBag.nvSimilar2 = nvSimilar2;
            }
            else if (listFinalSelection.Count == 2)
            {
                countSimilar = 2;

                nvSimilar0 = m_BuildControlsMgr.BuildEventControl(listSimilarEvents[0]);
                ViewBag.nvSimilar0 = nvSimilar0;

                nvSimilar1 = m_BuildControlsMgr.BuildEventControl(listSimilarEvents[1]);
                ViewBag.nvSimilar1 = nvSimilar1;

                nvSimilar2 = m_BuildControlsMgr.BuildEventControl(listSimilarEvents[1]);
                ViewBag.nvSimilar2 = nvSimilar2;
            }
            else if (listFinalSelection.Count >= 3)
            {
                countSimilar = 3;

                nvSimilar0 = m_BuildControlsMgr.BuildEventControl(listSimilarEvents[0]);
                ViewBag.nvSimilar0 = nvSimilar0;

                nvSimilar1 = m_BuildControlsMgr.BuildEventControl(listSimilarEvents[1]);
                ViewBag.nvSimilar1 = nvSimilar1;

                nvSimilar2 = m_BuildControlsMgr.BuildEventControl(listSimilarEvents[2]);
                ViewBag.nvSimilar2 = nvSimilar2;
            }
            else
            {
                countSimilar = 0;
            }

            NailVent nv = m_BuildControlsMgr.BuildEventControl(gEventGUID);
            ViewBag.nvDetails = nv;

            ViewBag.simCount = countSimilar;

            //Place into a separate method
            float dMapLat = 0;
            float dMapLong = 0;
            Guid gMapState = Guid.Empty;            

            var qryMapStates = from r in m_db.States
                               where r.strName == nv.State
                               select r;

            foreach (MVCEventBench.Models.States state in qryMapStates)
            {
                gMapState = state.gStateGUID;
            }

            var qryMapCities = from r in m_db.Cities
                               where r.gState == gMapState && r.strCity == nv.City
                               select r;

            foreach (MVCEventBench.Models.Cities city in qryMapCities)
            {
                dMapLat = city.nLatitude;                
                dMapLong = city.nLongitude;
            }

            ViewBag.mapAddress = nv.Address + " " + nv.City + " " + nv.State;

            ViewBag.mapLat = dMapLat;
            ViewBag.mapLong = dMapLong;
            
            //End region

            //Might need to add a view here that returns a event not found page if nv is null
            return View("/Views/Home/Details.cshtml", nv);
        }
        #endregion

        #region Utility

        /// <summary>
        /// Takes all of the filter paramaters and determines a linq query
        /// </summary>
        /// <param name="strCity">City</param>
        /// <param name="strState">State</param>
        /// <param name="strTag">Tag</param>
        /// <param name="strDate">Date</param>
        /// <returns>IQueryable linq query</returns>
        private IQueryable GetFilteredQuery(string strCity, string strState, string strTag, string strDate, string strRadius)
        {
            var eventsQRY = (IQueryable)null;
            var stateQRY = (IQueryable)null;
            var cityQRY = (IQueryable)null;

            DateTime dDate = new DateTime();
            DateTime.TryParse(strDate, out dDate);


            //Case where we have all information
            if (strRadius != string.Empty)
            {
                Guid gState = Guid.Empty;
                Dictionary<string, string> dictCitiesFiltered = new Dictionary<string, string>();
                Dictionary<double, double> dictLatLong = new Dictionary<double, double>();

                stateQRY = from r in m_db.States
                           where r.strName == strState
                           select r.gStateGUID;

                foreach (Guid state in stateQRY)
                {
                    gState = state;
                }

                cityQRY = from r in m_db.Cities
                          where r.strCity == strCity
                          && r.gState == gState
                          select r;

                foreach (MVCEventBench.Models.Cities city in cityQRY)
                {
                    dictLatLong.Add(city.nLongitude, city.nLatitude);
                }

                //We have all the cities and states in the radius
                dictCitiesFiltered = GetClosestCities(dictLatLong.FirstOrDefault().Value.ToString(), dictLatLong.FirstOrDefault().Key.ToString(), strRadius);
                string strWhereQRY = string.Empty;
                object[] strArry = new object[(dictCitiesFiltered.Count * 2) + 2];

                int count = 0;
                foreach (KeyValuePair<string, string> kvp in dictCitiesFiltered)
                {
                    if (count == 0)
                    {
                        strWhereQRY += "((strEventCity == @" + count + " ";
                        strArry[count] = kvp.Key;
                        count++;
                        strWhereQRY += "AND strEventState == @" + count + ") ";
                        strArry[count] = kvp.Value;
                        count++;
                    }
                    else
                    {
                        strWhereQRY += "OR (strEventCity == @" + count + " ";
                        strArry[count] = kvp.Key;
                        count++;
                        strWhereQRY += "AND strEventState == @" + count + ") ";
                        strArry[count] = kvp.Value;
                        count++;
                    }
                }
                strWhereQRY += ")";

                //Tack on all the other paramaters here
                if (strTag != string.Empty)
                {
                    strWhereQRY += " AND (strTag1 == @" + count + " OR strTag2 == @" + count + " OR strTag3 == @" + count + " ) ";
                    strArry[count] = strTag;
                    count++;
                }
                if (strDate != string.Empty)
                {
                    strWhereQRY += " AND (dEventDateStart == @" + count + " ) ";
                    strArry[count] = dDate;
                    count++;
                }

                //Handle the case where we don't want deleted records
                strWhereQRY += " AND (bIsDeleted = false)";

                eventsQRY = m_db.Event.Where(strWhereQRY, strArry);
            }
            else
            {
                string strWhereQRY = string.Empty;
                object[] strArry = new object[25];
                int count = 0;

                strWhereQRY += "(strEventCity == @" + count + " ";
                strArry[count] = strCity;
                count++;

                strWhereQRY += "AND strEventState == @" + count + " ) ";
                strArry[count] = strState;
                count++;

                if (strTag != string.Empty)
                {
                    strWhereQRY += " AND (strTag1 == @" + count + " OR strTag2 == @" + count + " OR strTag3 == @" + count + " ) ";
                    strArry[count] = strTag;
                    count++;
                }
                if (strDate != string.Empty)
                {
                    strWhereQRY += " AND (dEventDateStart == @" + count + " ) ";
                    strArry[count] = dDate;
                    count++;
                }

                //Handle the case where we don't want deleted records
                strWhereQRY += " AND (bIsDeleted = false)";

                eventsQRY = m_db.Event.Where(strWhereQRY, strArry);
            }

            return eventsQRY;
        }

        /// <summary>
        /// Takes an img guid, queries the database for that image then returns an img result
        /// </summary>
        /// <param name="gEventGUID">guid of the event</param>
        /// <returns>Image Result</returns>
        public ActionResult ShowPhoto(string id)
        {
            MVCEventBench.Models.dbEventModel db = new MVCEventBench.Models.dbEventModel();

            //TODO: Add error handling here for when the id is null
            Guid gGuid = new Guid(id);

            IEnumerable<MVCEventBench.Models.Image> result = from r in db.Image
                                                             where r.gEvent == gGuid
                                                             select r;


            MVCEventBench.Models.Image imgToShow = new MVCEventBench.Models.Image();

            //TODO: There should only ever be one here. Handle the case where no image is found.
            foreach (MVCEventBench.Models.Image img in result)
            {
                imgToShow.gEventImageGUID = img.gEventImageGUID;
                imgToShow.imgContent = img.imgContent;
                imgToShow.strFileName = img.strFileName;
                imgToShow.strMIMEType = img.strMIMEType;
            }

            //Pass back an image result 
            ImageResult imgResult = new ImageResult(imgToShow.imgContent, imgToShow.strMIMEType);

            return imgResult;
        }

        private Dictionary<string, string> GetClosestCities(string strLat, string strLong, string strHowFar)
        {
            string strClosestCity = string.Empty;
            int intHowFar = int.Parse(strHowFar);        // Number of miles in which to select the cities - normally selected by user.            

            // Used to extract data from the LINQ query
            string strKeyContainer = string.Empty;     // Stores the key and places it into the dictReturn
            string strValueContainer = string.Empty;   // Stores the value and places it into the dictReturn

            double deltaLat;
            double deltaLong;
            double a, c, d;
            double lat1, lat2, long1, long2;        // lat1 and long 1 corespond to strLat/strLong and lat2 and long2 corespond to the database fields
            const double earthRadius = 3959;        // in miles

            Dictionary<string, Guid> dictCityState = new Dictionary<string, Guid>();        // City and state guid selected from the City table
            List<string> listStateGuids = new List<string>();                               // List of states queried based on the gState guids we found in the values of dictCityState
            Dictionary<string, string> dictReturn = new Dictionary<string, string>();       // City key and state value dictionary used to return the final result

            var qryCities = from r in m_db.Cities   // Selects all the cities from the DB
                            select r;

            foreach (MVCEventBench.Models.Cities city in qryCities)     // Collects all relevant data from the query (cityname, lat, long)
            {

                // Using the "haversine" formula to calculate the distance in miles
                // First, take all the lats/longs and convert them to radians.
                lat1 = double.Parse(strLat) * (Math.PI / 180);
                lat2 = (double)(city.nLatitude) * (Math.PI / 180);
                long1 = double.Parse(strLong) * (Math.PI / 180);
                long2 = (double)(city.nLongitude) * (Math.PI / 180);

                // Formula used to calculate d, the distance between city selected by user and the one being tested in this particular iteration of foreach.
                deltaLat = lat1 - lat2;
                deltaLong = long1 - long2;
                a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(deltaLong / 2) * Math.Sin(deltaLong / 2);
                c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                d = earthRadius * c;

                // Determines if the distance in miles is within the specified distances "intHowFar" and then assigns it to the dictionary
                if (d < intHowFar)
                {
                    dictCityState.Add(city.strCity, (Guid)city.gState);
                }


            }

            // Takes the collection we have of cities and state guids within the specified distance, matches the states with the state guids, then collects the string city/state name into a dictionary 
            foreach (KeyValuePair<string, Guid> kvp in dictCityState)
            {
                var qryStates = from r in m_db.States
                                where r.gStateGUID == kvp.Value
                                select r.strName;

                foreach (string s in qryStates)
                {
                    strKeyContainer = s;
                }

                dictReturn.Add(kvp.Key, strKeyContainer);

            }

            return dictReturn;
        }

        private Dictionary<string, string> GetClosestCity(string strLat, string strLong)
        {
            //double a, b, c;     // pythagorean theorum                                   
            double intNearest = 0;  // Holds previous distance to test against next distance.

            double deltaLat;
            double deltaLong;
            double a, c, d;
            double lat1, lat2, long1, long2;        // lat1 and long 1 corespond to strLat/strLong and lat2 and long2 corespond to the database fields
            const double earthRadius = 3959;        // in miles

            Dictionary<string, Guid> dictCityState = new Dictionary<string, Guid>();        // City and state guid selected from the City table
            Dictionary<string, string> dictReturn = new Dictionary<string, string>();       // City key and state value dictionary used to return the final result

            var qryCities = from r in m_db.Cities   // Selects all the cities from the DB
                            select r;

            int i = 0;
            foreach (MVCEventBench.Models.Cities city in qryCities)     // Collects all relevant data from the query (cityname, lat, long)
            {
                // Using the "haversine" formula to calculate the distance in miles
                // First, take all the lats/longs and convert them to radians.
                lat1 = double.Parse(strLat) * (Math.PI / 180);
                lat2 = (double)(city.nLatitude) * (Math.PI / 180);
                long1 = double.Parse(strLong) * (Math.PI / 180);
                long2 = (double)(city.nLongitude) * (Math.PI / 180);

                deltaLat = lat1 - lat2;
                deltaLong = long1 - long2;
                a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(deltaLong / 2) * Math.Sin(deltaLong / 2);
                c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                d = earthRadius * c;

                if (i == 0)
                {
                    intNearest = d;        // gets the first city's distance and initializes it as the closest distance
                    dictCityState.Add(city.strCity, (Guid)city.gState);     // sets nearest city to the nearest city string
                }
                else
                {
                    if (d < intNearest)
                    {
                        intNearest = d;     // If the next collected distance is small, reset the nearest distance to new distance
                        dictCityState.Clear();
                        dictCityState.Add(city.strCity, (Guid)city.gState);     // sets nearest city to the nearest city string
                    }
                }

                i++;


            }

            Guid str = dictCityState.FirstOrDefault().Value;


            var qryState = from r in m_db.States
                           where r.gStateGUID == str
                           select r;

            foreach (MVCEventBench.Models.States state in qryState)
            {
                dictReturn.Add(dictCityState.FirstOrDefault().Key, state.strName);
            }

            return dictReturn;
        }

        public ActionResult GetCityStateCBValues(string strCityLat, string strCityLong)
        {
            Dictionary<string, string> dictCityState = GetClosestCity(strCityLat, strCityLong);

            string strCity = string.Empty;
            string strState = string.Empty;

            foreach (KeyValuePair<string, string> kvp in dictCityState)
            {
                strCity = kvp.Key.ToString();
                strState = kvp.Value.ToString();
            }

            return Json(new { success = true, strCityReturn = strCity, strStateReturn = strState });
        }

        private void GetRadiusSelection()
        {
            //Create the combobox for filtering cities based on a radius around the selected city
            List<string> listRadiusCombobox = new List<string>();

            listRadiusCombobox.Add(" ");
            listRadiusCombobox.Add("15");
            listRadiusCombobox.Add("25");
            listRadiusCombobox.Add("35");
            listRadiusCombobox.Add("50");
            listRadiusCombobox.Add("75");
            listRadiusCombobox.Add("100");

            SelectList cbRadiusList = new SelectList(listRadiusCombobox);
            ViewBag.radiusList = cbRadiusList;
        }

        private void GetTagsCB()
        {
            //Create the state combobox on the page
            List<String> listTagsombobox = new List<String>();
            var tagsQRY = from r in m_db.Tags
                          orderby r.strName
                          select r;

            listTagsombobox.Add(" ");

            foreach (MVCEventBench.Models.Tags tag in tagsQRY)
            {
                listTagsombobox.Add(tag.strName);
            }

            SelectList cbTagList = new SelectList(listTagsombobox);
            ViewBag.tagsList = cbTagList;
        }

        public ActionResult GetCitiesCB(string strState)
        {
            string strMessage = string.Empty;
            List<String> listCitiesCB = new List<String>();

            if (strState != string.Empty)
            {
                var stateQRY = from r in m_db.States
                               where r.strName == strState
                               select r.gStateGUID;

                var citiesQRY = from r in m_db.Cities
                                where r.gState == stateQRY.FirstOrDefault()
                                orderby r.strCity
                                select r;

                foreach (MVCEventBench.Models.Cities city in citiesQRY)
                {
                    listCitiesCB.Add(city.strCity);
                }
            }

            SelectList listCities = new SelectList(listCitiesCB);
            ViewBag.citiesList = listCities;

            int count = 0;
            foreach (string city in listCitiesCB)
            {
                if (count != 0)
                {
                    strMessage = strMessage + "," + city;
                }
                else
                {
                    strMessage = city;
                }
                count++;
            }

            return Json(new { success = true, message = strMessage });
        }

        private void GetStateCB()
        {
            //Create the state combobox on the page
            List<String> listStatesCombobox = new List<String>();
            var statesQRY = from r in m_db.States
                            where r.bIsDeleted == false
                            orderby r.strName
                            select r;

            listStatesCombobox.Add(" ");

            foreach (MVCEventBench.Models.States state in statesQRY)
            {
                listStatesCombobox.Add(state.strName);
            }

            SelectList cbStateList = new SelectList(listStatesCombobox);
            ViewBag.stateList = cbStateList;
        }

        //Method to quickly create test events
        public void CreateTestEvents(List<NailVent> listNailVentToday, List<NailVent> listNailVentWeek, List<NailVent> listNailVentMonth, string strState)
        {
            //First clear all events in the database
            MVCEventBench.Models.dbEventModel db = new MVCEventBench.Models.dbEventModel();
            var qry = from r in db.Event
                      select r;

            foreach (MVCEventBench.Models.Event evnt in qry)
            {
                var qry2 = from r2 in db.Image
                           where r2.gEvent == evnt.gEventGUID
                           select r2;

                foreach (MVCEventBench.Models.Image imgs in qry2)
                {
                    db.Image.DeleteObject(imgs);
                }

                db.Event.DeleteObject(evnt);
            }

            db.SaveChanges();


            //Create a List of test cities and countries
            List<string> listCities = new List<string>();
            listCities.Add("Clarksville");
            listCities.Add("Clarksville");
            listCities.Add("Martin");
            listCities.Add("Martin");
            listCities.Add("Dover");
            listCities.Add("Dover");
            listCities.Add("Nasville");
            listCities.Add("Nashville");
            listCities.Add("Dickson");
            listCities.Add("Dickson");

            int count = 0;

            //Day column test events
            while (count < 10)
            {
                //create day events
                try
                {
                    //create an empty event
                    MVCEventBench.Models.Event myTestEvent = new Models.Event();

                    //Create a guid for the event
                    Guid gEvent = Guid.NewGuid();

                    //Create the event
                    myTestEvent.gEventGUID = gEvent;
                    myTestEvent.bIsDeleted = false;
                    myTestEvent.dEventDateStart = DateTime.Now.Date;
                    myTestEvent.dEventDateEnd = DateTime.Now.Date;
                    myTestEvent.strAddressFontFamily = "Lucida";
                    myTestEvent.strAddressFontSize = "12";
                    myTestEvent.strContact = "Test Contact";
                    myTestEvent.strDateFontFamily = "Lucida";
                    myTestEvent.strDateFontSize = "12";
                    myTestEvent.strDescription = "Test Description";
                    myTestEvent.strDetails = "Test Details";
                    myTestEvent.strEventAddress = "Test Address";
                    myTestEvent.strEventCity = listCities[count];
                    myTestEvent.strEventLocation = "Test Location";
                    myTestEvent.strEventName = "Test Day";
                    myTestEvent.strEventState = strState;
                    myTestEvent.strPhoneNumber = "931-216-0359";
                    myTestEvent.strSponsor = "Test sponsor";
                    myTestEvent.strTag1 = "Music";
                    myTestEvent.strTag2 = "Gaming";
                    myTestEvent.strTag3 = "Sports";
                    myTestEvent.strTitleFontFamily = "Lucida";
                    myTestEvent.strWebpage = "www.TestWebpage.com";

                    db.AddToEvent(myTestEvent);
                    db.SaveChanges();

                    //Upload the event image..There should only ever be but one

                    MVCEventBench.Models.Image myImage = new MVCEventBench.Models.Image();
                    myImage.gEventImageGUID = Guid.NewGuid();
                    myImage.gEvent = gEvent;

                    Image ImageTest = Image.FromFile(@"C:\Documents and Settings\Owner\My Documents\Visual Studio 2010\Projects\Event\Event\bin\MVCEventBench.root\MVCEventBench\MVCEventBench\Content\Winter.jpg");
                    MemoryStream ms = new MemoryStream();
                    ImageTest.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);


                    myImage.strAltImageName = "test";
                    myImage.bIsDeleted = false;
                    myImage.imgContent = ms.ToArray();
                    myImage.strMIMEType = "image/jpeg";
                    myImage.strFileName = "stripe.jpg";

                    db.AddToImage(myImage);
                    db.SaveChanges();

                    count++;
                }
                catch { };
            }

            //Week column test events
            count = 0;
            while (count < 10)
            {
                //create day events
                try
                {
                    //create an empty event
                    MVCEventBench.Models.Event myTestEvent = new Models.Event();

                    //Create a guid for the event
                    Guid gEvent = Guid.NewGuid();

                    //Create the event
                    myTestEvent.gEventGUID = gEvent;
                    myTestEvent.bIsDeleted = false;
                    myTestEvent.dEventDateStart = DateTime.Now.Date.AddDays(2.0);
                    myTestEvent.dEventDateEnd = DateTime.Now.Date;
                    myTestEvent.strAddressFontFamily = "Lucida";
                    myTestEvent.strAddressFontSize = "12";
                    myTestEvent.strContact = "Test Contact";
                    myTestEvent.strDateFontFamily = "Lucida";
                    myTestEvent.strDateFontSize = "12";
                    myTestEvent.strDescription = "Test Description";
                    myTestEvent.strDetails = "Test Details";
                    myTestEvent.strEventAddress = "Test Address";
                    myTestEvent.strEventCity = listCities[count];
                    myTestEvent.strEventLocation = "Test Location";
                    myTestEvent.strEventName = "Test Week";
                    myTestEvent.strEventState = strState;
                    myTestEvent.strPhoneNumber = "931-216-0359";
                    myTestEvent.strSponsor = "Test sponsor";
                    myTestEvent.strTag1 = "Music";
                    myTestEvent.strTag2 = "Gaming";
                    myTestEvent.strTag3 = "Sports";
                    myTestEvent.strTitleFontFamily = "Lucida";
                    myTestEvent.strWebpage = "www.TestWebpage.com";

                    db.AddToEvent(myTestEvent);
                    db.SaveChanges();

                    //Upload the event image..There should only ever be but one

                    MVCEventBench.Models.Image myImage = new MVCEventBench.Models.Image();
                    myImage.gEventImageGUID = Guid.NewGuid();
                    myImage.gEvent = gEvent;

                    Image ImageTest = Image.FromFile(@"C:\Documents and Settings\Owner\My Documents\Visual Studio 2010\Projects\Event\Event\bin\MVCEventBench.root\MVCEventBench\MVCEventBench\Content\Winter.jpg");
                    MemoryStream ms = new MemoryStream();
                    ImageTest.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);


                    myImage.strAltImageName = "test";
                    myImage.bIsDeleted = false;
                    myImage.imgContent = ms.ToArray();
                    myImage.strMIMEType = "image/jpeg";
                    myImage.strFileName = "stripe.jpg";

                    db.AddToImage(myImage);
                    db.SaveChanges();

                    count++;
                }
                catch { };
            }

            //Month column test events
            count = 0;
            while (count < 10)
            {
                //create day events
                try
                {
                    //create an empty event
                    MVCEventBench.Models.Event myTestEvent = new Models.Event();

                    //Create a guid for the event
                    Guid gEvent = Guid.NewGuid();

                    //Create the event
                    myTestEvent.gEventGUID = gEvent;
                    myTestEvent.bIsDeleted = false;
                    myTestEvent.dEventDateStart = DateTime.Now.Date.AddDays(12.0);
                    myTestEvent.dEventDateEnd = DateTime.Now.Date;
                    myTestEvent.strAddressFontFamily = "Lucida";
                    myTestEvent.strAddressFontSize = "12";
                    myTestEvent.strContact = "Test Contact";
                    myTestEvent.strDateFontFamily = "Lucida";
                    myTestEvent.strDateFontSize = "12";
                    myTestEvent.strDescription = "Test Description";
                    myTestEvent.strDetails = "Test Details";
                    myTestEvent.strEventAddress = "Test Address";
                    myTestEvent.strEventCity = listCities[count];
                    myTestEvent.strEventLocation = "Test Location";
                    myTestEvent.strEventName = "Test Month";
                    myTestEvent.strEventState = strState;
                    myTestEvent.strPhoneNumber = "931-216-0359";
                    myTestEvent.strSponsor = "Test sponsor";
                    myTestEvent.strTag1 = "Music";
                    myTestEvent.strTag2 = "Gaming";
                    myTestEvent.strTag3 = "Sports";
                    myTestEvent.strTitleFontFamily = "Lucida";
                    myTestEvent.strWebpage = "www.TestWebpage.com";

                    db.AddToEvent(myTestEvent);
                    db.SaveChanges();

                    //Upload the event image..There should only ever be but one

                    MVCEventBench.Models.Image myImage = new MVCEventBench.Models.Image();
                    myImage.gEventImageGUID = Guid.NewGuid();
                    myImage.gEvent = gEvent;

                    Image ImageTest = Image.FromFile(@"C:\Documents and Settings\Owner\My Documents\Visual Studio 2010\Projects\Event\Event\bin\MVCEventBench.root\MVCEventBench\MVCEventBench\Content\Winter.jpg");
                    MemoryStream ms = new MemoryStream();
                    ImageTest.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                    myImage.strAltImageName = "test";
                    myImage.bIsDeleted = false;
                    myImage.imgContent = ms.ToArray();
                    myImage.strMIMEType = "image/jpeg";
                    myImage.strFileName = "stripe.jpg";

                    db.AddToImage(myImage);
                    db.SaveChanges();

                    count++;
                }
                catch { };
            }
        }

        #endregion
    }
}
