<p align="center">
 <img width="400" src="https://strusoft.com/uploads/products/fem-design/FEM-Design_text.png">
</p>

# femdesign-api
This repository contains source code for the FEM-Design Dynamo and Grasshopper toolboxes. 

## Contents

* [Branches](https://github.com/andosca/femdesign-api/blob/master/README.md#branches)
* [Contributing](https://github.com/andosca/femdesign-api/blob/master/README.md#contributing)
* [Versioning](https://github.com/andosca/femdesign-api/blob/master/README.md#versioning)
* [Authors](https://github.com/andosca/femdesign-api/blob/master/README.md#authors)
* [Disclaimer](https://github.com/andosca/femdesign-api/blob/master/README.md#disclaimer)
* [License](https://github.com/andosca/femdesign-api/blob/master/README.md#license)


## Branches
The repository contains four branches: 
* master
* [core](https://github.com/andosca/femdesign-api/tree/core)
* [dynamo](https://github.com/andosca/femdesign-api/tree/dynamo)
* [grasshopper](https://github.com/andosca/femdesign-api/tree/grasshopper)

The repository is split up in order to handle package references in the compiled class libraries.

### master
The master branch contains all source code used for the grasshopper and dynamo branches. The master branch is not intended to be compiled.

Grasshopper specific code is contained within code block:

```c#
#region grasshopper
#endregion
```

Dynamo specific code is contained within code block:

```c#
#region dynamo
#endregion
```
 
Moreover some classes and methods are decorated with Dynamo specific attributes:

```c#
[IsVisibleInDynamoLibrary()]
[IsLacingDisabled()]
```

These library specific code regions and attributes are stripped for each repesctive branch using [convertLibrary.py](scripts/convertLibrary/convertLibrary.py).

### core
The core branch contains the source code stripped from any Dynamo or Grasshopper methods.

### dynamo 
The dynamo branch contains the code for the dynamo solution. To setup a solution in Visual studio for the grasshopper branch: 
* [setup-dynamo-sln.md](https://github.com/andosca/femdesign-api/blob/dynamo/setup-dynamo-sln.md)

### grasshopper
The grasshopper branch contains the grasshopper solution. To setup a solution in Visual Studio for the dynamo branch: 
* [setup-grasshopper-sln.md](https://github.com/andosca/femdesign-api/blob/grasshopper/setup-grasshopper-sln.md)

## Contributing
Grasshopper and dynamo branches are not intended to be merged back with master at any point, rather they are created from master using [convertLibrary.py](scripts/convertLibrary/convertLibrary.py) when the master branch has been updated. 

Feel free to fork grasshopper and dynamo branches as you seem fit. However if you want to contribute your updates to the repository please add the changes to a fork of the master branch and create a pull request to ask the upstream repository (this repository) to accept these changes.

## Versioning
Future versioning will be using the following structure: major.minor.patch.

## Authors
The FEM-Design API repository is developed and maintained by StruSoft. The main developers are:
* [Andreas Oscarsson](https://github.com/andosca)

## Disclaimer
In addition to the disclaimer under the license ([LICENSE.md](LICENSE)) - All files and related documentation is for illustrative and educational purposes and may not interact with FEM-Design in a reliable way depending on your version, installation and content of the files. Furthermore, Strusoft won´t guarantee full support of the package.

## License
[LICENSE.md](LICENSE)

