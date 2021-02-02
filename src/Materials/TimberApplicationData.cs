using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Materials
{
    /// <summary>
    /// timber_application_data
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class TimberApplicationData
    {
        /// <summary>
        /// factors
        /// </summary>
        [XmlElement("factors", Order = 1)]
        public List<TimberFactors> _timberFactors;
        [XmlIgnore]
        public TimberFactors TimberFactors
        {
            get
            {
                if (_timberFactors == null)
                {
                    return null;
                }
                else
                {
                    return this._timberFactors[0];
                }
            }
            set
            {
                this._timberFactors = new List<TimberFactors>{value};
            }
        }

        [XmlAttribute("panel_type")]
        public System.Guid PanelType;

        /// <summary>
        /// shear_coupling
        /// </summary>
        [XmlAttribute("shear_coupling")]
        public bool _shearCoupling = true;
        [XmlIgnore]
        public bool ShearCoupling
        {
            get
            {
                return this._shearCoupling;
            }
            set
            {
                this._shearCoupling = value;
            }
        }

        /// <summary>
        /// glued_narrow_sides
        /// </summary>
        [XmlAttribute("glued_narrow_sides")]
        public bool _gluedNarrowSides = true;
        [XmlIgnore]
        public bool GluedNarrowSides
        {
            get
            {
                return this._gluedNarrowSides;
            }
            set
            {
                this._gluedNarrowSides = value;
            }
        }
    }
}