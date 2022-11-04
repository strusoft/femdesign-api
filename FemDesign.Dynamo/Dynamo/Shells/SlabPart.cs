
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Shells
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class SlabPart
    { 
        #region dynamo

        /// <summary>
        /// Get Dynamo Surface from SlabPart Contours (Region).
        /// </summary>
        internal Autodesk.DesignScript.Geometry.Surface GetDynamoSurface()
        {
            return this.Region.ToDynamoSurface();
        }

        /// <summary>
        /// Get Dynamo Curves from SlabPart Contours (Region). Nested list.
        /// </summary>
        internal List<List<Autodesk.DesignScript.Geometry.Curve>> GetDynamoCurvesNested()
        {
            return this.Region.ToDynamoCurves();
        }

        /// <summary>
        /// Get Dynamo Curves from SlabPart Contours (Region).
        /// </summary>
        internal List<Autodesk.DesignScript.Geometry.Curve> GetDynamoCurves()
        {
            var rtn = new List<Autodesk.DesignScript.Geometry.Curve>();
            foreach (List<Autodesk.DesignScript.Geometry.Curve> container in this.GetDynamoCurvesNested())
            {
                foreach (Autodesk.DesignScript.Geometry.Curve obj in container)
                {
                    rtn.Add(obj);
                }
            }
            return rtn;
        }
        
        #endregion
    }
}