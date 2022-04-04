using System.Reflection;
using System;
using System.Linq;


var myAssembly = Assembly.GetAssembly(typeof(FemDesign.Model));
var nameSpace = "StruSoft.Interop.StruXml.Data";

var typeList = myAssembly.GetTypes()
          .Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal) && t.IsEnum == false)
          .ToArray();

var enumType = myAssembly.GetTypes()
          .Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal) && t.IsEnum == true)
          .ToArray();


string template =    "using System.Collections.Generic;\n" +
                     "using System.Xml.Serialization;\n\n" +
                     "#region dynamo\n" +
                     "using Autodesk.DesignScript.Runtime;\n" +
                     "#endregion\n\n" +
                     "namespace StruSoft.Interop.StruXml.Data\n{";

// Generate a C# class programmaticall
// adding [IsVisibleInDynamoLibrary(false)] to all the partial class
// for public enum, there is no solution yet

for (int i = 0; i < typeList.Length; i++)
{
    string attribute = "[IsVisibleInDynamoLibrary(false)]\n";
    string className = string.Format("public partial class {0}\n", typeList[i].Name);
    string bracket = "{\n}\n\n";
    template += attribute + className + bracket;
}

template += "}";

// Find the Current Directory and look for the right folder where the modified FD_21.00.001.cs needs to be saved
string currentDirectory = Directory.GetCurrentDirectory();

var targetDirectoryNames = currentDirectory.Split(Path.DirectorySeparatorChar);
int folderLength = targetDirectoryNames.Length;

var targetDirectory = Path.Join(targetDirectoryNames[..(folderLength-4)]);

// FD name should reference a filename and not manually type
string fileName = "FD_21.00.001.cs";

string fileOutput = Path.Join(targetDirectory + @"\FemDesign.Dynamo\Dynamo\StruSoft\Interop\StruXml\Data", fileName);

Console.WriteLine(fileOutput);

await File.WriteAllTextAsync(fileOutput, template);