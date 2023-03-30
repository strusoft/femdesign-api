using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FemDesign.Geometry
{
    public partial class Line3d
    {
        public Point3d Start { get; set; }
        public Point3d End { get; set; } 
        public Point3d Mid
        {
            get
            {
                return this.PointAtParameter(0.5);
            }
        }

        public Vector3d Tangent
        {
            get
            {
                return new Vector3d(this.Start, this.End).Normalize();
            }
        }

        public Vector3d Direction
        {
            get
            {
                return this.End - this.Start;
            }
        }

        public double Length
        {
            get
            {
                return (this.End - this.Start).Length();
            }
        }

        public Line3d Reversed
        {
            get
            {
                return new Line3d(this.End, this.Start);
            }
        }

        public Line3d(Point3d start, Point3d end)
        {
            this.Start = start;
            this.End = end;
        }


        public Line3d(Point3d start, Vector3d direction)
        {
            this.Start = start;
            this.End = start + direction;
        }

        public Point3d PointAtParameter(double parameter)
        {
            return this.Start.Translate(this.Tangent.Scale(parameter * this.Length));
        }

        public double ParameterByPoint(Point3d p, double tol)
        {
            if (p.Equals(this.Start, tol))
            {
                return 0.0;
            }
            else
            {
                var v = new Vector3d(this.Start, p);
                int par = this.Tangent.IsParallel(v);
                if (par == 0)
                {
                    throw new System.ArgumentException("Vector from start point of line to p is not parallell with line");
                }
                else
                {
                    return par * v.Length() / this.Length;
                }
            }
        }

        public bool IsLineFullyOverlapping(Line3d line, double tol)
        {
            int par = this.Tangent.IsParallel(line.Tangent);
            if (par == 0)
            {
                return false;
            }
            else if (par == -1)
            {
                return (this.ParameterByPoint(line.End, tol) <= 0 && this.ParameterByPoint(line.Start, tol) >= 1);
            }
            else if (par == 1)
            {
                return (this.ParameterByPoint(line.Start, tol) <= 0 && this.ParameterByPoint(line.End, tol) >= 1);
            }
            else
            {
                throw new System.ArgumentException($"Par: {par} should be -1, 0 or 1. Something went wrong with parallel evaluation.");
            }
        }

        public bool IsLinePartiallyOverlapping(Line3d line, double tol)
        {
            if (this.Tangent.IsParallel(line.Tangent) != 0)
            {
                return this.IsPointOnLine(line.Start, tol) || this.IsPointOnLine(line.End, tol);
            }
            else
            {
                return false;
            }
        }

        public bool IsLineOnLineAxis(Line3d otherLine, double tol)
        {
            return (this.IsPointOnLineAxis(otherLine.Start, tol) && (this.Tangent.IsParallel(otherLine.Tangent) != 0));
        }

        public bool IsPointOnLine(Point3d p, double tol)
        {
            if (this.IsPointOnLineAxis(p, tol))
            {
                double param = this.ParameterByPoint(p, tol);
                return (0 <= param && param <= 1);
            }
            else
            {
                return false;
            }
        }

        public bool IsPointOnLineAxis(Point3d p, double tol)
        {
            if (p.Equals(this.Start, tol))
            {
                return true;
            }
            else
            {
                var v = new Vector3d(this.Start, p);
                int par = this.Tangent.IsParallel(v);
                return (par == -1 || par == 1);
            }
        }
    }
}