// https://strusoft.com/

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Releases
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class Rotations: StiffnessType
    {
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Rotations()
        {

        }

        /// <summary>
        /// Private constructor [kNm/rad or kNm/m/rad].
        /// </summary>
        /// <param name="xNeg">Cx' compression. </param>
        /// <param name="xPos">Cx' tension.</param>
        /// <param name="yNeg">Cy' compression.</param>
        /// <param name="yPos">Cy' tension.</param>
        /// <param name="zNeg">Cz' compression.</param>
        /// <param name="zPos">Cz' tension.</param>
        private Rotations(double xNeg, double xPos, double yNeg, double yPos, double zNeg, double zPos)
        {
            this.XNeg = xNeg;
            this.XPos = xPos;
            this.YNeg = yNeg;
            this.YPos = yPos;
            this.ZNeg = zNeg;
            this.ZPos = zPos; 
        }
        
        /// <summary>
        /// Define a new rotations release [kNm/rad or kNm/m/rad].
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="xNeg">Cx' compression [kNm/rad or kNm/m/rad].</param>
        /// <param name="xPos">Cx' tension [kNm/rad or kNm/m/rad].</param>
        /// <param name="yNeg">Cy' compression [kNm/rad or kNm/m/rad].</param>
        /// <param name="yPos">Cy' tension [kNm/rad or kNm/m/rad].</param>
        /// <param name="zNeg">Cz' compression [kNm/rad or kNm/m/rad].</param>
        /// <param name="zPos">Cz' tension [kNm/rad or kNm/m/rad].</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Rotations Define(double xNeg = 0, double xPos = 0, double yNeg = 0, double yPos = 0, double zNeg = 0, double zPos = 0)
        {
            return new Rotations(xNeg, xPos, yNeg, yPos, zNeg, zPos);
        }
        /// <summary>
        /// Define a rigid rotations release for a point-type release (1e+10 kNm/rad).
        /// </summary>
        [IsVisibleInDynamoLibrary(true)]
        public static Rotations RigidPoint()
        {
            return new Rotations(10000000000, 10000000000, 10000000000, 10000000000, 10000000000, 10000000000);
        }
        /// <summary>
        /// Define a rigid rotations release for a line-type release (1e+07 kNm/m/rad).
        /// </summary>
        [IsVisibleInDynamoLibrary(true)]
        public static Rotations RigidLine()
        {
            return new Rotations(10000000, 10000000, 10000000, 10000000,  10000000, 10000000);
        }
        /// <summary>
        /// Define a free rotations release.
        /// </summary>
        [IsVisibleInDynamoLibrary(true)]
        public static Rotations Free()
        {
            return new Rotations(0,0,0,0,0,0);
        }   
    }
}