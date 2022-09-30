// https://strusoft.com/
using System.Collections.Generic;


namespace FemDesign
{
    /// <summary>
    /// Contains string restrictions used for property definitions.
    /// 
    /// Restrictions are defined by strusoft.xsd
    /// </summary>
    public partial class RestrictedObject
    {
        /// <summary>
        /// Check if val is a non zero 3d vector.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        internal static Geometry.Vector3d NonZeroFdVector3d(Geometry.Vector3d val)
        {
            if (val.IsZero())
            {
                throw new System.ArgumentException("Vector must be non zero.");
            }
            else
            {
                return val;
            }
        }

        /// <summary>
        /// Check if val is a non zero 2d vector.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        internal static Geometry.Vector2d NonZeroFdVector2d(Geometry.Vector2d val)
        {
            if (val.IsZero())
            {
                throw new System.ArgumentException("Vector must be non zero.");
            }
            else
            {
                return val;
            }
        }
    }
}