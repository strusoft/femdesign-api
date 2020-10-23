// https://strusoft.com/
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Bars
{
    /// <summary>
    /// connectivity_type
    /// 
    /// Connectivity / End-condition releases
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class Connectivity
    {
        // binary-rigid
        /// <summary>Translation local-x axis.</summary>
        [XmlAttribute("m_x")]
        public bool Mx { get; set; }
        /// <summary>Translation local-y axis. </summary>
        [XmlAttribute("m_y")]
        public bool My { get; set; }
        /// <summary>Translation local-z axis.</summary>
        [XmlAttribute("m_z")]
        public bool Mz { get; set; }
        /// <summary>Rotation around local-x axis.</summary>
        [XmlAttribute("r_x")]
        public bool Rx { get; set; }
        /// <summary>Rotation around local-y axis.</summary>
        [XmlAttribute("r_y")]
        public bool Ry { get; set; }
        /// <summary>Rotation around local-z axis.</summary>
        [XmlAttribute("r_z")]
        public bool Rz { get; set; }

        // semi-rigid       
        [XmlAttribute("m_x_release")]
        public double _mxRelease; // non_neg_max_1e10. Default = 0. Valid only if m_x is false.
        [XmlIgnore]
        public double MxRelease
        {
            get {return this._mxRelease;}
            set {this._mxRelease = RestrictedDouble.NonNegMax_1e10(value);}
        }
        [XmlAttribute("m_y_release")]
        public double _myRelease; // non_neg_max_1e10. Default = 0. Valid only if m_y is false.
        [XmlIgnore]
        public double MyRelease
        {
            get {return this._myRelease;}
            set {this._myRelease = RestrictedDouble.NonNegMax_1e10(value);}
        }
        [XmlAttribute("m_z_release")]
        public double _mzRelease; // non_neg_max_1e10. Default = 0. Valid only if m_z is false.
        [XmlIgnore]
        public double MzRelease
        {
            get {return this._mzRelease;}
            set {this._mzRelease = RestrictedDouble.NonNegMax_1e10(value);}
        }
        [XmlAttribute("r_x_release")]
        public double _rxRelease; // non_neg_max_1e10. Default = 0. Valid only if r_x is false.
        [XmlIgnore]
        public double RxRelease
        {
            get {return this._rxRelease;}
            set {this._rxRelease = RestrictedDouble.NonNegMax_1e10(value);}
        }
        [XmlAttribute("r_y_release")]
        public double _ryRelease; // non_neg_max_1e10. Default = 0. Valid only if r_y is false.
        [XmlIgnore]
        public double RyRelease
        {
            get {return this._ryRelease;}
            set {this._ryRelease = RestrictedDouble.NonNegMax_1e10(value);}
        }
        [XmlAttribute("r_z_release")]
        public double _rzRelease; // non_neg_max_1e10. Default = 0. Valid only if r_z is false.
        [XmlIgnore]
        public double RzRelease
        {
            get {return this._rzRelease;}
            set {this._rzRelease = RestrictedDouble.NonNegMax_1e10(value);}
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Connectivity()
        {
            
        }
        
        /// <summary>
        /// Private constructor for binary-rigid definition.
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="my"></param>
        /// <param name="mz"></param>
        /// <param name="rx"></param>
        /// <param name="ry"></param>
        /// <param name="rz"></param>
        private Connectivity(bool mx, bool my, bool mz, bool rx, bool ry, bool rz)
        {
            this.Mx = mx;
            this.My = my;
            this.Mz = mz;
            this.Rx = rx;
            this.Ry = ry;
            this.Rz = rz;
        }

        /// <summary>
        /// Define releases for a bar-element.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="mx">Translation local-x axis. True if rigid, false if free.</param>
        /// <param name="my">Translation local-y axis. True if rigid, false if free.</param>
        /// <param name="mz">Translation local-z axis. True if rigid, false if free.</param>
        /// <param name="rx">Rotation around local-x axis. True if rigid, false if free.</param>
        /// <param name="ry">Rotation around local-y axis. True if rigid, false if free.</param>
        /// <param name="rz">Rotation around local-z axis. True if rigid, false if free.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Connectivity Define(bool mx, bool my, bool mz, bool rx, bool ry, bool rz)
        {
            return new Connectivity(mx, my, mz, rx, ry, rz);
        }

        /// <summary>
        /// Define default (rigid) releases for a bar-element.
        /// </summary>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static Connectivity Default()
        {
            return Connectivity.Rigid();
        }

        /// <summary>
        /// Define hinged releases for a bar-element.
        /// </summary>
        /// <remarks>Create</remarks>
        [IsVisibleInDynamoLibrary(true)]
        public static Connectivity Hinged()
        {
            Connectivity connectivity = new Connectivity(true, true, true, true, false, false);
            return connectivity;
        }
        /// <summary>
        /// Define rigid releases for a bar-element.
        /// </summary>
        /// <remarks>Create</remarks>
        [IsVisibleInDynamoLibrary(true)]
        public static Connectivity Rigid()
        {
            Connectivity connectivity = new Connectivity(true, true, true, true, true, true);
            return connectivity;
        }
    }
}