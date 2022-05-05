using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Runtime;

namespace FemDesign.Bars
{
    [IsVisibleInDynamoLibrary(false)]
    [System.Serializable]
    public enum SectionType
    {
        Truss,
        RegularBeamColumn, // Complex Section and NOT Delta Beam Complex Section
        CompositeBeamColumn, // Complex Composite and NOT Delta Beam Complex Section
        DeltaBeamColumn // Complex Section and Delta Beam
    };
}