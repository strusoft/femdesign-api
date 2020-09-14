using System;
using System.Xml.Serialization;


namespace FemDesign
{
    [System.Serializable]
    public class VirtualBar: EntityBase
    {
        private static int Instance = 0;
        [XmlElement("edge", Order = 1)]
        public Geometry.Edge Edge { get; set; }
        [XmlElement("local-y", Order = 2)]
        public Geometry.FdVector3d _localY;
        [XmlIgnore]
        public Geometry.FdVector3d LocalY
        {
            get
            {
                return this._localY;
            }
            set
            {
                Geometry.FdVector3d val = value.Normalize();
                double dot = this.Edge.CoordinateSystem.LocalX.Dot(val);
                if (Math.Abs(dot) < Tolerance.DotProduct)
                {
                    this._localY = val;
                }

                else
                {
                    throw new System.ArgumentException($"X-axis is not perpendicular to y-axis: {value}. The dot-product is {dot}, but should be 0");
                }
            }
        }
        [XmlElement("connectivity", Order = 3)]
        public Bars.Connectivity[] _connectivity = new Bars.Connectivity[2];
        [XmlIgnore]
        public Bars.Connectivity StartConnectivity
        {
            get
            {
                return this._connectivity[0];
            }
            set
            {
                this._connectivity[0] = value;
            }
        }
        [XmlIgnore]
        public Bars.Connectivity EndConnectivity
        {
            get
            {
                return this._connectivity[1];
            }
            set
            {
                this._connectivity[1] = value;
            }
        }
        [XmlAttribute("name")]
        public string _name;
        [XmlIgnore]
        public string Name
        { 
            get
            {
                return this._name;
            }
            set
            {
                VirtualBar.Instance++;
                this._name = RestrictedString.Length(value, 40) + "." + VirtualBar.Instance.ToString();
            }
        }
        [XmlAttribute("AE")]
        public double _ae;
        [XmlIgnore]
        public double AE
        {
            get
            {
                return this._ae;
            }
            set
            {
                this._ae = RestrictedDouble.Positive(value);
            }
        }
        [XmlAttribute("ItG")]
        public double _itg;
        [XmlIgnore]
        public double ItG
        {
            get
            {
                return this._itg;
            }
            set
            {
                this._itg = RestrictedDouble.Positive(value);
            }
        }
        [XmlAttribute("I1E")]
        public double _i1e;
        [XmlIgnore]
        public double I1E
        {
            get
            {
                return this._i1e;
            }
            set
            {
                this._i1e = RestrictedDouble.Positive(value);
            }
        }
        [XmlAttribute("I2E")]
        public double _i2e;
        [XmlIgnore]
        public double I2E
        {
            get
            {
                return this._i2e;
            }
            set
            {
                this._i2e = RestrictedDouble.Positive(value);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private VirtualBar()
        {

        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        public VirtualBar(Geometry.Edge edge, Geometry.FdVector3d localY, Bars.Connectivity startConnectivity, Bars.Connectivity endConnectivity, string name, double ae, double itg, double i1e, double i2e)
        {
            this.EntityCreated();
            this.Edge = edge;
            this.LocalY = localY;
            this.StartConnectivity = startConnectivity;
            this.EndConnectivity = endConnectivity;
            this.Name = name;
            this.AE = ae;
            this.ItG = itg;
            this.I1E = i1e;
            this.I2E = i2e;
        }
    }
}