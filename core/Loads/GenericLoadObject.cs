// https://strusoft.com/
using System.Collections.Generic;

namespace FemDesign.Loads
{
    /// <summary>
    /// Object to contain load instances in GH when passing generic objects.
    /// </summary>
    [System.Serializable]
    public class GenericLoadObject
    {
        public PointLoad PointLoad { get; set; }
        public LineLoad LineLoad { get; set; }
        public LineStressLoad LineStressLoad { get; set; }
        public LineTemperatureLoad LineTemperatureLoad { get; set; }
        public SurfaceLoad SurfaceLoad { get; set; }
        public SurfaceTemperatureLoad SurfaceTemperatureLoad { get; set; }
        public PressureLoad PressureLoad { get; set; }
        public MassConversionTable MassConversionTable { get; set; }
        internal GenericLoadObject()
        {
            
        }
        public GenericLoadObject(PointLoad obj)
        {
            this.PointLoad = obj;
        }
        public GenericLoadObject(LineLoad obj)
        {
            this.LineLoad = obj;
        }

        public GenericLoadObject(LineStressLoad obj)
        {
            this.LineStressLoad = obj;
        }

        public GenericLoadObject(LineTemperatureLoad obj)
        {
            this.LineTemperatureLoad = obj;
        }
        public GenericLoadObject(SurfaceLoad obj)
        {
            this.SurfaceLoad = obj;
        }

        public GenericLoadObject(SurfaceTemperatureLoad obj)
        {
            this.SurfaceTemperatureLoad = obj;
        }
        public GenericLoadObject(PressureLoad obj)
        {
            this.PressureLoad = obj;
        }
        public GenericLoadObject(MassConversionTable obj)
        {
            this.MassConversionTable = obj;
        }
        public static List<object> ToObjectList(List<GenericLoadObject> objs)
        {
            List<object> list = new List<object>();
            foreach (GenericLoadObject obj in objs)
            {
                if (obj.PointLoad != null)
                {
                    list.Add(obj.PointLoad);
                }
                else if (obj.LineLoad != null)
                {
                    list.Add(obj.LineLoad);
                }
                else if (obj.LineTemperatureLoad != null)
                {
                    list.Add(obj.LineTemperatureLoad);
                }
                else if (obj.SurfaceLoad != null)
                {
                    list.Add(obj.SurfaceLoad);
                }
                else if (obj.SurfaceTemperatureLoad != null)
                {
                    list.Add(obj.SurfaceTemperatureLoad);
                }
                else if (obj.PressureLoad != null)
                {
                    list.Add(obj.PressureLoad);
                }                
                else if (obj.MassConversionTable != null)
                {
                    list.Add(obj.MassConversionTable);
                }
            }
            
            // return
            return list;
        }
    } 
}