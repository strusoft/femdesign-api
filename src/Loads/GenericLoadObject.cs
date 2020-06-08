// https://strusoft.com/
using System.Collections.Generic;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Loads
{
    /// <summary>
    /// Object to contain load instances in GH when passing generic objects.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class GenericLoadObject
    {
        public PointLoad pointLoad { get; set; }
        public LineLoad lineLoad { get; set; }
        public SurfaceLoad surfaceLoad { get; set; }
        public PressureLoad pressureLoad { get; set; }
        internal GenericLoadObject()
        {
            
        }
        internal GenericLoadObject(PointLoad obj)
        {
            this.pointLoad = obj;
        }
        internal GenericLoadObject(LineLoad obj)
        {
            this.lineLoad = obj;
        }
        internal GenericLoadObject(SurfaceLoad obj)
        {
            this.surfaceLoad = obj;
        }
        internal GenericLoadObject(PressureLoad obj)
        {
            this.pressureLoad = obj;
        }
        public static List<object> ToObjectList(List<GenericLoadObject> objs)
        {
            List<object> list = new List<object>();
            foreach (GenericLoadObject obj in objs)
            {
                if (obj.pointLoad != null)
                {
                    list.Add(obj.pointLoad);
                }
                else if (obj.lineLoad != null)
                {
                    list.Add(obj.lineLoad);
                }
                else if (obj.surfaceLoad != null)
                {
                    list.Add(obj.surfaceLoad);
                }
                else if (obj.pressureLoad != null)
                {
                    list.Add(obj.pressureLoad);
                }
            }
            
            // return
            return list;
        }
    } 
}