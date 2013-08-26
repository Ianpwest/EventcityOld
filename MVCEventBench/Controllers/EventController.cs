using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCEventBench.Classes;
using System.Web.Helpers;
using System.IO;
using System.Drawing;

namespace MVCEventBench.Controllers
{
    public class EventController : Controller
    {
        /// <summary>
        /// Local instance of the dbEventModel
        /// </summary>
        private MVCEventBench.Models.dbEventModel m_db = new Models.dbEventModel();

        /// <summary>
        /// Pulls the event description information from the database to use in the jquery qtip
        /// </summary>
        /// <param name="strEventGUID">GUID of the selected event</param>
        /// <returns>Json object containing the description information</returns>
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
        /// <summary>
        /// Event Creation controller. Combobox creation is handled here
        /// </summary>
        /// <returns>View/insantiated objects passed through the viewbag</returns>
        public ActionResult Create()
        {
            GetComboboxItems();
            List<String> listCities = new List<String>();
            SelectList myCitiesList = new SelectList(listCities);
            ViewBag.myCitiesList = myCitiesList;
                        
            return View();
        } 

        //
        // POST: /Event/Create
        /// <summary>
        /// Create post. Collects Event model info and posts to the database
        /// </summary>
        /// <param name="collection">Form Collection</param>
        /// <returns>View</returns>
        [HttpPost]
        public ActionResult Create(MVCEventBench.Models.Event eventSave)
        {
            //// Validation on fields before passing the information to the database
            //// When the flag = true, the value is not valid.
            //bool bFlagTitle = false;
            //bool bFlagImage = false;
            //bool bFlagAddress = false;
            //bool bFlagState = false;            
            //bool bFlagSponsor = false;
            //bool bFlagPhone = false;            
            //bool bFlagLocation = false;
            //bool bFlagDate = false;
            //int intParseTest;
            //bool bParseTest;
            //int intCompareDateTime;
            //DateTime dParseTest;

            //bool bFlagInvalidData = false;
            //string strInvalidData = "Some of the data you entered is incorrect. Please correct these values and re-submit the form broski!";

            ////Event Title Validation - cannot be less than 4 characters
            //if (eventSave.strEventName.Length < 4)
            //    bFlagTitle = true;
            ////Image Validation - must have selected a file, but will wait on this for now.
            ////Address Validation - cannot be less than 4 characters
            //if (eventSave.strEventAddress.Length < 4)
            //    bFlagAddress = true;
            ////State Validation - cannot be less than 2 characters and cannot be a number
            ////Actually, we might just use comboboxes for these, but meh.
            //if (eventSave.strEventState.Length < 2 || int.TryParse(eventSave.strEventState.ToString(), out intParseTest))
            //    bFlagState = true;
            ////City Validation - probably going to have drop downs, so ignoring for now
            ////Sponsor Validation - cannot be less than 4 characters
            //if (eventSave.strSponsor.Length < 4)
            //    bFlagSponsor = true;
            ////Phone Validation - cannot include characters or be less than 7 digits
            //if (!int.TryParse(eventSave.strPhoneNumber.ToString(), out intParseTest) || eventSave.strPhoneNumber.Length < 7)
            //    bFlagPhone = true;
            ////Location Validation - cannot be less than 4 characters
            //if (eventSave.strEventLocation.Length < 4)
            //    bFlagLocation = true;
            ////Date Validation - must be a datetime valid input and also not earlier than current time
            ////Added try-catch because the comparison test requires a non-null DateTime.
            //try
            //{
            //    if (!DateTime.TryParse(eventSave.dEventDateStart.ToString(), out dParseTest))
            //        bFlagDate = true;
            //    intCompareDateTime = DateTime.Compare(DateTime.Today, (DateTime)eventSave.dEventDateStart);
            //    if (intCompareDateTime > 0)
            //        bFlagDate = true;
            //}
            //catch
            //{
            //    bFlagDate = true;
            //}

            //if(bFlagAddress || bFlagDate || bFlagImage || bFlagLocation || bFlagPhone || bFlagSponsor || bFlagState || bFlagTitle)
            //    bFlagInvalidData = true;
            //if(bFlagInvalidData)
            //{
            //    return View();
            //}

            MVCEventBench.Models.dbEventModel db = new MVCEventBench.Models.dbEventModel();         

            try
            {
                //Create a guid for the event
                Guid gEvent = Guid.NewGuid();

                //Create the event
                eventSave.gEventGUID = gEvent;
                db.AddToEvent(eventSave);
                db.SaveChanges();

                //Upload the event image..There should only ever be but one
                foreach (string upload in Request.Files)
                {
                    //Add logic here to only accept certain MIME types?
                    string strMimeType = Request.Files[upload].ContentType;
                    Stream streamFileStream = Request.Files[upload].InputStream;
                    string strFileName = Path.GetFileName(Request.Files[upload].FileName);
                    
                    int fileLength = Request.Files[upload].ContentLength;
                    byte[] fileData = new byte[fileLength];
                    streamFileStream.Read(fileData, 0, fileLength);
                    fileData = ResizeImage(fileData, fileLength);

                    MVCEventBench.Models.Image myImage = new MVCEventBench.Models.Image();
                    myImage.gEventImageGUID = Guid.NewGuid();
                    myImage.gEvent = gEvent;
                    myImage.imgContent = fileData;
                    myImage.strMIMEType = strMimeType;
                    myImage.strFileName = strFileName;

                    db.AddToImage(myImage);
                    db.SaveChanges();
                }
                  
                return RedirectToAction("/Home/Index");
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

        #region Utility
        /// <summary>
        /// Initializing static Combobox data for Event/Create 
        /// </summary>
        private void GetComboboxItems()
        {
            List<String> listFontCombobox = new List<String>();
            listFontCombobox.Add("Arial");
            listFontCombobox.Add("Comic Sans MS");
            listFontCombobox.Add("Georgia");
            listFontCombobox.Add("Lucida Console");
            listFontCombobox.Add("Times New Roman");
            listFontCombobox.Add("Verdana");
            listFontCombobox.Add("MS Sans Serif");
            SelectList myFontList = new SelectList(listFontCombobox);
            ViewBag.myFontList = myFontList;

            List<Int32> listFontSizeCombobox = new List<Int32>();
            for (int i = 15; i <= 25; i++)
            {
                listFontSizeCombobox.Add(i);
            }
            SelectList myFontSizeList = new SelectList(listFontSizeCombobox);
            ViewBag.myFontSizeList = myFontSizeList;

            List<String> listTag = new List<String>();
            listTag.Add("Animals");
            listTag.Add("Beach");
            listTag.Add("Beer");
            listTag.Add("Boating");
            listTag.Add("Carnival");
            listTag.Add("Celebration");
            listTag.Add("Climbing");
            listTag.Add("Concert");
            listTag.Add("Festival");
            listTag.Add("Fishing");
            listTag.Add("Food");
            listTag.Add("Forest");
            listTag.Add("Free");
            listTag.Add("Games");
            listTag.Add("Historical");
            listTag.Add("Holiday");
            listTag.Add("Mountains");
            listTag.Add("Movies");
            listTag.Add("Music");
            listTag.Add("Outdoors");
            listTag.Add("Racing");
            listTag.Add("Sports");
            listTag.Add("Swimming");
            listTag.Add("Video Games");
            listTag.Add("Wine");                                                            
            SelectList myTagList = new SelectList(listTag);
            ViewBag.myTagList = myTagList;

            List<String> listColors = new List<string>();
            listColors.Add("Black");
            listColors.Add("Blue");
            listColors.Add("Brown");
            listColors.Add("Gold");
            listColors.Add("Grey");
            listColors.Add("Green");
            listColors.Add("Orange");
            listColors.Add("Pink");
            listColors.Add("Purple");
            listColors.Add("Red");
            listColors.Add("Silver");
            listColors.Add("Yellow");
            listColors.Add("White");
            SelectList myColorList = new SelectList(listColors);
            ViewBag.myColorList = myColorList;
            
            List<String> listStates = new List<String>();

            //Add in the blank option
            listStates.Add("");
            var statesQuery = from r in m_db.States
                              orderby r.strName
                              select r.strName;
            foreach (var str in statesQuery)
            {
                listStates.Add(str.ToString());
            }
            SelectList myStatesList = new SelectList(listStates);
            ViewBag.myStatesList = myStatesList;
        }

       
        /// <summary>
        /// Takes an image byte array and resizes it to a set height, width
        /// </summary>
        /// <param name="arryImage">byte array containing the image data</param>
        /// <returns>Resized byte array data</returns>
        private byte[] ResizeImage(byte[] arryImage, int fileLength)
        {
            //Memory stream for old image
            MemoryStream msImage = new MemoryStream();

            //Memory stream for the resized image
            MemoryStream msNewImage = new MemoryStream();

            msImage.Write(arryImage, 0, fileLength);

            Bitmap bmpImage = new Bitmap(msImage);
            
            //Set the constant for the size of the image here.
            Bitmap resizedImage = new Bitmap(400, 200);
            
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.DrawImage(bmpImage, new Rectangle(0, 0, 400, 200),
                    new Rectangle(0, 0, bmpImage.Width, bmpImage.Height), GraphicsUnit.Pixel);
            }

            resizedImage.Save(msNewImage, System.Drawing.Imaging.ImageFormat.Jpeg);

            return msNewImage.ToArray();
  
        }
        #endregion

    }
}
