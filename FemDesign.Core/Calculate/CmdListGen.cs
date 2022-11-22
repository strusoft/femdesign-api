// https://strusoft.com/
using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace FemDesign.Calculate
{
    /// <summary> 
    /// fdscript.xsd
    /// CMDLISTGEN
    /// </summary>
    [XmlRoot("cmdlistgen")]
    [System.Serializable]
    public partial class CmdListGen : CmdCommand
    {
        [XmlAttribute("command")]
        public string Command = "$ MODULECOM LISTGEN"; // token, fixed.
        [XmlAttribute("bscfile")]
        public string BscFile { get; set; } // string
        [XmlAttribute("outfile")]
        public string OutFile { get; set; } // string
        [XmlAttribute("regional")]
        public int _regional { get; set; }
        [XmlIgnore]
        public bool Regional
        {
            get
            {
                return Convert.ToBoolean(this._regional);
            }
            set
            {
                this._regional = Convert.ToInt32(value);
            }
        }
        [XmlAttribute("headers")]
        public int _headers { get; set; }
        [XmlIgnore]
        public bool Headers
        {
            get
            {
                return Convert.ToBoolean(this._headers);
            }
            set
            {
                this._headers = Convert.ToInt32(value);
            }
        }
        [XmlAttribute("fillcells")]
        public int _fillCells { get; set; }
        [XmlIgnore]
        public bool FillCells
        {
            get
            {
                return Convert.ToBoolean(this._fillCells);
            }
            set
            {
                this._fillCells = Convert.ToInt32(value);
            }
        }

        [XmlAttribute("ignorecasename")]
        public int _ignoreCaseName { get; set; }
        [XmlIgnore]
        public bool IgnoreCaseName
        {
            get
            {
                return Convert.ToBoolean(this._ignoreCaseName);
            }
            set
            {
                this._ignoreCaseName = Convert.ToInt32(value);
            }
        }


        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdListGen()
        {
        }

        public CmdListGen(string bscPath, string outPath, bool regional = false)
        {
            OutFile = Path.GetFullPath(outPath);
            BscFile = Path.GetFullPath(bscPath);
            Regional = regional;
            FillCells = true;
            Headers = true;
        }

        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdListGen>(this);
        }
    }
}