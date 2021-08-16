"""
FemDesign API (C#) from python using Python.NET (clr)
Date:   2021-08-05
Author: Alexander Radne, @xRadne
Github: https://github.com/strusoft/femdesign-api

Documentation at https://www.fuget.org/packages/FemDesign.Core
Read more about Python.NET (clr) at http://pythonnet.github.io/
"""

# Load FemDesign API as a python module.
# Needs the file C# assembly FemDesign.Samples\\Python\\bin\\FemDesign.Core.dll.
# Download FemDesign.Core.dll from https://github.com/strusoft/femdesign-api/releases
# and place in the folder.
import os, sys, clr
sys.path.append(os.path.abspath("FemDesign.Samples\\Python\\bin"))
clr.AddReference("FemDesign.Core")
import FemDesign

# Create a new model with country specified
model = FemDesign.Model(FemDesign.Country.S)

clr.AddReference("System.Collections")
from System.Collections.Generic import List

# Add elements to model
supports = List[FemDesign.Supports.PointSupport]()
for i in range(11):
    p = FemDesign.Geometry.FdPoint3d(float(i), float(10-i), 0.0)
    support = FemDesign.Supports.PointSupport.Hinged(point=p)
    supports.Add(support)

model.AddElements[FemDesign.Supports.PointSupport](supports)

# Add loadcases to model
loadcases = List[FemDesign.Loads.LoadCase]()

LC1 = FemDesign.Loads.LoadCase("LC1", FemDesign.Loads.LoadCaseType.DEAD_LOAD, FemDesign.Loads.LoadCaseDuration.PERMANENT)
loadcases.Add(LC1)
LC2 = FemDesign.Loads.LoadCase("LC2", FemDesign.Loads.LoadCaseType.STATIC, FemDesign.Loads.LoadCaseDuration.PERMANENT)
loadcases.Add(LC2)

model.AddLoadCases(loadcases)

# Add loads to model
loads = List[FemDesign.GenericClasses.ILoadElement]()
p1 = FemDesign.Geometry.FdPoint3d(5.0, 5.0, 0.0)
v1 = FemDesign.Geometry.FdVector3d(0.0, 0.0, -5.0)
load = FemDesign.Loads.PointLoad(p1, v1, LC2, "", FemDesign.Loads.ForceLoadType.Force)
loads.Add(load)

model.AddLoads[FemDesign.GenericClasses.ILoadElement](loads)

# Save model to file
path = os.path.abspath("model.struxml")
model.SerializeModel(path)

# Open model in FemDesign
app = FemDesign.Calculate.Application()
app.OpenStruxml(path, killProcess=True)

