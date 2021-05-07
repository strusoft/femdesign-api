// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Releases
{
    /// <summary>
    /// stiffness_type
    /// </summary>
    [System.Serializable]
    public partial class StiffnessType
    {
        [XmlAttribute("x_neg")]
        public double _xNeg; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double XNeg
        {
            get {return this._xNeg;}
            set {this._xNeg = RestrictedDouble.NonNegMax_1e15(value);}
        }
        [XmlAttribute("x_pos")]
        
        public double _xPos; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double XPos
        {
            get {return this._xPos;}
            set {this._xPos = RestrictedDouble.NonNegMax_1e15(value);}
        }
        [XmlAttribute("y_neg")]
        
        public double _yNeg; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double YNeg
        {
            get {return this._yNeg;}
            set {this._yNeg = RestrictedDouble.NonNegMax_1e15(value);}
        }
        [XmlAttribute("y_pos")]
        
        public double _yPos; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double YPos
        {
            get {return this._yPos;}
            set {this._yPos = RestrictedDouble.NonNegMax_1e15(value);}
        }
        [XmlAttribute("z_neg")]
        
        public double _zNeg; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double ZNeg
        {
            get {return this._zNeg;}
            set {this._zNeg = RestrictedDouble.NonNegMax_1e15(value);}
        }
        [XmlAttribute("z_pos")]
        public double _zPos; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double ZPos
        {
            get {return this._zPos;}
            set {this._zPos = RestrictedDouble.NonNegMax_1e15(value);}
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public StiffnessType()
        {

        }
    }
}