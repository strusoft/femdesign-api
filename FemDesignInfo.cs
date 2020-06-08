using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace FemDesign.GH
{
    public class FemDesignInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "FemDesign";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return FemDesign.Properties.Resources.fdlogo_00101.ToBitmap();
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "FEM-Design 19 3D Structure API toolbox for Grasshopper | https://strusoft.com/ | Disclaimer: This package and related documentation is for illustrative and educational purposes and may not interact with FEM-Design in a reliable way depending on your version, installation and content of the files. Furthermore, StruSoft won´t guarantee full support of the package.";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("d0b2aac4-8e8e-485f-844f-9e69506b8c2e");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "StruSoft";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "support.femdesign@strusoft.com";
            }
        }
    }
}
