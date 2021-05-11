### Dynamo Changes
- Bars/Byckling/BucklingLength **visible**
- Bars/Byckling/BucklingData **visible**
- Bars/Connectivity **Not fixed**
- Bars/Eccentricity **visible**
- Materials/Material **Not fixed**
- Materials/MaterialDatabase **Not fixed**
- Model/Model **Missing visible properties**
- ModellingTools/FictitiousShell **Missing method**
- Releases/DetachType **Not hidden enum**
- RevitTools/ProjectSettings **removed**
- Materials/MaterialDatabase **Not fixed**
- Shells/ShellEccentricity **Missing method**
- Shells/ShellEdgeConnection **Missing 4 methods**
- Shells/ShellOrthotropy **Missing 2 methods**




Methods named somethoing like `GetXXXXX` or `SetXXXXX` that should be visible in dynamo on a hidden class has now a Core method named `XXXXX` while Dynamo have a wrapper method named `GetXXXXX` as previously
