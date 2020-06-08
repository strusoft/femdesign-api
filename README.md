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
The repository contains three branches: 
* master
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

These library specific code regions and attributes are stripped for each repesctive branch.

### dynamo 
The dynamo branch contains the code for the dynamo solution. To setup a solution in Visual studio for the grasshopper branch: 
* [setup-dynamo-sln.md](https://github.com/andosca/femdesign-api/blob/master/setup-dynamo-sln.md)

### grasshopper
The grasshopper branch contains the grasshopper solution. To setup a solution in Visual Studio for the dynamo branch: 
* [setup-grasshopper-sln.md](https://github.com/andosca/femdesign-api/blob/master/setup-grasshopper-sln.md)

## Contributing

## Versioning

## Authors
The FEM-Design API repository is developed and maintained by StruSoft. The main developers are:
* [Andreas Oscarsson](https://github.com/andosca)

## Disclaimer

## License
[LICENSE.md](LICENSE)

