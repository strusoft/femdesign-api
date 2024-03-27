"""
FemDesign API (C#) from python using Python.NET (clr)
Date:       2024-02-027
API:        23.0.1
Authors:    Alexander Radne - @xRadne, Marco Pellegrino - @Marco-Pellegrino 
Github:     https://github.com/strusoft/femdesign-api

Documentation at https://www.fuget.org/packages/FemDesign.Core
Read more about Python.NET (clr) at http://pythonnet.github.io/
"""

import os, sys, clr, math
import pandas as pd
import numpy as np

clr.AddReference("FemDesign.Core")
import FemDesign
import FemDesign.Bars
import FemDesign.Calculate
import FemDesign.Results



# Create a new model with country specified
model = FemDesign.Model(country= FemDesign.Country.S)


# Import List from .NET
clr.AddReference("System.Collections")
from System.Collections.Generic import List

# Add elements to model
points = List[FemDesign.Geometry.Point3d]()
for i in range(11):
    x = float(i)
    y = 0.0
    z = math.cos(2 * math.pi * i / 10) + 2
    p = FemDesign.Geometry.Point3d(x, y, z)
    points.Add(p)

beamtype = FemDesign.Bars.BarType.Beam
materialDB = FemDesign.Materials.MaterialDatabase.GetDefault()
material = materialDB.MaterialByName("S355")
sectionDB = FemDesign.Sections.SectionDatabase.GetDefault()
section = sectionDB.SectionByName("Steel sections, UKB, 127x76x13")

_section = List[FemDesign.Sections.Section]()
_section.Add(section)

connectivity = List[FemDesign.Bars.Connectivity]()
connectivity.Add(FemDesign.Bars.Connectivity.Default)

eccentricity = List[FemDesign.Bars.Eccentricity]()
eccentricity.Add(FemDesign.Bars.Eccentricity.Default)

bars = List[FemDesign.Bars.Bar]()
for i in range(points.Count - 1):
    p1, p2 = points[i], points[i + 1]
    line = FemDesign.Geometry.Edge(p1, p2, FemDesign.Geometry.Vector3d.UnitY)
    bar = FemDesign.Bars.Beam(line, material, _section.ToArray(), eccentricity.ToArray(), connectivity.ToArray(), f"B.{i+1}")
    bar.BarPart.OrientCoordinateSystemToGCS()
    bars.Add(bar)

model.AddElements[FemDesign.Bars.Bar](bars)

# Add supports to mode
supports = List[FemDesign.Supports.PointSupport]()

point = points[0]
vector1 = FemDesign.Geometry.Vector3d(1,0,0)
vector2 = FemDesign.Geometry.Vector3d(0,1,0)
plane = FemDesign.Geometry.Plane(point, vector1, vector2)

support1 = FemDesign.Supports.PointSupport.Rigid(plane)

supports.Add(support1)

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
v1 = FemDesign.Geometry.Vector3d(0.0, 0.0, -5.0)
load = FemDesign.Loads.PointLoad(p1, v1, LC2, "", FemDesign.Loads.ForceLoadType.Force)
loads.Add(load)

model.AddLoads[FemDesign.GenericClasses.ILoadElement](loads)


# Establish a Live Link connection in FemDesign
femdesign = FemDesign.FemDesignConnection(keepOpen = True)

# Open a Model
femdesign.Open(model)

# Run an Analysis
analysis = FemDesign.Calculate.Analysis(calcCase= True, calcComb= False)
femdesign.RunAnalysis(analysis)

# Read Results
bar_displacements = femdesign.GetResults[FemDesign.Results.BarDisplacement]()
node_displacements = femdesign.GetResults[FemDesign.Results.NodalDisplacement]()
support_reactions = femdesign.GetLoadCaseResults[FemDesign.Results.PointSupportReaction](LC1.Name)


# Disconnect the Live Link Connection
femdesign.Disconnect()

print("BARS DISPLACEMENT")
for obj in bar_displacements:
    print(f"Pos: {obj.Pos} - Ez: {obj.Ez} m, LoadCase: {obj.CaseIdentifier}")

print("")
print("NODES DISPLACEMENT")
for obj in node_displacements:
    print(f"NodeId: {obj.NodeId} - Ez: {obj.Ez} m, LoadCase: {obj.CaseIdentifier}")

print("")
print("REACTION FORCES")
for obj in support_reactions:
    print(f"NodeId: {obj.NodeId} - Fz: {obj.Fz} kN, LoadCase: {obj.CaseIdentifier}")