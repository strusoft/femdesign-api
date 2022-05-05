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
# and place in the folder. (Can be found in FemDesign.Grasshopper.zip)
# Remember to unblock the dll or clr.AddReference(FemDesign.Core) will fail.
import os, sys, clr, math
sys.path.append(os.path.abspath("FemDesign.Samples\\Python\\bin"))
clr.AddReference("FemDesign.Core")
import FemDesign

# Create a new model with country specified
model = FemDesign.Model(FemDesign.Country.S)

# Import List from .NET
clr.AddReference("System.Collections")
from System.Collections.Generic import List

# Add elements to model
points = List[FemDesign.Geometry.FdPoint3d]()
for i in range(11):
    x = float(i)
    y = math.cos(2 * math.pi * i / 10) + 2
    z = 0.0
    p = FemDesign.Geometry.FdPoint3d(x, y, z)
    points.Add(p)

beamtype = FemDesign.Bars.BarType.Beam
materialDB = FemDesign.Materials.MaterialDatabase.GetDefault()
material = materialDB.MaterialByName("S 355")
sectionDB = FemDesign.Sections.SectionDatabase.GetDefault()
section = sectionDB.SectionByName("Steel sections, UKB, 127x76x13")
_section = List[FemDesign.Sections.Section]()
_section.Add(section)
connectivity = List[FemDesign.Bars.Connectivity]()
connectivity.Add(FemDesign.Bars.Connectivity.GetDefault())
eccentricity = List[FemDesign.Bars.Eccentricity]()
eccentricity.Add(FemDesign.Bars.Eccentricity.GetDefault())

bars = List[FemDesign.Bars.Bar]()
for i in range(points.Count - 1):
    p1, p2 = points[i], points[i + 1]
    line = FemDesign.Geometry.Edge(p1, p2, FemDesign.Geometry.FdVector3d.UnitZ())
    bar = FemDesign.Bars.Bar(line, beamtype, material, _section.ToArray(), connectivity.ToArray(), eccentricity.ToArray(), f"B.{i+1}")
    bar.BarPart.OrientCoordinateSystemToGCS()
    bars.Add(bar)

model.AddElements[FemDesign.Bars.Bar](bars)

# Add supports to mode
supports = List[FemDesign.Supports.PointSupport]()
support1 = FemDesign.Supports.PointSupport.Rigid(points[0])
support2 = FemDesign.Supports.PointSupport.Rigid(points[10])
supports.Add(support1)
supports.Add(support2)

model.AddElements[FemDesign.Supports.PointSupport](supports)

# Add loadcases to model
loadcases = List[FemDesign.Loads.LoadCase]()

LC1 = FemDesign.Loads.LoadCase("LC1", FemDesign.Loads.LoadCaseType.DeadLoad, FemDesign.Loads.LoadCaseDuration.Permanent)
loadcases.Add(LC1)
LC2 = FemDesign.Loads.LoadCase("LC2", FemDesign.Loads.LoadCaseType.Static, FemDesign.Loads.LoadCaseDuration.Permanent)
loadcases.Add(LC2)

model.AddLoadCases(loadcases)

# Add loads to model
loads = List[FemDesign.GenericClasses.ILoadElement]()
p1 = points[5]
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

