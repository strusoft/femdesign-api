// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign
{
    /// <summary>
    /// cover_referencelist_type
    /// </summary>
    [System.Serializable]
    public partial class CoverReferenceList
    {
        [XmlElement("ref")]
        public List<GuidListType> RefGuid = new List<GuidListType>(); // guidtype

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
                    refList.RefGuid.Add(new GuidListType(((Bars.Bar)elem).BarPart.Guid));
                }
                else if (elem.GetType() == typeof(Shells.Slab))
                {
                    refList.RefGuid.Add(new GuidListType(((Shells.Slab)elem).SlabPart.Guid));
                }
                else
                {
                    throw new System.ArgumentException("Type of supporting structure is not supported. Can be bar or slab.");
                }
            }
            return refList;
        }

        internal static CoverReferenceList FromObjectList(List<FemDesign.GenericClasses.IStructureElement> objs)
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
                    refList.RefGuid.Add(new GuidListType(((Bars.Bar)elem).BarPart.Guid));
                }
                else if (elem.GetType() == typeof(Shells.Slab))
                {
                    refList.RefGuid.Add(new GuidListType(((Shells.Slab)elem).SlabPart.Guid));
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