// https://strusoft.com/
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// fdscript root
    /// </summary>
    [XmlRoot("fdscript")]
    public class FdScript
    {
        [XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string xmlattrib { get; set; }
        [XmlElement("fdscriptheader", Order = 1)]
        public FdScriptHeader fdScriptHeader { get; set; } // FDSCRIPTHEADER
        [XmlElement("cmdopen", Order = 2)]
        public CmdOpen cmdOpen { get; set; } // CMDOPEN
        [XmlElement("cmduser", Order = 3)]
        public CmdUser cmdUser { get; set;} // CMDUSER
        [XmlElement("cmdcalculation", Order = 4)]
        public CmdCalculation cmdCalculation { get; set; } // CMDCALCULATION
        [XmlElement("cmdlistgen", Order = 5)]
        public List<CmdListGen> cmdListGen { get; set; } // CMDLISTGEN
        [XmlElement("cmdchild", Order = 6)]
        public string docxTemplatePath { get; set; } // DOCXTEMPLATEPATH
        [XmlElement("cmdsavedocx", Order = 7)]
        public CmdSaveDocx cmdSaveDocx { get; set;} // CMDSAVEDOCX
        [XmlElement("cmdsave", Order = 8)]
        public CmdSave cmdSave { get; set; } // CMDSAVE
        [XmlElement("cmdendsession", Order = 9)]
        public CmdEndSession cmdEndSession { get; set; } // CMDENDSESSION
        [XmlIgnore]
        internal string struxmlPath {get; set; } // path to struxml file, string 259
        [XmlIgnore]
        internal string filename { get; set; } // file name of struxlm file, string 259
        [XmlIgnore]
        internal string cwd { get; set; } // current work directory, string
        [XmlIgnore]
        internal string fdScriptPath { get; set; } // path to fdscript file, string

        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private FdScript()
        {
            
        }

        /// <summary>
        /// Create fdscript to perform a calculation.
        /// </summary>
        internal static FdScript CalculateStruxml(string struxmlPath, string mode, string bscPath, string docxTemplatePath, bool endSession)
        {
            FdScript obj = new FdScript();

            //
            obj.xmlattrib = "fdscript.xsd";
            obj.struxmlPath = struxmlPath;
            obj.filename = Path.GetFileName(obj.struxmlPath).Split('.')[0];
            obj.cwd = Path.GetDirectoryName(obj.struxmlPath);
            obj.fdScriptPath = obj.cwd + @"\" + obj.filename + ".fdscript";

            // set header and logfile
            obj.fdScriptHeader = new FdScriptHeader("Generated script.", obj.cwd + @"\logfile.log");
            
            // set open
            obj.cmdOpen = new CmdOpen(obj.struxmlPath);

            // set user
            obj.cmdUser = new CmdUser(mode);

            // set batch
            if (bscPath != "")
            {
                obj.cmdListGen = new List<CmdListGen>{new CmdListGen(bscPath, obj.cwd)};
            }

            // set save docx
            if (docxTemplatePath != "")
            {
                obj.docxTemplatePath = docxTemplatePath;
                obj.cmdSaveDocx = new CmdSaveDocx(obj.filename + ".docx");
            }

            // set save
            obj.cmdSave = new CmdSave(obj.cwd + @"\" + obj.filename + ".str");

            // set endsession
            if (endSession)
            {
                obj.cmdEndSession = new CmdEndSession();
            }

            // return
            return obj;
        }

        /// <summary>
        /// Create fdscript to read a str-model.
        /// </summary>
        internal static FdScript ReadStr(string strPath, List<string> bscPath = null)
        {
            //
            FdScript obj = new FdScript();

            //
            obj.xmlattrib = "fdscript.xsd";
            obj.filename = Path.GetFileName(strPath).Split('.')[0];
            obj.cwd = Path.GetDirectoryName(strPath);
            obj.struxmlPath = obj.cwd + @"\" + obj.filename + ".struxml";
            obj.fdScriptPath = obj.cwd + @"\" + obj.filename + ".fdscript";

            // set header and logfile
            obj.fdScriptHeader = new FdScriptHeader("Generated script.", obj.cwd + @"\logfile.log");

            // open str
            obj.cmdOpen = new CmdOpen(strPath);

            // listgen
            if (bscPath != null)
            {
                obj.cmdListGen = new List<CmdListGen>();
                foreach (string item in bscPath)
                {
                    obj.cmdListGen.Add(new CmdListGen(item, obj.cwd));
                }  
            }

            // save as .struxml
            obj.cmdSave = new CmdSave(obj.struxmlPath);

            // end session
            obj.cmdEndSession = new CmdEndSession();

            // return
            return obj;
        }

        /// <summary>
        /// Create fdscript to run analysis.
        /// </summary>
        internal static FdScript Analysis(string struxmlPath, Analysis analysis, string bscPath, string docxTemplatePath, bool endSession)
        {
            string mode = "RESMODE";
            FdScript fdScript = FdScript.CalculateStruxml(struxmlPath, mode, bscPath, docxTemplatePath, endSession);
            fdScript.cmdCalculation = new CmdCalculation(analysis);
            return fdScript;
        }

        /// <summary>
        /// Create fdscript to run analysis and design.
        /// </summary>
        internal static FdScript Design(string mode, string struxmlPath, Analysis analysis, Design design, string bscPath, string docxTemplatePath, bool endSession)
        {
            // get mode
            switch (mode)
            {
                case "rc":
                    mode = "RCDESIGN";
                    break;
                case "steel":
                    mode = "STEELDESIGN";
                    break;
                case "timber":
                    mode = "TIMBERDESIGN";
                    break;
                default:
                    throw new System.ArgumentException("Mode is not supported. Mode should be rc, steel or timber");
            }
            
            FdScript fdScript = FdScript.CalculateStruxml(struxmlPath, mode, bscPath, docxTemplatePath, endSession);
            fdScript.cmdCalculation = new CmdCalculation(analysis, design);
            return fdScript;
        }

        /// <summary>
        /// Serialize fdscript.
        /// </summary>
        internal void SerializeFdScript()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FdScript));
            using (TextWriter writer = new StreamWriter(this.fdScriptPath))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}