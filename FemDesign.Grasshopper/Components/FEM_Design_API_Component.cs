using FemDesign.Utils;
using Grasshopper.Kernel;
using FemDesign.Grasshopper.Extension;

namespace FemDesign.Grasshopper
{
    public abstract class FEM_Design_API_Component : GH_Component
    {
        public FEM_Design_API_Component(string name, string nickname, string description, string category, string subCategory)
            : base(name, nickname, description, category, subCategory)
        {
        }

        protected override string HtmlHelp_Source()
        {
            string result;
            try
            {
                result = FemDesign.Grasshopper.Extension.HtmlWriter.FemDesignHtml(this);
            }
            catch
            {
                result = base.HtmlHelp_Source();
            }
            return result;
        }
    }
}