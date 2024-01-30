// https://strusoft.com/
// https://strusoft.com/
using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Collections.Generic;

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
        public bool _regional { get; set; }

        [XmlIgnore]
        public bool Regional
        {
            get
            {
                return this._regional;
            }
            set
            {
                this._regional = value;
            }
        }

        [XmlAttribute("headers")]
        public bool _headers { get; set; }

        [XmlIgnore]
        public bool Headers
        {
            get
            {
                return this._headers;
            }
            set
            {
                this._headers = value;
            }
        }

        [XmlAttribute("fillcells")]
        public bool _fillCells { get; set; }

        [XmlIgnore]
        public bool FillCells
        {
            get
            {
                return this._fillCells;
            }
            set
            {
                this._fillCells = value;
            }
        }

        [XmlAttribute("ignorecasename")]
        public bool _ignoreCaseName { get; set; }

        [XmlIgnore]
        public bool IgnoreCaseName
        {
            get
            {
                return this._ignoreCaseName;
            }
            set
            {
                this._ignoreCaseName = value;
            }
        }

        [XmlIgnore]
        [Obsolete("This object is no longer available and will be deprecated in version 23.00.0. You can specify the load case/combination using the Suffix property in DocTable.")]
        public MapCase MapCase;

        [XmlIgnore]
        [Obsolete("This object is no longer available and will be deprecated in version 23.00.0. You can specify the load case/combination using the Suffix property in DocTable.")]
        public MapComb MapComb;

        [XmlIgnore]
        public List<FemDesign.GenericClasses.IStructureElement> StructureElements { get; set; }

        [XmlElement("GUID")]
        public List<Guid> _elementGuids
        {
            get
            {
                List<Guid> result = new List<Guid>();

                if (this.StructureElements != null)
                    foreach(var element in StructureElements)
                        result.Add(element.Guid);
                return result;
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

        public CmdListGen(string bscPath, string outPath, FemDesign.GenericClasses.IStructureElement elements, bool regional = false) : this(bscPath, outPath, regional)
        {
            if(elements != null)
                StructureElements = new List<FemDesign.GenericClasses.IStructureElement> { elements };
        }

        public CmdListGen(string bscPath, string outPath, List<FemDesign.GenericClasses.IStructureElement> elements, bool regional = false) : this(bscPath, outPath, regional)
        {
            if(elements != null && elements.Count != 0)
                StructureElements = elements;
        }

        private CmdListGen(Bsc bsc, string outPath, bool regional = false) : this(bsc.BscPath, outPath, regional)
        {
        }

        [Obsolete("MapCase/MapComb is no longer available and will be romoved in version 23.00.0. You can specify the load case/combination using the Suffix property in DocTable.")]
        internal CmdListGen(Bsc bsc, string outPath, bool regional, MapCase mapCase) : this(bsc, outPath, regional)
        {
            if (bsc.DocTable.AllCaseComb == true && (mapCase != null))
                throw new Exception("Bsc file has been setup to return all loadCase/loadCombination. MapCase, MapComb are not necessary");

            if (bsc.DocTable.AllCaseComb == false)
            {
                MapCase = mapCase;
            }
        }

        [Obsolete("MapCase/MapComb is no longer available and will be romoved in version 23.00.0. You can specify the load case/combination using the Suffix property in DocTable.")]
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

        [Obsolete("MapCase/MapComb is no longer available and will be romoved in version 23.00.0. You can specify the load case/combination using the Suffix property in DocTable.")]
        public CmdListGen(string bscPath, string outPath, bool regional, MapCase mapcase, FemDesign.GenericClasses.IStructureElement elements = null) : this(bscPath, outPath, elements, regional)
        {
            MapCase = mapcase;
        }
                
        [Obsolete("MapCase/MapComb is no longer available and will be romoved in version 23.00.0. You can specify the load case/combination using the Suffix property in DocTable.")]
        public CmdListGen(string bscPath, string outPath, bool regional, MapComb mapComb, FemDesign.GenericClasses.IStructureElement elements = null) : this(bscPath, outPath, elements, regional)
        {
            MapComb = mapComb;
        }

        [Obsolete("OBSOLETE. IT WILL BE REMOVED IN 22.00.0")]
        internal static CmdListGen Default(string bscPath, string outputDir, bool regional = false, bool fillCells = true, bool headers = true)
        {
            return new CmdListGen(bscPath, outputDir, regional, fillCells, headers);
        }

        [Obsolete("OBSOLETE. IT WILL BE REMOVED IN 22.00.0")]
        private string FileName { get; set; }

        [Obsolete("OBSOLETE. IT WILL BE REMOVED IN 22.00.0")]
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


        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdListGen>(this);
        }
    }


    
    [Obsolete("This object is no longer available. You can specify the load case/combination using the Suffix property in DocTable.")]
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

    [Obsolete("This object is no longer available. You can specify the load case/combination using the Suffix property in DocTable.")]
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