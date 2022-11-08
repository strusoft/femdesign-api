using FemDesign.Examples;
using FemDesign;
using Newtonsoft.Json;

// Convert the csv file data to a Point3d object
var jsonObject = DataParser.ConvertCsvFileToJsonObject("PointLocation.txt");
var points = JsonConvert.DeserializeObject<FemDesign.Geometry.Point3d[]>(jsonObject);
if (points is null || points.Length < 1)
    throw new Exception("Empty json file.");

// Empty List to contain all the Structural Elements
var struElements = new List<FemDesign.GenericClasses.IStructureElement>();

// Define Stiffness
var motionsSurface = new FemDesign.Releases.Motions(10000, 10000, 10000, 10000, 10000, 10000);
var motionsPoint = new FemDesign.Releases.Motions(10, 10, 10, 10, 10, 10);


// Define Geometry
var region = FemDesign.Geometry.Region.RectangleXY(FemDesign.Geometry.Point3d.Origin, 4, 4);

// Create a Surface Support
var surfaceSupport = new FemDesign.Supports.SurfaceSupport(region, motionsSurface);
struElements.Add(surfaceSupport);


// Create the stiffness points
foreach (var point in points)
{
	var stiffPoints = new FemDesign.Supports.StiffnessPoint(surfaceSupport, point, motionsPoint);
	struElements.Add(stiffPoints);
}

// Create Model
var model = new FemDesign.Model(FemDesign.Country.S);
model.AddElements(struElements);

model.Open();