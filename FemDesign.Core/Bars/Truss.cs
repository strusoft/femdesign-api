using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;

namespace FemDesign.Bars
{
	[System.Serializable]
	public partial class Truss : Bar
	{
		internal Truss()
		{

		}

		public Truss(Geometry.Edge edge, Materials.Material material, Sections.Section section, string identifier) :base(edge, material, section, identifier)
		{

		}
	}
}
