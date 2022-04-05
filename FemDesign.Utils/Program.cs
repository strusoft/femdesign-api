using System.Reflection;
using System;
using System.Linq;


var myAssembly = Assembly.GetAssembly(typeof(FemDesign.Model));
var nameSpace = "StruSoft.Interop.StruXml.Data";

var classList = myAssembly.GetTypes()
          .Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal) && t.IsEnum == false)
          .ToArray();

var enumTypeList = myAssembly.GetTypes()
          .Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal) && t.IsEnum == true)
          .ToArray();


string template =    "using System.Collections.Generic;\n" +
                     "using System.Xml.Serialization;\n\n" +
                     "#region dynamo\n" +
                     "using Autodesk.DesignScript.Runtime;\n" +
                     "#endregion\n\n" +
                     "namespace StruSoft.Interop.StruXml.Data\n{\n";

// Generate a C# class programmaticall
// adding [IsVisibleInDynamoLibrary(false)] to all the partial class
// for public enum, there is no solution yet

for (int i = 0; i < classList.Length; i++)
{
    string attribute = "[IsVisibleInDynamoLibrary(false)]\n";
    string className = string.Format("public partial class {0}\n", classList[i].Name);
    string bracket = "{\n}\n\n";
    template += attribute + className + bracket;
}

for (int i = 0; i < enumTypeList.Length; i++)
{
    string attribute = "[IsVisibleInDynamoLibrary(false)]\n";
    string className = string.Format("public partial class {0}\n", classList[i].Name);
    string bracket = "{\n}\n\n";
    template += attribute + className + bracket;
}



template += "}";

// Find the Current Directory and look for the right folder where the modified FD_21.00.001.cs needs to be saved
string currentDirectory = Directory.GetCurrentDirectory();

var targetDirectoryNames = currentDirectory.Split(Path.DirectorySeparatorChar);
int folderLength = targetDirectoryNames.Length;

var targetDirectory = Path.Join(targetDirectoryNames[..(folderLength-3)]);
Console.WriteLine(targetDirectory);




// fd name should reference a filename and not manually type
string filename = "FD-21.00.001.cs";

string fileoutput = Path.Join(targetDirectory + @"\femdesign.dynamo\dynamo\strusoft\interop\struxml\data", filename);

Console.WriteLine(fileoutput);

await File.WriteAllTextAsync(fileoutput, template);


