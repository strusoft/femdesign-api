using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using FemDesign;
using FemDesign.GenericClasses;
using FemDesign.Releases;

namespace FemDesign.Supports
{
	public partial class StiffnessPoint : EntityBase, IStructureElement, ISupportElement
	{
		[XmlIgnore]
		public FemDesign.Geometry.Point3d Point { get; set; }

		[XmlAttribute("x")]
		public double X;

		[XmlAttribute("y")]
		public double Y;

		[XmlAttribute("z")]
		public double Z;

		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlIgnore]
		public FemDesign.Supports.SurfaceSupport Surface { get; set; }

		[XmlAttribute("surface_support")]
		public Guid SurfaceSupport;

		[XmlElement("rigidity")]
		public RigidityDataType0 Rigidity { get; set; }
		public Motions Motions { get { return Rigidity?.Motions; } }
		public MotionsPlasticLimits MotionsPlasticityLimits { get { return Rigidity?.PlasticLimitForces; } }

		public StiffnessPoint()
		{
		}

		private void Initialise(FemDesign.Supports.SurfaceSupport surface, FemDesign.Geometry.Point3d point)
		{
			this.SurfaceSupport = surface.Guid;
			this.X = point.X;
			this.Y = point.Y;
			this.Z = point.Z;
			this.EntityCreated();
		}

		public StiffnessPoint(FemDesign.Supports.SurfaceSupport surface, FemDesign.Geometry.Point3d point, Motions motions, MotionsPlasticLimits MotionsPlasticityLimits = null, string name = null)
		{
			this.Initialise(surface, point);
			this.Rigidity = new RigidityDataType0(motions, MotionsPlasticityLimits);
			this.Name = name;
		}
	}
}