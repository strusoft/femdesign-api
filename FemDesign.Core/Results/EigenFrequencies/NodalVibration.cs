using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using FemDesign.GenericClasses;


namespace FemDesign.Results
{
    /// <summary>
    /// FemDesign "Nodal displacements" result
    /// </summary>
    public partial class NodalVibration : IResult
    {
        /// <summary>
        /// Support name identifier
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Finite element node id
        /// </summary>
        public int NodeId { get; }
        /// <summary>
        /// Displacement in global x
        /// </summary>
        public double Ex { get; }
        /// <summary>
        /// Displacement in global y
        /// </summary>
        public double Ey { get; }
        /// <summary>
        /// Displacement in global z
        /// </summary>
        public double Ez { get; }
        /// <summary>
        /// Rotation around global x
        /// </summary>
        public double Fix { get; }
        /// <summary>
        /// Rotation around global y
        /// </summary>
        public double Fiy { get; }
        /// <summary>
        /// Rotation around global z
        /// </summary>
        public double Fiz { get; }
        /// <summary>
        /// Load case or combination name
        /// </summary>
        public int ShapeId { get; }

        internal NodalVibration(string id, int nodeId, double ex, double ey, double ez, double fix, double fiy, double fiz, int shapeId)
        {
            this.Id = id;
            this.NodeId = nodeId;
            this.Ex = ex;
            this.Ey = ey;
            this.Ez = ez;
            this.Fix = fix;
            this.Fiy = fiy;
            this.Fiz = fiz;
            this.ShapeId = shapeId;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Mode Shape No. {ShapeId}. Node Id: {NodeId}, x: {Ex}, y: {Ey}, z: {Ez}";
        }

        //Nodal vibration shapes, 3
        internal static Regex IdentificationExpression
        {
            get
            {
                return new Regex(@"^(?'type'Nodal vibration) shapes,\s*(?'shapeid'\d*)");
            }
        }

        internal static Regex HeaderExpression
        {
            get
            {
                return new Regex(@"^(?'type'Nodal vibration) shapes,\s*(?'shapeid'\d*)|^ID.*|\[.*\]");
            }
        }

        internal static NodalVibration Parse(string[] row, CsvParser reader, Dictionary<string, string> HeaderData)
        {
            string identifier = row[0];
            int nodeId = Int32.Parse(row[1], CultureInfo.InvariantCulture);
            double ex = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double ey = Double.Parse(row[3], CultureInfo.InvariantCulture);
            double ez = Double.Parse(row[4], CultureInfo.InvariantCulture);
            double fix = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double fiy = Double.Parse(row[6], CultureInfo.InvariantCulture);
            double fiz = Double.Parse(row[7], CultureInfo.InvariantCulture);
            int shapeId = Int32.Parse(HeaderData["shapeid"]);
            return new NodalVibration(identifier, nodeId, ex, ey, ez, fix, fiy, fiz, shapeId);
        }
    }
}