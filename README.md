<p align="center">
 <img width="400" src="https://strusoft.com/uploads/products/fem-design/FEM-Design_text.png">
</p>

# femdesign-api
This repository contains source code for the FEM-Design API wrapper. It consists of the `C#`-Core together with the `Dynamo` and `Grasshopper` plugins. 

## Contents
* [Contributing](#contributing)
* [Installation](#Installation)
* [Examples](#Examples)
* [Versioning](#versioning)
* [Authors](#authors)
* [Disclaimer](#disclaimer)
* [License](#license)

## Contributing
Feel free to fork this repo as you seem fit. Please let us know with an issue what feature you want and how you plan on implement it and we (the [authors](#Authors)) will guide you. 

## Installation
See installation for your framework below. 
All releases can also be found on the [releases page](https://github.com/strusoft/femdesign-api/releases).
### C#
The C# package can be installed in two ways:
1. Adding as a `NuGet` package. (See the package on [nuget.org](https://www.nuget.org/packages/FemDesign.Core/) for reference)
2. Download this repo and compile it from source.

**Note** that for some C# applications the setting `Prefer 32-Bit` might have to be unchecked for your program to work properly with this API. The package have been developed and tested for `.NET Framework 4.7.2`.

### Grasshopper
Download the grasshopper package from [food4rhino.com](https://www.food4rhino.com/en/app/fem-design-api-toolbox)

### Dynamo
Install it from within Dynamo using Online Package Search. Search for `FemDesign`. (Guide on [installing dynamo package](https://www.dynamonow.com/downloading-installing-packages/))

## Examples
Examples can be found in the folder [FemDesign.Samples/](https://github.com/strusoft/femdesign-api/tree/master/FemDesign.Samples). More examples are planned to be added in the future.

## Versioning
Future versioning will be using the following structure: `major.minor.patch` where major follows the FEM-Design version.

## Authors
The FEM-Design API repository is developed and maintained by StruSoft. 

## Disclaimer
In addition to the disclaimer under the license ([LICENSE.md](LICENSE)) - All files and related documentation is for illustrative and educational purposes and may not interact with FEM-Design in a reliable way depending on your version, installation and content of the files. Furthermore, Strusoft won´t guarantee full support of the package.

## License
[LICENSE.md](LICENSE)

