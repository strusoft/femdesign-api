// https://strusoft.com/

using System.Collections.Generic;

namespace FemDesign.Supports
{
    /// <summary>
    /// Object to contain support instances in GH when passing generic objects.
    /// </summary>
    [System.Serializable]
    public class GenericSupportObject
    {
        public PointSupport pointSupport { get; set; }
        public LineSupport lineSupport { get; set; }
        public SurfaceSupport surfaceSupport { get; set; }
        internal GenericSupportObject()
        {
            
        }

        public static List<object> ToObjectList(List<GenericSupportObject> objs)
        {
            List<object> list = new List<object>();
            foreach (GenericSupportObject obj in objs)
            {
                if (obj.pointSupport != null)
                {
                    list.Add(obj.pointSupport);
                }
                else if (obj.lineSupport != null)
                {
                    list.Add(obj.lineSupport);
                }
                else if (obj.surfaceSupport != null)
                {
                    list.Add(obj.surfaceSupport);
                }
            }

            // return
            return list;
        }
        
        #region grasshopper
        internal GenericSupportObject(PointSupport obj)
        {
            this.pointSupport = obj;
        }
        internal GenericSupportObject(LineSupport obj)
        {
            this.lineSupport = obj;
        }
        internal GenericSupportObject(SurfaceSupport obj)
        {
            this.surfaceSupport = obj;
        }
        #endregion
    }
}