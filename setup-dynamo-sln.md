# Setup Dynamo solution

## 1. Create local repository
Clone and create a local repository

## 2. Create new project from existing code
Create new Visual Studio C# class library project from existing code and chose your repository. Set .NET Framework to match references.

## 3. Add references
Add necessary references
* From NuGet
  * DynamoVisualProgramming.DynamoServices
  * DynamoVisualProgramming.ZeroTouchLibrary
* From local Revit installation
  * RevitAPI
  * RevitServices

## 4. Add resources (optional)
Add necessary embedded resources according to [Resource.Designer.cs](https://github.com/andosca/femdesign-api/blob/dynamo/Properties/Resources.Designer.cs)
* Resources/materialLibrary/
* Resources/sectionLibrary/

If these resources are not added MaterialDatabase.Default and SectionDatabase.Default will not find relevant embedded resources. MaterialDatabase.FromStruxml and SectionDatabase.FromStruxml can still be used. Necessary resources can be created by exporting material libraries and section libraries as .struxml in FEM-Design. 

materials_XX.struxml can contain both materials and reinforcing materials:
```xml
<database>
 <materials>
  ...
 </materials>
 <reinforcing_materials>
  ...
 </reinforcing_materials>
 <end></end>
</database>
```

sections.struxml can contain sections to be used:
```xml
<database>
 <sections>
  ...
 </sections>
 <end></end>
</database>
```


## 5. Set post-build events
Set post-build events:
```
xcopy /Y "$(TargetDir)*.dll" "$(AppData)\Dynamo\Dynamo Revit\2.1\packages\$(ProjectName)\bin\"
xcopy /Y "$(TargetDir)FemDesign.xml" "$(AppData)\Dynamo\Dynamo Revit\2.1\packages\$(ProjectName)\bin\"
xcopy /Y "$(ProjectDir)FemDesign_DynamoCustomization.xml" "$(AppData)\Dynamo\Dynamo Revit\2.1\packages\$(ProjectName)\bin\"
xcopy /Y "$(ProjectDir)pkg.json" "$(AppData)\Dynamo\Dynamo Revit\2.1\packages\$(ProjectName)"
```

## Additional information
For additional information about Dynamo Zero Touch Nodes in C# please visit:
http://teocomi.com/dynamo-unchained-1-learn-how-to-develop-zero-touch-nodes-in-csharp/
