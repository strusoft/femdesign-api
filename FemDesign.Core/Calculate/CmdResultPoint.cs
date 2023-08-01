using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using FemDesign;
using FemDesign.AuxiliaryResults;
using FemDesign.Bars.Buckling;
using FemDesign.Geometry;
using static FemDesign.AuxiliaryResults.ResultPoint;

namespace FemDesign.Calculate
{
    [XmlRoot("cmdrespoint")]
    [System.Serializable]
    public class CmdResultPoint : CmdCommand
    {
        [XmlAttribute("command")]
        public string Command = "$ MODULECOM RESPOINT";

        [XmlIgnore]
        public FemDesign.Geometry.Point3d _point;

        [XmlIgnore]
        public FemDesign.Geometry.Point3d Point
        {
            get
            {
                return _point;
            }
            set
            {
                _point = value;

                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        [XmlAttribute("x")]
        public double X
        {
            get
            {
                return Point.X;
            }
            set { Point.X = value; }
        }

        [XmlAttribute("y")]
        public double Y
        {
            get
            {
                return Point.Y;
            }
            set { Point.Y = value; }
        }

        [XmlAttribute("z")]
        public double Z
        {
            get
            {
                return Point.Z;
            }
            set { Point.Z = value; }
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("guid")]
        public Guid BaseEntity { get; set; }

        private CmdResultPoint()
        {

        }

        public CmdResultPoint(Point3d pos, FemDesign.GenericClasses.IStructureElement element, string name)
        {
            Point = pos;

            Guid refGuid;
            if (element is Shells.Slab slab)
            {
                refGuid = new Guid(slab.SlabPart.Guid.ToString());
            }
            else if (element is Bars.Bar bar)
            {
                refGuid = new Guid(bar.BarPart.Guid.ToString());
            }
            else
            {
                throw new NotImplementedException($"result point at {element.GetType()} has not been implemented");
            }

            BaseEntity = refGuid;
            Name = name;
        }

        public CmdResultPoint(Point3d pos, Guid refGuid, string name)
        {
            Point = pos;
            BaseEntity = refGuid;
            Name = name;
        }

        public static implicit operator CmdResultPoint(FemDesign.AuxiliaryResults.ResultPoint obj)
        {
            var pos = obj.Position;
            var refGuid = obj.BaseEntity;
            var name = obj.Name;
            var cmdResPoint = new CmdResultPoint(pos, refGuid, name);
            return cmdResPoint;
        }

        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdResultPoint>(this);
        }
    }
}