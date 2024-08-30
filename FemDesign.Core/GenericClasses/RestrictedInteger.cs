namespace FemDesign
{
    /// <summary>
    /// Contains integer restrictions used for property definitions.
    /// 
    /// Restrictions are defined by strusoft.xsd
    /// </summary>
    internal class RestrictedInteger
    {
        /// <summary>
        /// Check if val in range (min, max)
        /// </summary>
        internal static int ValueInRange(int val, int min, int max)
        {
            if (val >= min && val <= max)
            {
                return val;
            }
            else
            {
                throw new System.ArgumentOutOfRangeException($"Value should be between or equal to {min} and {max}");
            }
        }

        /// <summary>
        /// Check if val in range [1, 250]
        /// </summary>
        internal static int DefaultBarElemDiv(int val)
        {
            return RestrictedInteger.ValueInRange(val, 1, 250);
        }

        /// <summary>
        /// Check if val in range [1, 50]
        /// </summary>
        internal static int MeshSmoothSteps(int val)
        {
            return RestrictedInteger.ValueInRange(val, 1, 50);
        }

        /// <summary>
        /// timber_service_class
        /// </summary>
        internal static int TimberServiceClass(int val)
        {
            return RestrictedInteger.ValueInRange(val, 0, 2);
        }
    }
}