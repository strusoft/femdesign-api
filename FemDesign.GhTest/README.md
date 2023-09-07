### 📖 How to use it

##### Install Tenrec.gha

1. Place Tenrec.gha in _C:\Users\<YOUR USERNAME>\AppData\Roaming\Grasshopper\Libraries_.
2. Restart Rhinoceros and Grasshopper.
3. Find _Tenrec_ group and _Assert_ component in _Params > Util_.

##### Create a Grasshopper Unit Test file

![gif](generate_test.gif)

1. Create a Grasshopper definition to make a Unit Test.
2. Add the _Assert_ component, set input _Assert (A)_ to false when your Unit Test fail (and true otherwise) and include a message for when that happens.
3. Select all components that are part of the Unit Test.
4. Add a _Tenrec_ group to the canvas, it will group all selected components.
5. Change the _Tenrec_ group name by double click on it.
6. Repeat step 1 to 5 if you want to include more Unit Test in the same file.
7. Save the Grasshopper file as `.ghx` in your test folder.
8. In the top right corner, click on _settings > Source Code Generator_.
9. The _Unit Test Source Code Generator_ window should appear, _Grashopper file folder_ section should be set with the `.ghx` file folder location, _Output folder_ section should be set with `.ghx` the file folder location and _Output name_ is the name of the `.cs` file.
10. Press _Generate_ and close the window. The code file will be located at _Output folder_/_Output name_.cs

##### Create a Unit Test Project

1. In your project from _Solution Explorer_, _Add > Existing item..._ and include the code file generated previosly => _Output folder_/_Output name_.cs.
7. Build the project.

##### Run the Unit Tests

1. In Visual Studio 2019 _toolbar > Test > Test Explorer_. 
2. Press Run All Test In View. Make sure to do not have the .gh script opens.
3. You should see the results in all your Grasshopper tests.


### 🌈 Acknowledge

FEM-Design API use [Tenrec](https://github.com/DanielAbalde/Tenrec/tree/master) tool to generate and test the grasshopper files.
