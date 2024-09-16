using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign;
using FemDesign.Loads;
using FemDesign.Materials;
using FemDesign.Calculate;
using FemDesign.Shells;


namespace Practical_example___Excel_to_FEM_Design
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            string project;
            string description;
            string designer;
            string signature;
            string comment;

            var model = new Model(Country.COMMON);

            using (var package = new ExcelPackage(new FileInfo("FD_Input.xlsx")))
            {
                // read input data from Excel
                // read all the value in the secondo row of the sheet
                var row = 2;
                var sheet = package.Workbook.Worksheets["Project"];

                var data = sheet.Cells[row, 1, sheet.Dimension.End.Row, sheet.Dimension.End.Column]
                .Select(cell => (string)cell.Value)
                .ToArray();

                project = data[0];
                description = data[1];
                designer = data[2];
                signature = data[3];
                comment = data[4];

                // read values from the second row of the "Load Case" sheet
                row = 2;
                var sheet1 = package.Workbook.Worksheets["Load Case"];

                var datas = sheet1.Cells[row, 1, sheet1.Dimension.End.Row, sheet1.Dimension.End.Column].Value;
                var loadData = datas as object[,];
                // iterate over the loadData array
                for (int i = 0; i < loadData.GetLength(0); i++)
                {
                    var name = (string)loadData[i, 0];
                    var type = FemDesign.GenericClasses.EnumParser.Parse<FemDesign.Loads.LoadCaseType>((string)loadData[i, 1]);
                    var duration = FemDesign.GenericClasses.EnumParser.Parse<FemDesign.Loads.LoadCaseDuration>((string)loadData[i, 2]);

                    // create a new load case
                    var loadCase = new FemDesign.Loads.LoadCase(name, type, duration);
                    model.AddLoadCases(loadCase);
                }


                // read values from the second row of the "Slab" sheet
                var database = MaterialDatabase.GetDefault();
                row = 2;
                var sheet2 = package.Workbook.Worksheets["Slab"];

                datas = sheet2.Cells[row, 1, sheet2.Dimension.End.Row, sheet2.Dimension.End.Column].Value;
                loadData = datas as object[,];
                // iterate over the loadData array
                for (int i = 0; i < loadData.GetLength(0); i++)
                {
                    var basePoint = new FemDesign.Geometry.Point3d(0, 0, 0);
                    var width = (double)loadData[i, 0];
                    var height = (double)loadData[i, 1];
                    var thickness = (double)loadData[i, 2];
                    var materialName = (string)loadData[i, 3];
                    var material = database.MaterialByName(materialName);
                    var slab = FemDesign.Shells.Slab.Plate(basePoint, width, height, thickness, material);

                    model.AddElements(slab);

                    var edge = slab.Region.Contours[0].Edges;

                    var lineSupport = FemDesign.Supports.LineSupport.Rigid(edge[0], false);
                    model.AddSupports(lineSupport);
                }

                

            }



            using (var femDesignConnection = new FemDesignConnection(
                        fdInstallationDir: "",
                        minimized: false,
                        keepOpen: true))
            {

                femDesignConnection.Open(model);
                femDesignConnection.SetProjDescription(project, description, designer, signature, comment);

                var analysis = Analysis.StaticAnalysis(calccomb: false);
                femDesignConnection.RunAnalysis(analysis);

                var displacement = femDesignConnection.GetResults<FemDesign.Results.ShellDisplacement>().Select(x => Math.Abs(x.Ez)).Max();
                var quantities = femDesignConnection.GetResults<FemDesign.Results.QuantityEstimationConcrete>().Select(x => x.CO2Footprint).Max();


                // write the displacement to the Excel file in the sheet called "Results"
                using (var package = new ExcelPackage(new FileInfo("FD_Input.xlsx")))
                {
                    var sheet = package.Workbook.Worksheets["Results"];
                    sheet.Cells[1, 1].Value = "displacement";
                    sheet.Cells[1, 2].Value = displacement;

                    sheet.Cells[2, 1].Value = "CO2Footprint";
                    sheet.Cells[2, 2].Value = quantities;

                    package.Save();
                }
            }

            Console.WriteLine("FEM-Design project created!");
        }
    }
}
