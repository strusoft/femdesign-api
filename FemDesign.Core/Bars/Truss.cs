using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FemDesign.Geometry;
using FemDesign.Materials;
using FemDesign.Sections;

namespace FemDesign.Bars
{
	[XmlRoot("database", Namespace = "urn:strusoft")]
	[System.Serializable]
	public partial class Truss : Bar
	{
		private Truss()
		{
		}
		public Truss(Geometry.Edge edge, Materials.Material material, Sections.Section section, string identifier) :base(edge, material, section, identifier)
		{
		}
	}
}
