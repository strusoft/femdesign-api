// https://strusoft.com/

using System;
using System.Globalization;
using System.Xml.Serialization;

namespace FemDesign.Releases
{
    /// <summary>
    /// stiffness_type
    /// </summary>
    [Serializable]
    public partial class StiffnessType
    {
        [XmlAttribute("x_neg")]
        public string _xNeg; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double XNeg
        {
            get { return double.Parse(this._xNeg); }
            set { this._xNeg = RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture); }
        }
        [XmlAttribute("x_pos")]
        
        public string _xPos; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double XPos
        {
            get { return double.Parse(this._xPos); }
            set { this._xPos = RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture); }
        }
        [XmlAttribute("y_neg")]
        
        public string _yNeg; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double YNeg
        {
            get { return double.Parse(this._yNeg); }
            set { this._yNeg = RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture); }
        }
        [XmlAttribute("y_pos")]
        
        public string _yPos; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double YPos
        {
            get { return double.Parse(this._yPos); }
            set { this._yPos = RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture); }
        }
        [XmlAttribute("z_neg")]
        
        public string _zNeg; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double ZNeg
        {
            get { return double.Parse(this._zNeg); }
            set { this._zNeg = RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture); }
        }
        [XmlAttribute("z_pos")]
        public string _zPos; // stiffnessdata_type: stiffness_data_range // non_neg_max_1e15
        [XmlIgnore]
        public double ZPos
        {
            get { return double.Parse(this._zPos); }
            set { this._zPos = RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public StiffnessType()
        {

        }

        public override string ToString()
        {
            return $"{this.GetType().Name} x: (-{this.XNeg}/{this.XPos}), y: (-{this.YNeg}/{this.YPos}), z: (-{this.ZNeg}/{this.ZPos})";
        }
    }
}