namespace FemDesign
{
    #region dynamo
    internal class DefaultArgument
    {
        /// <summary>
        /// Pass null as default argument for Zero-touch nodes.
        /// https://github.com/DynamoDS/Dynamo/issues/6564
        /// </summary>
        /// <returns></returns>
        internal static object GetNull()
        {
            return null;
        }
    }
    #endregion
}