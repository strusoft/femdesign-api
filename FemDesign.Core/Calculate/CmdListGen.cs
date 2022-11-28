// https://strusoft.com/
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

        [XmlIgnore]
        public Bsc Bsc { get; set; }

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

        [XmlElement("mapcase")]
        public MapCase MapCase { get; set; }

        [XmlElement("mapcomb")]
        public MapComb MapComb { get; set; }

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

        private CmdListGen(Bsc bsc, string outPath, bool regional = false) : this(bsc.BscPath, outPath, regional)
        {
        }

        internal CmdListGen(Bsc bsc, string outPath, bool regional, MapCase mapCase) : this(bsc, outPath, regional)
        {
            if (bsc.DocTable.AllCaseComb == true && (mapCase != null))
                throw new Exception("Bsc file has been setup to return all loadCase/loadCombination. MapCase, MapComb are not necessary");

            if (bsc.DocTable.AllCaseComb == false)
            {
                MapCase = mapCase;
            }
        }

        internal CmdListGen(Bsc bsc, string outPath, bool regional, MapComb mapComb) : this(bsc, outPath, regional)
        {
            if (bsc.DocTable.AllCaseComb == true && (mapComb != null))
                throw new Exception("Bsc file has been setup to return all loadCase/loadCombination. MapCase, MapComb are not necessary");

            if (bsc.DocTable.AllCaseComb == false)
            {
                MapComb = mapComb;
            }
        }



        private CmdListGen(string bscPath, string outputDir, bool regional = false, bool fillCells = true, bool headers = true)
        {
            Initialize(bscPath, outputDir);
            this.Regional = regional;
            this.FillCells = fillCells;
            this.Headers = headers;
        }

        /// <summary>
        /// OBSOLETE. IT WILL BE REMOVE IN 22.00.0
        /// </summary>
        /// <param name="bscPath"></param>
        /// <param name="outputDir"></param>
        /// <param name="regional"></param>
        /// <param name="fillCells"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        internal static CmdListGen Default(string bscPath, string outputDir, bool regional = false, bool fillCells = true, bool headers = true)
        {
            return new CmdListGen(bscPath, outputDir, regional, fillCells, headers);
        }

        /// <summary>
        /// OBSOLETE. IT WILL BE REMOVE IN 22.00.0
        /// </summary>
        private string FileName { get; set; }

        /// <summary>
        /// OBSOLETE. IT WILL BE REMOVE IN 22.00.0
        /// </summary>
        /// <param name="bscPath"></param>
        /// <param name="outputDir"></param>
        /// <exception cref="System.ArgumentException"></exception>
        private void Initialize(string bscPath, string outputDir)
        {
            string _fileName = Path.GetFileNameWithoutExtension(bscPath);
            string _extension = Path.GetExtension(bscPath);

            if (_extension != ".bsc")
            {
                throw new System.ArgumentException("Incorrect file-extension. Expected .bsc. CmdListGen failed.");
            }

            this.BscFile = bscPath;
            this.FileName = _fileName;
            this.OutFile = Path.Combine(outputDir, this.FileName + ".csv");
        }



        public CmdListGen(string bscPath, string outPath, bool regional, MapCase mapcase)
        {
            OutFile = Path.GetFullPath(outPath);
            BscFile = Path.GetFullPath(bscPath);
            Regional = regional;
            FillCells = true;
            Headers = true;
            MapCase = mapcase;
        }

        public CmdListGen(string bscPath, string outPath, bool regional, MapComb mapComb)
        {
            OutFile = Path.GetFullPath(outPath);
            BscFile = Path.GetFullPath(bscPath);
            Regional = regional;
            FillCells = true;
            Headers = true;
            MapComb = mapComb;
        }

        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdListGen>(this);
        }
    }







    public partial class MapCase
    {

        public static readonly string _oname = "loadcasename";

        [XmlAttribute("oname")]
        public string _refCaseName = MapCase._oname;

        [XmlAttribute("nname")]
        public string _loadCaseName { get; set; }

        // We are not using index in our API.
        // The API reference the Load Case by Name 
        //[XmlAttribute("idx")]
        //public int Index { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MapCase()
        {

        }

        public MapCase(string loadCaseName)
        {
            this._loadCaseName = loadCaseName;
        }
    }


    public partial class MapComb
    {
        public static readonly string _oname = "loadcombname";

        [XmlAttribute("oname")]
        public string _refCombName = MapComb._oname;

        [XmlAttribute("nname")]
        public string _loadCombName { get; set; }

        // We are not using index in our API.
        // The API reference the Load Combination by Name 
        //[XmlAttribute("idx")]
        //public int Index { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MapComb()
        {

        }

        public MapComb(string loadCombName)
        {
            this._loadCombName = loadCombName;
        }
    }
}