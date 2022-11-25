// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;

namespace FemDesign.Calculate
{
    
    [XmlRoot("cmdglobalcfg")]
    [System.Serializable]
    public partial class CmdGlobalCfg : CmdCommand
    {
        [XmlAttribute("command")]
        public string Command = "$ FEM $CODE(GLOBALCFG)"; // token

        [XmlElement("mesh_general")]
        public MeshGeneral MeshGeneral { get; set; }

        [XmlElement("mesh_elements")]
        public MeshElements MeshElements { get; set; }

        [XmlElement("mesh_functions")]
        public MeshFunctions Meshfunctions { get; set; }

        [XmlElement("mesh_prepare")]
        public MeshPrepare MeshPrepare { get; set; }

        [XmlElement("peaksm_method")]
        public PeaksmMethod PeaksmMethod { get; set; }

        [XmlElement("peaksm_auto")]
        public PeaksmAuto PeaksmAuto { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdGlobalCfg()
        {

        }

        public CmdGlobalCfg(MeshGeneral meshGeneral, MeshElements meshElements, MeshFunctions meshFunctions, MeshPrepare meshPrepare, PeaksmMethod peaksmMethod, PeaksmAuto peaksmAuto)
        {
            this.MeshGeneral = meshGeneral;
            this.MeshElements = meshElements;
            this.Meshfunctions = meshFunctions;
            this.MeshPrepare = meshPrepare;
            this.PeaksmMethod = peaksmMethod;
            this.PeaksmAuto = peaksmAuto;
        }

        public static CmdGlobalCfg Default()
        {
            var meshGeneral = MeshGeneral.Default();
            var meshElements = MeshElements.Default();
            var meshfunctions = MeshFunctions.Default();
            var meshPrepare = MeshPrepare.Default();
            var peaksmMethod = PeaksmMethod.Default();
            var peaksmAuto = PeaksmAuto.Default();

            var cmdGlobalCfg = new CmdGlobalCfg(meshGeneral,
                                                meshElements,
                                                meshfunctions,
                                                meshPrepare,
                                                peaksmMethod,
                                                peaksmAuto);

            return cmdGlobalCfg;
        }

        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdGlobalCfg>(this);
        }

    }

























    public partial class MeshGeneral
    {
        [XmlAttribute("fAdjustToLoads")]
        public int _adjustToLoads { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MeshGeneral()
        {

        }
        public MeshGeneral(bool adjustToLoad = false)
        {
            this._adjustToLoads = System.Convert.ToInt32(adjustToLoad);
        }

        public static MeshGeneral Default()
        {
            return new MeshGeneral(adjustToLoad : false);
        }

    }

    public partial class MeshElements
    {
        [XmlAttribute("fAdjustToLoads")]
        public int _elemCalcRegion { get; set; }

        [XmlAttribute("rElemSizeDiv")]
        public double ElemSizeDiv { get; set; }

        [XmlAttribute("fCorrectToMinDivNum")]
        public int _correctToMinDivNum { get; set; }

        [XmlAttribute("sDefaultDivision")]
        public int DefaultDivision { get; set; }

        [XmlAttribute("rDefaultAngle")]
        public double DefaultAngle { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MeshElements()
        {

        }

        public MeshElements(bool elemCalcRegion = true, double elemeSizeDiv = 6.0, bool correctToMinDivNum = true, int defaultDiv = 2, double defaultAngle = 15.0)
        {
            this._elemCalcRegion = System.Convert.ToInt32(elemCalcRegion);
            this.ElemSizeDiv = elemeSizeDiv;
            this._correctToMinDivNum = System.Convert.ToInt32(correctToMinDivNum);
            this.DefaultDivision = defaultDiv;
            this.DefaultAngle = defaultAngle;
        }

        public static MeshElements Default()
        {
            return new MeshElements(elemCalcRegion : true);
        }

    }

    public partial class MeshFunctions
    {
        [XmlAttribute("fRefineLocally")]
        public int _refineLocally { get; set; }

        [XmlAttribute("sRefineMaxStepNum")]
        public int RefineMaxStepNum { get; set; }

        [XmlAttribute("fMaxIterWarning")]
        public int _maxIterWarning { get; set; }

        [XmlAttribute("fReduceSize")]
        public int _reduceSize { get; set; }

        [XmlAttribute("sSmoothStepNum")]
        public int SmoothStepNum { get; set; }

        [XmlAttribute("fCheckMeshGeom")]
        public int _checkMeshGeom { get; set; }

        [XmlAttribute("rCheckGeomMinAngle")]
        public double CheckGeomMinAngle { get; set; }

        [XmlAttribute("rCheckGeomMaxAngle")]
        public double CheckGeomMaxAngle { get; set; }

        [XmlAttribute("rCheckGeomMaxSideRatio")]
        public double CheckGeomMaxSideRatio { get; set; }

