using Dynamo.Graph.Nodes;
using Newtonsoft.Json;
using ProtoCore.AST.AssociativeAST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Runtime;
using Dynamo.Controls;
using Dynamo.UI.Commands;
using Dynamo.Wpf;


namespace FemDesign.Dynamo
{
    [NodeName("PointSupport")]
    [NodeCategory("FemDesign.Supports")]
    [NodeDescription("Testing to write custom Dynamo node", typeof(Properties.Resources))]
    [InPortNames("point", "identifier")]
    [InPortTypes("FemDesign.Geometry.FdPoint3d", "string")]
    [InPortDescriptions("Identifier name")]
    [OutPortNames("PointSupport")]
    [OutPortTypes("FemDesign.Supports.PointSupport")]
    [OutPortDescriptions("PointSupport")]
    [IsDesignScriptCompatible]
    public class PointSupportNodeModel : NodeModel
    {
        public PointSupportNodeModel()
        {
            InPorts.Add(new PortModel(PortType.Input, this, new PortData("inputPoint", "Input FdPoint3d")));
            InPorts.Add(new PortModel(PortType.Input, this, new PortData("inputString", "Input string")));

            // Nodes can have an arbitrary number of inputs and outputs.
            // If you want more ports, just create more PortData objects.
            OutPorts.Add(new PortModel(PortType.Output, this, new PortData("outputString", "Output string")));

            RegisterAllPorts();
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            if (!InPorts[0].IsConnected || !InPorts[1].IsConnected)
            {
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
            }

            var functionCall =
              AstFactory.BuildFunctionCall(
                new Func<Geometry.FdPoint3d, string, FemDesign.Supports.PointSupport>(FemDesign.Supports.PointSupport.Rigid),
                new List<AssociativeNode> { inputAstNodes[0], inputAstNodes[1] });

            return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
        }

        [JsonConstructor]
        public PointSupportNodeModel(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts) { }

        public static FemDesign.Supports.PointSupport DefinePointSupport(string identifier)
        {
            return new Supports.PointSupport(new Geometry.FdPoint3d(), Releases.Motions.Define(), Releases.Rotations.Define(), identifier);
        }
    }
}
