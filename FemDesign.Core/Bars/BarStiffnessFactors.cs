using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace FemDesign.Bars
{
    public partial class BarStiffnessFactors
    {
        //[XmlElement("stiffness_modifiers", Order = 1)]
        //public List<StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record> Stiffness_modifiers
        //{
        //    get
        //    {
        //        if (_stiffness_modifiers == null)
        //            return null;
        //        else if (_stiffness_modifiers.Count == 0)
        //            return null;
        //        else
        //            return _stiffness_modifiers;
        //    }
        //    set
        //    {
        //        if (value == null)
        //            _stiffness_modifiers = null;
        //        else if (value.Count == 0)
        //            _stiffness_modifiers = null;
        //        else
        //            _stiffness_modifiers = value;
        //    }
        //}

        //[XmlIgnore]
        //public List<StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record> _stiffness_modifiers;


        [XmlElement("factors", Order = 1)]
        public List<StruSoft.Interop.StruXml.Data.Bar_stiffness_factor_record> StiffnessModifiers { get; set; }

    }
}
