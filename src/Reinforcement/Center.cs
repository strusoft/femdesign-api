// https://strusoft.com/

using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Reinforcement
{
    /// <summary>
    /// center (child of shell_rf_params_type)
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class Center
    {
        [XmlAttribute("x")]
        public double x { get; set; } // double
        [XmlAttribute("y")]
        public double y { get; set; } // double
        [XmlAttribute("z")]
        public double z { get; set; } // double
        [XmlAttribute("polar_system")]
        public bool polarSystem { get; set; } // bool

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Center()
        {

        }

        /// <summary>
        /// Private constructor accessed by static methods.
        /// </summary>
        private Center(double x, double y, double z, bool polarSystem)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.polarSystem = polarSystem;
        }

        /// <summary>
        /// Create a Center element representing a Straight lay-out.
        /// </summary>
        /// <returns></returns>
        public static Center Straight()
        {
            return new Center(0,0,0, false);
        }
    }
}