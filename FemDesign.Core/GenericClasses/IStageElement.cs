using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace FemDesign.GenericClasses
{
    public interface IStageElement
    {
        //[XmlAttribute("stage")]
        int StageId { get; set; }
    }
}
