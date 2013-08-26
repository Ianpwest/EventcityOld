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
        private string m_strTitle;
        private string m_strAddress;
        private string m_strTime;
        private string m_strImagePath;
        private DateTime m_dDate;

        //Control structures attributes
        private string m_strTitleFontFamily;
        private string m_strAddressFontFamily;
        private string m_strDateFontFamily;
        private string m_strTimeFontFamily;

        #endregion

        #region Accessors for Member Variables
        
        public string Title
        {
            get { return m_strTitle; }
            set { m_strTitle = value; }
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


        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with no parameters
        /// </summary>
        public NailVent()
        {
            m_strTitle = "Default Title";
            m_strTime = "4:20pm";
            m_dDate = DateTime.Now.Date;
            m_strAddress = "42 Wallaby Way, Sydney, Australia";
            m_strImagePath = string.Empty;

            m_strTitleFontFamily = new FontFamily("Harlow Solid Italic").ToString();
            m_strAddressFontFamily = new FontFamily("Harlow Solid Italic").ToString();
            m_strDateFontFamily = new FontFamily("Harlow Solid Italic").ToString();
            m_strTimeFontFamily = new FontFamily("Harlow Solid Italic").ToString();
        }

        #endregion
    }
}