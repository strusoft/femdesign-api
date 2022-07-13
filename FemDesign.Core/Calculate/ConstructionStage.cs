// https://strusoft.com/
using System;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    /// <summary>    
    /// fdscript.xsd
    /// ANALSTAGE
    /// </summary>
    public partial class ConstructionStage
    {
        [XmlAttribute("ghost")]
        public int _ghost { get; set; } // bool // int(?)

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private ConstructionStage()
        {
            
        }

        private ConstructionStage(bool ghost = false)
        {
            this._ghost = Convert.ToInt32(ghost);
        }

        /// <summary>
        /// Default Construction stages method (incremental).
        /// </summary>
        public static ConstructionStage Default()
        {
            return new ConstructionStage(false);
        }
        /// <summary>
        /// Construction stages method.
        /// </summary>
        /// <param name="ghost">Ghost construction method. True/false. If false incremental method is used.</param>
        public static ConstructionStage Define(bool ghost = false)
        {
            return new ConstructionStage(ghost);
        }

        /// <summary>
        /// Incremental construction stage method.
        /// </summary>
        /// <returns></returns>
        public static ConstructionStage Ghost()
        {
            return new ConstructionStage(true);
        }

        /// <summary>
        /// Ghost construction stage method.
        /// </summary>
        /// <returns></returns>
        public static ConstructionStage Tracking()
        {
            return new ConstructionStage(false);
        }
    }
}