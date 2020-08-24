// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Supports
{
    /// <summary>
    /// supports
    /// </summary>
    [System.Serializable]
    public class Supports
    {
        [XmlElement("point_support", Order = 1)]
        public List<PointSupport> pointSupport = new List<PointSupport>(); // point_support_type
        [XmlElement("line_support", Order = 2)]
        public List<LineSupport> lineSupport = new List<LineSupport>(); // line_support_type
        [XmlElement("surface_support", Order = 3)] 
        public List<SurfaceSupport> surfaceSupport = new List<SurfaceSupport>(); // surface_support

        internal List<object> ListSupports()
        {
            var objs = new List<object>();
            objs.AddRange(this.pointSupport);
            objs.AddRange(this.lineSupport);
            objs.AddRange(this.surfaceSupport);
            return objs;
        }

        #region grasshopper
        internal List<GenericSupportObject> GetGenericSupportObjectsForPointSupport()
        {
            var objs = new List<GenericSupportObject>();
            foreach (PointSupport obj in this.pointSupport)
            {
                objs.Add(new GenericSupportObject(obj));
            }
            return objs;
        }

        internal List<GenericSupportObject> GetGenericSupportObjectsForLineSupport()
        {
            var objs = new List<GenericSupportObject>();
            foreach (LineSupport obj in this.lineSupport)
            {
                objs.Add(new GenericSupportObject(obj));
            }
            return objs;
        }
        
        internal List<GenericSupportObject> GetGenericSupportObjects()
        {
            var list = new List<GenericSupportObject>();
            foreach (GenericSupportObject obj in this.GetGenericSupportObjectsForPointSupport())
            {
                list.Add(obj);
            }
            foreach (GenericSupportObject obj in this.GetGenericSupportObjectsForLineSupport())
            {
                list.Add(obj);
            }
            return list;
        }
        #endregion
    }
}