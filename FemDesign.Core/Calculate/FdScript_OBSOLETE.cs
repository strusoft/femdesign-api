// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FemDesign.Results;
using System.Reflection;


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
        public FdScriptHeader Header { get; set; } // FDSCRIPTHEADER

        [XmlElement("cmdopen", Order = 2)]
        public CmdOpen CmdOpen { get; set; } // CMDOPEN

        [XmlElement("cmduser", Order = 3)]
        public CmdUser CmdUser { get; set; } // CMDUSER

        [XmlElement("cmdglobalcfg", Order = 4)]
        public CmdGlobalCfg CmdGlobalCfg { get; set; }// CMDGLOBALCFG

        [XmlElement("cmdcalculation", Order = 5)]
        public CmdCalculation CmdCalculation { get; set; }// CMDCALCULATION

        [XmlElement("cmduser", Order = 6)]
        public CmdDesignDesignChanges CmdDesignDesignChanges { get; set; } // CMDUSER

        [XmlElement("cmdlistgen", Order = 7)]
        public List<CmdListGen> CmdListGen { get; set; } // CMDLISTGEN

        [XmlElement("cmdchild", Order = 8)]
        public string DocxTemplatePath { get; set; } // DOCXTEMPLATEPATH

        [XmlElement("cmdsavedocx", Order = 9)]
        public CmdSaveDocx CmdSaveDocx { get; set; } // CMDSAVEDOCX

        [XmlElement("cmdsave", Order = 10)]
        public CmdSave CmdSave { get; set; } // CMDSAVE

        [XmlElement("cmdendsession", Order = 11)]
        public CmdEndSession CmdEndSession { get; set; } // CMDENDSESSION

        [XmlIgnore]
        public string StruxmlPath { get; set; } // path to struxml file, string 259
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
        internal static FdScript CalculateStruxml(string struxmlPath, CmdUserModule mode, List<string> bscPath, string docxTemplatePath, bool endSession, CmdGlobalCfg cmdGlobalCfg = null)
        {
            FdScript fdScript = new FdScript();

            fdScript.XmlAttrib = "fdscript.xsd";
            fdScript.StruxmlPath = struxmlPath;
            fdScript.FileName = Path.GetFileNameWithoutExtension(struxmlPath);
            fdScript.Cwd = Path.GetDirectoryName(fdScript.StruxmlPath);

            fdScript.FdScriptPath = Path.Combine(fdScript.Cwd, fdScript.FileName, "scripts", "Analysis.fdscript");

            // set header and logfile
            fdScript.Header = new FdScriptHeader("Generated script.", Path.Combine(fdScript.Cwd, fdScript.FileName, "logfile.log"));

            // set open
            fdScript.CmdOpen = new CmdOpen(fdScript.StruxmlPath);

            // set user
            fdScript.CmdUser = new CmdUser(mode);


            // listgen
            if (bscPath != null && bscPath.Any())
            {
                fdScript.CmdListGen = new List<CmdListGen>();
                foreach (string item in bscPath)
                {
                    //fdScript.CmdListGen.Add(new CmdListGen(item, Path.Combine(fdScript.Cwd, fdScript.FileName, "results")));
                    var cmdListGen = Calculate.CmdListGen.Default(item, Path.Combine(fdScript.Cwd, fdScript.FileName, "results"));
                    fdScript.CmdListGen.Add(cmdListGen);
                }
            }

            // set save docx
            if (docxTemplatePath != "" && docxTemplatePath != null)
            {
                // path to .dsc-file (template file)
                fdScript.DocxTemplatePath = docxTemplatePath;

                // object containing command to generate .docx and path to generated .docx
                fdScript.CmdSaveDocx = new CmdSaveDocx(fdScript.FileName + ".docx");
            }

            if (cmdGlobalCfg == null)
            {
                fdScript.CmdGlobalCfg = CmdGlobalCfg.Default();
            }

            // set save
            fdScript.CmdSave = new CmdSave(fdScript.Cwd + @"\" + fdScript.FileName + ".str");

            // set endsession
            if (endSession)
            {
                fdScript.CmdEndSession = new CmdEndSession();
            }

            // return
            return fdScript;
        }

        /// Create fdscript to read a str-model.
        public static FdScript ReadStr(string strPath, List<string> bscPaths = null)
        {
            FdScript fdScript = new FdScript();
            fdScript.XmlAttrib = "fdscript.xsd";

            strPath = Path.GetFullPath(strPath);
            fdScript.FileName = Path.GetFileNameWithoutExtension(strPath);
            fdScript.Cwd = Path.GetDirectoryName(strPath);
            fdScript.StruxmlPath = Path.Combine(fdScript.Cwd, fdScript.FileName + ".struxml");

            fdScript.FdScriptPath = Path.Combine(fdScript.Cwd, fdScript.FileName, "scripts", "Analysis.fdscript");

            // set header and logfile
            fdScript.Header = new FdScriptHeader("Generated script.", Path.Combine(fdScript.Cwd, fdScript.FileName, "logfile.log"));

            // open str
            fdScript.CmdOpen = new CmdOpen(strPath);

            // listgen
            if (bscPaths != null && bscPaths.Any())
            {
                fdScript.CmdListGen = new List<CmdListGen>();
                foreach (string bscPath in bscPaths)
                {
                    var cmdListGen = Calculate.CmdListGen.Default(bscPath, Path.Combine(fdScript.Cwd, fdScript.FileName, "results"));
                    fdScript.CmdListGen.Add(cmdListGen);
                }
            }

            // save as .struxml
            fdScript.CmdSave = new CmdSave(fdScript.StruxmlPath);

            // end session
            fdScript.CmdEndSession = new CmdEndSession();

            // return
            return fdScript;
        }

        /// <summary>
        /// Create fdscript to run analysis.
        /// </summary>
        public static FdScript Analysis(string struxmlPath, Analysis analysis, List<string> bscPath, string docxTemplatePath, bool endSession, CmdGlobalCfg cmdGlobalCfg = null)
        {
            CmdUserModule mode = CmdUserModule.RESMODE;
            FdScript fdScript = FdScript.CalculateStruxml(struxmlPath, mode, bscPath, docxTemplatePath, endSession, cmdGlobalCfg);
            fdScript.CmdCalculation = new CmdCalculation(analysis);
            return fdScript;
        }

        /// <summary>
        /// Create fdscript to run analysis and design.
        /// </summary>
        public static FdScript Design(string mode, string struxmlPath, Analysis analysis, Design design, List<string> bscPath, string docxTemplatePath, bool endSession, CmdGlobalCfg cmdGlobalCfg = null)
        {
            CmdUserModule _mode = CmdUserModule.RCDESIGN;
            switch (mode)
            {
                case "rc":
                case "Rc":
                case "RC":
                case "RCDESIGN":
                    _mode = CmdUserModule.RCDESIGN;
                    break;
                case "steel":
                case "Steel":
                case "STEEL":
                case "STEELDESIGN":
                    _mode = CmdUserModule.STEELDESIGN;
                    break;
                case "timber":
                case "Timber":
                case "TIMBER":
                case "TIMBERDESIGN":
                    _mode = CmdUserModule.TIMBERDESIGN;
                    break;
                default:
                    throw new ArgumentException("Mode is not supported. Mode should be rc, steel or timber");
            }

            FdScript fdScript = FdScript.CalculateStruxml(struxmlPath, _mode, bscPath, docxTemplatePath, endSession, cmdGlobalCfg);
            fdScript.CmdCalculation = new CmdCalculation(analysis, design);
            if (design.ApplyChanges)
            {
                fdScript.CmdDesignDesignChanges = new CmdDesignDesignChanges();
            }
            return fdScript;
        }

        /// <summary>
        /// Generate a FEM-Design documentation.
        /// </summary>
        /// <param name="strPath">The .str file to generate base the documentation on.</param>
        /// <param name="docxTemplatePath">The .docx template path for the documentation.</param>
        /// <param name="endSession">Close the FEM-Design program after successfully generating documentation.</param>
        /// <returns>An <see cref="FdScript"/> for generating documentation.</returns>
        public static FdScript CreateDocumentation(string strPath, string docxTemplatePath, bool endSession = true)
        {
            FdScript fdScript = FdScript.ReadStr(strPath, null);
            fdScript.CmdSave = null;

            fdScript.DocxTemplatePath = docxTemplatePath;
            fdScript.CmdSaveDocx = new CmdSaveDocx(fdScript.StruxmlPath.Replace(".struxml", ".docx"));

            // set endsession
            if (endSession)
            {
                fdScript.CmdEndSession = new CmdEndSession();
            }

            return fdScript;
        }

        /// <summary>
        /// Create fdscript to open and extract results from a FEM-Design .str model.
        /// </summary>
        /// <param name="strPath">Path to model with results to be extracted.</param>
        /// <param name="results">Results to be extracted.</param>
        public static FdScript ExtractResults(string strPath, IEnumerable<Type> results = null)
        {
            if (results == null)
                return ReadStr(strPath);

            var notAResultType = results.Where(r => !typeof(Results.IResult).IsAssignableFrom(r)).FirstOrDefault();
            if (notAResultType != null)
                throw new ArgumentException($"{notAResultType.Name} is not a result type. (It does not inherit from {typeof(FemDesign.Results.IResult).FullName})");

            var listProcs = results.SelectMany(r => r.GetCustomAttribute<ResultAttribute>().ListProcs);

            var dir = Path.GetDirectoryName(strPath);
            var batchResults = listProcs.Select(lp => new Bsc(lp, $"{dir}\\{lp}.bsc"));
            var bscPaths = batchResults.Select(bsc => bsc.BscPath).ToList();

            return ReadStr(strPath, bscPaths);
        }

        /// <summary>
        /// Serialize fdscript.
        /// </summary>
        public void SerializeFdScript()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FdScript));

            if (!Directory.Exists(Path.GetDirectoryName(this.Header.LogFile)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(this.Header.LogFile));
            }

            if (this.CmdListGen != null)
            {
                foreach (var cmdListGen in this.CmdListGen)
                    if (!Directory.Exists(Path.GetDirectoryName(cmdListGen.OutFile)))
                        Directory.CreateDirectory(Path.GetDirectoryName(cmdListGen.OutFile));
            }

            if (!Directory.Exists(Path.GetDirectoryName(this.FdScriptPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(this.FdScriptPath));
            }

            using (TextWriter writer = new StreamWriter(this.FdScriptPath))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}