        [XmlAttribute("fCheckMeshOverlap")]
        public int _checkMeshOverLap { get; set; }

        [XmlAttribute("fCheckMeshTopology")]
        public int _checkMeshTopology { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MeshFunctions()
        {

        }

        public MeshFunctions(bool refineLocally = true, int refineMaxStepNum = 5, bool iterWarning = false, bool reduceSize = true, int smoothStepNum = 3, bool checkMeshGeom = true, double checkGeomMinangle = 10.0, double checkGeomMaxangle = 170.0, double checkGeomMaxSideRatio = 8.0, bool checkMeshOverlap = true, bool checkMeshTopology = true)
        {
            this._refineLocally = System.Convert.ToInt32(refineLocally);
            this.RefineMaxStepNum = refineMaxStepNum;
            this._maxIterWarning = System.Convert.ToInt32(iterWarning);
            this._reduceSize = System.Convert.ToInt32(reduceSize);
            this.SmoothStepNum = smoothStepNum;
            this._checkMeshGeom = System.Convert.ToInt32(checkMeshGeom);
            this.CheckGeomMinAngle = checkGeomMinangle;
            this.CheckGeomMaxAngle = checkGeomMaxangle;
            this.CheckGeomMaxSideRatio = checkGeomMaxSideRatio;
            this._checkMeshOverLap = System.Convert.ToInt32(checkMeshOverlap);
            this._checkMeshTopology = System.Convert.ToInt32(checkMeshTopology);
        }

        public static MeshFunctions Default()
        {
            return new MeshFunctions(refineLocally : true);
        }
    }

    public partial class MeshPrepare
    {
        [XmlAttribute("fAutoRegen")]
        public int _autoRegen { get; set; }

        [XmlAttribute("fThPeak")]
        public int _thPeak { get; set; }

        [XmlAttribute("fThBeam")]
        public int _thBeam { get; set; }

        [XmlAttribute("fThColumn")]
        public int _thColumn { get; set; }

        [XmlAttribute("fThTruss")]
        public int _thTruss { get; set; }

        [XmlAttribute("fThFicBeam")]
        public int _thFicBeam { get; set; }

        [XmlAttribute("fThFreeEdge")]
        public int _thFreeEdge { get; set; }

        [XmlAttribute("fThRegionBorder")]
        public int _thRegionBordger { get; set; }

        [XmlAttribute("fThSuppPt")]
        public int _thSupptPt { get; set; }

        [XmlAttribute("fThSuppLn")]
        public int _thSuppLn { get; set; }

        [XmlAttribute("fThSuppSf")]
        public int _thSuppSf { get; set; }

        [XmlAttribute("fThEdgeConn")]
        public int _thEdgeConn { get; set; }

        [XmlAttribute("fThConnPt")]
        public int _thConnPt { get; set; }

        [XmlAttribute("fThConnLn")]
        public int _thConnLn { get; set; }

        [XmlAttribute("fThConnSf")]
        public int _thConnSf { get; set; }

        [XmlAttribute("fThLoadPt")]
        public int _thLoadPt { get; set; }

        [XmlAttribute("fThLoadLn")]
        public int _thLoadLn { get; set; }

        [XmlAttribute("fThLoadSf")]
        public int _thLoadSf { get; set; }

        [XmlAttribute("fThFixPt")]
        public int _thFixPt { get; set; }

        [XmlAttribute("fThFixLn")]
        public int _thFixLn { get; set; }

        [XmlAttribute("fAutoRebuild")]
        public int _autoRebuild { get; set; }

        [XmlAttribute("fAutoSmooth")]
        public int _autoSmooth { get; set; }

        [XmlAttribute("fAutoCheck")]
        public int _autoCheck { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MeshPrepare()
        {

        }
        public MeshPrepare(bool autoRegen = true, bool thPeak = true, bool thBeam = false, bool thColumn = true, bool thTruss = false, bool thFicBeam = false, bool thFreeEdge = false, bool thRegionBorder = false, bool thSuppPt = true, bool thSuppLn = false, bool thSuppSf = false, bool thEdgeConn = false, bool thConnPt = false, bool thConnLn = false, bool thConnSf = false, bool thLoadPt = false, bool thLoadLn = false, bool thLoadSf = false, bool thFixPt = false, bool thFixLn = false, bool autoRebuild = true, bool autoSmooth = true, bool autoCheck = false)
        {
            this._autoRegen = System.Convert.ToInt32(autoRegen);
            this._thPeak = System.Convert.ToInt32(thPeak);
            this._thBeam = System.Convert.ToInt32(thBeam);
            this._thColumn = System.Convert.ToInt32(thColumn);
            this._thTruss = System.Convert.ToInt32(thTruss);
            this._thFicBeam = System.Convert.ToInt32(thFicBeam);
            this._thFreeEdge = System.Convert.ToInt32(thFreeEdge);
            this._thRegionBordger = System.Convert.ToInt32(thRegionBorder);
            this._thSupptPt = System.Convert.ToInt32(thSuppPt); 
            this._thSuppLn = System.Convert.ToInt32(thSuppLn);
            this._thSuppSf = System.Convert.ToInt32(thSuppSf);
            this._thEdgeConn = System.Convert.ToInt32(thEdgeConn);
            this._thConnPt = System.Convert.ToInt32(thConnPt);
            this._thConnLn = System.Convert.ToInt32(thConnLn);
            this._thConnSf = System.Convert.ToInt32(thConnSf);
            this._thLoadPt = System.Convert.ToInt32(thLoadPt);
            this._thLoadLn = System.Convert.ToInt32(thLoadLn);
            this._thLoadSf = System.Convert.ToInt32(thLoadSf);
            this._thFixPt = System.Convert.ToInt32(thFixPt);
            this._thFixLn = System.Convert.ToInt32(thFixLn);
            this._autoRebuild = System.Convert.ToInt32(autoRebuild);
            this._autoSmooth = System.Convert.ToInt32(autoSmooth);
            this._autoCheck = System.Convert.ToInt32(autoCheck);
        }

        public static MeshPrepare Default()
        {
            return new MeshPrepare(autoRegen : true);
        }
    }

