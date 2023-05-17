using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Results
{
    public partial class InteractionSurface
    {
        public Dictionary<int, FemDesign.Geometry.Point3d> Vertices { get; set; }
        public Dictionary<int, FemDesign.Geometry.Face> Faces { get; set; }
        
        public InteractionSurface(Dictionary<int, FemDesign.Geometry.Point3d> vertices, Dictionary<int, FemDesign.Geometry.Face> faces)
        {
            Vertices = vertices;
            Faces = faces;
        }

        internal static InteractionSurface ReadFromFile(string filepath)
        {
            var vertices = new Dictionary<int, Geometry.Point3d>();
            var faces = new Dictionary<int, Geometry.Face>();

            bool isFace = false;

            using (System.IO.StreamReader reader = new StreamReader(filepath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("#node") || line.StartsWith("id"))
                    {
                        continue;
                    }

                    if (line.StartsWith("#face"))
                    {
                        isFace = true;
                        continue;
                    }

                    if (line.StartsWith("t:"))
                    {
                        break;
                    }

                    string[] values = line.Split();
                    int key = int.Parse( values[0] ) - 1;

                    if (isFace)
                    {
                        int node_i = int.Parse( values[1] ) - 1;
                        int node_j = int.Parse( values[2] ) - 1;
                        int node_k = int.Parse( values[3] ) - 1;
                        faces[key] = new Geometry.Face(node_i, node_j, node_k);
                    }
                    else
                    {
                        var x = Double.Parse( values[1] );
                        var y = Double.Parse( values[2] );
                        var z = Double.Parse( values[3] );
                        vertices[key] = new Geometry.Point3d(x, y, z);
                    }
                }
            }

            return new InteractionSurface(vertices, faces);
        }
    }
}