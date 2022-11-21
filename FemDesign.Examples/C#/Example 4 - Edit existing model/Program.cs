using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;

namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main()
        {
            // EXAMPLE 4: EDITING AN EXISTING MODEL
            // In this example, we will edit an existing model by isolating a floor and replacing supporting
            // walls and pillars with appropriate supports. Using height as a point of comparison we can find
            // which elements to reuse from the old model, and create a new model with our selected elements.

            // This example was last updated using the ver. 21.6.0 FEM-Design API.


            // READ THE MODEL:
            // Deserialize the current model to access all the data in the .struxml file.
            FemDesign.Model model = FemDesign.Model.DeserializeFromFilePath("Example 4 - model.struxml");


            // ISOLATE A FLOOR:
            // Choose which floor will be singled out.
            int floor = 2;

            FemDesign.StructureGrid.Storey storey = model.Entities.Storeys.Storey[floor];
            List<GenericClasses.IStructureElement> storeyAsList = new List<GenericClasses.IStructureElement> { storey };
            double zCoord = storey.Origo.Z;


            // POINT SUPPORTS:
            // Find all columns supporting the chosen floor, and place point supports in their place.
            // We can use the fact that point [1] of any pillar is always the highest one.
            var supports = new List<GenericClasses.ISupportElement>();
            for (int i = 0; i < model.Entities.Bars.Count; i++)
            {
                Bars.Bar tempBar = model.Entities.Bars[i];
                if (tempBar.BarPart.Type != Bars.BarType.Column)
                {
                    continue;
                }

                if (Math.Abs(tempBar.BarPart.Edge.Points[1].Z - zCoord) < Tolerance.LengthComparison)
                {
                    var tempSupport = new Supports.PointSupport(
                        point: new Geometry.Point3d(tempBar.BarPart.Edge.Points[1].X, tempBar.BarPart.Edge.Points[1].Y, zCoord),
                        motions: Releases.Motions.RigidPoint(),
                        rotations: Releases.Rotations.Free()
                        );
                    supports.Add(tempSupport);
                }
            }


            // ELEMENTS:
            // The model only contains plates and walls, so we will not be looking for beams etc.
            // We are looking for the floor plate at the correct height, and the walls below it to
            // replace them with line supports.
            var elements = new List<GenericClasses.IStructureElement>();

            // TESTING SLABS:
            // Slabs have a property which indicates if they are floors (plate) or walls (wall).
            // Based on this, we can sort out if we want to use them as an element or place a
            // line support in their stead.
            for (int i = 0; i < model.Entities.Slabs.Count; i++)
            {
                Shells.Slab tempSlab = model.Entities.Slabs[i];
                if (tempSlab.Type == Shells.SlabType.Plate && Math.Abs(tempSlab.SlabPart.LocalPos.Z - zCoord) < Tolerance.LengthComparison)
                {
                    elements.Add(tempSlab);
                }
                else if (tempSlab.Type == Shells.SlabType.Wall)
                {
                    if (Math.Abs(tempSlab.SlabPart.Region.Contours[0].Edges[2].Points[0].Z - zCoord) < Tolerance.LengthComparison)
                    {
                        // Creating supports with translational stiffnes in the Z direction only.
                        var tempSupport = new Supports.LineSupport(
                            edge: tempSlab.SlabPart.Region.Contours[0].Edges[2],
                            motions: new Releases.Motions(0, 0, 0, 0, 10E7, 10E7),
                            rotations: new Releases.Rotations(0, 0, 0, 0, 0, 0),
                            movingLocal: true
                            );
                        supports.Add(tempSupport);
                    }
                }
            }

            // TESTING PANELS:
            // Panels do not have the same property as slabs. Instead, we compare the height of all the
            // edge curves of the panel to discern if it is horizontal or not.
            for (int i = 0; i < model.Entities.Panels.Count; i++)
            {
                Shells.Panel tempPanel = model.Entities.Panels[i];
                bool isSlab = true;
                for (int j = 0; j < tempPanel.Region.Contours[0].Edges.Count; j++)
                {
                    if (tempPanel.Region.Contours[0].Edges[j].Points[0].Z != tempPanel.Region.Contours[0].Edges[j].Points[1].Z)
                    {
                        isSlab = false;
                        break;
                    }
                }
                if (isSlab && Math.Abs(tempPanel.Region.Contours[0].Edges[0].Points[0].Z - zCoord) < Tolerance.LengthComparison)
                {
                    elements.Add(tempPanel);
                }
                else if (!isSlab && Math.Abs(tempPanel.Region.Contours[0].Edges[2].Points[0].Z - zCoord) < Tolerance.LengthComparison)
                {
                    // Creating supports with translational stiffnes in the Z direction only.
                    var tempSupport = new Supports.LineSupport(
                            edge: tempPanel.Region.Contours[0].Edges[2],
                            motions: new Releases.Motions(0, 0, 0, 0, 10E7, 10E7),
                            rotations: new Releases.Rotations(0, 0, 0, 0, 0, 0),
                            movingLocal: true
                            );
                    supports.Add(tempSupport);
                }
            }


            // LOADS:
            // Similar to supports and elements, we will reuse loads from the model if they are on the correct height
            var loads = new List<GenericClasses.ILoadElement>();
            for (int i = 0; i < model.Entities.Loads.LineLoads.Count; i++)
            {
                if (Math.Abs(model.Entities.Loads.LineLoads[i].Edge.XAxis.Z - zCoord) < Tolerance.LengthComparison)
                {
                    loads.Add(model.Entities.Loads.LineLoads[i]);
                }
            }
            for (int i = 0; i < model.Entities.Loads.SurfaceLoads.Count; i++)
            {
                Loads.SurfaceLoad tempLoad = model.Entities.Loads.SurfaceLoads[i];
                if (Math.Abs(tempLoad.Region.Contours[0].Edges[0].Points[0].Z - zCoord) < Tolerance.LengthComparison)
                {
                    loads.Add(tempLoad);
                }
            }


            // CREATE NEW MODEL:
            // With a new model, we can add all our gathered elements to it. We can also take load cases,
            // load combinations, and the storey marker directly from the old model.
            FemDesign.Model newModel = new FemDesign.Model(Country.S);
            newModel.AddElements(elements);
            newModel.AddSupports(supports);
            newModel.AddLoads(loads);
            newModel.AddLoadCases(model.Entities.Loads.LoadCases);
            newModel.AddLoadCombinations(model.Entities.Loads.LoadCombinations);
            newModel.AddElements(storeyAsList);

            
            // OPEN MODEL IN FEM-DESIGN:
            using (var femDesign = new FemDesignConnection(keepOpen: true))
            {
                femDesign.Open(newModel);
            }
        }
    }
}
