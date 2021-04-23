// https://strusoft.com/

using System.Collections.Generic;

namespace FemDesign.Supports
{
    /// <summary>
    /// Object to contain support instances in GH when passing generic objects.
    /// </summary>
    [System.Serializable]
    public partial class GenericSupportObject
    {
        public PointSupport PointSupport { get; set; }
        public LineSupport LineSupport { get; set; }
        public SurfaceSupport SurfaceSupport { get; set; }
        public GenericSupportObject()
        {
            
        }

        public GenericSupportObject(PointSupport obj)
        {
            this.PointSupport = obj;
        }
        public GenericSupportObject(LineSupport obj)
        {
            this.LineSupport = obj;
        }
        public GenericSupportObject(SurfaceSupport obj)
        {
            this.SurfaceSupport = obj;
        }

        public static List<object> ToObjectList(List<GenericSupportObject> objs)
        {
            List<object> list = new List<object>();
            foreach (GenericSupportObject obj in objs)
            {
                if (obj.PointSupport != null)
                {
                    list.Add(obj.PointSupport);
                }
                else if (obj.LineSupport != null)
                {
                    list.Add(obj.LineSupport);
                }
                else if (obj.SurfaceSupport != null)
                {
                    list.Add(obj.SurfaceSupport);
                }
            }

            // return
            return list;
        }
        
    }
}