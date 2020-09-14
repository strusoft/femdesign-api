// https://strusoft.com/
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd    
    /// FDSCRIPTHEADER
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class FdScriptHeader
    {
        [XmlElement("title")]
        public string Title { get; set; } // SZBUF
        [XmlElement("version")]
        public string Version { get; set; } // SZNAME
        [XmlElement("module")]
        public string Module { get; set; } // SZPATH (?)
        [XmlElement("logfile")]
        public string LogFile { get; set; } // SZPATH

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private FdScriptHeader()
        {
            
        }
        public FdScriptHeader(string title, string logfile)
        {
            this.Title = title;
            this.Version = "1900";
            this.Module = "SFRAME";
            this.LogFile = logfile;
        }
    }
}