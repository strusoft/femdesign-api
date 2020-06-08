// https://strusoft.com/

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Shells
{
    /// <summary>
    /// Not a strusoft.xsd type.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class ShellOrthotropy
    {
        private double _orthoAlfa; // two_quadrants.
        public double orthoAlfa
        {
            get {return this._orthoAlfa;}
            set {this._orthoAlfa = RestrictedDouble.TwoQuadrantsRadians(value);}
        }
        private double _orthoRatio; // orthotropy_type
        public double orthoRatio
        {
            get {return this._orthoRatio;}
            set {this._orthoRatio = RestrictedDouble.NonNegMax_1(value);}
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private ShellOrthotropy()
        {

        }
        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="orthoAlfa">Degrees</param>
        /// <param name="orthoRatio">Double</param>
        internal ShellOrthotropy(double orthoAlfa, double orthoRatio)
        {
            this.orthoAlfa = FemDesign.Geometry.Degree.ToRadians(orthoAlfa);
            this.orthoRatio = orthoRatio;
        }
        /// <summary>
        /// Create a definition for ShellOrthotropy.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="orthoAlfa">Alpha in degrees.</param>
        /// <param name="orthoRatio">E2/E1</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static ShellOrthotropy Create(double orthoAlfa = 0, double orthoRatio = 1)
        {
            return new ShellOrthotropy(orthoAlfa, orthoRatio);
        }
        /// <summary>
        /// Create a default definition for ShellOrthotropy.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static ShellOrthotropy Default()
        {
            return new ShellOrthotropy(0, 1);
        }
    }
}