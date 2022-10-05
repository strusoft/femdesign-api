// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FemDesign.Results;
using System.Reflection;
using System.Xml.Linq;


namespace FemDesign.Calculate
{
    /// <summary>
    /// Base class for all commands that can be run in FEM-Design
    /// </summary>
    public abstract class CmdCommand
    {
        public abstract XElement ToXElement();
    }

    /// <summary>
    /// Open a file in FEM-Design
    /// </summary>
    public class CmdOpen2 : CmdCommand
    {
        public const string Command = "; CXL CS2SHELL OPEN";
        public string Filename { get; set; }
        public CmdOpen2(string filepath)
        {
            this.Filename = Path.GetFullPath(filepath);
        }

        public override XElement ToXElement()
        {
            return new XElement(
                "cmdopen",
                new XAttribute("command", Command),
                new XElement(
                    "filename", 
                    new XText(Filename)
                )
            );
        }
    }

    public class CmdUserModule2 : CmdCommand
    {
        public const string Command = "; CXL $MODULE {0}";
        public CmdUserModule Module;
        public CmdUserModule2(CmdUserModule module)
        {
            Module = module;
        }
        public override XElement ToXElement()
        {
            return new XElement("cmduser", new XAttribute("command", string.Format(Command, Module)));
        }
    }

    public class CmdCalculation2 : CmdCommand
    {
        public const string Command = "; CXL $MODULE CALC";
        public Analysis Analysis;
        public CmdCalculation2(Analysis analysis)
        {
            Analysis = analysis;
        }

        public override XElement ToXElement()
        {
            return new XElement("cmdcalculation", new XAttribute("command", Command),
                new XElement("analysis",
                    new XAttribute("calcCase", Analysis.CalcCase),
                    new XAttribute("calcCstage", Analysis.CalcCStage),
                    new XAttribute("calcCImpf", Analysis.CalcCImpf),
                    new XAttribute("calcComb", Analysis.CalcComb),
                    new XAttribute("calcGmax", Analysis.CalcGMax),
                    new XAttribute("calcStab", Analysis.CalcStab),
                    new XAttribute("calcFreq", Analysis.CalcFreq),
                    new XAttribute("calcSeis", Analysis.CalcSeis),
                    new XAttribute("calcDesign", Analysis.CalcDesign),
                    new XAttribute("calcFootfall", Analysis.CalcFootfall),
                    new XAttribute("elemfine", Analysis.ElemFine),
                    new XAttribute("diaphragm", Analysis.Diaphragm),
                    new XAttribute("peaksmoothing", Analysis.PeakSmoothing)

                    // TODO add <comb>
                )
            );
        }
    }

    public class CmdListGen2 : CmdCommand
    {
        public const string Command = "$ MODULECOM LISTGEN";
        public string BscPath;
        public string OutPath;
        public bool Regional;

        public CmdListGen2(string outPath, string bscPath, bool regional = false)
        {
            OutPath = Path.GetFullPath(outPath);
            BscPath = Path.GetFullPath(bscPath);
            Regional = regional;
        }

        public override XElement ToXElement()
        {
            return new XElement(
                "cmdlistgen",
                new XAttribute("command", Command),
                new XAttribute("outfile", OutPath),
                new XAttribute("bscfile", BscPath),
                new XAttribute("regional", Regional),
                new XAttribute("headers", "1"),
                new XAttribute("fillcells", "1")
            );
        }
    }

    /// <summary>
    /// Save the model to a file
    /// </summary>
    public class CmdSave2 : CmdCommand
    {
        public const string Command = "; CXL CS2SHELL SAVE";
        public string Filename { get; set; }

        /// <inheritdoc cref="CmdSave2"/>
        /// <param name="filepath">The target path of the saved model. Typically the file should have the extension .str or .struxml</param>
        public CmdSave2(string filepath)
        {
            this.Filename = Path.GetFullPath(filepath);
        }

        public override XElement ToXElement()
        {
            return new XElement(
                "cmdopen",
                new XAttribute("command", Command),
                new XElement(
                    "filename",
                    new XText(Filename)
                )
            );
        }
    }

    /// <summary>
    /// Fdscript root class
    /// </summary>
    public partial class FdScript2
    {
        public class FdScriptHeader2
        {
            public string LogFilePath { get; private set; }
            public FdScriptHeader2(string logFilePath)
            {
                LogFilePath = Path.GetFullPath(logFilePath);
            }

            public XElement ToXElement()
            {
                return new XElement(
                    "fdscriptheader",
                    new XElement("title", new XText("FEM-Design example script")),
                    new XElement("version", new XText("1900")),
                    new XElement("module", new XText("SFRAME")),
                    new XElement("logfile", new XText(LogFilePath))
                );
            }
        }

        public FdScriptHeader2 Header { get; set; }
        public List<CmdCommand> Commands = new List<CmdCommand>();

        public FdScript2(string logFilePath, params CmdCommand[] commands)
        {
            Header = new FdScriptHeader2(logFilePath);
            Commands = commands.ToList();
        }

        public void Add(CmdCommand command)
        {
            Commands.Add(command);
        }

        public void Serialize(string path)
        {
            XDocument doc = new XDocument();

            var root = new XElement(
                "fdscript", 
                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance")
                );

            root.Add(Header.ToXElement());

            Commands.ForEach(c => root.Add(c.ToXElement()));

            doc.Add(root);
            doc.Save(path);
        }
    }
}
