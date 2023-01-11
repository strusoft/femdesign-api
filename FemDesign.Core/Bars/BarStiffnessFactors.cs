using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign.GenericClasses;
using System.Xml.Serialization;

namespace FemDesign.Bars
{
    public partial class BarStiffnessFactors
    {
        [XmlElement("factors", Order = 1)]
        public List<StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record> StiffnessModifiers
        {
            get
            {
                return this._stiffnessModifiers.Values.ToList();
            }
            set { this.StiffnessModifiers = value; }
        }

        [XmlIgnore]
        public Dictionary<StiffnessAnalysisType, StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record> _stiffnessModifiers { get; set; }


        public static Dictionary<StiffnessAnalysisType, StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record> Default()
        {
            var stiffRecordDefault = new StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record(1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
            return SameAllCalculation(stiffRecordDefault);
        }

        public static Dictionary<StiffnessAnalysisType, StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record> SameAllCalculation(StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record stiffRecord)
        {
            var defaultValues = new Dictionary<StiffnessAnalysisType, StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record>();

            defaultValues.Add(StiffnessAnalysisType.FirstOrderU, stiffRecord);

            defaultValues.Add(StiffnessAnalysisType.FirstOrderSq, stiffRecord);
            defaultValues.Add(StiffnessAnalysisType.FirstOrderSf, stiffRecord);
            defaultValues.Add(StiffnessAnalysisType.FirstOrderSc, stiffRecord);

            defaultValues.Add(StiffnessAnalysisType.SecondOrderU, stiffRecord);
            defaultValues.Add(StiffnessAnalysisType.SecondOrderSq, stiffRecord);
            defaultValues.Add(StiffnessAnalysisType.SecondOrderSf, stiffRecord);
            defaultValues.Add(StiffnessAnalysisType.SecondOrderSc, stiffRecord);

            defaultValues.Add(StiffnessAnalysisType.Stability, stiffRecord);
            defaultValues.Add(StiffnessAnalysisType.Dynamic, stiffRecord);

            return defaultValues;
        }
    }
}