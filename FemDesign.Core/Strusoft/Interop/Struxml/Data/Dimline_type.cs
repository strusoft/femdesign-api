namespace StruSoft.Interop.StruXml.Data
{
    public partial class Dimline_type
    {  
        private System.DateTime last_changeField;

        private Modification_type actionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "last_change")]
        public System.DateTime Last_change
        {
            get
            {
                return this.last_changeField;
            }
            set
            {
                this.last_changeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "action")]
        public Modification_type Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

    }
}
