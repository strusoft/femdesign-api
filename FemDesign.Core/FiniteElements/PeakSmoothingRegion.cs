﻿using System;
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
        public List<Geometry.Contour> _contours;

        [XmlIgnore]
        public List<Geometry.Contour> Contours
        {
            get { return this._contours; }
            set { this._contours = value; }
        }

        [XmlIgnore]
        public Region Region
        {
            get { return new Region(this.Contours); }
            set { this._contours = value.Contours; }
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
            this.Region = region;
            this.Inactive = inactive;
        }
        public PeakSmoothingRegion(List<Geometry.Contour> contours, bool inactive = false)
        {
            this.EntityCreated();
            this.Contours = contours;
            this.Inactive = inactive;
        }
        public override string ToString()
        {
            if (this.Inactive)
            {
                return $"{this.GetType().Name}, Inactive";
            }
            else
            {
                return $"{this.GetType().Name}, Active";
            }
        }
    }
}
