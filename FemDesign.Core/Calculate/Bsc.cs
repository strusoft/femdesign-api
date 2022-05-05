// https://strusoft.com/
using System;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// fdscript root
    /// </summary>
    [XmlRoot("fdscript")]
    public partial class Bsc
    {
        [XmlAttribute("noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public const string XmlAttrib = "fdscript.xsd";
        [XmlElement("fdscriptheader", Order = 1)]
        public FdScriptHeader FdScriptHeader { get; set; } // FDSCRIPTHEADER

        [XmlElement("cmddoctable", Order = 2)]
        public CmdDocTable _cmdDocTable;  // CMDDOCTABLE

        [XmlIgnore]
        public DocTable DocTable
        {
            get
            {
                return _cmdDocTable.DocTable;
            }
            set
            {
                this._cmdDocTable = new CmdDocTable(value);
            }
        }

        [XmlElement("cmdendsession", Order = 3)]
        public CmdEndSession CmdEndSession { get; set; } // CMDENDSESSION
        [XmlIgnore]
        internal string Cwd { get; set; } // current work directory, string
        [XmlIgnore]
        public string BscPath { get; set; } // path to fdscript file, string

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Bsc()
        {

        }

        public Bsc(ListProc resultType, string bscPath)
        {
            if (Path.GetExtension(bscPath) != ".bsc")
            {
                throw new ArgumentException($"File path must be '.bsc' but got '{bscPath}'");
            }
            BscPath = Path.GetFullPath(bscPath);
            Cwd = Path.GetDirectoryName(BscPath);
            DocTable = new DocTable(resultType);
            FdScriptHeader = new FdScriptHeader("Generated script.", Path.Combine(Cwd, "logfile.log"));
            CmdEndSession = new CmdEndSession();
            SerializeBsc();
        }

        public Bsc(ListProc resultType, int caseIndex, string bscPath) : this(resultType, bscPath)
        {
            DocTable.CaseIndex = caseIndex;
        }

        public static implicit operator string(Bsc bsc)
        {
            return bsc.BscPath;
        }

        public static implicit operator List<string>(Bsc bsc)
        {
            return new List<string>() {bsc.BscPath};
        }

        /// <summary>
        /// Serialize bsc.
        /// </summary>
        public void SerializeBsc()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Bsc));
            using (TextWriter writer = new StreamWriter(this.BscPath))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}
