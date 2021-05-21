// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.Geometry;


namespace FemDesign.Loads
{
    /// <summary>
    /// footfall_analysis_data
    /// </summary>
    [System.Serializable]
    public partial class Footfall : EntityBase
    {
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("comment")]
        public string Comment;
        [XmlElement("contour")] 
        public List<Contour> Contours;
        [XmlElement("position")]
        public FdPoint3d Position;

        [XmlIgnore]
        public bool IsSelfExcitation
        {
            get { return Contours != null && Contours.Count > 0 && Position == null; }
        }

        [XmlIgnore]
        public bool IsFullExcitation
        {
            get { return Position != null && Contours == null; }
        }

        private static int selfExcitationInstances = 0;
        private static int fullExcitationInstances = 0;

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Footfall()
        {

        }

        /// <summary>
        /// Create a Footfall self excitation
        /// </summary>
        /// <param name="contours">Contours defining a region to apply footfall analysis on. </param>
        /// <param name="identifier"></param>
        /// <param name="comment"></param>
        public Footfall(List<Contour> contours, string identifier = "SE", string comment = null)
        {
            InitializeSelfExcitation(contours, identifier, comment);
        }

        /// <summary>
        /// Create a Footfall self excitation
        /// </summary>
        /// <param name="region">Region to apply footfall analysis on. </param>
        /// <param name="identifier"></param>
        /// <param name="comment"></param>
        public Footfall(Geometry.Region region, string identifier = "SE", string comment = null)
        {
            InitializeSelfExcitation(region.Contours, identifier, comment);
        }

        /// <summary>
        /// Create a Footfall full excitation
        /// </summary>
        /// <param name="position"></param>
        /// <param name="identifier"></param>
        /// <param name="comment"></param>
        public Footfall(FdPoint3d position, string identifier = "FE", string comment = null)
        {
            InitializeFullExcitation(position, identifier, comment);
        }

        private void InitializeSelfExcitation(List<Contour> contours, string identifier, string comment)
        {
            Contours = contours;
            Footfall.selfExcitationInstances++;
            Name = $"{identifier}.{selfExcitationInstances}";
            Comment = string.IsNullOrEmpty(comment) ? null : comment;
            this.EntityCreated();
        }

        private void InitializeFullExcitation(FdPoint3d position, string identifier, string comment)
        {
            Position = position;
            Footfall.selfExcitationInstances++;
            Name = $"{identifier}.{selfExcitationInstances}";
            Comment = string.IsNullOrEmpty(comment) ? null : comment;
            this.EntityCreated();
        }
    }
}