// https://strusoft.com/

using System.Collections.Generic;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Supports
{
    /// <summary>
    /// Object to contain support instances in GH when passing generic objects.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class GenericSupportObject
    {
        public PointSupport pointSupport { get; set; }
        public LineSupport lineSupport { get; set; }
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
            }

            // return
            return list;
        }
        
    }
}