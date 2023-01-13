using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FemDesign;

namespace FemDesign.Shells
{
    public partial class SlabStiffnessRecord
    {

        private double bending_1_1Field;

        private double bending_2_2Field;

        private double bending_1_2Field;

        private double membran_1_1Field;

        private double membran_2_2Field;

        private double membran_1_2Field;

        private double shear_1_3Field;

        private double shear_2_3Field;

        /// <remarks/>
        [XmlAttribute("bending_1_1")]
        public double Bending_1_1
        {
            get
            {
                return this.bending_1_1Field;
            }
            set
            {
                this.bending_1_1Field = value;
            }
        }

        /// <remarks/>
        [XmlAttribute("bending_2_2")]
        public double Bending_2_2
        {
            get
            {
                return this.bending_2_2Field;
            }
            set
            {
                this.bending_2_2Field = value;
            }
        }

        /// <remarks/>
        [XmlAttribute("bending_1_2")]
        public double Bending_1_2
        {
            get
            {
                return this.bending_1_2Field;
            }
            set
            {
                this.bending_1_2Field = value;
            }
        }

        /// <remarks/>
        [XmlAttribute("membran_1_1")]
        public double Membran_1_1
        {
            get
            {
                return this.membran_1_1Field;
            }
            set
            {
                this.membran_1_1Field = value;
            }
        }

        /// <remarks/>
        [XmlAttribute("membran_2_2")]
        public double Membran_2_2
        {
            get
            {
                return this.membran_2_2Field;
            }
            set
            {
                this.membran_2_2Field = value;
            }
        }

        /// <remarks/>
        [XmlAttribute("membran_1_2")]
        public double Membran_1_2
        {
            get
            {
                return this.membran_1_2Field;
            }
            set
            {
                this.membran_1_2Field = value;
            }
        }

        /// <remarks/>
        [XmlAttribute("shear_1_3")]
        public double Shear_1_3
        {
            get
            {
                return this.shear_1_3Field;
            }
            set
            {
                this.shear_1_3Field = value;
            }
        }

        /// <remarks/>
        [XmlAttribute("shear_2_3")]
        public double Shear_2_3
        {
            get
            {
                return this.shear_2_3Field;
            }
            set
            {
                this.shear_2_3Field = value;
            }
        }

        public SlabStiffnessRecord(double bending11, double bending22, double bending12, double membrane11, double membrane22, double membrane12, double shear13, double shear23)
        {
            this.Bending_1_1 = RestrictedDouble.NonNegMax_10(bending11);
            this.Bending_2_2 = RestrictedDouble.NonNegMax_10(bending22);
            this.Bending_1_2 = RestrictedDouble.NonNegMax_10(bending12);
            this.Membran_1_1 = RestrictedDouble.NonNegMax_10(membrane11);
            this.Membran_2_2 = RestrictedDouble.NonNegMax_10(membrane22);
            this.Membran_1_2 = RestrictedDouble.NonNegMax_10(membrane12);
            this.Shear_1_3 = RestrictedDouble.NonNegMax_10(shear13);
            this.Shear_2_3 = RestrictedDouble.NonNegMax_10(shear23);
        }

        public SlabStiffnessRecord()
        {
        }
    }
}
