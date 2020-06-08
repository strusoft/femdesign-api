# Setup Grasshopper components solution

## 1. Create local repository
Clone and create a local repository

## 2. Create new project from existing code
Create new Visual Studio C# class library project from existing code and chose your repository. Set .NET Framework to match references.

## 3. Add references
Add necessary references
* From NuGet
  * RhinoCommon
  * Grasshopper
* From Reference Manager
  * System.Drawing
  

## 4. Edit resources
### Remove icon references
Find in files using regular expression:
```
return FemDesign.Properties.Resources.+;$
```
 and replace in files:
 ```c#
 return null;
 ```

### Add material library and section library (optional)
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

```c#
Copy "$(TargetPath)" "$(TargetDir)$(ProjectName).gha"
Erase "$(TargetPath)"
```

## Additional informaiton
For more information about Grasshopper components please visit:
* https://developer.rhino3d.com/guides/grasshopper/your-first-component-windows/
* https://developer.rhino3d.com/guides/grasshopper/simple-component/
