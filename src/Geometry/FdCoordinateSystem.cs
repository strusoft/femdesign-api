// https://strusoft.com/

namespace FemDesign.Geometry
{
    [System.Serializable]
    public class FdCoordinateSystem
    {
        public FdPoint3d origin { get; set; }
        public FdVector3d localX { get; set; }
        public FdVector3d localY { get; set; }
        public FdVector3d localZ { get; set; }

        /// <summary>
        /// Construct FdCoordinateSystem from origin point and local x, y, z axes.
        /// </summary>
        public FdCoordinateSystem(FdPoint3d _origin, FdVector3d _localX, FdVector3d _localY, FdVector3d _localZ)
        {
            this.origin = _origin;
            this.localX = _localX;
            this.localY = _localY;
            this.localZ = _localZ;
        }


        #region grasshopper

        /// <summary>
        /// Create FdCoordinateSystem from Rhino plane.
        /// </summary>
        internal static FdCoordinateSystem FromRhinoPlane(Rhino.Geometry.Plane obj)
        {
            FdPoint3d origin = FdPoint3d.FromRhino(obj.Origin);
            FdVector3d localX = FdVector3d.FromRhino(obj.XAxis);
            FdVector3d localY = FdVector3d.FromRhino(obj.YAxis);
            FdVector3d localZ = FdVector3d.FromRhino(obj.ZAxis);
            return new FdCoordinateSystem(origin, localX, localY, localZ);
        }

        /// <summary>
        /// Create FdCoordinateSystem from Rhino plane on curve mid u-point.
        /// </summary>
        internal static FdCoordinateSystem FromRhinoCurve(Rhino.Geometry.Curve obj)
        {
            // reparameterize if neccessary
            if (obj.Domain.T0 == 0 && obj.Domain.T1 == 1)
            {
                // pass
            }
            else
            {
                obj.Domain = new Rhino.Geometry.Interval(0, 1);
            }

            // get frame @ mid-point
            Rhino.Geometry.Plane plane;
            obj.FrameAt(0.5, out plane);
            return FdCoordinateSystem.FromRhinoPlane(plane);
        }

        /// <summary>
        /// Create FdCoordinateSystem from Rhino plane on surface mid u/v-point.
        /// </summary>
        internal static FdCoordinateSystem FromRhinoSurface(Rhino.Geometry.Surface obj)
        {
            // reparameterize if necessary
            if (obj.Domain(0).T0 == 0 && obj.Domain(1).T0 == 0 && obj.Domain(0).T1 == 1 && obj.Domain(1).T1 == 1)
            {
                // pass
            }
            else
            {
                obj.SetDomain(0, new Rhino.Geometry.Interval(0, 1));
                obj.SetDomain(1, new Rhino.Geometry.Interval(0, 1));
            }

            Rhino.Geometry.Plane plane;
            obj.FrameAt(0.5, 0.5, out plane);
            return FdCoordinateSystem.FromRhinoPlane(plane);
        }
        #endregion
    }
}