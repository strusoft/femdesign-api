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
        
        [XmlIgnore]
        private bool _mx;

        // binary-rigid
        /// <summary>Translation local-x axis.</summary>
        [XmlAttribute("m_x")]
        public bool Mx
        {
            get
            {
                return this._mx;
            }
            set
            {
                this._mx = value;
            }
        }
        [XmlIgnore]
        private bool _my;

        /// <summary>Translation local-y axis. </summary>
        [XmlAttribute("m_y")]
        public bool My
        {
            get
            {
                return this._my;
            }
            set
            {
                this._my = value;
            }
        }

        [XmlIgnore]
        private bool _mz;

        /// <summary>Translation local-z axis.</summary>
        [XmlAttribute("m_z")]
        public bool Mz
        {
            get
            {
                return this._mz;
            }
            set
            {
                this._mz = value;
            }
        }

        [XmlIgnore]
        private bool _rx;

        /// <summary>Rotation around local-x axis.</summary>
        [XmlAttribute("r_x")]
        public bool Rx
        {
            get
            {
                return this._rx;
            }
            set
            {
                this._rx = value;
            }
        }

        [XmlIgnore]
        private bool _ry;

        /// <summary>Rotation around local-y axis.</summary>
        [XmlAttribute("r_y")]
        public bool Ry
        {
            get
            {
                return this._ry;
            }
            set
            {
                this._ry = value;
            }
        }

        [XmlIgnore]
        private bool _rz;
        
        /// <summary>Rotation around local-z axis.</summary>
        [XmlAttribute("r_z")]
        public bool Rz
        {
            get
            {
                return this._rz;
            }
            set
            {
                this._rz = value;
            }
        }

        // semi-rigid       
        [XmlAttribute("m_x_release")]
        public double _mxRelease; // non_neg_max_1e10. Default = 0. Valid only if m_x is false.
        [XmlIgnore]
        public double MxRelease
        {
            get
            {
                return this._mxRelease;
            }
            set
            {
                this._mxRelease = RestrictedDouble.NonNegMax_1e10(value);
            }
        }
        [XmlAttribute("m_y_release")]
        public double _myRelease; // non_neg_max_1e10. Default = 0. Valid only if m_y is false.
        [XmlIgnore]
        public double MyRelease
        {
            get
            {
                return this._myRelease;
            }
            set
            {
                this._myRelease = RestrictedDouble.NonNegMax_1e10(value);
            }
        }
        [XmlAttribute("m_z_release")]
        public double _mzRelease; // non_neg_max_1e10. Default = 0. Valid only if m_z is false.
        [XmlIgnore]
        public double MzRelease
        {
            get
            {
                return this._mzRelease;
            }
            set
            {
                this._mzRelease = RestrictedDouble.NonNegMax_1e10(value);
            }
        }
        [XmlAttribute("r_x_release")]
        public double _rxRelease; // non_neg_max_1e10. Default = 0. Valid only if r_x is false.
        [XmlIgnore]
        public double RxRelease
        {
            get
            {
                return this._rxRelease;
            }
            set
            {
                this._rxRelease = RestrictedDouble.NonNegMax_1e10(value);
            }
        }
        [XmlAttribute("r_y_release")]
        public double _ryRelease; // non_neg_max_1e10. Default = 0. Valid only if r_y is false.
        [XmlIgnore]
        public double RyRelease
        {
            get
            {
                return this._ryRelease;
            }
            set
            {
                this._ryRelease = RestrictedDouble.NonNegMax_1e10(value);
            }
        }
        [XmlAttribute("r_z_release")]
        public double _rzRelease; // non_neg_max_1e10. Default = 0. Valid only if r_z is false.
        [XmlIgnore]
        public double RzRelease
        {
            get
            {
                return this._rzRelease;
            }
            set
            {
                this._rzRelease = RestrictedDouble.NonNegMax_1e10(value);
            }
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
        /// Private constructor for semi-rigid definition.
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="my"></param>
        /// <param name="mz"></param>
        /// <param name="rx"></param>
        /// <param name="ry"></param>
        /// <param name="rz"></param>
        /// <param name="mxRelease"></param>
        /// <param name="myRelease"></param>
        /// <param name="mzRelease"></param>
        /// <param name="rxRelease"></param>
        /// <param name="ryRelease"></param>
        /// <param name="rzRelease"></param>
        public Connectivity(bool mx, bool my, bool mz, bool rx, bool ry, bool rz, double mxRelease, double myRelease, double mzRelease, double rxRelease, double ryRelease, double rzRelease)
        {
            this.Mx = mx;
            this.My = my;
            this.Mz = mz;
            this.Rx = rx;
            this.Ry = ry;
            this.Rz = rz;
            this.MxRelease = mxRelease;
            this.MyRelease = myRelease;
            this.MzRelease = mzRelease;
            this.RxRelease = rxRelease;
            this.RyRelease = ryRelease;
            this.RzRelease = rzRelease;
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
        /// Define releases for a bar-element.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="mx">Release stiffness. Translation local-x axis. Bool or number: true if rigid, false if free, positive number if semi-rigid with stiffness [kN/m]. Optional, default value is fully rigid.</param>
        /// <param name="my">Release stiffness. Translation local-y axis. Bool or number: true if rigid, false if free, positive number if semi-rigid with stiffness [kN/m]. Optional, default value is fully rigid.</param>
        /// <param name="mz">Release stiffness. Translation local-z axis. Bool or number: true if rigid, false if free, positive number if semi-rigid with stiffness [kN/m]. Optional, default value is fully rigid.</param>
        /// <param name="rx">Release stiffness. Rotation around local-x axis. Bool or number: true if rigid, false if free, positive number if semi-rigid with stiffness [kNm/rad]. Optional, default value is fully rigid.</param>
        /// <param name="ry">Release stiffness. Rotation around local-y axis. Bool or number: true if rigid, false if free, positive number if semi-rigid with stiffness [kNm/rad]. Optional, default value is fully rigid.</param>
        /// <param name="rz">Release stiffness. Rotation around local-z axis. Bool or number: true if rigid, false if free, positive number if semi-rigid with stiffness [kNm/rad]. Optional, default value is fully rigid.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Connectivity SemiRigid([DefaultArgument("true")] System.Object mx, [DefaultArgument("true")] System.Object my, [DefaultArgument("true")] System.Object mz, [DefaultArgument("true")] System.Object rx, [DefaultArgument("true")] System.Object ry, [DefaultArgument("true")] System.Object rz)
        {
            System.Object[] items = new System.Object[]
            {
                mx,
                my,
                mz,
                rx,
                ry,
                rz
            };

            bool[] releases = new bool[6];
            double[] stiffnesses = new double[6];

            for (int idx = 0; idx < items.Length; idx++)
            {

                if (items[idx].GetType() == typeof(bool))
                {
                    releases[idx] = (bool)items[idx];
                    stiffnesses[idx] = 0;
                }
                else if (items[idx].GetType() == typeof(System.Int64))
                {
                    releases[idx] = false;
                    stiffnesses[idx] = (System.Int64)items[idx];
                }
                else if (items[idx].GetType() == typeof(System.Double))
                {
                    releases[idx] = false;
                    stiffnesses[idx] = (double)items[idx];
                }
                else
                {
                    throw new System.ArgumentException($"{items[idx].GetType()} is not bool, {typeof(System.Int64)} or {typeof(System.Double)}.");
                }
            }

            return new Connectivity(releases[0], releases[1], releases[2], releases[3], releases[4], releases[5], stiffnesses[0], stiffnesses[1], stiffnesses[2], stiffnesses[3], stiffnesses[4], stiffnesses[5]);
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