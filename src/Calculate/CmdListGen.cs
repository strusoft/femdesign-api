// https://strusoft.com/
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
        public string command = "$ MODULECOM LISTGEN"; // token, fixed.
        [XmlAttribute("bscfile")]
        public string bscFile { get; set; } // string
        [XmlAttribute("outfile")]
        public string outFile { get; set; } // string
        [XmlAttribute("regional")]
        public int regional { get; set; } // bool // int (0,1)?
        [XmlAttribute("fillcells")]
        public int fillCells { get; set; } // bool // int (0,1)?
        private string fileName { get; set; }
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdListGen()
        {
            
        }
        public CmdListGen(string bscPath, string outputDir)
        {
            //
            string _fileName = Path.GetFileNameWithoutExtension(bscPath);
            string _extension = Path.GetExtension(bscPath);

            if (_extension != ".bsc")
            {
                throw new System.ArgumentException("Incorrect file-extension. Expected .bsc. CmdListGen failed.");
            }

            //
            this.bscFile = bscPath;
            this.fileName = _fileName;
            this.outFile = outputDir + @"\" + this.fileName + @".csv";
            this.regional = 0;
            this.fillCells = 0;
        }
    }
}