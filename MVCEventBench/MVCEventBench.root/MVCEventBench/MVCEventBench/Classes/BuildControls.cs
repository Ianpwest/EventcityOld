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
                nv.Address = results.strEventAddress;
                nv.Date = (DateTime)results.dEventDate;
                nv.Time = results.strEventTime;
                nv.Contact = results.strContact;
                nv.Description = results.strDescription;
                nv.Details = results.strDetails;
                nv.PhoneNumber = results.strPhoneNumber;
                nv.Sponsor = results.strSponsor;
                nv.Webpage = results.strWebpage;

                if (results.imgEvent != null)
                {
                    nv.ImgPath = results.imgEvent.ToString();
                }

                //Initialize the properties
                nv.AddressFontFamily = results.strAddressFontFamily;
                nv.AddressFontSize = results.strAddressFontSize;
                nv.DateFontFamily = results.strDateFontFamily;
                nv.DateFontSize = results.strDateFontSize;
                nv.TimeFontFamily = results.strTimeFontFamily;
                nv.TimeFontSize = results.strTimeFontSize;

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
                nv.Date = (DateTime)result.dEventDate;
                nv.Time = result.strEventTime;
                nv.Contact = result.strContact;
                nv.Description = result.strDescription;
                nv.Details = result.strDetails;
                nv.PhoneNumber = result.strPhoneNumber;
                nv.Sponsor = result.strSponsor;
                nv.Webpage = result.strWebpage;
              
                if (result.imgEvent != null)
                {
                    nv.ImgPath = result.imgEvent.ToString();
                }

                //Initialize the properties
                nv.AddressFontFamily = result.strAddressFontFamily;
                nv.AddressFontSize = result.strAddressFontSize;
                nv.DateFontFamily = result.strDateFontFamily;
                nv.DateFontSize = result.strDateFontSize;
                nv.TimeFontFamily = result.strTimeFontFamily;
                nv.TimeFontSize = result.strTimeFontSize;

                //If this is an event that is past date don't process it
                DateTime.TryParse(strDateNow, out dateNow);
                if (nv.Date < dateNow)
                {
                    continue;
                }
                else
                {
                    string strTimeCategory = FindEventTimeCategory(nv.Date);
                    switch (strTimeCategory)
                    {
                        case "day":
                            {
                                listNailVentToday.Add(nv);
                                break;
                            }
                        case "week":
                            {
                                listNailVentWeek.Add(nv);
                                break;
                            }
                        case "month":
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
            DateTime dateNextMonday;
            DateTime dateMonday = StartOfWeek(dateNow, DayOfWeek.Monday);
            DateTime dateStartOfMonth = new DateTime(dateNow.Year, dateNow.Month, 1);

            bool bUseNextWeek = false;

            //Logic to decide what range we use for the "week" column
            if (dateNow.DayOfWeek >= DayOfWeek.Saturday)
            {
                if (dateNow.DayOfWeek == DayOfWeek.Saturday)
                {
                    dateNextMonday = dateNow.AddDays(2);
                }
                else
                {
                    dateNextMonday = dateNow.AddDays(1);
                }

                if (dateEvent <= dateNextMonday.AddDays(6))
                {
                    bUseNextWeek = true;
                }
            }

            //If the event is today
            if (dateEvent.Day == dateNow.Day)
            {
                return "day";
            }
            //If it falls in the current week
            else if (dateEvent < dateMonday.AddDays(6) || (bUseNextWeek == true))
            {
                return "week";
            }

            //If it falls in the current month
            else if (dateEvent <= dateStartOfMonth.AddDays(DateTime.DaysInMonth(dateNow.Year, dateNow.Month)))
            {
                return "month";
            }
            //It's more than a month away
            else
            {
                return "later";
            }

        }


        /// <summary>
        /// Determines the start day of the week
        /// </summary>
        /// <param name="dt">Todays date</param>
        /// <param name="startOfWeek">The day you want to be the start</param>
        /// <returns>Start date of the week</returns>
        private static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }
        #endregion
    }
}