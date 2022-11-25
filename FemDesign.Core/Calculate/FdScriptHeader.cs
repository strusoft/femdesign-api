// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;

namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd    
    /// FDSCRIPTHEADER
    /// </summary>
    [XmlRoot("fdscriptheader")]
    [System.Serializable]
    public partial class FdScriptHeaderPipe : CmdCommand
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
        private FdScriptHeaderPipe()
        {
            
        }
        public FdScriptHeaderPipe(string title, string logfile)
        {
            this.Title = title;
            this.Version = "2100";
            this.Module = "sframe";
            this.LogFile = logfile;
        }

        public FdScriptHeaderPipe(string logFilePath)
        {
            Title = "FEM-Design script";
            Version = "2100";
            Module = "SFRAME";
            LogFile = System.IO.Path.GetFullPath(logFilePath);
        }

        public override XElement ToXElement()
        {
            return Extension.ToXElement<FdScriptHeaderPipe>(this);
        }

    }


    /// <summary>
    /// fdscript.xsd    
    /// FDSCRIPTHEADER
    /// </summary>
    [System.Serializable]
    public partial class FdScriptHeader
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
            this.Version = "2100";
            this.Module = "sframe";
            this.LogFile = logfile;
        }

    }



}