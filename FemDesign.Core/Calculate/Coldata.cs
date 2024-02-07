using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    [System.Serializable]
    public class Coldata
    {
        [XmlElement("num")]
        public int Num { get; set; }

        [XmlElement("flags")]
        public int Flags { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Coldata()
        {
        }


        public Coldata(int num, int flags)
        {
            this.Num = num;
            this.Flags = flags;
        }


        public static List<Coldata> Default()
        {
            List<Coldata> coldata = new List<Coldata>();
            for (int i = 0; i < 61; i++)
            {
                coldata.Add(new Coldata(i, 0));
            }
            return coldata;
        }
    }
}