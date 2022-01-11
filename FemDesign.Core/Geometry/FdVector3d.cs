// https://strusoft.com/

using System;
using System.Xml.Serialization;


namespace FemDesign.Geometry
{
    [System.Serializable]
    public partial class FdVector3d
    {
        [XmlAttribute("x")]
        public double X { get; set; }
        [XmlAttribute("y")]
        public double Y { get; set; }
        [XmlAttribute("z")]
        public double Z { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public FdVector3d()
        {
            
        }

        /// <summary>
        /// Construct FdVector3d by x, y and z components.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public FdVector3d(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Construct FdVector3d by start and end points.
        /// </summary>
        /// <param name="p0">Start point</param>
        /// <param name="p1">End point</param>
        public FdVector3d(FdPoint3d p0, FdPoint3d p1)
        {
            this.X = p1.X - p0.X;
            this.Y = p1.Y - p0.Y;
            this.Z = p1.Z - p0.Z;
        }

        /// <summary>
        /// Reverse vector
        /// </summary>
        public static FdVector3d operator -(FdVector3d v) => v.Reverse();
        
        /// <summary>
        /// Vector difference
        /// </summary>
        public static FdVector3d operator -(FdVector3d v1, FdVector3d v2) => new FdVector3d(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);

        /// <summary>
        /// Scale vector
        /// </summary>
        public static FdVector3d operator *(FdVector3d v, int n) => new FdVector3d(n * v.X, n * v.Y, n * v.Z);

        /// <summary>
        /// Scale vector
        /// </summary>
        public static FdVector3d operator *(int n, FdVector3d v) => new FdVector3d(n * v.X, n * v.Y, n * v.Z);

        /// <summary>
        /// Scale vector
        /// </summary>
        public static FdVector3d operator *(FdVector3d v, double n) => new FdVector3d(n * v.X, n * v.Y, n * v.Z);

        /// <summary>
        /// Scale vector
        /// </summary>
        public static FdVector3d operator *(double n, FdVector3d v) => new FdVector3d(n * v.X, n * v.Y, n * v.Z);

        /// <summary>
        /// Returns the unit x vector.
        /// </summary>
        /// <returns></returns>
        public static FdVector3d UnitX()
        {
            return new FdVector3d(1, 0, 0);
        }
        
        /// <summary>
        /// Returns the unit y vector.
        /// </summary>
        /// <returns></returns>
        public static FdVector3d UnitY()
        {
            return new FdVector3d(0, 1, 0);
        }
        
        /// <summary>
        /// Return the unit z vector.
        /// </summary>
        /// <returns></returns>
        public static FdVector3d UnitZ()
        {
            return new FdVector3d(0, 0, 1);
        }

        /// <summary>
        /// Calculate length of FdVector3d.
        /// </summary>
        public double Length()
        {
            double len = Math.Pow((Math.Pow(this.X, 2) + Math.Pow(this.Y, 2) + Math.Pow(this.Z, 2)), (1.0/2.0));
            return len;
        }

        /// <summary>
        /// Normalize FdVector3d (i.e. scale so that length equals 1).
        /// </summary>
        public FdVector3d Normalize()
        {
            double l = this.Length();
            FdVector3d normal = new FdVector3d(this.X / l, this.Y / l, this.Z / l);
            return normal;
        }

        /// <summary>
        /// Calculate cross-product of this FdVector3d and v FdVector3d.
        /// </summary>
        public FdVector3d Cross(FdVector3d v)
        {
            FdVector3d v0 = this;
            FdVector3d v1 = v;

            double i, j, k;
            // https://en.wikipedia.org/wiki/Cross_product#Coordinate_notation
            i = v0.Y * v1.Z - v0.Z * v1.Y;
            j = v0.Z * v1.X - v0.X * v1.Z;
            k = v0.X * v1.Y - v0.Y * v1.X;

            FdVector3d v2 = new FdVector3d(i, j, k);;
            return v2;
        }

        /// <summary>
        /// Calculate dot-product of this FdVector3d and v FdVector3d.
        /// </summary>
        public double Dot(FdVector3d v)
        {
            FdVector3d v0 = this;
            FdVector3d v1 = v;

            // https://en.wikipedia.org/wiki/Dot_product#Algebraic_definition
            double s = v0.X*v1.X + v0.Y*v1.Y + v0.Z*v1.Z;
            return s;

        }

        /// <summary>
        /// Scale this FdVector3d by s.
        /// </summary>
        public FdVector3d Scale(double s)
        {   
            return new FdVector3d(this.X * s, this.Y * s, this.Z * s);
        }

        /// <summary>
        /// Reverse this by negative scaling.
        /// </summary>
        /// <returns></returns>
        public FdVector3d Reverse()
        {
            return this.Scale(-1);
        }

        /// <summary>
        /// Rotate this vector by an angle around an axis
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public FdVector3d RotateAroundAxis(double angle, FdVector3d axis)
        {
            // normalize vector
            FdVector3d _axis = axis.Normalize();

            // https://en.wikipedia.org/wiki/Rotation_matrix#Rotation_matrix_from_axis_and_angle
            double[,] rotationMatrix = new double[3,3];
            rotationMatrix[0,0] = Math.Cos(angle) + Math.Pow(_axis.X, 2)*(1 - Math.Cos(angle));
            rotationMatrix[0,1] = _axis.X*_axis.Y*(1 - Math.Cos(angle)) - _axis.Z*Math.Sin(angle);
            rotationMatrix[0,2] = _axis.X*_axis.Z*(1 - Math.Cos(angle)) + _axis.Y*Math.Sin(angle);
            rotationMatrix[1,0] = _axis.Y*_axis.X*(1 - Math.Cos(angle)) + _axis.Z*Math.Sin(angle);
            rotationMatrix[1,1] = Math.Cos(angle) + Math.Pow(_axis.Y, 2)*(1 - Math.Cos(angle));
            rotationMatrix[1,2] = _axis.Y*_axis.Z*(1 - Math.Cos(angle)) - _axis.X*Math.Sin(angle);
            rotationMatrix[2,0] = _axis.Z*_axis.X*(1 - Math.Cos(angle)) - _axis.Y*Math.Sin(angle);
            rotationMatrix[2,1] = _axis.Z*_axis.Y*(1 - Math.Cos(angle)) + _axis.X*Math.Sin(angle);
            rotationMatrix[2,2] = Math.Cos(angle) + Math.Pow(_axis.Z, 2)*(1 - Math.Cos(angle));

            // matrix multiplication
            double x = this.X*rotationMatrix[0,0] + this.Y*rotationMatrix[0,1] + this.Z*rotationMatrix[0,2];
            double y = this.X*rotationMatrix[1,0] + this.Y*rotationMatrix[1,1] + this.Z*rotationMatrix[1,2];
            double z = this.X*rotationMatrix[2,0] + this.Y*rotationMatrix[2,1] + this.Z*rotationMatrix[2,2];

            // return new vector
            return new FdVector3d(x, y, z);
        }

        /// <summary>
        /// Check if this FdVector3d is parallel to v.
        /// Returns 1 if parallel, -1 if antiparallel, 0 if not parallel
        /// </summary>
        public int Parallel(FdVector3d v)
        {
            FdVector3d v0 = this.Normalize();
            FdVector3d v1 = v.Normalize();
            if (v0.Equals(v1, Tolerance.Point3d))
            {
                return 1;
            }
            else if (v0.Scale(-1).Equals(v, Tolerance.Point3d))
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Check if z-component is 0 and convert to 2d vector in XY-plane.
        /// </summary>
        /// <returns></returns>
        public FdVector2d To2d()
        {
            if (this.Z == 0)
            {
                return new FdVector2d(this.X, this.Y);
            }
            else
            {
                throw new System.ArgumentException("Z-component of Vector is not zero. Vector is not in XY plane.");
            }

        }

        /// <summary>
        /// Check if zero vector.
        /// </summary>
        /// <returns></returns>
        public bool IsZero()
        {
            if (this.Length() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            FdVector3d v = obj as FdVector3d;
            if ((System.Object)v == null)
            {
                return false;
            }
            return (X == v.X) && (Y == v.Y) && (Z == v.Z);            
        }

        public bool Equals(FdVector3d v)
        {
            if ((object)v == null)
            {
                return false;
            }
            return (X == v.X) && (Y == v.Y) && (Z == v.Z);
        }

        public bool Equals(FdVector3d v, double tol)
        {
            if ((object)v == null)
            {
                return false;
            }
            return (Math.Abs(X - v.X) < tol) && (Math.Abs(Y - v.Y) < tol) && (Math.Abs(Z - v.Z) < tol);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }
    }
}