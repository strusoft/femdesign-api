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
        /// Fea Bar Object
        /// </summary>
        public List<FeaBar> FeaBar { get; set; }

        /// <summary>
        /// Fea Shell Object
        /// </summary>
        public List<FeaShell> FeaShell { get; set; }


        public FDfea(List<FeaNode> feaNode, List<FeaBar> feaBar, List<FeaShell> feaShell)
        {
            this.FeaNode = feaNode;
            this.FeaBar = feaBar;
            this.FeaShell = feaShell;
        }
    }
}