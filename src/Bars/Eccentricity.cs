// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Bars
{
    /// <summary>
    /// ecc_value_type
    /// 
    /// This class called Eccentricity as it is the Dynamo facing class.
    /// </summary>
    [System.Serializable]
    public class Eccentricity
    {
        [XmlAttribute("x")]
        public double _x; // abs_max_1000
        [XmlIgnore]
        public double x
        {
            get {return this._x;}
            set {this._x = RestrictedDouble.AbsMax_1000(value);}
        }
        /// <summary>Eccentricity local-y.</summary>
        [XmlAttribute("y")]
        public double _y; // abs_max_1000
        [XmlIgnore]
        public double y
        {
            get {return this._y;}
            set {this._y = RestrictedDouble.AbsMax_1000(value);}
        }
        /// <summary>Eccentricity local-z.</summary>
        [XmlAttribute("z")]
        public double _z; // abs_max_1000
        [XmlIgnore]
        public double z
        {
            get {return this._z;}
            set {this._z = RestrictedDouble.AbsMax_1000(value);}
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Eccentricity()
        {
            
        }

        internal Eccentricity(double y = 0, double z = 0)
        {
            this.x = 0;
            this.y = y;
            this.z = z;
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Eccentricity p = obj as Eccentricity;
            if ((System.Object)p == null)
            {
                return false;
            }
            return (x == p.x) && (y == p.y) && (z == p.z);            
        }

        public bool Equals(Eccentricity p)
        {
            if ((object)p == null)
            {
                return false;
            }
            return (x == p.x) && (y == p.y) && (z == p.z);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }
        
        /// <summary>
        /// Define the eccentricity of bar-element along its local axes.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="y">Eccentricity local-y</param>
        /// <param name="z">Eccentricity local-z</param>
        public static Eccentricity Define(double y = 0, double z = 0)
        {
            return new Eccentricity(y, z);
        }

        /// <summary>
        /// Create a default eccentricity, i.e. y=z=0.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        public static Eccentricity Default()
        {
            return new Eccentricity(0, 0);
        }
    }
}