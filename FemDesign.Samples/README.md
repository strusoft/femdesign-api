# Examples
This section contains examples and sample files to help you get acquainted with the FEM-Design API via C#, Grasshopper or Python.

## Running the example files

### 👩‍💻 C#
To properly run the C# examples, we recommend that you clone the entirety of the FEM-Design API repository with Git. You can find instructions for this [here](https://github.com/git-guides/git-clone). You can also download the entire repository as a .zip file and unpack it on your computer.

The examples are then found in the femdesign-api/FemDesign.Samples/C# folder. Open the `FemDesign.Samples.csproj` file. Within the project, the `Main.cs` file is used to run any files or examples - simply `//` comment to hide scripts from running, and leave one open to run it.

### 🦗 Grasshopper
The Grasshopper examples can be run without cloning the repository - all you need is to install the package, instructions for this can be found on the main page. Once installed, you can download any of the files in the femdesign-api/FemDesign.Samples/Grasshopper folder and open them in Grasshopper.

The examples are ready to run out-of-the-box, but you might have to set a file path or file name. You'll know either from instructions in the file, or because some components are orange or red.

### 🤖 Dynamo
_Coming soon_

### 🐍 Python
The python example folder contains an example (`using_pythonnet.py`) on how to run the C# (`FemDesign.Core`) API from Python. This example uses the python package Python.NET (clr) (http://pythonnet.github.io/).

**NOTE**: A complete Python wrapper is [planned in the future](https://github.com/strusoft/femdesign-api/issues/221), but pythonnet can be used already today to access all of the functionality of the C# API.

The example have been tested using
- IronPython 2.7 (pythonnet pre-installed)
- CPython 3.7 (with [pip package pythonnet](https://pypi.org/project/pythonnet/))

### 🐉 Python-old-examples
The Python-old-examples is an API wrapper with different call functions to make it easier to create the struxml-model and run the analysis. Please see a detailed description in the documentation below.

https://wiki.fem-design.strusoft.com/xwiki/bin/view/FEM-Design%20API/Python/API%20wrapper/

## Maintenance
The examples and samples here are created and mainatained by StruSoft. However, 

## Questions and feedback
If you have any questions regarding the examples and how they work, feel free to contact us. We also welcome any feedback, as well as suggestions for future examples. You can reach us at femdesign.api@strusoft.com.

## Disclaimer
Any and all sample and example files are for educational purposes, and may not interact with FEM-Design in a reliable way depending on your version, installation and content of the files. Additionally, as part of the Maintenance policy, we cannot guarantee that all files will be up to date at all times.
