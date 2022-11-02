using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.GenericClasses;


namespace FemDesign.ModellingTools
{
    [System.Serializable]
    public partial class FictitiousBar: NamedEntityBase, IStructureElement
    {
        [XmlIgnore]
        private static int _ficticiousBarInstances = 0;
        protected override int GetUniqueInstanceCount() => ++_ficticiousBarInstances;

        [XmlElement("edge", Order = 1)]
        public Geometry.Edge Edge { get; set; }

        [XmlIgnore]
        private Geometry.CoordinateSystem _coordinateSystem;

        [XmlIgnore]
        private Geometry.CoordinateSystem CoordinateSystem
        {
            get
            {
                if (this._coordinateSystem == null)
                {
                    this._coordinateSystem = this.Edge.CoordinateSystem;
                    return this._coordinateSystem;
                }
                else
                {
                    return this._coordinateSystem;
                }
            }
            set
            {
                this._coordinateSystem = value;
                this._localY = value.LocalY;
            }
        }

        [XmlIgnore]
        public Geometry.Point3d LocalOrigin
        {
            get
            {
                return this.CoordinateSystem.Origin;
            }
        }

        [XmlIgnore]
        public Geometry.Vector3d LocalX
        {
            get
            {
                return this.CoordinateSystem.LocalX;
            }
        }

        [XmlElement("local-y", Order = 2)]
        public Geometry.Vector3d _localY;

        [XmlIgnore]
        public Geometry.Vector3d LocalY
        {
            get
            {
                return this._localY;
            }
            set
            {
                this.CoordinateSystem.SetYAroundX(value);
                this._localY = this.CoordinateSystem.LocalY;
            }
        }

        [XmlIgnore]
        public Geometry.Vector3d LocalZ
        {
            get
            {
                return this.CoordinateSystem.LocalZ;
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
        private FictitiousBar()
        {

        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        public FictitiousBar(Geometry.Edge edge, Geometry.Vector3d localY, Bars.Connectivity startConnectivity, Bars.Connectivity endConnectivity, string identifier, double ae, double itg, double i1e, double i2e)
        {
            this.EntityCreated();
            this.Edge = edge;
            this.LocalY = localY;
            this.StartConnectivity = startConnectivity;
            this.EndConnectivity = endConnectivity;
            this.Identifier = identifier;
            this.AE = ae;
            this.ItG = itg;
            this.I1E = i1e;
            this.I2E = i2e;
        }

        /// <summary>
        /// Orient this object's coordinate system to GCS.
        /// </summary>
        public void OrientCoordinateSystemToGCS()
        {
            var cs = this.CoordinateSystem;
            cs.OrientEdgeTypeLcsToGcs();
            this.CoordinateSystem = cs;
        }

    }
}