using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// cmdprojdescr
    /// </summary>
    [XmlRoot("cmdprojdescr")]
    [System.Serializable]
    public class CmdProjDescr : CmdCommand
    {
        [XmlAttribute("command")]
        public string Command = "$ MODULECOM PROJDESCR";

        [XmlAttribute("read")]
        public int _read = 0;

        [XmlIgnore]
        public bool Read
        {
            get
            {
                return Convert.ToBoolean(this._read);
            }
            set
            {
                this._read = Convert.ToInt32(value);
            }
        }

        [XmlAttribute("reset")]
        public int _reset = 0;

        [XmlIgnore]
        public bool Reset
        {
            get
            {
                return Convert.ToBoolean(this._reset);
            }
            set
            {
                this._reset = Convert.ToInt32(value);
            }
        }

        [XmlAttribute("szProject")]
        public string Project { get; set; }

        [XmlAttribute("szDescription")]
        public string Description { get; set; }

        [XmlAttribute("szDesigner")]
        public string Designer { get; set; }

        [XmlAttribute("szSignature")]
        public string Signature { get; set; }

        [XmlAttribute("szComment")]
        public string Comment { get; set; }

        [XmlElement("item")]
        public List<UserDefinedData> Items { get; set; }


        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdProjDescr()
        {
        }
        public CmdProjDescr(string project, string description, string designer, string signature, string comment)
        {
            this.Project = project;
            this.Description = description;
            this.Designer = designer;
            this.Signature = signature;
            this.Comment = comment;
        }

        public CmdProjDescr(string project, string description, string designer, string signature, string comment, List<UserDefinedData> items) : this(project, description, designer, signature, comment)
        {
            this.Items = items;
        }

        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdProjDescr>(this);
        }

    }

    public class UserDefinedData
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("txt")]
        public string Txt { get; set; }

        private UserDefinedData()
        {
        }

        public UserDefinedData(string id, string txt)
        {
            this.Id = id;
            this.Txt = txt;
        }
    }
}