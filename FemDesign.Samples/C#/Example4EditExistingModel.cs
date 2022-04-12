using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Samples
{
    public partial class SampleProgram
    {
        private static void Example4EditExistingModel()
        {
            // Step 1: Read the model
            FemDesign.Model model = FemDesign.Model.DeserializeFromFilePath(@"C:\Users\SamuelNyberg\Documents\GitHub\femdesign-api\FemDesign.Samples\C#\ExampleModels\Example 4 - model.struxml");


            int floor = 3;

            FemDesign.StructureGrid.Storey storey = model.Entities.Storeys.Storey[floor];
            double zCoord = storey.Origo.Z;

            int n;

            // POINTSUPPORTS: Find all pillars supporting the floor; place new point supports there
            var supports = new List<GenericClasses.ISupportElement>();
            for (int i = 0; i < model.Entities.Bars.Count; i++)
            {
                Bars.Bar tempBar = model.Entities.Bars[i];
                if (tempBar.BarPart.Type != Bars.BarType.Column)
                {
                    continue;
                }

                if (Math.Abs(tempBar.BarPart.Edge.Points[0].Z-zCoord) < Tolerance.LengthComparison)
                {
                    var tempSupport = new Supports.PointSupport(
                        point: new Geometry.FdPoint3d(tempBar.BarPart.Edge.Points[0].X, tempBar.BarPart.Edge.Points[0].Y, zCoord),
                        motions: Releases.Motions.RigidPoint(),
                        rotations: Releases.Rotations.Free()
                        );
                    supports.Add(tempSupport);
                }
            }
            

            // ELEMENTS: Pick out a certain floor. Also add supports if you find any.
            var elements = new List<GenericClasses.IStructureElement>();

            // Testing slabs
            for (int i = 0; i < model.Entities.Slabs.Count; i++)
            {
                Shells.Slab tempSlab = model.Entities.Slabs[i];
                if (tempSlab.Type == Shells.SlabType.Plate)
                {
                    elements.Add(tempSlab);
                }
                else if (tempSlab.Type == Shells.SlabType.Wall)
                {
                    if (tempSlab.SlabPart.Region.Contours[0].Edges[2].Points[0].Z == zCoord)        // Funkar för Walls, men ej för panels
                    {
                        var tempSupport = new Supports.LineSupport(
                            edge: tempSlab.SlabPart.Region.Contours[0].Edges[2],
                            motions: new Releases.Motions(0, 0, 0, 0, 10 ^ 7, 10 ^ 7),
                            rotations: new Releases.Rotations(0, 0, 0, 0, 0, 0),
                            movingLocal: true
                            );
                        supports.Add(tempSupport);
                    }
                }
            }

            // Testing panels
            for (int i = 0; i < model.Entities.Panels.Count; i++)
            {
                Shells.Panel tempPanel = model.Entities.Panels[i];
                n = 0;
                for (int j = 0; j < tempPanel.Region.Contours[0].Edges.Count; j++)
                {
                    if (tempPanel.Region.Contours[0].Edges[j].Points[0].Z != tempPanel.Region.Contours[0].Edges[j].Points[1].Z)
                    {
                        n++;
                        return;
                    }
                }
                if (n == 0 && tempPanel.Region.Contours[0].Edges[0].Points[0].Z == zCoord)
                {
                    elements.Add(tempPanel);
                }
                else if (n != 0 && tempPanel.Region.Contours[0].Edges[2].Points[0].Z == zCoord)
                {
                    var tempSupport = new Supports.LineSupport(
                            edge: tempPanel.Region.Contours[0].Edges[2],
                            motions: new Releases.Motions(0, 0, 0, 0, 10 ^ 7, 10 ^ 7),
                            rotations: new Releases.Rotations(0, 0, 0, 0, 0, 0),
                            movingLocal: true
                            );
                    supports.Add(tempSupport);
                }
            }

            // LOADS
            var loads = new List<GenericClasses.ILoadElement>();
            for (int i = 0; i < model.Entities.Loads.LineLoads.Count; i++)
            {
                if (model.Entities.Loads.LineLoads[i].Edge.XAxis.Z == zCoord)
                {
                    loads.Add(model.Entities.Loads.LineLoads[i]);
                }
            }
            for (int i = 0; i < model.Entities.Loads.SurfaceLoads.Count; i++)                                   
                // ATT GÖRA: Den här måste sålla ut alla irrelevanta laster på något sätt
                //       .ex på höjd? skulle nog kunna gå att bygga om modellen från tempPanel
            {
                Loads.SurfaceLoad tempLoad = model.Entities.Loads.SurfaceLoads[i];
                if (tempLoad.Region.Contours[i].Edges[0].Points[0].Z == zCoord)
                {
                    loads.Add(tempLoad);
                }
            }

            // Create a new model and add all constructed elements etc. to it
            FemDesign.Model newModel = new FemDesign.Model(Country.S);
            newModel.AddElements(elements);
            newModel.AddSupports(supports);
            newModel.AddLoadCases(model.Entities.Loads.LoadCases);
            newModel.AddLoadCombinations(model.Entities.Loads.LoadCombinations);
            //newModel.AddLoads(lineLoads, surfaceLoads);

            string path = @"C:\Users\SamuelNyberg\OneDrive - StruSoft AB\Samuels arbetshörna\14. C#-exempel\edited_model.struxml";
            newModel.SerializeModel(path);

            // var app = new Calculate.Application();
            // app.OpenStruxml(path, true);

            Console.ReadKey();
        }
    }
}
