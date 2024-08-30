// https://strusoft.com/
using System;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using FemDesign.GenericClasses;

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

        [XmlElement("soil_calculation")]
        public SoilCalculation SoilCalculation { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdGlobalCfg()
        {

        }

        public CmdGlobalCfg(MeshGeneral meshGeneral, MeshElements meshElements, MeshFunctions meshFunctions, MeshPrepare meshPrepare, PeaksmMethod peaksmMethod, PeaksmAuto peaksmAuto, SoilCalculation soilCalculation)
        {
            this.MeshGeneral = meshGeneral;
            this.MeshElements = meshElements;
            this.Meshfunctions = meshFunctions;
            this.MeshPrepare = meshPrepare;
            this.PeaksmMethod = peaksmMethod;
            this.PeaksmAuto = peaksmAuto;
            this.SoilCalculation = soilCalculation;
        }

        public CmdGlobalCfg(params GlobConfig[] globConfigs)
        {
            this.Initialize(globConfigs.ToList());
        }

        public CmdGlobalCfg(List<GlobConfig> globConfigs)
        {
            this.Initialize(globConfigs);
        }

        private void Initialize(List<GlobConfig> globConfigs)
        {
            //this.MeshGeneral = MeshGeneral.Default();
            //this.MeshElements = MeshElements.Default();
            //this.Meshfunctions = MeshFunctions.Default();
            //this.MeshPrepare = MeshPrepare.Default();
            //this.PeaksmMethod = PeaksmMethod.Default();
            //this.PeaksmAuto = PeaksmAuto.Default();
            //this.SoilCalculation = SoilCalculation.Default();

            List<string> types = new List<string>();
            foreach (var config in globConfigs)
            {
                string type = config.GetType().Name;
                if (types.Contains(type))
                    throw new Exception($"The input list contains items of the same type. You can only specify one {type} object in the input list!");

                switch (type)
                {
                    case nameof(Calculate.MeshGeneral):
                        this.MeshGeneral = (MeshGeneral)config;
                        break;
                    case nameof(Calculate.MeshElements):
                        this.MeshElements = (MeshElements)config;
                        break;
                    case nameof(Calculate.MeshFunctions):
                        this.Meshfunctions = (MeshFunctions)config;
                        break;
                    case nameof(Calculate.MeshPrepare):
                        this.MeshPrepare = (MeshPrepare)config;
                        break;
                    case nameof(Calculate.PeaksmMethod):
                        this.PeaksmMethod = (PeaksmMethod)config;
                        break;
                    case nameof(Calculate.PeaksmAuto):
                        this.PeaksmAuto = (PeaksmAuto)config;
                        break;
                    case nameof(Calculate.SoilCalculation):
                        this.SoilCalculation = (SoilCalculation)config;
                        break;
                    case null:
                        throw new ArgumentNullException("Input has null elements!");
                    default:
                        throw new ArgumentException($"Input has elemets with invalid type! Valid types are: {nameof(Calculate.SoilCalculation)}, {nameof(Calculate.MeshGeneral)}, {nameof(Calculate.MeshElements)}, " +
                            $"{nameof(Calculate.MeshFunctions)}, {nameof(Calculate.MeshPrepare)}, {nameof(Calculate.PeaksmMethod)}, {nameof(Calculate.PeaksmAuto)}");
                }

                types.Add(type);
            }
        }

        public static CmdGlobalCfg Default()
        {
            var meshGeneral = MeshGeneral.Default();
            var meshElements = MeshElements.Default();
            var meshfunctions = MeshFunctions.Default();
            var meshPrepare = MeshPrepare.Default();
            var peaksmMethod = PeaksmMethod.Default();
            var peaksmAuto = PeaksmAuto.Default();
            var soilCalculation = SoilCalculation.Default();

            var cmdGlobalCfg = new CmdGlobalCfg(meshGeneral,
                                                meshElements,
                                                meshfunctions,
                                                meshPrepare,
                                                peaksmMethod,
                                                peaksmAuto,
                                                soilCalculation);

            return cmdGlobalCfg;
        }


        /// <summary>
        /// Deserialize CmdGlobalCfg from resource.
        /// </summary>
        public static CmdGlobalCfg DeserializeCmdGlobalCfgFromFilePath(string filePath)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(CmdGlobalCfg));
            TextReader reader = new StreamReader(filePath);
            object obj = deserializer.Deserialize(reader);
            var materialDatabase = (CmdGlobalCfg)obj;
            reader.Close();
            return materialDatabase;
        }


        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdGlobalCfg>(this);
        }

    }


    public partial class MeshGeneral : GlobConfig
    {
        [XmlAttribute("fAdjustToLoads")]
        public int _adjustToLoads;

        [XmlIgnore]
        public bool AdjustToLoads 
        { 
            get => Convert.ToBoolean(_adjustToLoads);
            set => _adjustToLoads = Convert.ToInt32(value);
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MeshGeneral()
        {

        }

        public MeshGeneral(bool adjustToLoad = false)
        {
            AdjustToLoads = adjustToLoad;
        }

        public static MeshGeneral Default()
        {
            return new MeshGeneral(adjustToLoad : false);
        }
    }

    public partial class MeshElements : GlobConfig
    {
        [XmlAttribute("fElemCalcRegion")]
        public int _elemCalcRegion;

        [XmlIgnore]
        public bool ElemCalcRegion
        {
            get => Convert.ToBoolean(_elemCalcRegion);
            set => _elemCalcRegion = Convert.ToInt32(value);
        }

        [XmlAttribute("rElemSizeDiv")]
        public double ElemSizeDiv { get; set; }

        [XmlAttribute("fCorrectToMinDivNum")]
        public int _correctToMinDivNum;

        [XmlIgnore]
        public bool CorrectToMinDivNum
        {
            get => Convert.ToBoolean(_correctToMinDivNum);
            set => _correctToMinDivNum = Convert.ToInt32(value);
        }

        [XmlAttribute("sDefaultDivision")]
        public int _defaultDivision;

        [XmlIgnore]
        public int DefaultDivision 
        {
            get => _defaultDivision;
            set => _defaultDivision = RestrictedInteger.DefaultBarElemDiv(value);
        }

        [XmlAttribute("rDefaultAngle")]
        public double _defaultAngle;

        [XmlIgnore]
        public double DefaultAngle
        {
            get => _defaultAngle;
            set => _defaultAngle = RestrictedDouble.NonNegMax_90(value);
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MeshElements()
        {

        }

        public MeshElements(bool elemCalcRegion = true, double elemeSizeDiv = 6.0, bool correctToMinDivNum = true, int defaultDiv = 2, double defaultAngle = 15.0)
        {
            this.ElemCalcRegion = elemCalcRegion;
            this.ElemSizeDiv = elemeSizeDiv;
            this.CorrectToMinDivNum = correctToMinDivNum;
            this.DefaultDivision = defaultDiv;
            this.DefaultAngle = defaultAngle;
        }

        public static MeshElements Default()
        {
            return new MeshElements(elemCalcRegion : true);
        }

    }

    public partial class MeshFunctions : GlobConfig
    {
        [XmlAttribute("fRefineLocally")]
        public int _refineLocally;

        [XmlIgnore]
        public bool RefineLocally
        {
            get => Convert.ToBoolean(_refineLocally);
            set => _refineLocally = Convert.ToInt32(value);
        }

        [XmlAttribute("sRefineMaxStepNum")]
        public int RefineMaxStepNum { get; set; }

        [XmlAttribute("fMaxIterWarning")]
        public int _maxIterWarning;

        [XmlIgnore]
        public bool MaxIterWarning
        {
            get => Convert.ToBoolean(_maxIterWarning);
            set => _maxIterWarning = Convert.ToInt32(value);
        }

        [XmlAttribute("fReduceSize")]
        public int _reduceSize;

        [XmlIgnore]
        public bool ReduceSize
        {
            get => Convert.ToBoolean(_reduceSize);
            set => _reduceSize = Convert.ToInt32(value);
        }

        [XmlAttribute("sSmoothStepNum")]
        public int _smoothStepNum;

        [XmlIgnore]
        public int SmoothStepNum
        {
            get => _smoothStepNum;
            set => _smoothStepNum = RestrictedInteger.MeshSmoothSteps(value);
        }

        [XmlAttribute("fCheckMeshGeom")]
        public int _checkMeshGeom;

        [XmlIgnore]
        public bool CheckMeshGeom
        {
            get => Convert.ToBoolean(_checkMeshGeom);
            set => _checkMeshGeom = Convert.ToInt32(value);
        }

        [XmlAttribute("rCheckGeomMinAngle")]
        public double _checkGeomMinAngle;

        [XmlIgnore]
        public double CheckGeomMinAngle
        {
            get => _checkGeomMinAngle;
            set => _checkGeomMinAngle = RestrictedDouble.NonNegMax_90(value);
        }

        [XmlAttribute("rCheckGeomMaxAngle")]
        public double _checkGeomMaxAngle;

        [XmlIgnore]
        public double CheckGeomMaxAngle
        {
            get => _checkGeomMaxAngle;
            set => _checkGeomMaxAngle = RestrictedDouble.MeshMaxAngle(value);
        }

        [XmlAttribute("rCheckGeomMaxSideRatio")]
        public double _checkGeomMaxSideRatio;

        [XmlIgnore]
        public double CheckGeomMaxSideRatio
        {
            get => _checkGeomMaxSideRatio;
            set => _checkGeomMaxSideRatio = RestrictedDouble.MeshMaxRatio(value);
        }

        [XmlAttribute("fCheckMeshOverlap")]
        public int _checkMeshOverLap;

        [XmlIgnore]
        public bool CheckMeshOverLap
        {
            get => Convert.ToBoolean(_checkMeshOverLap);
            set => _checkMeshOverLap = Convert.ToInt32(value);
        }

        [XmlAttribute("fCheckMeshTopology")]
        public int _checkMeshTopology;

        [XmlIgnore]
        public bool CheckMeshTopology
        {
            get => Convert.ToBoolean(_checkMeshTopology);
            set => _checkMeshTopology = Convert.ToInt32(value);
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MeshFunctions()
        {

        }

        public MeshFunctions(bool refineLocally = true, int refineMaxStepNum = 5, bool iterWarning = false, bool reduceSize = true, int smoothStepNum = 3, bool checkMeshGeom = true, double checkGeomMinangle = 10.0, double checkGeomMaxangle = 170.0, double checkGeomMaxSideRatio = 8.0, bool checkMeshOverlap = true, bool checkMeshTopology = true)
        {
            this.RefineLocally = refineLocally;
            this.RefineMaxStepNum = refineMaxStepNum;
            this.MaxIterWarning = iterWarning;
            this.ReduceSize = reduceSize;
            this.SmoothStepNum = smoothStepNum;
            this.CheckMeshGeom = checkMeshGeom;
            this.CheckGeomMinAngle = checkGeomMinangle;
            this.CheckGeomMaxAngle = checkGeomMaxangle;
            this.CheckGeomMaxSideRatio = checkGeomMaxSideRatio;
            this.CheckMeshOverLap = checkMeshOverlap;
            this.CheckMeshTopology = checkMeshTopology;
        }

        public static MeshFunctions Default()
        {
            return new MeshFunctions(refineLocally : true);
        }
    }

    public partial class MeshPrepare : GlobConfig
    {
        [XmlAttribute("fAutoRegen")]
        public int _autoRegen;

        [XmlIgnore]
        public bool AutoRegen
        {
            get => Convert.ToBoolean(_autoRegen);
            set => _autoRegen = Convert.ToInt32(value);
        }

        [XmlAttribute("fThPeak")]
        public int _thPeak;

        [XmlIgnore]
        public bool ThPeak
        {
            get => Convert.ToBoolean(_thPeak);
            set => _thPeak = Convert.ToInt32(value);
        }

        [XmlAttribute("fThBeam")]
        public int _thBeam;

        [XmlIgnore]
        public bool ThBeam
        {
            get => Convert.ToBoolean(_thBeam);
            set => _thBeam = Convert.ToInt32(value);
        }

        [XmlAttribute("fThColumn")]
        public int _thColumn;

        [XmlIgnore]
        public bool ThColumn
        {
            get => Convert.ToBoolean(_thColumn);
            set => _thColumn = Convert.ToInt32(value);
        }

        [XmlAttribute("fThTruss")]
        public int _thTruss;

        [XmlIgnore]
        public bool ThTruss
        {
            get => Convert.ToBoolean(_thTruss);
            set => _thTruss = Convert.ToInt32(value);
        }

        [XmlAttribute("fThFicBeam")]
        public int _thFicBeam;

        [XmlIgnore]
        public bool ThFicBeam
        {
            get => Convert.ToBoolean(_thFicBeam);
            set => _thFicBeam = Convert.ToInt32(value);
        }

        [XmlAttribute("fThFreeEdge")]
        public int _thFreeEdge;

        [XmlIgnore]
        public bool ThFreeEdge
        {
            get => Convert.ToBoolean(_thFreeEdge);
            set => _thFreeEdge = Convert.ToInt32(value);
        }

        [XmlAttribute("fThRegionBorder")]
        public int _thRegionBordger;

        [XmlIgnore]
        public bool ThRegionBordger
        {
            get => Convert.ToBoolean(_thRegionBordger);
            set => _thRegionBordger = Convert.ToInt32(value);
        }

        [XmlAttribute("fThSuppPt")]
        public int _thSupptPt;

        [XmlIgnore]
        public bool ThSupptPt
        {
            get => Convert.ToBoolean(_thSupptPt);
            set => _thSupptPt = Convert.ToInt32(value);
        }

        [XmlAttribute("fThSuppLn")]
        public int _thSuppLn;

        [XmlIgnore]
        public bool ThSuppLn
        {
            get => Convert.ToBoolean(_thSuppLn);
            set => _thSuppLn = Convert.ToInt32(value);
        }

        [XmlAttribute("fThSuppSf")]
        public int _thSuppSf;

        [XmlIgnore]
        public bool ThSuppSf
        {
            get => Convert.ToBoolean(_thSuppSf);
            set => _thSuppSf = Convert.ToInt32(value);
        }

        [XmlAttribute("fThEdgeConn")]
        public int _thEdgeConn;

        [XmlIgnore]
        public bool ThEdgeConn
        {
            get => Convert.ToBoolean(_thEdgeConn);
            set => _thEdgeConn = Convert.ToInt32(value);
        }

        [XmlAttribute("fThConnPt")]
        public int _thConnPt;

        [XmlIgnore]
        public bool ThConnPt
        {
            get => Convert.ToBoolean(_thConnPt);
            set => _thConnPt = Convert.ToInt32(value);
        }

        [XmlAttribute("fThConnLn")]
        public int _thConnLn;

        [XmlIgnore]
        public bool ThConnLn
        {
            get => Convert.ToBoolean(_thConnLn);
            set => _thConnLn = Convert.ToInt32(value);
        }

        [XmlAttribute("fThConnSf")]
        public int _thConnSf;

        [XmlIgnore]
        public bool ThConnSf
        {
            get => Convert.ToBoolean(_thConnSf);
            set => _thConnSf = Convert.ToInt32(value);
        }

        [XmlAttribute("fThLoadPt")]
        public int _thLoadPt;

        [XmlIgnore]
        public bool ThLoadPt
        {
            get => Convert.ToBoolean(_thLoadPt);
            set => _thLoadPt = Convert.ToInt32(value);
        }

        [XmlAttribute("fThLoadLn")]
        public int _thLoadLn;

        [XmlIgnore]
        public bool ThLoadLn
        {
            get => Convert.ToBoolean(_thLoadLn);
            set => _thLoadLn = Convert.ToInt32(value);
        }

        [XmlAttribute("fThLoadSf")]
        public int _thLoadSf;

        [XmlIgnore]
        public bool ThLoadSf
        {
            get => Convert.ToBoolean(_thLoadSf);
            set => _thLoadSf = Convert.ToInt32(value);
        }

        [XmlAttribute("fThFixPt")]
        public int _thFixPt;

        [XmlIgnore]
        public bool ThFixPt
        {
            get => Convert.ToBoolean(_thFixPt);
            set => _thFixPt = Convert.ToInt32(value);
        }

        [XmlAttribute("fThFixLn")]
        public int _thFixLn;

        [XmlIgnore]
        public bool ThFixLn
        {
            get => Convert.ToBoolean(_thFixLn);
            set => _thFixLn = Convert.ToInt32(value);
        }

        [XmlAttribute("fAutoRebuild")]
        public int _autoRebuild;

        [XmlIgnore]
        public bool AutoRebuild
        {
            get => Convert.ToBoolean(_autoRebuild);
            set => _autoRebuild = Convert.ToInt32(value);
        }

        [XmlAttribute("fAutoSmooth")]
        public int _autoSmooth;

        [XmlIgnore]
        public bool AutoSmooth
        {
            get => Convert.ToBoolean(_autoSmooth);
            set => _autoSmooth = Convert.ToInt32(value);
        }

        [XmlAttribute("fAutoCheck")]
        public int _autoCheck;

        [XmlIgnore]
        public bool AutoCheck
        {
            get => Convert.ToBoolean(_autoCheck);
            set => _autoCheck = Convert.ToInt32(value);
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MeshPrepare()
        {

        }
        public MeshPrepare(bool autoRegen = true, bool thPeak = true, bool thBeam = false, bool thColumn = true, bool thTruss = false, bool thFicBeam = false, bool thFreeEdge = false, bool thRegionBorder = false, 
            bool thSuppPt = true, bool thSuppLn = false, bool thSuppSf = false, bool thEdgeConn = false, bool thConnPt = false, bool thConnLn = false, bool thConnSf = false, bool thLoadPt = false, 
            bool thLoadLn = false, bool thLoadSf = false, bool thFixPt = false, bool thFixLn = false, bool autoRebuild = true, bool autoSmooth = true, bool autoCheck = false)
        {
            this.AutoRegen = autoRegen;
            this.ThPeak = thPeak;
            this.ThBeam = thBeam;
            this.ThColumn = thColumn;
            this.ThTruss = thTruss;
            this.ThFicBeam = thFicBeam;
            this.ThFreeEdge = thFreeEdge;
            this.ThRegionBordger = thRegionBorder;
            this.ThSupptPt = thSuppPt; 
            this.ThSuppLn = thSuppLn;
            this.ThSuppSf = thSuppSf;
            this.ThEdgeConn = thEdgeConn;
            this.ThConnPt = thConnPt;
            this.ThConnLn = thConnLn;
            this.ThConnSf = thConnSf;
            this.ThLoadPt = thLoadPt;
            this.ThLoadLn = thLoadLn;
            this.ThLoadSf = thLoadSf;
            this.ThFixPt = thFixPt;
            this.ThFixLn = thFixLn;
            this.AutoRebuild = autoRebuild;
            this.AutoSmooth = autoSmooth;
            this.AutoCheck = autoCheck;
        }

        public static MeshPrepare Default()
        {
            return new MeshPrepare(autoRegen : true);
        }
    }

    public partial class PeaksmMethod : GlobConfig
    {
        [XmlAttribute("sPeakFormFunc_M")]
        public int _peakFormM { get; set; }

        [XmlIgnore]
        public PeaksmMethodOptions PeakFormM 
        {
            get => (PeaksmMethodOptions)_peakFormM;
            set => _peakFormM = (int)value;
        }

        [XmlAttribute("sPeakFormFunc_N")]
        public int _peakFormN { get; set; }

        [XmlIgnore]
        public PeaksmMethodOptions PeakFormN
        {
            get => (PeaksmMethodOptions)_peakFormN;
            set => _peakFormN = (int)value;
        }

        [XmlAttribute("sPeakFormFunc_V")]
        public int _peakFormV { get; set; }

        [XmlIgnore]
        public PeaksmMethodOptions PeakFormV
        {
            get => (PeaksmMethodOptions)_peakFormV;
            set => _peakFormV = (int)value;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private PeaksmMethod()
        {

        }

        public PeaksmMethod(int peakFormM = 1, int peakFormN = 1, int peakFormV = 1)
        {
            this._peakFormM = peakFormM;
            this._peakFormN = peakFormN;
            this._peakFormV = peakFormV;
        }

        public PeaksmMethod(PeaksmMethodOptions peakFormM = PeaksmMethodOptions.HigherOrderShapeFunc, PeaksmMethodOptions peakFormN = PeaksmMethodOptions.HigherOrderShapeFunc, PeaksmMethodOptions peakFormV = PeaksmMethodOptions.HigherOrderShapeFunc)
        {
            this.PeakFormM = peakFormM;
            this.PeakFormN = peakFormN;
            this.PeakFormV = peakFormV;
        }

        public static PeaksmMethod Default()
        {
            return new PeaksmMethod(peakFormM : 1);
        }

        public enum PeaksmMethodOptions
        {
            [XmlEnum("0")]
            [Parseable("DontSmooth", "dontSmooth", "dontsmooth", "Don't Smooth", "Don't smooth", "don't smooth", "No smooth", "Don't", "don't", "Dont", "dont")]
            DontSmooth = 0,

            [XmlEnum("1")]
            [Parseable("HigherOrderShapeFunc", "higherOrderShapeFunc", "HigherOrderShapeFunction", "HigherOrderShape", "higherOrderShape", "HigherOrder", "higherOrder", "Higher", "Use higher order shape function", "Use higher order shape functions", "Use higher order shape", "Higher order shape function", "higher order shape function", "higher order shape", "higher order", "Higher order shape", "Higher order", "higher")]
            HigherOrderShapeFunc = 1,

            [XmlEnum("2")]
            [Parseable("ConstShapeFunc", "ConstantShapeFunc", "constShapeFunc", "constantShapeFunc", "ConstShapeFunction", "ConstantShapeFunction", "constShapeFunction", "constantShapeFunction", "ConstShape", "constShape", "Const", "const", "Use constant shape function", "Use const shape function", "Use const shape", "Constant shape function", "constant shape function", "Use constant shape", "constant shape", "const shape", "Constant", "constant")]
            ConstShapeFunc = 2,

            [XmlEnum("3")]
            [Parseable("SetToZero", "Set To Zero", "Set to zero", "Zero", "zero")]
            SetToZero = 3
        }
    }
    

    public partial class PeaksmAuto : GlobConfig
    {
        [XmlAttribute("fPeakBeam")]
        public int _peakBeam;

        [XmlIgnore]
        public bool PeakBeam
        {
            get => Convert.ToBoolean(_peakBeam);
            set => _peakBeam = Convert.ToInt32(value);
        }

        [XmlAttribute("fPeakColumn")]
        public int _peakColumn;

        [XmlIgnore]
        public bool PeakColumn
        {
            get => Convert.ToBoolean(_peakColumn);
            set => _peakColumn = Convert.ToInt32(value);
        }

        [XmlAttribute("fPeakTruss")]
        public int _peakTruss;

        [XmlIgnore]
        public bool PeakTruss
        {
            get => Convert.ToBoolean(_peakTruss);
            set => _peakTruss = Convert.ToInt32(value);
        }

        [XmlAttribute("fPeakFicBeam")]
        public int _peakFicBeam;

        [XmlIgnore]
        public bool PeakFicBeam
        {
            get => Convert.ToBoolean(_peakFicBeam);
            set => _peakFicBeam = Convert.ToInt32(value);
        }

        [XmlAttribute("fPeakPlate")]
        public int _peakPlate;

        [XmlIgnore]
        public bool PeakPlate
        {
            get => Convert.ToBoolean(_peakPlate);
            set => _peakPlate = Convert.ToInt32(value);
        }

        [XmlAttribute("fPeakWall")]
        public int _peakWall;

        [XmlIgnore]
        public bool PeakWall
        {
            get => Convert.ToBoolean(_peakWall);
            set => _peakWall = Convert.ToInt32(value);
        }

        [XmlAttribute("fPeakFicShell")]
        public int _peakFicShell;

        [XmlIgnore]
        public bool PeakFicShell
        {
            get => Convert.ToBoolean(_peakFicShell);
            set => _peakFicShell = Convert.ToInt32(value);
        }

        [XmlAttribute("fPeakSuppPt")]
        public int _peakSuppPt;

        [XmlIgnore]
        public bool PeakSuppPt
        {
            get => Convert.ToBoolean(_peakSuppPt);
            set => _peakSuppPt = Convert.ToInt32(value);
        }

        [XmlAttribute("fPeakSuppLn")]
        public int _peakSuppLn;

        [XmlIgnore]
        public bool PeakSuppLn
        {
            get => Convert.ToBoolean(_peakSuppLn);
            set => _peakSuppLn = Convert.ToInt32(value);
        }

        [XmlAttribute("fPeakSuppSf")]
        public int _peakSuppSf;

        [XmlIgnore]
        public bool PeakSuppSf
        {
            get => Convert.ToBoolean(_peakSuppSf);
            set => _peakSuppSf = Convert.ToInt32(value);
        }

        [XmlAttribute("fPeakConnPt")]
        public int _peakConnPt;

        [XmlIgnore]
        public bool PeakConnPt
        {
            get => Convert.ToBoolean(_peakConnPt);
            set => _peakConnPt = Convert.ToInt32(value);
        }

        [XmlAttribute("fPeakConnLn")]
        public int _peakConnLn;

        [XmlIgnore]
        public bool PeakConnLn
        {
            get => Convert.ToBoolean(_peakConnLn);
            set => _peakConnLn = Convert.ToInt32(value);
        }

        [XmlAttribute("fPeakConnSf")]
        public int _peakConnSf;

        [XmlIgnore]
        public bool PeakConnSf
        {
            get => Convert.ToBoolean(_peakConnSf);
            set => _peakConnSf = Convert.ToInt32(value);
        }

        [XmlAttribute("rPeakFactor")]
        public double _peakFactor;

        [XmlIgnore]
        public double PeakFactor 
        {
            get => _peakFactor;
            set => _peakFactor = RestrictedDouble.NonNegMax_5(value);
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private PeaksmAuto()
        {

        }
        public PeaksmAuto(bool peakBeam = false, bool peakColumn = true, bool peakTruss = false, bool peakficBeam = false, bool peakPlate = false, bool peakWall = false, bool peakFicShell = false, bool peakSuppPt = true, bool peakSuppLn = false, bool peakSuppSf = false, bool peakConnPt = false, bool peakConnLn = false, bool peakConnSf = false, double peakFactor = 0.5)
        {
            this.PeakBeam = peakBeam;
            this.PeakColumn = peakColumn;
            this.PeakTruss = peakTruss;
            this.PeakFicBeam = peakficBeam;
            this.PeakPlate = peakPlate;
            this.PeakWall = peakWall;
            this.PeakFicShell = peakFicShell;
            this.PeakSuppPt = peakSuppPt;
            this.PeakSuppLn = peakSuppLn;
            this.PeakSuppSf = peakSuppSf;
            this.PeakConnPt = peakConnPt;
            this.PeakConnLn = peakConnLn;
            this.PeakConnSf = peakConnSf;
            this.PeakFactor = peakFactor;
        }

        public static PeaksmAuto Default()
        {
            return new PeaksmAuto(peakBeam : false);
        }
    }

    public partial class SoilCalculation : GlobConfig
    {
        [XmlAttribute("fSoilAsSolid")]
        public int _soilAsSolid;

        [XmlIgnore]
        public bool SoilAsSolid
        {
            get => Convert.ToBoolean(_soilAsSolid);
            set => _soilAsSolid = Convert.ToInt32(value);
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private SoilCalculation()
        {

        }

        public SoilCalculation(bool soilAsSolid = true)
        {
            this.SoilAsSolid = soilAsSolid;
        }

        public static SoilCalculation Default()
        {
            return new SoilCalculation(soilAsSolid : false);
        }
    }

    /// <summary>
    /// Base class for all GlobalConfigs that can be use for CmdGlobalCfg
    /// </summary>
    public abstract class GlobConfig
    {
        public override string ToString()
        {
            return Results.ResultsReader.ObjectRepresentation(this);
        }
    }
}
