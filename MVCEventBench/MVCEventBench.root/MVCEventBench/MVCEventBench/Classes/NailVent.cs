using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Media;

//Conversion notes
//Must add the presentationcore.dll to the references
//for access to the FontFamily class

namespace MVCEventBench.Classes
{
    public class NailVent
    {
        #region Private Member Variables

        //Control structures
        private Guid m_gEventGUID;
        private string m_strName;
        private string m_strAddress;
        private string m_strTime;
        private string m_strImagePath;
        private DateTime m_dDate;
        private string m_strDescription;
        private string m_strDetails;
        private string m_strSponsor;
        private string m_strContact;
        private string m_strWebpage;
        private string m_strPhoneNumber;
        private string m_strTag1, m_strTag2, m_strTag3, m_strTag4, m_strTag5;

        //Control structures attributes
        private string m_strTitleFontFamily;
        private string m_strAddressFontFamily;
        private string m_strDateFontFamily;
        private string m_strTimeFontFamily;
        private string m_strAddressFontSize;
        private string m_strDateFontSize;
        private string m_strTimeFontSize;


        #endregion

        #region Accessors for Member Variables

        public Guid EventGUID
        {
            get { return m_gEventGUID; }
            set { m_gEventGUID = value; }
        }

        public string PhoneNumber
        {
            get { return m_strPhoneNumber; }
            set { m_strPhoneNumber = value; }
        }

        public string Webpage
        {
            get { return m_strWebpage; }
            set { m_strWebpage = value; }
        }

        public string Contact
        {
            get { return m_strContact; }
            set { m_strContact = value; }
        }

        public string Sponsor
        {
            get { return m_strSponsor; }
            set { m_strSponsor = value; }
        }

        public string Details
        {
            get { return m_strDetails; }
            set { m_strDetails = value; }
        }

        public string Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }

        public string Name
        {
            get { return m_strName; }
            set { m_strName = value; }
        }

        public string Address
        {
            get { return m_strAddress; }
            set { m_strAddress = value; }
        }

        public string Time
        {
            get { return m_strTime; }
            set { m_strTime = value; }
        }
        
        public string ImgPath
        {
            get { return m_strImagePath; }
            set { m_strImagePath = value; }
        }

        public DateTime Date
        {
            get { return m_dDate; }
            set { m_dDate = value; }
        }

        public string TitleFontFamily
        {
            get { return m_strTitleFontFamily; }
            set { m_strTitleFontFamily = value; }
        }

        public string AddressFontFamily
        {
            get { return m_strAddressFontFamily; }
            set { m_strAddressFontFamily = value; }
        }

        public string DateFontFamily
        {
            get { return m_strDateFontFamily; }
            set { m_strDateFontFamily = value; }
        }

        public string TimeFontFamily
        {
            get { return m_strTimeFontFamily; }
            set { m_strTimeFontFamily = value; }
        }

        public string TimeFontSize
        {
            get { return m_strTimeFontSize; }
            set { m_strTimeFontSize = value; }
        }

        public string DateFontSize
        {
            get { return m_strDateFontSize; }
            set { m_strDateFontSize = value; }
        }

        public string AddressFontSize
        {
            get { return m_strAddressFontSize; }
            set { m_strAddressFontSize = value; }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with no parameters
        /// </summary>
        public NailVent()
        {
            //Default information
            m_strName = "Default Title";
            m_strTime = "4:20pm";
            m_dDate = DateTime.Now.Date;
            m_strAddress = "42 Wallaby Way, Sydney, Australia";
            m_strImagePath = string.Empty;

            //Not all font families work with all browsers/computers. Might want to revisit this problem sometime
            m_strTitleFontFamily = new FontFamily("Lucida Calligraphy").ToString();
            m_strAddressFontFamily = new FontFamily("Harlow Solid Italic").ToString();
            m_strDateFontFamily = new FontFamily("Harlow Solid Italic").ToString();
            m_strTimeFontFamily = new FontFamily("Harlow Solid Italic").ToString();

            //This is set as a string value due to the way css handles font-size properties
            m_strAddressFontSize = "12px";
            m_strDateFontSize ="12px";
            m_strTimeFontSize = "12px";
        }

        #endregion

    }
}