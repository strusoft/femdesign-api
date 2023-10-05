using FemDesign.Utils;
using Grasshopper.Kernel;

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
                result = this.FemDesignHtml();
            }
            catch
            {
                result = base.HtmlHelp_Source();
            }
            return result;
        }

        private string FemDesignHtml()
        {
            HtmlWriter htmlWriter = new HtmlWriter("Help file for " + this.Name, null, HtmlCss.Standard);
            htmlWriter.AddOpenTag("body");
            htmlWriter.AddClosedTag("div", "class=\"small\"", string.Concat(new string[]
            {
                this.Category,
                " &raquo; ",
                this.SubCategory,
                " &raquo; ",
                this.NickName
            }));
            htmlWriter.AddOpenTag("h2");
            htmlWriter.AddImageTag(FemDesign.Properties.Resources.Fd_TabIcon_24_24, base.GetType().Name, 24, 24);
            htmlWriter.AddLine(this.NickName);
            htmlWriter.CloseLastTag();
            htmlWriter.AddClosedTag("div", "class=\"medium\"", this.Description);
            htmlWriter.AddLine().AddClosedTag("hr").AddLine();
            htmlWriter.AddClosedTag("div", "class=\"small\"", "Input parameters:");
            if (base.Params.Input.Count > 0)
            {
                htmlWriter.AddOpenTag("table", "border=\"0\" cellspacing=\"1\" cellpadding=\"2\"");
                for (int i = 0; i < base.Params.Input.Count; i++)
                {
                    IGH_Param iGH_Param = base.Params.Input[i];
                    htmlWriter.AddOpenTag("tr");
                    htmlWriter.AddOpenTag("td", "valign=\"top\" class=\"medium\"");
                    htmlWriter.AddLine(iGH_Param.NickName);
                    htmlWriter.CloseLastTag();
                    htmlWriter.AddOpenTag("td", "class=\"mPale\"");
                    htmlWriter.AddLine(iGH_Param.Name + " <em>(" + iGH_Param.TypeName + ")</em>");
                    htmlWriter.AddClosedTag("br");
                    htmlWriter.AddLine(HtmlWriter.ParseTextIntoHtml(iGH_Param.Description));
                    htmlWriter.CloseLastTag();
                    htmlWriter.CloseLastTag();
                }
                htmlWriter.CloseLastTag();
            }
            else
            {
                htmlWriter.AddClosedTag("div", "class=\"medium\"", "None");
            }
            htmlWriter.AddLine().AddClosedTag("hr").AddLine();
            htmlWriter.AddClosedTag("div", "class=\"small\"", "Output parameters:");
            if (base.Params.Output.Count > 0)
            {
                htmlWriter.AddOpenTag("table", "border=\"0\" cellspacing=\"1\" cellpadding=\"2\"");
                for (int j = 0; j < base.Params.Output.Count; j++)
                {
                    IGH_Param iGH_Param2 = base.Params.Output[j];
                    htmlWriter.AddOpenTag("tr");
                    htmlWriter.AddOpenTag("td", "valign=\"top\" class=\"medium\"");
                    htmlWriter.AddLine(iGH_Param2.NickName);
                    htmlWriter.CloseLastTag();
                    htmlWriter.AddOpenTag("td", "class=\"mPale\"");
                    htmlWriter.AddLine(iGH_Param2.Name + " <em>(" + iGH_Param2.TypeName + ")</em>");
                    htmlWriter.AddClosedTag("br");
                    htmlWriter.AddLine(HtmlWriter.ParseTextIntoHtml(iGH_Param2.Description));
                    htmlWriter.CloseLastTag();
                    htmlWriter.CloseLastTag();
                }
                htmlWriter.CloseLastTag();
            }
            else
            {
                htmlWriter.AddClosedTag("div", "class=\"medium\"", "None");
            }
            htmlWriter.AddLine().AddClosedTag("hr").AddLine();
            htmlWriter.AddOpenTag("div", "class=\"mPale\"", htmlWriter.Generator);

            htmlWriter.AddClosedTag("a", "title=\"Visit the webpage of this command for supports.\" href=\"https://femdesign-api.discourse.group/\" target=\"_blank\"", "contact");
            htmlWriter.AddClosedTag("a", "title=\"Visit the webpage of this command for additional updated information, or reading and leaving comments.\" href=\"https://femdesign-api-docs.onstrusoft.com/\" target=\"_blank\"", "website");
            htmlWriter.CloseLastTag();
            return htmlWriter.ToString();
        }
    }
}