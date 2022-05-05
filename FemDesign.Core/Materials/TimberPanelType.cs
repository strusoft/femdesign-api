using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Materials
{
    /// <summary>
    /// timber_application_data
    /// </summary>
    [System.Serializable]
    public partial class TimberPanelType : IMaterial
    {
        /// <summary>
        /// factors
        /// </summary>
        [XmlElement("factors")]
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
        public System.Guid _panelTypeReference;

        [XmlIgnore]
        private IPanelLibraryType _panelType;
        /// <summary>
        ///  Can be either timberPanelLibraryData, glcDataLibraryType, cltDataLibraryType
        /// </summary>
        /// <value></value>
        [XmlIgnore]
        public IPanelLibraryType PanelType
        {
            get
            {
                return this._panelType;
            }
            set
            {
                this._panelTypeReference = value.Guid;
                this._panelType = value;
            }
        }

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

        [XmlIgnore]
        public string Name {
            get {
                if (PanelType is CltPanelLibraryType clt) return clt.Name;
                else if (PanelType is GlcPanelLibraryType glc) return glc.Name;
                else if (PanelType is OrthotropicPanelLibraryType timber) return timber.Name;
                else throw new NotImplementedException();
            }
            set {
                if (PanelType is CltPanelLibraryType clt) clt.Name = value;
                else if (PanelType is GlcPanelLibraryType glc) glc.Name = value;
                else if (PanelType is OrthotropicPanelLibraryType timber) timber.Name = value;
                else throw new NotImplementedException();
            }
        }

        [XmlIgnore]
        public Guid Guid { 
            get => PanelType.Guid; 
            set => PanelType.Guid = value;
        }

        private TimberPanelType()
        {
            
        }

        public TimberPanelType(IPanelLibraryType panelType)
        {
            PanelType = panelType;
        }

        /// <summary>
        /// Create a TimberPlateMaterial
        /// </summary>
        /// <param name="cltPanelLibraryType"></param>
        /// <param name="factors">TimberFactors</param>
        /// <param name="shearCoupling">Consider shear coupling between layers</param>
        /// <param name="gluedNarrowSides">Glue at narrow sides</param>
        public TimberPanelType(CltPanelLibraryType cltPanelLibraryType, TimberFactors factors, bool shearCoupling = true, bool gluedNarrowSides = true)
        {
            PanelType = cltPanelLibraryType;
            TimberFactors = factors;
            ShearCoupling = shearCoupling;
            GluedNarrowSides = gluedNarrowSides;
        }
    }
}