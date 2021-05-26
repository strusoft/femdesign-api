// https://strusoft.com/
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// fdscript root
    /// </summary>
    [XmlRoot("fdscript")]
    public partial class FdScript
    {
        [XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string XmlAttrib { get; set; }
        [XmlElement("fdscriptheader", Order = 1)]
        public FdScriptHeader FdScriptHeader { get; set; } // FDSCRIPTHEADER
        [XmlElement("cmdopen", Order = 2)]
        public CmdOpen CmdOpen { get; set; } // CMDOPEN
        [XmlElement("cmduser", Order = 3)]
        public CmdUser CmdUser { get; set;} // CMDUSER
        [XmlElement("cmdcalculation", Order = 4)]
        public CmdCalculation CmdCalculation { get; set; } // CMDCALCULATION
        [XmlElement("cmdlistgen", Order = 5)]
        public List<CmdListGen> CmdListGen { get; set; } // CMDLISTGEN
        [XmlElement("cmdchild", Order = 6)]
        public string DocxTemplatePath { get; set; } // DOCXTEMPLATEPATH
        [XmlElement("cmdsavedocx", Order = 7)]
        public CmdSaveDocx CmdSaveDocx { get; set;} // CMDSAVEDOCX
        [XmlElement("cmdsave", Order = 8)]
        public CmdSave CmdSave { get; set; } // CMDSAVE
        [XmlElement("cmdendsession", Order = 9)]
        public CmdEndSession CmdEndSession { get; set; } // CMDENDSESSION
        [XmlIgnore]
        public string StruxmlPath {get; set; } // path to struxml file, string 259
        [XmlIgnore]
        public string FileName { get; set; } // file name of struxlm file, string 259
        [XmlIgnore]
        public string Cwd { get; set; } // current work directory, string
        [XmlIgnore]
        public string FdScriptPath { get; set; } // path to fdscript file, string

        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private FdScript()
        {
            
        }

        /// <summary>
        /// Create fdscript to perform a calculation.
        /// </summary>
        internal static FdScript CalculateStruxml(string struxmlPath, string mode, List<string> bscPath, string docxTemplatePath, bool endSession)
        {
            FdScript obj = new FdScript();

            //
            obj.XmlAttrib = "fdscript.xsd";
            obj.StruxmlPath = struxmlPath;
            obj.FileName = Path.GetFileName(obj.StruxmlPath).Split('.')[0];
            obj.Cwd = Path.GetDirectoryName(obj.StruxmlPath);
            obj.FdScriptPath = obj.Cwd + @"\" + obj.FileName + ".fdscript";

            // set header and logfile
            obj.FdScriptHeader = new FdScriptHeader("Generated script.", obj.Cwd + @"\logfile.log");
            
            // set open
            obj.CmdOpen = new CmdOpen(obj.StruxmlPath);

            // set user
            obj.CmdUser = new CmdUser(mode);

            // listgen
            if (bscPath != null && bscPath.Any())
            {
                obj.CmdListGen = new List<CmdListGen>();
                foreach (string item in bscPath)
                {
                    obj.CmdListGen.Add(new CmdListGen(item, obj.Cwd));
                }  
            }

            // set save docx
            if (docxTemplatePath != "" || docxTemplatePath != null)
            {
                // path to .dsc-file (template file)
                obj.DocxTemplatePath = docxTemplatePath;

                // object containing command to generate .docx and path to generated .docx
                obj.CmdSaveDocx = new CmdSaveDocx(obj.FileName + ".docx");
            }

            // set save
            obj.CmdSave = new CmdSave(obj.Cwd + @"\" + obj.FileName + ".str");

            // set endsession
            if (endSession)
            {
                obj.CmdEndSession = new CmdEndSession();
            }

            // return
            return obj;
        }

        /// Create fdscript to read a str-model.
        public static FdScript ReadStr(string strPath, List<string> bscPath = null)
        {
            //
            FdScript obj = new FdScript();

            //
            obj.XmlAttrib = "fdscript.xsd";
            obj.FileName = Path.GetFileName(strPath).Split('.')[0];
            obj.Cwd = Path.GetDirectoryName(strPath);
            obj.StruxmlPath = obj.Cwd + @"\" + obj.FileName + ".struxml";
            obj.FdScriptPath = obj.Cwd + @"\" + obj.FileName + ".fdscript";

            // set header and logfile
            obj.FdScriptHeader = new FdScriptHeader("Generated script.", obj.Cwd + @"\logfile.log");

            // open str
            obj.CmdOpen = new CmdOpen(strPath);

            // listgen
            if (bscPath != null && bscPath.Any())
            {
                obj.CmdListGen = new List<CmdListGen>();
                foreach (string item in bscPath)
                {
                    obj.CmdListGen.Add(new CmdListGen(item, obj.Cwd));
                }  
            }

            // save as .struxml
            obj.CmdSave = new CmdSave(obj.StruxmlPath);

            // end session
            obj.CmdEndSession = new CmdEndSession();

            // return
            return obj;
        }

        /// <summary>
        /// Create fdscript to run analysis.
        /// </summary>
        public static FdScript Analysis(string struxmlPath, Analysis analysis, List<string> bscPath, string docxTemplatePath, bool endSession)
        {
            string mode = "RESMODE";
            FdScript fdScript = FdScript.CalculateStruxml(struxmlPath, mode, bscPath, docxTemplatePath, endSession);
            fdScript.CmdCalculation = new CmdCalculation(analysis);
            return fdScript;
        }

        /// <summary>
        /// Create fdscript to run analysis and design.
        /// </summary>
        public static FdScript Design(string mode, string struxmlPath, Analysis analysis, Design design, List<string> bscPath, string docxTemplatePath, bool endSession)
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
            fdScript.CmdCalculation = new CmdCalculation(analysis, design);
            return fdScript;
        }

        public static FdScript CreateDocumentation(string strPath, string docxTemplatePath, bool endSession = true)
        {
            FdScript fdScript = FdScript.ReadStr(strPath, null);
            fdScript.CmdSave = null;
            
            fdScript.DocxTemplatePath = docxTemplatePath;
            fdScript.CmdSaveDocx = new CmdSaveDocx(fdScript.FileName + ".docx");

            // set endsession
            if (endSession)
            {
                fdScript.CmdEndSession = new CmdEndSession();
            }
            
            return fdScript;
        }

        /// <summary>
        /// Serialize fdscript.
        /// </summary>
        public void SerializeFdScript()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FdScript));
            using (TextWriter writer = new StreamWriter(this.FdScriptPath))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}