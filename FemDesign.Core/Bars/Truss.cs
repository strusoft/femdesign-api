using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FemDesign.Geometry;
using FemDesign.Materials;
using FemDesign.Sections;
using StruSoft.Interop.StruXml.Data;

namespace FemDesign.Bars
{
	[XmlRoot("database", Namespace = "urn:strusoft")]
	[System.Serializable]
	public partial class Truss : Bar
	{
		private Truss()
		{
		}
		public Truss(Geometry.Edge edge, Materials.Material material, Sections.Section section, string identifier, StruSoft.Interop.StruXml.Data.Truss_chr_type trussBehaviour = null) :base(edge, material, section, identifier, trussBehaviour)
		{
		}
	}
}
