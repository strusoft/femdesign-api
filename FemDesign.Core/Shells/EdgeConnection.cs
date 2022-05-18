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
    public partial class EdgeConnection: EdgeConnectionBase
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public bool Release { get; set; } = true;
        [XmlAttribute("name")]
        public string Name { get; set; } // identifier
        [XmlElement("rigidity")]
        public Releases.RigidityDataType3 Rigidity { get; set; } // rigidity_data_type2(3?)
        [XmlElement("predefined_rigidity")]
        public GuidListType _predefRigidityRef; // reference_type
        [XmlIgnore]
        public Releases.RigidityDataLibType3 _predefRigidity;
        [XmlIgnore]
        public Releases.RigidityDataLibType3 PredefRigidity
        {
            get
            {
                return this._predefRigidity;
            }
            set
            {
                this._predefRigidity = value;
                this._predefRigidityRef = new GuidListType(value.Guid);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private EdgeConnection()
        {

        }

        /// <summary>
        /// Private constructor.
        /// </summary>
        private EdgeConnection(Releases.RigidityDataType3 rigidity)
        {
            this.EntityCreated();
            this.MovingLocal = false;
            this.JoinedStartPoint = true;
            this.JoinedEndPoint = true;
            this.Rigidity = rigidity;
        }

        /// <summary>
        /// Copy properties from a EdgeConnection with a new name.
        /// </summary>
        internal static EdgeConnection CopyExisting(EdgeConnection shellEdgeConnection, string name)
        {
            // deep clone. downstreams objs contain changes made in this method, upstream objs will not.
            EdgeConnection ec = shellEdgeConnection.DeepClone();

            // downstream and uppstream objs will NOT share guid.
            ec.EntityCreated();

            //
            ec.Name = name;

            // return
            return ec;
        }

        /// <summary>
        /// EdgeConnection
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="motions">Motions.</param>
        /// <param name="rotations">Rotations.</param>
        /// <returns></returns>
        public EdgeConnection(Releases.Motions motions, Releases.Rotations rotations) : this(new Releases.RigidityDataType3(motions, rotations))
        {

        }

        /// <summary>
        /// EdgeConnection
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="motions">Motions.</param>
        /// <param name="motionsPlasticLimits">Motions plastic limit forces</param>
        /// <param name="rotations">Rotations.</param>
        /// <param name="rotationsPlasticLimits">Rotations plastic limit forces</param>
        public EdgeConnection(Releases.Motions motions, Releases.MotionsPlasticLimits motionsPlasticLimits, Releases.Rotations rotations, Releases.RotationsPlasticLimits rotationsPlasticLimits) : this(new Releases.RigidityDataType3(motions, motionsPlasticLimits, rotations, rotationsPlasticLimits))
        {

        }

        /// <summary>
        /// Create a default (rigid) EdgeConnection.
        /// </summary>
        /// <returns></returns>
        public static EdgeConnection GetDefault()
        {
            return EdgeConnection.GetRigid();
        }

        /// <summary>
        /// Create a hinged EdgeConnection.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        public static EdgeConnection GetHinged()
        {
            EdgeConnection _shellEdgeConnection = new EdgeConnection(Releases.RigidityDataType3.HingedLine());
            _shellEdgeConnection.Release = true;
            return _shellEdgeConnection;
        }
        
        /// <summary>
        /// Create a rigid EdgeConnection.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        public static EdgeConnection GetRigid()
        {
            EdgeConnection _shellEdgeConnection = new EdgeConnection(Releases.RigidityDataType3.RigidLine());
            _shellEdgeConnection.Release = false;
            return _shellEdgeConnection;
        }
    }
}