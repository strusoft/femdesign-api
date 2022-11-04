// https://strusoft.com/
using System;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    /// <summary>    
    /// fdscript.xsd
    /// ANALSTAGE
    /// </summary>
    public partial class Stage
    {
        [XmlAttribute("ghost")]
        public int _ghost { get; set; } // bool // int(?)

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Stage()
        {
            
        }

        public Stage(bool ghost = false)
        {
            this._ghost = Convert.ToInt32(ghost);
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
        /// <param name="ghost">Ghost construction method. True/false. If false incremental method is used.</param>
        public static Stage Define(bool ghost = false)
        {
            return new Stage(ghost);
        }

        /// <summary>
        /// Incremental construction stage method.
        /// </summary>
        /// <returns></returns>
        public static Stage Ghost()
        {
            return new Stage(true);
        }

        /// <summary>
        /// Ghost construction stage method.
        /// </summary>
        /// <returns></returns>
        public static Stage Tracking()
        {
            return new Stage(false);
        }
    }
}