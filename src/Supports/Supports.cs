// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Supports
{
    /// <summary>
    /// supports
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class Supports
    {
        [XmlElement("point_support", Order = 1)]
        public List<PointSupport> PointSupport = new List<PointSupport>(); // point_support_type
        [XmlElement("line_support", Order = 2)]
        public List<LineSupport> LineSupport = new List<LineSupport>(); // line_support_type
        [XmlElement("surface_support", Order = 3)] 
        public List<SurfaceSupport> SurfaceSupport = new List<SurfaceSupport>(); // surface_support

        internal List<object> ListSupports()
        {
            var objs = new List<object>();
            objs.AddRange(this.PointSupport);
            objs.AddRange(this.LineSupport);
            objs.AddRange(this.SurfaceSupport);
            return objs;
        }

        #region grasshopper
        internal List<GenericSupportObject> GetGenericSupportObjectsForPointSupport()
        {
            var objs = new List<GenericSupportObject>();
            foreach (PointSupport obj in this.PointSupport)
            {
                objs.Add(new GenericSupportObject(obj));
            }
            return objs;
        }

        internal List<GenericSupportObject> GetGenericSupportObjectsForLineSupport()
        {
            var objs = new List<GenericSupportObject>();
            foreach (LineSupport obj in this.LineSupport)
            {
                objs.Add(new GenericSupportObject(obj));
            }
            return objs;
        }

        internal List<GenericSupportObject> GetGenericSupportObjectsForSurfaceSupport()
        {
            var objs = new List<GenericSupportObject>();
            foreach (SurfaceSupport obj in this.SurfaceSupport)
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
            foreach (GenericSupportObject obj in this.GetGenericSupportObjectsForSurfaceSupport())
            {
                list.Add(obj);
            }
            return list;
        }
        #endregion
    }
}