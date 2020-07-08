// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign
{
    /// <summary>
    /// cover_referencelist_type
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class CoverReferenceList
    {
        [XmlElement("ref")]
        public List<GuidListType> refGuid = new List<GuidListType>(); // guidtype

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CoverReferenceList()
        {
            
        }

        /// <summary>
        /// Get guid from objects (only bars and slabs supported) in list.
        /// </summary>
        /// <param name="objs">Bars and/or Slabs</param>
        internal static CoverReferenceList FromObjectList(List<object> objs)
        {
            if (objs == null)
            {
                return null;
            }

            if (objs.Count == 0)
            {
                return null;
            }

            CoverReferenceList refList = new CoverReferenceList();
            foreach (object elem in objs)
            {
                if (elem.GetType() == typeof(Bars.Bar))
                {
                    refList.refGuid.Add(new GuidListType(((Bars.Bar)elem).barPart.guid));
                }
                else if (elem.GetType() == typeof(Shells.Slab))
                {
                    refList.refGuid.Add(new GuidListType(((Shells.Slab)elem).slabPart.guid));
                }
                else
                {
                    throw new System.ArgumentException("Type of supporting structure is not supported. Can be bar or slab.");
                }
            }
            return refList;
        }
    }
}