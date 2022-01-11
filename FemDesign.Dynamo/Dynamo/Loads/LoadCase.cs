
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Loads
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class LoadCase: EntityBase
    {
        /// <summary>
        /// Creates a load case.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="name">Name of LoadCase.</param>
        /// <param name="type">LoadCase type. "static"/"dead_load"/"soil_dead_load"/"shrinkage"/"prestressing"/"fire"/"seis_sxp"/"seis_sxm"/"seis_syp"/"seis_sym"</param>
        /// <param name="durationClass">LoadCase duration class. "permanent"/"long-term"/"medium-term"/"short-term"/"instantaneous"</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static LoadCase CreateLoadCase(string name, string type = "static", string durationClass = "permanent")
        {

            LoadCaseType _type = FemDesign.GenericClasses.EnumParser.Parse<LoadCaseType>(type);
            LoadCaseDuration _durationClass = FemDesign.GenericClasses.EnumParser.Parse<LoadCaseDuration>(durationClass);
            LoadCase loadCase = new LoadCase(name, _type, _durationClass);
            return loadCase;
        }

        /// <summary>
        /// Returns a LoadCase from a list of LoadCases by name. The first LoadCase with a matching name will be returned.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="loadCases">List of LoadCase.</param>
        /// <param name="name">Name of LoadCase.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static LoadCase GetLoadCaseFromListByName(List<LoadCase> loadCases, string name)
        {
            foreach (LoadCase _loadCase in loadCases)
            {
                if (_loadCase.Name == name)
                {
                    return _loadCase;
                }
            }
            return null;
        }

    }
}