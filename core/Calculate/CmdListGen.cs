// https://strusoft.com/
using System;
using System.IO;
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    /// <summary> 
    /// fdscript.xsd
    /// CMDLISTGEN
    /// </summary>
    public class CmdListGen
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
        private string FileName { get; set; }
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdListGen()
        {
            
        }

        public CmdListGen(string bscPath, string outputDir, bool regional = false, bool fillCells = true, bool headers = true)
        {
            Initialize(bscPath, outputDir);
            this.Regional = regional;
            this.FillCells = fillCells;
            this.Headers = headers;
        }

        private void Initialize(string bscPath, string outputDir) {
            string _fileName = Path.GetFileNameWithoutExtension(bscPath);
            string _extension = Path.GetExtension(bscPath);

            if (_extension != ".bsc")
            {
                throw new System.ArgumentException("Incorrect file-extension. Expected .bsc. CmdListGen failed.");
            }

            this.BscFile = bscPath;
            this.FileName = _fileName;
            this.OutFile = outputDir + @"\" + this.FileName + @".csv";
        }
    }
}