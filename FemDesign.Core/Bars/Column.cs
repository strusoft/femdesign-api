using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign.Geometry;
using FemDesign.Materials;
using FemDesign.Sections;

namespace FemDesign.Bars
{
	[System.Serializable]
	public partial class Column : Bar
	{
		internal Column()
		{

		}

		public Column(Geometry.Edge edge, Materials.Material material, Sections.Section startSection, Sections.Section endSection, Eccentricity startEccentricity, Eccentricity endEccentricity, Connectivity startConnectivity, Connectivity endConnectivity, string identifier) : base(edge, BarType.Column, material, startSection, endSection, startEccentricity, endEccentricity, startConnectivity, endConnectivity, identifier)
		{
		}

		public Column(Geometry.Edge edge, Materials.Material material, Sections.Section[] sections, Eccentricity[] eccentricities, Connectivity[] connectivities, string identifier) : base(edge, BarType.Column, material, sections, eccentricities, connectivities, identifier)
		{
		}
	}
}
