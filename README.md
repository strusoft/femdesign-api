<p align="center">
 <img src="https://strusoft.com/wp-content/uploads/2022/02/MicrosoftTeams-image-69.png">
</p>

# femdesign-api
This repository contains source code for the FEM-Design API wrapper. It consists of the `C#`-Core together with the `Dynamo` and `Grasshopper` plugins. 

## Contents
* [Contributing](#contributing)
* [Road-map](#road-map)
* [Installation](#Installation)
* [Examples](#Examples)
* [Versioning](#versioning)
* [Authors](#authors)
* [Disclaimer](#disclaimer)
* [License](#license)

## Installation
See installation for your framework below. 
All releases can also be found on the [releases page](https://github.com/strusoft/femdesign-api/releases).
### 👩‍💻 C#
The C# package can be installed in two ways:
1. Adding as a `NuGet` package. (See the package on [nuget.org](https://www.nuget.org/packages/FemDesign.Core/) for reference)
2. Download this repo and compile it from source.

**Note** that for some C# applications the setting `Prefer 32-Bit` might have to be unchecked for your program to work properly with this API. The package have been developed and tested for `.NET Framework 4.7.2`.

### 🦗 Grasshopper
Either
- Download the grasshopper package from [food4rhino.com](https://www.food4rhino.com/en/app/fem-design-api-toolbox)
- Or download it manually by downloading `FemDesign.Grasshopper.zip` from [latest release](https://github.com/strusoft/femdesign-api/releases/latest), unblock the file and unzip to the grasshopper libraries folder (e.g `C:\Users\USERNAME\AppData\Roaming\Grasshopper\Libraries`).

### 🤖 Dynamo
Either
- Install it from within Dynamo using Online Package Search. Search for `FemDesign`. (Guide on [installing dynamo package](https://www.dynamonow.com/downloading-installing-packages/))
- Or Install it manually by downloading `FemDesign.Dynamo.zip` from [latest release](https://github.com/strusoft/femdesign-api/releases/latest), unblock the file and unzip to the dynamo package folder of the version of Revit/Dynamo of your choice (e.g `C:\Users\USERNAME\AppData\Roaming\Dynamo\Dynamo Revit\2.6\packages` for Dynamo Revit 2.6 etc.)

## Examples
Examples can be found in the folder [FemDesign.Samples/](https://github.com/strusoft/femdesign-api/tree/master/FemDesign.Samples). More examples are planned to be added in the future.

## Contributing
If you want to contribute please follow our [contribution guide](https://github.com/strusoft/femdesign-api/wiki/Contribute).

## Road-map
The road-map of the project is expressed by the [milestones](https://github.com/strusoft/femdesign-api/milestones).

## Versioning
Future versioning will be using the following structure: `major.minor.patch` where major follows the FEM-Design version.

## Authors
The FEM-Design API repository is developed and maintained by StruSoft. 

## Disclaimer
In addition to the disclaimer under the license ([LICENSE.md](LICENSE)) - All files and related documentation is for illustrative and educational purposes and may not interact with FEM-Design in a reliable way depending on your version, installation and content of the files. Furthermore, Strusoft won´t guarantee full support of the package. 😊

## License
[LICENSE.md](LICENSE)

