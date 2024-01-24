using FemDesign;
using FemDesign.Loads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StruSoft.Interop.StruXml.Data
{
    public partial class Vehicle_lib_type : EntityBase
    {
        private Vehicle_lib_type()
        {
        }

        public Vehicle_lib_type(string name, List<StruSoft.Interop.StruXml.Data.Caseless_point_load_type> caselessPointLoads, List<StruSoft.Interop.StruXml.Data.Caseless_line_load_type> caselessLineLoads = null, List<StruSoft.Interop.StruXml.Data.Caseless_surface_load_type> caselessSurfaceLoads = null)
        {
            this.Name = name;
            this.Point_load = caselessPointLoads;
            this.Line_load = caselessLineLoads;
            this.Surface_load = caselessSurfaceLoads;
            this.EntityCreated();
        }

        public Vehicle_lib_type(string name, List<PointLoad> caselessPointLoads, List<LineLoad> caselessLineLoads = null, List<SurfaceLoad> caselessSurfaceLoads = null)
        {
            this.Name = name;
            this.Point_load = caselessPointLoads.ConvertAll(source => (Caseless_point_load_type)source);
            this.Line_load = caselessLineLoads.ConvertAll(source => (Caseless_line_load_type)source);
            this.Surface_load = caselessSurfaceLoads.ConvertAll(source => (Caseless_surface_load_type)source);
            this.EntityCreated();
        }

        public override string ToString()
        {
            return $"{this.Name}";
        }
    }
}