using FemDesign.Geometry;
using StruSoft.Interop.StruXml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StruSoft.Interop.StruXml.Data
{
    public partial class Caseless_point_load_type
    {
        public static implicit operator Caseless_point_load_type(FemDesign.Loads.PointLoad obj)
        {
            var caseless = new Caseless_point_load_type();

            caseless.Action = Modification_type.Added;
            caseless.Last_change = obj.LastChange;
            caseless.Guid = obj.Guid.ToString();

            caseless.Direction = obj.Direction;

            var locationValue = new Location_value();
            locationValue.Val = obj.Load.Value;
            locationValue.X = obj.Load.X;
            locationValue.Y = obj.Load.Y;
            locationValue.Z = obj.Load.Z;

            caseless.Load = locationValue;

            caseless.Load_type = Force_load_type.Force;

            return caseless;
        }
    }

    public partial class Caseless_line_load_type
    {
        public static implicit operator Caseless_line_load_type(FemDesign.Loads.LineLoad obj)
        {
            var caseless = new Caseless_line_load_type();

            caseless.Action = Modification_type.Added;
            caseless.Last_change = obj.LastChange;
            caseless.Guid = obj.Guid.ToString();

            caseless.Direction = obj.Direction;
            caseless.Normal = obj.Normal;

            caseless.Edge = obj.Edge;
        
            caseless.Load = new List<Location_value> { obj.Load[0], obj.Load[1] };
            
            caseless.Load_dir = Load_dir_type.Constant;
            caseless.Load_projection = false;
            caseless.Load_type = Force_load_type.Force;

            return caseless;
        }
    }

    public partial class Caseless_surface_load_type
    {
        public static implicit operator Caseless_surface_load_type(FemDesign.Loads.SurfaceLoad obj)
        {
            var caseless = new Caseless_surface_load_type();

            caseless.Action = Modification_type.Added;
            caseless.Last_change = obj.LastChange;
            caseless.Guid = obj.Guid.ToString();

            caseless.Direction = obj.Direction;

            caseless.Load = new List<Location_value> { obj.Loads[0] };

            caseless.Load_projection = false;
            caseless.Load_type = Force_load_type.Force;

            caseless.Region = obj.Region;

            return caseless;
        }
    }


}
