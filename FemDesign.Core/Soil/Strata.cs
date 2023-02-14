using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Drawing;
using StruSoft.Interop.StruXml.Data;
using System.ComponentModel;

namespace FemDesign.Soil
{
    [System.Serializable]
    public partial class Strata : NamedEntityBase
    {
        [XmlIgnore]
        internal static int _instance = 0; // Shared instance counter for both PointSupport and LineSupport
        protected override int GetUniqueInstanceCount() => 1; // Only ONE instance can be created.

        [XmlElement("contour", Order = 1)]
        public Geometry.HorizontalPolygon2d Contour { get; set; }

        [XmlElement("stratum", Order = 2)]
        public List<Stratum> Stratum { get; set; }

        [XmlElement("water_level", Order = 3)]
        public List<GroundWater> _groundWater { get; set; }

        [XmlIgnore]
        public List<GroundWater> GroundWater
        {
            get
            {
                return _groundWater;
            }
            set
            {
                if( value.GroupBy(x => x.Name).Any(g => g.Count() > 1))
                {
                    throw new Exception("Duplicate Name found. WaterLevel names must be unique.");
                }
                _groundWater = value;
            }
        }


        [XmlAttribute("depth_level_limit")]
        public double _depthLevelLimit { get; set; }

        [XmlIgnore]
        public double DepthLevelLimit
        {
            get
            {
                return this._depthLevelLimit;
            }
            set
            {
                this._depthLevelLimit = RestrictedDouble.ValueInRange(value, -1000000, 0);
            }
        }

        /// <remarks/>
        [XmlAttribute("default_fillings_colour")]
        public string _defaultFillingsColour { get; set; } = "B97A57";

        [XmlIgnore]
        public Color DefaultFillingsColour
        {
            get
            {
                Color col = System.Drawing.ColorTranslator.FromHtml("#" + this._defaultFillingsColour);
                return col;
            }
            set
            {
                this._defaultFillingsColour = ColorTranslator.ToHtml(value).Substring(1);
            }
        }


        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Strata()
        {
        }

        public Strata(List<Stratum> stratum, List<GroundWater> waterLevel, List<Geometry.Point2d> contour, double levelLimit, string identifier = "SOIL")
        {
            this.Stratum = stratum;
            this.GroundWater = waterLevel;
            this.Contour = new Geometry.HorizontalPolygon2d(contour);
            this.DepthLevelLimit = levelLimit;
            this.Identifier = identifier;

            // Strata Object does not have a Guid. Therefore this.EntityCreated() should not be use
            this.EntityModified();
        }
        public override string ToString()
        {
            return $"{this.GetType().Name} {this.Name}, Level Limit {DepthLevelLimit} [m], Stratum: {Stratum.Count} layer, Ground water: {GroundWater.Count} layer";
        }
    }

    public partial class Stratum
    {
        [XmlAttribute("material")]
        public Guid Guid { get; set; }

        [XmlIgnore]
        public Materials.Material _material { get; set; }

        [XmlIgnore]
        public Materials.Material Material
        {
            get
            {
                return _material;
            }
            set
            {
                if (value.Family != "Stratum")
                    throw new ArgumentException("Material should be type of Stratum!");
                this._material = value;
                this.Guid = this._material.Guid;
            }
        }

        [XmlAttribute("colour")]
        public string _colour { get; set; }
        [XmlIgnore]
        public Color? Color
        {
            get
            {
                Color col = System.Drawing.ColorTranslator.FromHtml("#" + this._colour);
                return col;
            }
            set
            {
                this._colour = ColorTranslator.ToHtml((Color)value).Substring(1);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Stratum() { }

        public Stratum(Materials.Material soilMaterial, Color? color = null)
        {
            this.Material = soilMaterial;
            if(color == null)
            {
                var rnd = new Random(Guid.NewGuid().GetHashCode());
                color = System.Drawing.Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            }
            this.Color = color;
        }

        public override string ToString()
        {
            return base.ToString();
        }

    }


    public partial class GroundWater
    {
        [XmlAttribute("colour")]
        public string _colour { get; set; }
        [XmlIgnore]
        public Color? Color
        {
            get
            {
                Color col = System.Drawing.ColorTranslator.FromHtml("#" + this._colour);
                return col;
            }
            set
            {
                this._colour = ColorTranslator.ToHtml((Color)value).Substring(1);
            }
        }

        [XmlAttribute("name")]
        public string Name { get; set; }




        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private GroundWater() { }

        public GroundWater(string name, Color? color = null)
        {
            this.Name = name;
            if (color == null)
            {
                var rnd = new Random();
                color = System.Drawing.Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            }
            this.Color = color;
        }




    }
}