# Examples
This section contains examples to help you get acquainted with the FEM-Design API via C#, Grasshopper or Python.

The _numbered examples_ are meant to give you an introduction to the API and how to work with it, starting simple in Example 1 and growing in complexity. These examples showcase the same thing for all languages and toolboxes, meaning you can run the same example in C# or Grasshopper and get the same result.

The _Practical examples_ are all meant to showcase different functions, projects and applications of the API. They are not necessarily the same for all languages, unlike the numbered examples. If there is a particular example you'd like to see, please let us know!

## Running the example files

### 👩‍💻 C#
To properly run the C# examples, we recommend that you clone the entirety of the FEM-Design API repository with Git. You can find instructions for this [here](https://docs.github.com/en/repositories/creating-and-managing-repositories/cloning-a-repository). You can also download the entire repository as a .zip file and unpack it on your computer.

There are two ways to run the C# examples: either find a specific example in the `femdesign-api/FemDesign.Examples/C#` folder, then open the project file (`.csproj`) found there and run the program. Or, open the main solution (`femdesign-api.sln`) found in the `femdesign-api` folder, and select which of the example projects you want to run. 

### 🦗 Grasshopper
The Grasshopper examples can be run without cloning the repository - all you need is to install the package, instructions for this can be found on the [main page](https://github.com/strusoft/femdesign-api#-grasshopper). Once installed, you can download any of the files in the `femdesign-api/FemDesign.Examples/Grasshopper` folder and open them in Grasshopper.

The examples are ready to run out-of-the-box, but you might have to set a file path or file name. You'll know either from instructions in the file, or because some components are orange or red.

### 🤖 Dynamo
With Dynamo installed, any of the Dynamo examples are ready to run. Download an entire example folder and run the - or one of the - `.dyn` files within. Any `.dyf` files are there to define a cluster of components, but as long as they are in the same folder as the `.dyn` files you run, you don't have to touch them.

The examples are ready to run out-of-the-box, but you might have to set a file path or file name. You'll know either from instructions in the file, or because some components give an error.

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
The examples and samples here are created and mainatained by StruSoft. 
## Questions and feedback
If you have any questions regarding the examples and how they work, feel free to contact us. We also welcome any feedback, as well as suggestions for future examples. You can reach us at support@strusoft.freshdesk.com or at https://strusoft.freshdesk.com/en/support/tickets/new.

## Disclaimer
Any and all sample and example files are for educational purposes, and may not interact with FEM-Design in a reliable way depending on your version, installation and content of the files. Additionally, as part of the Maintenance policy, we cannot guarantee that all files will be up to date at all times.
