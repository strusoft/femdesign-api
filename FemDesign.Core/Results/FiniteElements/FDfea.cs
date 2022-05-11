using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Results
{
    public partial class FDfea
    {
        /// <summary>
        /// Fea Nodes Object
        /// </summary>
        public List<FeaNode> FeaNode { get; set; }

        /// <summary>
        /// Fea Shell Object
        /// </summary>
        public List<FeaShell> FeaShell { get; set; }


        public FDfea(List<FeaNode> feaNode, List<FeaShell> feaShell)
        {
            this.FeaNode = feaNode;
            this.FeaShell = feaShell;
        }
    }
}