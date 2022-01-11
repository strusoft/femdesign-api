// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;
using GH = Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class ResultsType : GH_ValueList
    {
        public ResultsType()
        {
            Name = "Results.ResultsType";
            NickName = "ResultsType";
            Description = "ResultsType enum.";
            Category = "FemDesign";
            SubCategory = "Results";

            ListItems.Clear();
            var values = Enum.GetValues(typeof(ResultType)).Cast<ResultType>();
            foreach (ResultType value in values)
            {
                GH_ValueListItem vi = new GH_ValueListItem(value.ToString(), String.Format("\"{0}\"", value.ToString()));
                ListItems.Add(vi);
            }
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("c7d38a7a-32da-49ba-a6b5-f182ac8e6212"); }
        }
    }
}