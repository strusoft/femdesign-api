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
                throw new System.ArgumentOutOfRangeException($"Value should be between {min} and {max}");
            }
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