using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.ModellingTools
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class FictitiousBar: EntityBase
    {
        [XmlIgnore]
        private static int Instance = 0;

        [XmlElement("edge", Order = 1)]
        public Geometry.Edge Edge { get; set; }

        [XmlIgnore]
        private Geometry.FdCoordinateSystem _coordinateSystem;

        [XmlIgnore]
        private Geometry.FdCoordinateSystem CoordinateSystem
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
        public Geometry.FdPoint3d LocalOrigin
        {
            get
            {
                return this.CoordinateSystem.Origin;
            }
        }

        [XmlIgnore]
        public Geometry.FdVector3d LocalX
        {
            get
            {
                return this.CoordinateSystem.LocalX;
            }
        }

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
                this.CoordinateSystem.SetYAroundX(value);
                this._localY = this.CoordinateSystem.LocalY;
            }
        }

        [XmlIgnore]
        public Geometry.FdVector3d LocalZ
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
                FictitiousBar.Instance++;
                this._name = RestrictedString.Length(value, 40) + "." + FictitiousBar.Instance.ToString();
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
        public FictitiousBar(Geometry.Edge edge, Geometry.FdVector3d localY, Bars.Connectivity startConnectivity, Bars.Connectivity endConnectivity, string name, double ae, double itg, double i1e, double i2e)
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

        /// <summary>
        /// Orient this object's coordinate system to GCS.
        /// <summary>
        public void OrientCoordinateSystemToGCS()
        {
            var cs = this.CoordinateSystem;
            cs.OrientEdgeTypeLcsToGcs();
            this.CoordinateSystem = cs;
        }

        #region dynamo
        [IsVisibleInDynamoLibrary(true)]
        /// <summary>
        /// Create a fictitious bar element.
        /// </summary>
        /// <param name="curve">Line or Arc.</param>
        /// <param name="AE">AE</param>
        /// <param name="ItG">ItG</param>
        /// <param name="I1E">I1E</param>
        /// <param name="I2E">I2E</param>
        /// <param name="connectivity">Connectivity. If 1 item this item defines both start and end connectivity. If two items the first item defines the start connectivity and the last item defines the end connectivity.</param>
        /// <param name="localY">Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS</param>
        /// <param name="orientLCS">Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.</param>
        /// <param name="identifier">Identifier. Optional.</param>
        /// <returns></returns>
        public static FictitiousBar Define(Autodesk.DesignScript.Geometry.Curve curve, [DefaultArgument("10000000")] double AE, [DefaultArgument("10000000")] double ItG, [DefaultArgument("10000000")] double I1E, [DefaultArgument("10000000")] double I2E, [DefaultArgument("FemDesign.Bars.Connectivity.Default()")] List<Bars.Connectivity> connectivity, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localY, [DefaultArgument("true")] bool orientLCS, string identifier = "BF")
        {
            // convert geometry
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc2(curve);
            Geometry.FdVector3d y = Geometry.FdVector3d.FromDynamo(localY);

            // get connectivity
            Bars.Connectivity startConnectivity;
            Bars.Connectivity endConnectivity;
            if (connectivity.Count == 1)
            {
                startConnectivity = connectivity[0];
                endConnectivity = connectivity[0];
            }
            else if (connectivity.Count == 2)
            {
                startConnectivity = connectivity[0];
                endConnectivity = connectivity[1];
            }
            else
            {
                throw new System.ArgumentException($"Connectivity must contain 1 or 2 items. Number of items is {connectivity.Count}");
            }

            // create virtual bar
            FictitiousBar bar = new FictitiousBar(edge, edge.CoordinateSystem.LocalY, startConnectivity, endConnectivity, identifier, AE, ItG, I1E, I2E);

            // set local y-axis
            if (!localY.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                bar.LocalY = FemDesign.Geometry.FdVector3d.FromDynamo(localY);
            }

            // else orient coordinate system to GCS
            else
            {
                if (orientLCS)
                {
                    bar.OrientCoordinateSystemToGCS();
                }
            }

            // return
            return bar;
        }
        #endregion
    }
}