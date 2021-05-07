using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Bars
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Connectivity
    {
        

        // [IsVisibleInDynamoLibrary(true)]

        #region dynamo
        /// <summary>
        /// Define releases for a bar-element.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="mx">Release stiffness. Translation local-x axis. Bool or number: true if rigid, false or 0 if free, positive number if semi-rigid with stiffness [kN/m]. Optional, default value is fully rigid.</param>
        /// <param name="my">Release stiffness. Translation local-y axis. Bool or number: true if rigid, false or 0 if free, positive number if semi-rigid with stiffness [kN/m]. Optional, default value is fully rigid.</param>
        /// <param name="mz">Release stiffness. Translation local-z axis. Bool or number: true if rigid, false or 0 if free, positive number if semi-rigid with stiffness [kN/m]. Optional, default value is fully rigid.</param>
        /// <param name="rx">Release stiffness. Rotation around local-x axis. Bool or number: true if rigid, false or 0 if free, positive number if semi-rigid with stiffness [kNm/rad]. Optional, default value is fully rigid.</param>
        /// <param name="ry">Release stiffness. Rotation around local-y axis. Bool or number: true if rigid, false or 0 if free, positive number if semi-rigid with stiffness [kNm/rad]. Optional, default value is fully rigid.</param>
        /// <param name="rz">Release stiffness. Rotation around local-z axis. Bool or number: true if rigid, false or 0 if free, positive number if semi-rigid with stiffness [kNm/rad]. Optional, default value is fully rigid.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Connectivity SemiRigid([DefaultArgument("true")] System.Object mx, [DefaultArgument("true")] System.Object my, [DefaultArgument("true")] System.Object mz, [DefaultArgument("true")] System.Object rx, [DefaultArgument("true")] System.Object ry, [DefaultArgument("true")] System.Object rz)
        {
            System.Object[] items = new System.Object[]
            {
                mx,
                my,
                mz,
                rx,
                ry,
                rz
            };

            bool[] releases = new bool[6];
            double[] stiffnesses = new double[6];

            for (int idx = 0; idx < items.Length; idx++)
            {

                if (items[idx].GetType() == typeof(bool))
                {
                    releases[idx] = (bool)items[idx];
                    stiffnesses[idx] = 0;
                }
                else if (items[idx].GetType() == typeof(System.Int64))
                {
                    releases[idx] = false;
                    stiffnesses[idx] = (System.Int64)items[idx];
                }
                else if (items[idx].GetType() == typeof(System.Double))
                {
                    releases[idx] = false;
                    stiffnesses[idx] = (double)items[idx];
                }
                else
                {
                    throw new System.ArgumentException($"{items[idx].GetType()} is not bool, {typeof(System.Int64)} or {typeof(System.Double)}.");
                }
            }

            return new Connectivity(releases[0], releases[1], releases[2], releases[3], releases[4], releases[5], stiffnesses[0], stiffnesses[1], stiffnesses[2], stiffnesses[3], stiffnesses[4], stiffnesses[5]);
        }
        #endregion

        // [IsVisibleInDynamoLibrary(false)]
        // {
        // }

        // [IsVisibleInDynamoLibrary(true)]
        // {
        // }
        // [IsVisibleInDynamoLibrary(true)]
        // {
        // }
    }
}