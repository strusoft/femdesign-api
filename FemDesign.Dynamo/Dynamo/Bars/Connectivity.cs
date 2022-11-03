using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Bars
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Connectivity
    {
        /// <summary>
        /// Define releases for a bar-element.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="mx">Translation local-x axis. True if rigid, false if free.</param>
        /// <param name="my">Translation local-y axis. True if rigid, false if free.</param>
        /// <param name="mz">Translation local-z axis. True if rigid, false if free.</param>
        /// <param name="rx">Rotation around local-x axis. True if rigid, false if free.</param>
        /// <param name="ry">Rotation around local-y axis. True if rigid, false if free.</param>
        /// <param name="rz">Rotation around local-z axis. True if rigid, false if free.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Connectivity Define(bool mx, bool my, bool mz, bool rx, bool ry, bool rz)
        {
            return new Connectivity(mx, my, mz, rx, ry, rz);
        }

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

        /// <summary>
        /// Define hinged releases for a bar-element.
        /// </summary>
        /// <remarks>Create</remarks>
        [IsVisibleInDynamoLibrary(true)]
        public static Connectivity GetHinged()
        {
            return Connectivity.Hinged;
        }
        /// <summary>
        /// Define rigid releases for a bar-element.
        /// </summary>
        /// <remarks>Create</remarks>
        [IsVisibleInDynamoLibrary(true)]
        public static Connectivity GetRigid()
        {
            return Connectivity.Rigid;
        }
    }
}