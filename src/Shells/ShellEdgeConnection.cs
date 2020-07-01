// https://strusoft.com/

using System.Globalization;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Shells
{
    /// <summary>
    /// ec_type
    /// </summary>
    [System.Serializable]
    public class ShellEdgeConnection: EdgeConnectionBase
    {
        [XmlIgnore]
        public bool release { get; set; }
        [XmlAttribute("name")]
        public string name { get; set; } // identifier
        [XmlElement("rigidity")]
        public Releases.RigidityDataType3 rigidity { get; set; } // rigidity_data_type2(3?)
        [XmlElement("predefined_rigidity")]
        public GuidListType predefinedRigidity { get; set; } // reference_type

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private ShellEdgeConnection()
        {

        }

        /// <summary>
        /// Private constructor.
        /// </summary>
        private ShellEdgeConnection(Releases.RigidityDataType3 rigidity)
        {
            this.EntityCreated();
            this.movingLocal = true;
            this.joinedStartPoint = true;
            this.joinedEndPoint = true;
            this.rigidity = rigidity;
        }

        /// <summary>
        /// Copy properties from a ShellEdgeConnection with a new name.
        /// </summary>
        internal static ShellEdgeConnection CopyExisting(ShellEdgeConnection shellEdgeConnection, string name)
        {
            // deep clone. downstreams objs contain changes made in this method, upstream objs will not.
            ShellEdgeConnection ec = shellEdgeConnection.DeepClone();

            // downstream and uppstream objs will NOT share guid.
            ec.EntityCreated();

            //
            ec.name = name;

            // return
            return ec;
        }

        /// <summary>
        /// Define a new ShellEdgeConnection
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="motions"></param>
        /// <param name="rotations"></param>
        /// <returns></returns>
        public static ShellEdgeConnection Define(Releases.Motions motions, Releases.Rotations rotations)
        {
            ShellEdgeConnection _shellEdgeConnection = new ShellEdgeConnection(Releases.RigidityDataType3.Define(motions, rotations));
            _shellEdgeConnection.release = true;
            return _shellEdgeConnection;
        }

        /// <summary>
        /// Create a default (rigid) ShellEdgeConnection.
        /// </summary>
        /// <returns></returns>
        public static ShellEdgeConnection Default()
        {
            return ShellEdgeConnection.Rigid();
        }

        /// <summary>
        /// Create a hinged ShellEdgeConnection.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        public static ShellEdgeConnection Hinged()
        {
            ShellEdgeConnection _shellEdgeConnection = new ShellEdgeConnection(Releases.RigidityDataType3.HingedLine());
            _shellEdgeConnection.release = true;
            return _shellEdgeConnection;
        }
        
        /// <summary>
        /// Create a rigid ShellEdgeConnection.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        public static ShellEdgeConnection Rigid()
        {
            ShellEdgeConnection _shellEdgeConnection = new ShellEdgeConnection(Releases.RigidityDataType3.RigidLine());
            _shellEdgeConnection.release = false;
            return _shellEdgeConnection;
        }
    }
}