using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StruSoft.Interop.StruXml.Data
{
    public partial class Truss_chr_type
    {
        private Truss_chr_type() { }

        public Truss_chr_type(Truss_behaviour_type compression, Truss_behaviour_type tension)
        {
            this.Compression = compression;
            this.Tension = tension;
        }
        public static Truss_chr_type Elastic()
        {
            var compression = Truss_behaviour_type.Elastic();
            var tension = Truss_behaviour_type.Elastic();

            var trussBehaviour = new Truss_chr_type(compression, tension);

            return trussBehaviour;
        }

    }

    public partial class Simple_truss_chr_type
    {
        private Simple_truss_chr_type() { }

        public Simple_truss_chr_type(Simple_truss_behaviour_type compression, Simple_truss_behaviour_type tension)
        {
            this.Compression = compression;
            this.Tension = tension;
        }
        public static Simple_truss_chr_type Elastic()
        {
            var compression = Simple_truss_behaviour_type.Elastic();
            var tension = Simple_truss_behaviour_type.Elastic();

            var trussBehaviour = new Simple_truss_chr_type(compression, tension);

            return trussBehaviour;
        }

    }


    public partial class Truss_behaviour_type
    {
        private Truss_behaviour_type() { }

        public Truss_behaviour_type(object item, ItemChoiceType behaviour)
        {
            this.Item = item;
            this.ItemElementName = behaviour;
        }

        public static Truss_behaviour_type Elastic()
        {
            var elastic = new Truss_behaviour_type(new Empty_type(), ItemChoiceType.Elastic);
            return elastic;
        }


        public static Truss_behaviour_type Plastic(List<double> values)
        {
            if (values.Count == 1)
            {
                var plastic = new Truss_behaviour_type(new Truss_capacity_type(values[0]), ItemChoiceType.Plastic);
                return plastic;
            }
            else
            {
                var plastic = new Truss_behaviour_type(new Truss_capacity_type(values), ItemChoiceType.Plastic);
                return plastic;
            }
        }

        public static Truss_behaviour_type Plastic(double value)
        {
            var plastic = new Truss_behaviour_type(new Truss_capacity_type(value), ItemChoiceType.Plastic);
            return plastic;
        }

        public static Truss_behaviour_type Brittle(List<double> values)
        {
            if(values.Count == 1)
            {
                var brittle = new Truss_behaviour_type(new Truss_capacity_type(values[0]), ItemChoiceType.Brittle);
                return brittle;
            }
            else
            {
                var brittle = new Truss_behaviour_type(new Truss_capacity_type(values), ItemChoiceType.Brittle);
                return brittle;
            }
        }

        public static Truss_behaviour_type Brittle(double value)
        {
            var brittle = new Truss_behaviour_type(new Truss_capacity_type(value), ItemChoiceType.Brittle);
            return brittle;
        }
    }

    public partial class Simple_truss_behaviour_type
    {
        private Simple_truss_behaviour_type() { }

        public Simple_truss_behaviour_type(object item, ItemChoiceType1 behaviour)
        {
            this.Item = item;
            this.ItemElementName = behaviour;
        }

        public static Simple_truss_behaviour_type Elastic()
        {
            var elastic = new Simple_truss_behaviour_type(new Empty_type(), ItemChoiceType1.Elastic);
            return elastic;
        }

        public static Simple_truss_behaviour_type Plastic(double value)
        {
            var plastic = new Simple_truss_behaviour_type(new Simple_truss_capacity_type(value), ItemChoiceType1.Plastic);
            return plastic;
        }

        public static Simple_truss_behaviour_type Brittle(double value)
        {
            var brittle = new Simple_truss_behaviour_type(new Simple_truss_capacity_type(value), ItemChoiceType1.Brittle);
            return brittle;
        }
    }

    public partial class Truss_capacity_type
    {
        private Truss_capacity_type() { }

        public Truss_capacity_type(List<double> values)
        {
            if (values.Count != 9)
            {
                throw new Exception("List must have a length of 9");
            }

            var trussLimits = new List<Truss_limit_type>();

            foreach(var value in values)
            {
                trussLimits.Add(new Truss_limit_type(value));
            }

            this.Limit_force = trussLimits;
        }

        public Truss_capacity_type(double value)
        {
            var trussLimits = new List<Truss_limit_type>();

            for(int i = 0; i < 9; i++)
            {
                trussLimits.Add(new Truss_limit_type(value));
            }

            this.Limit_force = trussLimits;
        }
    }

    public partial class Simple_truss_capacity_type
    {
        private Simple_truss_capacity_type() { }
        public Simple_truss_capacity_type(double value)
        {
            this.Limit_force = new Truss_limit_type(value);
        }
    }

    public partial class Truss_limit_type
    {
        private Truss_limit_type() { }

        public Truss_limit_type(double x)
        {
            this.Value = FemDesign.RestrictedDouble.NonNegMax_1e20(x);
        }
    }
}