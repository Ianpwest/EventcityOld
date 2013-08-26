using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCEventBench.Classes;
using System.Data;

namespace MVCEventBench.Classes
{
    public class BuildControls
    {
        /// <summary>
        /// Local instance of the database
        /// </summary>
        MVCEventBench.Models.dbEventModel m_db = new Models.dbEventModel();

        #region Utility

        /// <summary>
        /// Builds a nailvent control given a guid
        /// </summary>
        /// <param name="gEventGUID">guid of the event</param>
        /// <returns>nailvent control for the specified guid</returns>
        public NailVent BuildEventControl(Guid gEventGUID)
        {
            var qryResults = from r in m_db.Event
                          where (r.bIsDeleted == false && r.gEventGUID == gEventGUID)
                          select r;

            foreach (MVCEventBench.Models.Event results in qryResults)
            {

                NailVent nv = new NailVent();
                //Initialize the events main properties
                nv.EventGUID = results.gEventGUID;
                nv.Name = results.strEventName;
                nv.Location = results.strEventLocation;
                nv.City = results.strEventCity;
                nv.State = results.strEventState;
                nv.Address = results.strEventAddress;
                try
                {
                    nv.DateStart = (DateTime)results.dEventDateStart;
                }
                catch
                {
                    nv.DateStart = DateTime.Now;
                }
                try
                {
                    nv.DateEnd = (DateTime)results.dEventDateEnd;
                }
                catch
                {
                    nv.DateEnd = DateTime.MaxValue;
                }
                nv.Contact = results.strContact;
                nv.Description = results.strDescription;
                nv.Details = results.strDetails;
                nv.PhoneNumber = results.strPhoneNumber;
                nv.Sponsor = results.strSponsor;
                nv.Webpage = results.strWebpage;

                //Initialize the properties
                nv.AddressFontFamily = results.strAddressFontFamily;
                nv.AddressFontSize = results.strAddressFontSize;
                nv.DateFontFamily = results.strDateFontFamily;
                nv.DateFontSize = results.strDateFontSize;
                

                return nv;
            }

            //Nothing found
            return null;
        }

        /// <summary>
        /// Method to parse the Events result query into actual controls.
        /// </summary>
        /// <param name="qryResults">Results set or the database query</param>
        public void QryResultsToColumnControls(System.Data.Objects.ObjectQuery qryResults,List<NailVent> listNailVentToday,List<NailVent> listNailVentWeek,List<NailVent> listNailVentMonth)
        {
            
            DateTime dateNow = DateTime.Now;
            string strDateNow = dateNow.ToString("d");

            foreach (MVCEventBench.Models.Event result in qryResults)
            {
                //Create a new event
                NailVent nv = new NailVent();

                //Initialize the events main properties
                nv.EventGUID = result.gEventGUID;
                nv.Name = result.strEventName;
                nv.Address = result.strEventAddress;
                try
                {
                    nv.DateStart = (DateTime)result.dEventDateStart;
                }
                catch
                {
                    nv.DateStart = DateTime.Now;
                }
                nv.Location = result.strEventLocation;
                nv.City = result.strEventCity;
                nv.State = result.strEventState;
                nv.Contact = result.strContact;
                nv.Description = result.strDescription;
                nv.Details = result.strDetails;
                nv.PhoneNumber = result.strPhoneNumber;
                nv.Sponsor = result.strSponsor;
                nv.Webpage = result.strWebpage;
                
              

                //Initialize the properties
                nv.AddressFontFamily = result.strAddressFontFamily;
                nv.AddressFontSize = result.strAddressFontSize;
                nv.DateFontFamily = result.strDateFontFamily;
                nv.DateFontSize = result.strDateFontSize;
                nv.AddressFontColor = result.strAddressFontColor;
                nv.DateFontColor = result.strDateFontColor;
                nv.DateColor = result.strDateColor;                
                nv.LocationColor = result.strLocationColor;
                

                //If this is an event that is past date don't process it
                DateTime.TryParse(strDateNow, out dateNow);
                if (nv.DateStart < dateNow)
                {
                    continue;
                }
                else
                {
                    string strTimeCategory = FindEventTimeCategory(nv.DateStart);
                    switch (strTimeCategory)
                    {
                        case "week":
                            {
                                listNailVentToday.Add(nv);
                                break;
                            }
                        case "month":
                            {
                                listNailVentWeek.Add(nv);
                                break;
                            }
                        case "later":
                            {
                                listNailVentMonth.Add(nv);
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
        }

        /// <summary>
        /// Finds if the event is associated with this day, week, month or later
        /// </summary>
        /// <param name="dateEvent">Date of the event in question</param>
        /// <returns>string describing the most appropriate time frame</returns>
        private string FindEventTimeCategory(DateTime dateEvent)
        {
            DateTime dateNow = DateTime.Now;           
            TimeSpan tsTodayToEvent = dateEvent - dateNow;

            if ((dateEvent.Year < dateNow.Year) || (dateEvent.Year == dateNow.Year && dateEvent.Month < dateNow.Month) || ( dateEvent.Year == dateNow.Year && dateEvent.Month == dateNow.Month && dateEvent.Day < dateNow.Day))
            {
                return "prior";
            }
            else if ((dateEvent.DayOfWeek - dateNow.DayOfWeek >= 0) && (tsTodayToEvent.Days < 7))
            {
                return "week";
            }
            else if (dateEvent.Month == dateNow.Month && dateEvent.Year == dateNow.Year)
            {
                return "month";
            }
            else
            {
                return "later";
            }
           

        }     
        
        #endregion
    }
}