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
        private string m_strCity;
        private string m_strState;
        private string m_strLocation;
        private string m_strImagePath;
        private DateTime m_dDateEnd;
        private DateTime m_dDateStart;
        private string m_strDescription;
        private string m_strDetails;
        private string m_strSponsor;
        private string m_strContact;
        private string m_strWebpage;
        private string m_strPhoneNumber;
        private string m_strTag1, m_strTag2, m_strTag3;

        //Control structures attributes
        private string m_strTitleFontFamily;
        private string m_strAddressFontFamily;
        private string m_strDateFontFamily;
        private string m_strTimeFontFamily;
        private string m_strAddressFontSize;
        private string m_strDateFontSize;
        private string m_strTimeFontSize;
        private string m_strDateFontColor;
        private string m_strAddressFontColor;        
        private string m_strLocationColor;
        private string m_strDateColor;


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

        public string ImgPath
        {
            get { return m_strImagePath; }
            set { m_strImagePath = value; }
        }

        public string City
        {
            get { return m_strCity; }
            set { m_strCity = value; }
        }

        public string State
        {
            get { return m_strState; }
            set { m_strState = value; }
        }

        public string Location
        {
            get { return m_strLocation; }
            set { m_strLocation = value; }
        }

        public DateTime DateStart
        {
            get { return m_dDateStart; }
            set { m_dDateStart = value; }
        }

        public DateTime DateEnd
        {
            get { return m_dDateEnd; }
            set { m_dDateEnd = value; }
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

        public string AddressFontColor
        {
            get { return m_strAddressFontColor; }
            set { m_strAddressFontColor = value; }
        }

        public string DateFontColor
        {
            get { return m_strDateFontColor; }
            set { m_strDateFontColor = value; }
        }

        public string LocationColor
        {
            get { return m_strLocationColor; }
            set { m_strLocationColor = value; }
        }

        public string DateColor
        {
            get { return m_strDateColor; }
            set { m_strDateColor = value; }
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
            m_dDateStart = DateTime.Now.Date;
            m_strAddress = "42 Wallaby Way, Sydney, Australia";
            m_strLocation = "My House";
            m_strCity = "Clarksville";
            m_strState = "Tennessee";
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

            m_strDateFontColor = "Black";
            m_strAddressFontColor = "Black";
        }

        #endregion

    }
}