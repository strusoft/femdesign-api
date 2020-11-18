using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.ModellingTools
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class StiffnessMatrix2Type
    {
        [XmlAttribute("xz")]
        public double _xz;
        [XmlIgnore]
        public double XZ
        {
            get
            {
                return this._xz;
            }
            set
            {
                this._xz = RestrictedDouble.NonNegMax_1e20(value);
            }
        }
        
        [XmlAttribute("yz")]
        public double _yz;
        [XmlIgnore]
        public double YZ
        {
            get
            {
                return this._yz;
            }
            set
            {
                this._yz = RestrictedDouble.NonNegMax_1e20(value);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private StiffnessMatrix2Type()
        {

        }

        /// <summary>
        /// Construct a stiffness matrix 2 type
        /// </summary>
        /// <param name="xz"></param>
        /// <param name="yz"></param>
        public StiffnessMatrix2Type(double xz, double yz)
        {
            this.XZ = xz;
            this.YZ = yz;
        }

        #region dynamo
        /// <summary>
        /// Define a shear stiffness matrix H
        /// </summary>
        /// <param name="xz">XZ component in kN/m</param>
        /// <param name="yz">YZ component in kN/m</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static StiffnessMatrix2Type Define(double xz = 10000, double yz = 10000)
        {
            return new StiffnessMatrix2Type(xz, yz);
        }
        #endregion
    }
}