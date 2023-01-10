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
        public Dictionary<AnalysisModifier, StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record> _stiffnessModifiers { get; set; }


        public static Dictionary<AnalysisModifier, StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record> Default()
        {
            var stiffRecordDefault = new StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record(1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
            return SameAllCalculation(stiffRecordDefault);
        }

        public static Dictionary<AnalysisModifier, StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record> SameAllCalculation(StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record stiffRecord)
        {
            var defaultValues = new Dictionary<AnalysisModifier, StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record>();

            defaultValues.Add(AnalysisModifier.FirstOrderU, stiffRecord);

            defaultValues.Add(AnalysisModifier.FirstOrderSq, stiffRecord);
            defaultValues.Add(AnalysisModifier.FirstOrderSf, stiffRecord);
            defaultValues.Add(AnalysisModifier.FirstOrderSc, stiffRecord);

            defaultValues.Add(AnalysisModifier.SecondOrderU, stiffRecord);
            defaultValues.Add(AnalysisModifier.SecondOrderSq, stiffRecord);
            defaultValues.Add(AnalysisModifier.SecondOrderSf, stiffRecord);
            defaultValues.Add(AnalysisModifier.SecondOrderSc, stiffRecord);

            defaultValues.Add(AnalysisModifier.Stability, stiffRecord);
            defaultValues.Add(AnalysisModifier.Dynamic, stiffRecord);

            return defaultValues;
        }
    }

    public enum AnalysisModifier
    {
        [Parseable("SameForAllCalculation")]
        SameForAllCalculation,
        [Parseable("FirstOrderU")]
        FirstOrderU,
        [Parseable("FirstOrderSq")]
        FirstOrderSq,
        [Parseable("FirstOrderSf")]
        FirstOrderSf,
        [Parseable("FirstOrderSc")]
        FirstOrderSc,
        [Parseable("SecondOrderU")]
        SecondOrderU,
        [Parseable("SecondOrderSq")]
        SecondOrderSq,
        [Parseable("SecondOrderSf")]
        SecondOrderSf,
        [Parseable("SecondOrderSc")]
        SecondOrderSc,
        [Parseable("Stability")]
        Stability,
        [Parseable("Dynamic")]
        Dynamic
    }
}
