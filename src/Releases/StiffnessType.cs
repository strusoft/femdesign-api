// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Releases
{
    /// <summary>
    /// stiffness_type
    /// </summary>
    [System.Serializable]
    public class StiffnessType
    {
        [XmlAttribute("x_neg")]
        public double _x_neg; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double x_neg
        {
            get {return this._x_neg;}
            set {this._x_neg = RestrictedDouble.NonNegMax_1e15(value);}
        }
        [XmlAttribute("x_pos")]
        
        public double _x_pos; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double x_pos
        {
            get {return this._x_pos;}
            set {this._x_pos = RestrictedDouble.NonNegMax_1e15(value);}
        }
        [XmlAttribute("y_neg")]
        
        public double _y_neg; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double y_neg
        {
            get {return this._y_neg;}
            set {this._y_neg = RestrictedDouble.NonNegMax_1e15(value);}
        }
        [XmlAttribute("y_pos")]
        
        public double _y_pos; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double y_pos
        {
            get {return this._y_pos;}
            set {this._y_pos = RestrictedDouble.NonNegMax_1e15(value);}
        }
        [XmlAttribute("z_neg")]
        
        public double _z_neg; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double z_neg
        {
            get {return this._z_neg;}
            set {this._z_neg = RestrictedDouble.NonNegMax_1e15(value);}
        }
        [XmlAttribute("z_pos")]
        public double _z_pos; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double z_pos
        {
            get {return this._z_pos;}
            set {this._z_pos = RestrictedDouble.NonNegMax_1e15(value);}
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public StiffnessType()
        {

        }
    }
}