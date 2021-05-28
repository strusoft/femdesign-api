using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign.GenericClasses;

namespace FemDesign.Results
{
    /// <summary>
    /// FemDesign "Point support group, Reactions" result
    /// </summary>
    public class PointSupportReaction : IResult
    {
        /// <summary>
        /// Support name identifier
        /// </summary>
        public readonly string Id;
        /// <summary>
        /// X-coordinate
        /// </summary>
        public readonly double X;
        /// <summary>
        /// Y-coordinate
        /// </summary>
        public readonly double Y;
        /// <summary>
        /// Z-coordinate
        /// </summary>
        public readonly double Z;
        /// <summary>
        /// Finite element node id
        /// </summary>
        public readonly int NodeId;
        /// <summary>
        /// Local Fx'
        /// </summary>
        public readonly double Fx;
        /// <summary>
        /// Local Fy'
        /// </summary>
        public readonly double Fy;
        /// <summary>
        /// Local Fz'
        /// </summary>
        public readonly double Fz;
        /// <summary>
        /// Local Mx'
        /// </summary>
        public readonly double Mx;
        /// <summary>
        /// Local My'
        /// </summary>
        public readonly double My;
        /// <summary>
        /// Local Mz'
        /// </summary>
        public readonly double Mz;
        /// <summary>
        /// Force resultant
        /// </summary>
        public readonly double Fr;
        /// <summary>
        /// Moment resultant
        /// </summary>
        public readonly double Mr;
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public readonly string CaseIdentifier;

        internal PointSupportReaction(string id, double x, double y, double z, int nodeId, double fx, double fy, double fz, double mx, double my, double mz, double fr, double mr, string resultCase)
        {
            Id = id;
            X = x;
            Y = y;
            Z = z;
            NodeId = nodeId;
            Fx = fx;
            Fy = fy;
            Fz = fz;
            My = mx;
            My = my;
            Mz = mz;
            Fr = fr;
            Mr = mr;
            CaseIdentifier = resultCase;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {Id}, {CaseIdentifier}";
        }
    }
}
