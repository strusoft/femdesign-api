// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Releases
{
    [System.Serializable]
    public partial class RigidityDataLibType3: LibraryBase
    {
        // choice rigidity_data_type1
        [XmlElement("rigidity", Order = 1)]
        public RigidityDataType3 Rigidity { get; set; }

        // choice rigidity_group_typ1
        // [XmlElement("rigidity_group", Order = 2)]

        /// <summary>
        /// Parameter less constructor for serialization
        /// </summary>
        public RigidityDataLibType3() { }

        /// <summary>
        /// Library type for <see cref="RigidityDataType3"/>.
        /// </summary>
        /// <param name="rigidity">The rigidity to store in library</param>
        /// <param name="name">Library name</param>
        public RigidityDataLibType3(RigidityDataType3 rigidity, string name)
        {
            Rigidity = rigidity;
            this.Name = name;
            this.EntityCreated();
        }
    }
}