// https://strusoft.com/
using System.IO;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion


namespace FemDesign.Calculate
{
    /// <summary> 
    /// fdscript.xsd
    /// CMDLISTGEN
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class CmdListGen
    {
        [XmlAttribute("command")]
        public string Command = "$ MODULECOM LISTGEN"; // token, fixed.
        [XmlAttribute("bscfile")]
        public string BscFile { get; set; } // string
        [XmlAttribute("outfile")]
        public string OutFile { get; set; } // string
        [XmlAttribute("regional")]
        public int Regional { get; set; } // bool // int (0,1)?
        [XmlAttribute("headers")]
        public int Headers { get; set; } // bool // int (0,1)?
        [XmlAttribute("fillcells")]
        public int FillCells { get; set; } // bool // int (0,1)?
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
            this.Regional = regional ? 1 : 0;
            this.FillCells = fillCells ? 1 : 0;
            this.Headers = headers ? 1 : 0;
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