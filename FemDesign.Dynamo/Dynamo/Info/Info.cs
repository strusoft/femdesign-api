using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign
{
    [IsVisibleInDynamoLibrary(false)]
    public class Info
    {
        #region dynamo
        /// <summary>
        /// Create a FEM Design info component
        /// </summary>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new string[] { "Version", "Support", "Wiki", "Github" })]
        public static Dictionary<string, object> About()
        {
            //IF WE WANT TO MAKE THIS LOOK NICER AT SOME POINT, WE COULD BASE THE COMPONENT ON "NODEMODEL"
            //https://developer.dynamobim.org/03-Development-Options/3-5-nodemodel-derived-nodes.html


            //API version number               
            AssemblyName assembly = Assembly.GetExecutingAssembly().GetName();
            string assemblyVersion = assembly.Version.ToString();
            
            string version = assemblyVersion;
            string support = "https://strusoft.freshdesk.com/";
            string wiki = "https://wiki.fem-design.strusoft.com/";
            string github = "https://github.com/strusoft/femdesign-api";

            return new Dictionary<string, object>
            {
                {"Version", version},
                {"Support", support},
                {"Wiki", wiki},
                {"Github", github}             
            };


        }
        #endregion
    }
}
