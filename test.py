import FemDesignPython as FemDesign

print(FemDesign)
print(dir(FemDesign))

model = FemDesign.Model.DeserializeFromFilePath("C:/Temp/0001_pålbrygga/Pålbrygga12.struxml")
print(f"{len(model.Entities.Bars)} bars in the model")
print(f"{len(model.Entities.Slabs)} slabs in the model")

new_model = FemDesign.Model("S")
new_model.Entities
beam = FemDesign.Bars.Bar.BeamDefine()
