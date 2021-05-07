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
    [NodeName("TestMethod")]
    [NodeCategory("FemDesign.Tests")]
    [NodeDescription("Testing to write custom Dynamo node", typeof(Properties.Resources))]
    [IsDesignScriptCompatible]
    public class TestMethodNodeModel : NodeModel
    {
        public TestMethodNodeModel()
        {
            OutPorts.Add(new PortModel(PortType.Output, this, new PortData("outputString", "Output string")));

            RegisterAllPorts();
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            var functionCall =
              AstFactory.BuildFunctionCall(
                  new Func<FemDesign.Geometry.FdPoint3d>(FemDesign.Geometry.FdPoint3d.Origin),
                  new List<AssociativeNode> { });

            return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
        }

        [JsonConstructor]
        public TestMethodNodeModel(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts) { }
    }
}
