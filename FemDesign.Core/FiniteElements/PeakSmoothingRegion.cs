using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.Geometry;
using FemDesign.GenericClasses;


namespace FemDesign.FiniteElements
{
    [System.Serializable]
    public partial class PeakSmoothingRegion : EntityBase, IStructureElement
    {
        [XmlAttribute("inactive")]
        public bool _inactive = false;
        [XmlIgnore]
        public bool Inactive
        {
            get { return this._inactive; }
            set { this._inactive = value; }
        }
        [XmlElement("contour")]
        public List<Geometry.Contour> _contour;
        [XmlIgnore]
        public List<Geometry.Contour> Contours
        {
            get { return this._contour; }
            set { this._contour = value; }
        }
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private PeakSmoothingRegion()
        {

        }

        public PeakSmoothingRegion(Region region, bool inactive = false)
        {
            this.EntityCreated();
            this.Contours = region.Contours;
            this.Inactive = inactive;
        }
        public PeakSmoothingRegion(List<Geometry.Contour> contours, bool inactive = false)
        {
            this.EntityCreated();
            this.Contours = contours;
            this.Inactive = inactive;
        }
    }
}
