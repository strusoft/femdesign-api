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
    [IsVisibleInDynamoLibrary(true)]
    public class Connectivity
    {
        // binary-rigid
        /// <summary>Translation local-x axis.</summary>
        [XmlAttribute("m_x")]
        public bool m_x { get; set; }
        /// <summary>Translation local-y axis. </summary>
        [XmlAttribute("m_y")]
        public bool m_y { get; set; }
        /// <summary>Translation local-z axis.</summary>
        [XmlAttribute("m_z")]
        public bool m_z { get; set; }
        /// <summary>Rotation around local-x axis.</summary>
        [XmlAttribute("r_x")]
        public bool r_x { get; set; }
        /// <summary>Rotation around local-y axis.</summary>
        [XmlAttribute("r_y")]
        public bool r_y { get; set; }
        /// <summary>Rotation around local-z axis.</summary>
        [XmlAttribute("r_z")]
        public bool r_z { get; set; }

        // semi-rigid       
        [XmlAttribute("m_x_release")]
        public double _m_x_release; // non_neg_max_1e10. Default = 0. Valid only if m_x is false.
        [XmlIgnore]
        public double m_x_release
        {
            get {return this._m_x_release;}
            set {this._m_x_release = RestrictedDouble.NonNegMax_1e10(value);}
        }
        [XmlAttribute("m_y_release")]
        public double _m_y_release; // non_neg_max_1e10. Default = 0. Valid only if m_y is false.
        [XmlIgnore]
        public double m_y_release
        {
            get {return this._m_y_release;}
            set {this._m_y_release = RestrictedDouble.NonNegMax_1e10(value);}
        }
        [XmlAttribute("m_z_release")]
        public double _m_z_release; // non_neg_max_1e10. Default = 0. Valid only if m_z is false.
        [XmlIgnore]
        public double m_z_release
        {
            get {return this._m_z_release;}
            set {this._m_z_release = RestrictedDouble.NonNegMax_1e10(value);}
        }
        [XmlAttribute("r_x_release")]
        public double _r_x_release; // non_neg_max_1e10. Default = 0. Valid only if r_x is false.
        [XmlIgnore]
        public double r_x_release
        {
            get {return this._r_x_release;}
            set {this._r_x_release = RestrictedDouble.NonNegMax_1e10(value);}
        }
        [XmlAttribute("r_y_release")]
        public double _r_y_release; // non_neg_max_1e10. Default = 0. Valid only if r_y is false.
        [XmlIgnore]
        public double r_y_release
        {
            get {return this._r_y_release;}
            set {this._r_y_release = RestrictedDouble.NonNegMax_1e10(value);}
        }
        [XmlAttribute("r_z_release")]
        public double _r_z_release; // non_neg_max_1e10. Default = 0. Valid only if r_z is false.
        [XmlIgnore]
        public double r_z_release
        {
            get {return this._r_z_release;}
            set {this._r_z_release = RestrictedDouble.NonNegMax_1e10(value);}
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
        /// <param name="m_x"></param>
        /// <param name="m_y"></param>
        /// <param name="m_z"></param>
        /// <param name="r_x"></param>
        /// <param name="r_y"></param>
        /// <param name="r_z"></param>
        private Connectivity(bool m_x, bool m_y, bool m_z, bool r_x, bool r_y, bool r_z)
        {
            this.m_x = m_x;
            this.m_y = m_y;
            this.m_z = m_z;
            this.r_x = r_x;
            this.r_y = r_y;
            this.r_z = r_z;
        }

        /// <summary>
        /// Define releases for a bar-element.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="m_x">Translation local-x axis.</param>
        /// <param name="m_y">Translation local-y axis.</param>
        /// <param name="m_z">Translation local-z axis.</param>
        /// <param name="r_x">Rotation around local-x axis.</param>
        /// <param name="r_y">Rotation around local-y axis.</param>
        /// <param name="r_z">Rotation around local-z axis.</param>
        public static Connectivity Define(bool m_x, bool m_y, bool m_z, bool r_x, bool r_y, bool r_z)
        {
            return new Connectivity(m_x, m_y, m_z, r_x, r_y, r_z);
        }
        /// <summary>
        /// Define hinged releases for a bar-element.
        /// </summary>
        /// <remarks>Create</remarks>
        public static Connectivity Hinged()
        {
            Connectivity connectivity = new Connectivity(true, true, true, true, false, false);
            return connectivity;
        }
        /// <summary>
        /// Define rigid releases for a bar-element.
        /// </summary>
        /// <remarks>Create</remarks>
        public static Connectivity Rigid()
        {
            Connectivity connectivity = new Connectivity(true, true, true, true, true, true);
            return connectivity;
        }
    }
}