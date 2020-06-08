// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd    
    /// FDSCRIPTHEADER
    /// </summary>
    public class FdScriptHeader
    {
        [XmlElement("title")]
        public string title { get; set; } // SZBUF
        [XmlElement("version")]
        public string version { get; set; } // SZNAME
        [XmlElement("module")]
        public string module { get; set; } // SZPATH (?)
        [XmlElement("logfile")]
        public string logfile { get; set; } // SZPATH

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private FdScriptHeader()
        {
            
        }
        public FdScriptHeader(string title, string logfile)
        {
            this.title = title;
            this.version = "1900";
            this.module = "SFRAME";
            this.logfile = logfile;
        }
    }
}