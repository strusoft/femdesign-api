
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Sections
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Section: EntityBase
    {
        #region dynamo

        /// <summary>
        /// Get a Section from a SectionDatabase by name.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="sectionDatabase">SectionDatabase.</param>
        /// <param name="sectionName">Name of Section.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Section GetSectionByName(SectionDatabase sectionDatabase, string sectionName)
        {
            return sectionDatabase.SectionByName(sectionName);
        }

        /// <summary>
        /// Define a new custom section.
        /// </summary>
        /// <param name="surfaces">Item or list of surfaces of section. Surfaces must lie in the XY-plane at z=0.</param>
        /// <param name="name">Name of section</param>
        /// <param name="materialType">Material type. Choice: SteelRolled/SteelColdWorked/SteelWelded/Concrete/Timber</param>
        /// <param name="groupName">Name of section group</param>
        /// <param name="typeName">Name of section type</param>
        /// <param name="sizeName">Nem of section size</param>
        [IsVisibleInDynamoLibrary(true)]
        public static Section Define(List<Autodesk.DesignScript.Geometry.Surface> surfaces, string name, string materialType, string groupName, string typeName, string sizeName)
        {
            // check if string input is null
            if (name == null || materialType == null || groupName == null || typeName == null || sizeName == null)
            {
                throw new System.ArgumentException($"Some input is null. name: {name}, materialType: {materialType}, groupName: {groupName}, typeName: {typeName}, sizeName: {sizeName}");
            }

            // convert geometry
            List<Geometry.Region> regions = new List<Geometry.Region>();
            foreach (Autodesk.DesignScript.Geometry.Surface surface in surfaces)
            {
                regions.Add(Geometry.Region.FromDynamo(surface));
            }

            // create regions group
            Geometry.RegionGroup regionGroup = new Geometry.RegionGroup(regions);

            // get mat type
            FemDesign.Materials.MaterialTypeEnum matTypeEnum = (FemDesign.Materials.MaterialTypeEnum)Enum.Parse(typeof(FemDesign.Materials.MaterialTypeEnum), materialType);

            // create section
            Sections.Section section = new Sections.Section(regionGroup, name, "custom", matTypeEnum, groupName, typeName, sizeName);

            // return
            return section;
        }
        
        #endregion
    }
}