using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.ModellingTool
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
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
        /// <param name="localY">LocalY</param>
        /// <param name="identifier">Identifier. Optional.</param>
        /// <returns></returns>
        public static VirtualBar Define(Autodesk.DesignScript.Geometry.Curve curve, double AE, double ItG, double I1E, double I2E, [DefaultArgument("FemDesign.Bars.Connectivity.Default()")] List<Bars.Connectivity> connectivity, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localY, [DefaultArgument("BF")] string identifier)
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
            VirtualBar bar = new VirtualBar(edge, edge.CoordinateSystem.LocalY, startConnectivity, endConnectivity, identifier, AE, ItG, I1E, I2E);

            // set local y-axis
            if (!localY.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                bar.LocalY = FemDesign.Geometry.FdVector3d.FromDynamo(localY);
            }

            // return
            return bar;
        }
        #endregion
    }
}