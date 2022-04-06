using System.Reflection;
using System;
using System.Linq;
using System.Text;


// Find the Current Directory and look for the right folder where the modified FD_21.00.001.cs needs to be saved
string currentDirectory = Directory.GetCurrentDirectory();

var targetDirectoryNames = currentDirectory.Split(Path.DirectorySeparatorChar);
int folderLength = targetDirectoryNames.Length;

// use 4 instead of 3 when you test it
var SolutionDir = Path.Join(targetDirectoryNames[..(folderLength - 3)]);

string[] dirs = Directory.GetFiles(SolutionDir + @"\FemDesign.Core\StruSoft\Interop\StruXml\Data", "FD*", SearchOption.AllDirectories);
string fileNamePath = dirs[0];
string fileName = Path.GetFileName(fileNamePath);


string template =   "using System.Collections.Generic;\n" +
                    "using System.Xml.Serialization;\n\n" +
                    "#if ISDYNAMO\n" +
                    "#region dynamo\n" +
                    "using Autodesk.DesignScript.Runtime;\n" +
                    "#endregion\n" +
                    "#endif\n\n";


string dynamoAttribute =    "\t#if ISDYNAMO\n" +
                            "\t[IsVisibleInDynamoLibrary(false)]\n" +
                            "\t#endif\n";


var text = new StringBuilder();
text.AppendLine(template);


foreach (string s in File.ReadAllLines(fileNamePath))
{
    if (s.Contains("public partial class"))
    {
        text.AppendLine(s.Replace("public partial class", dynamoAttribute + "\tpublic partial class"));
    }
    else if(s.Contains("public enum"))
    {
        text.AppendLine(s.Replace("public enum", dynamoAttribute + "\tpublic enum"));
    }
    else if(s.Contains("#if ISDYNAMO"))
    {
        return;
        // throw new Exception("FD file already contains Dynano Attribute. Are you adding the attribute twice?");
    }
    else
    {
        text.AppendLine(s);
    }
}



using (var file = new StreamWriter(File.Create(fileNamePath)))
{
    file.Write(text.ToString());
}