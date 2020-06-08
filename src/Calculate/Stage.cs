// https://strusoft.com/
using System;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    /// <summary>    
    /// fdscript.xsd
    /// ANALSTAGE
    /// </summary>
    public class Stage
    {
        [XmlAttribute("ghost")]
        public int ghost { get; set; } // bool // int(?)

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Stage()
        {
            
        }

        private Stage(bool ghost = false)
        {
            //
            this.ghost = Convert.ToInt32(ghost);
        }

        /// <summary>
        /// Default Construction stages method (incremental).
        /// </summary>
        public static Stage Default()
        {
            return new Stage(false);
        }
        /// <summary>
        /// Construction stages method.
        /// </summary>
        /// <param name="method">Ghost construction method. True/false. If false incremental method is used.</param>
        public static Stage Define(bool ghost = false)
        {
            return new Stage(ghost);
        }
    }
}