    public partial class PeaksmMethod
    {
        [XmlAttribute("sPeakFormFunc_M")]
        public int PeakFormM { get; set; }

        [XmlAttribute("sPeakFormFunc_N")]
        public int PeakFormN { get; set; }

        [XmlAttribute("sPeakFormFunc_V")]
        public int PeakFormV { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private PeaksmMethod()
        {

        }

        public PeaksmMethod(int peakFormM = 1, int peakFormN = 1, int peakFormV = 1)
        {
            this.PeakFormM = peakFormM;
            this.PeakFormN = peakFormN;
            this.PeakFormV = peakFormV;
        }

        public static PeaksmMethod Default()
        {
            return new PeaksmMethod(peakFormM : 1);
        }
    }

    public partial class PeaksmAuto
    {
        [XmlAttribute("fPeakBeam")]
        public int _peakBeam { get; set; }

        [XmlAttribute("fPeakColumn")]
        public int _peakColumn { get; set; }

        [XmlAttribute("fPeakTruss")]
        public int _peakTruss { get; set; }

        [XmlAttribute("fPeakFicBeam")]
        public int _peakFicBeam { get; set; }

        [XmlAttribute("fPeakPlate")]
        public int _peakPlate { get; set; }

        [XmlAttribute("fPeakWall")]
        public int _peakWall { get; set; }

        [XmlAttribute("fPeakFicShell")]
        public int _peakFicShell { get; set; }

        [XmlAttribute("fPeakSuppPt")]
        public int _peakSuppPt { get; set; }

        [XmlAttribute("fPeakSuppLn")]
        public int _peakSuppLn { get; set; }

        [XmlAttribute("fPeakSuppSf")]
        public int _peakSuppSf { get; set; }

        [XmlAttribute("fPeakConnPt")]
        public int _peakConnPt { get; set; }

        [XmlAttribute("fPeakConnLn")]
        public int _peakConnLn { get; set; }

        [XmlAttribute("fPeakConnSf")]
        public int _peakConnSf { get; set; }

        [XmlAttribute("rPeakFactor")]
        public double PeakFactor { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private PeaksmAuto()
        {

        }
        public PeaksmAuto(bool peakBeam = false, bool peakColumn = true, bool peakTruss = false, bool peakficBeam = false, bool peakPlate = false, bool peakWall = false, bool peakFicShell = false, bool peakSuppPt = true, bool peakSuppLn = false, bool peakSuppSf = false, bool peakConnPt = false, bool peakConnLn = false, bool peakConnSf = false, double peakFactor = 0.5)
        {
            this._peakBeam = System.Convert.ToInt32(peakBeam);
            this._peakColumn = System.Convert.ToInt32(peakColumn);
            this._peakTruss = System.Convert.ToInt32(peakTruss);
            this._peakFicBeam = System.Convert.ToInt32(peakficBeam);
            this._peakPlate = System.Convert.ToInt32(peakPlate);
            this._peakWall = System.Convert.ToInt32(peakWall);
            this._peakFicShell = System.Convert.ToInt32(peakFicShell);
            this._peakSuppPt = System.Convert.ToInt32(peakSuppPt);
            this._peakSuppLn = System.Convert.ToInt32(peakSuppLn);
            this._peakSuppSf = System.Convert.ToInt32(peakSuppSf);
            this._peakConnPt = System.Convert.ToInt32(peakConnPt);
            this._peakConnLn = System.Convert.ToInt32(peakConnLn);
            this._peakConnSf = System.Convert.ToInt32(peakConnSf);
            this.PeakFactor = peakFactor;
        }

        public static PeaksmAuto Default()
        {
            return new PeaksmAuto(peakBeam : false);
        }
    }

}
