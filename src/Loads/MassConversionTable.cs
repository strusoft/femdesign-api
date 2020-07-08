using System.Xml.Serialization;
using System.Collections.Generic;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Loads
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class MassConversion
    {
        [XmlAttribute("factor")]
        public double _factor;
        [XmlIgnore]
        public double Factor
        {
            get
            {
                return this._factor;
            }
            set
            {
                this._factor = RestrictedDouble.NonNegMax_10(value);
            }
        }
        [XmlAttribute("load_case")]
        public System.Guid LoadCaseGuid { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MassConversion()
        {

        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="loadCaseGuid"></param>
        internal MassConversion(double factor, System.Guid loadCaseGuid)
        {
            this.Factor = factor;
            this.LoadCaseGuid = loadCaseGuid;
        }
    }

    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class MassConversionTable
    {
        [XmlElement("conversion", Order = 1)]
        public List<MassConversion> MassConversions = new List<MassConversion>();
        [XmlAttribute("last_change")]
        public string _lastChange;
        [XmlIgnore]
        internal System.DateTime LastChange
        {
            get
            {
                return System.DateTime.Parse(this._lastChange);
            }
            set
            {
                this._lastChange = value.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            }
        }

        [XmlAttribute("action")]
        public string Action { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private MassConversionTable()
        {

        }

        /// <summary>
        /// Invoke when an instance is created.
        /// 
        /// Creates a new guid, adds timestamp and changes action.
        /// </summary>
        internal void EntityCreated()
        {
            this.LastChange = System.DateTime.UtcNow;
            this.Action = "added";
        }

        /// <summary>
        /// Internal constructor from a list of massConversions.
        /// </summary>
        /// <param name="massConversions"></param>
        internal MassConversionTable(List<MassConversion> massConversions)
        {
            this.EntityCreated();
            this.MassConversions = massConversions;
        }

        /// <summary>
        /// Create a new mass conversion table.
        /// </summary>
        /// <param name="factors">Factors for each mass conversion.</param>
        /// <param name="loadCases">Load case for each mass conversion.</param>
        /// <returns></returns>
        public MassConversionTable(List<double> factors, List<LoadCase> loadCases)
        {
            if (factors.Count != loadCases.Count)
            {
                throw new System.ArgumentException($"List must have equal length. Length of factors is: {factors.Count} and length of loadCases is: {loadCases.Count}");
            }

            else if (factors == null || loadCases == null)
            {
                throw new System.ArgumentException("List can not be null");
            }

            else if (factors.Count == 0 || loadCases.Count == 0)
            {
                throw new System.ArgumentException("List must have atleast 1 item");
            }

            else
            {
                List<MassConversion> items = new List<MassConversion>();
                for (int idx = 0; idx < factors.Count; idx++)
                {
                    items.Add(new MassConversion(factors[idx], loadCases[idx].guid));
                }
                
                this.EntityCreated();
                this.MassConversions = items;
            }
        }

        #region dynamo
        /// <summary>
        /// Define a new mass conversion table.
        /// </summary>
        /// <param name="loadCases">LoadCase to include in MassConversionTable. Single LoadCase or list of LoadCases.</param>
        /// <param name="factors">Factor for mass conversion of each respective LoadCase. Single value or list of values.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static MassConversionTable Define(List<LoadCase> loadCases, List<double> factors)
        {
            return new MassConversionTable(factors, loadCases);
        }
        #endregion
    }
}