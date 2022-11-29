// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using System.Linq;
using StruSoft.Interop.StruXml.Data;

namespace FemDesign.Loads
{
    /// <summary>
    /// loads
    /// </summary>
    [System.Serializable]
    public partial class Loads
    {
        // dummy elements are needed to deserialize an .struxml model correctly as order of elements is needed.
        // if dummy elements are not used for undefined types deserialization will not work properly
        // when serializing these dummy elements must be nulled. 
        [XmlElement("point_load", Order = 1)]
        public List<PointLoad> PointLoads = new List<PointLoad>(); // point_load_type
        [XmlElement("line_load", Order = 2)]
        public List<LineLoad> LineLoads = new List<LineLoad>(); // line_load_type
        [XmlElement("pressure_load", Order = 3)]
        public List<PressureLoad> PressureLoads = new List<PressureLoad>(); // pressure_load_type
        [XmlElement("surface_load", Order = 4)]
        public List<SurfaceLoad> SurfaceLoads = new List<SurfaceLoad>(); // surface_load_type
        [XmlElement("line_temperature_variation_load", Order = 5)]
        public List<LineTemperatureLoad> LineTemperatureLoads = new List<LineTemperatureLoad>(); // line_temperature_load_type
        [XmlElement("surface_temperature_variation_load", Order = 6)]
        public List<SurfaceTemperatureLoad> SurfaceTemperatureLoads = new List<SurfaceTemperatureLoad>(); // surface_temperature_variation_load
        [XmlElement("line_stress_load", Order = 7)]
        public List<LineStressLoad> LineStressLoads = new List<LineStressLoad>(); // line_stress_load
        [XmlElement("surface_stress_load", Order = 8)]
        public List<DummyXmlObject> SurfaceStressLoads {get {return null;} set {value = null;}} // surface_stress_load
        [XmlElement("point_support_motion_load", Order = 9)]
        public List<DummyXmlObject> PointSupportMotionLoads {get {return null;} set {value = null;}} // point_support_motion_load_type
        [XmlElement("line_support_motion_load", Order = 10)]
        public List<DummyXmlObject> LineSupportMotionLoads {get {return null;} set {value = null;}} // line_support_motion_load_type
        
        [XmlElement("surface_support_motion_load", Order = 11)]
        public List<DummyXmlObject> SurfaceSupportMotionLoads {get {return null;} set {value = null;}} // surface_support_motion_load_type
        
        [XmlElement("mass", Order = 12)]
        public List<DummyXmlObject> Masses {get {return null;} set {value = null;}} // mass_point_type
        
        [XmlElement("load_case_mass_conversion_table", Order = 13)]
        public MassConversionTable LoadCaseMassConversionTable {get; set;} // mass_conversion_type
        
        [XmlElement("seismic_load", Order = 14)]
        public List<DummyXmlObject> SeismicLoads {get {return null;} set {value = null;}} // seismic_load_type

        [XmlElement("footfall_analysis_data", Order = 15)]
        public List<Footfall> FootfallAnalysisData = new List<Footfall>(); // footfall_type
        
        [XmlElement("moving_load", Order = 16)]
        public List<Moving_load_type> MovingLoads { get; set; } // moving_load_type
        
        [XmlElement("load_case", Order = 17)]
        public List<LoadCase> LoadCases = new List<LoadCase>(); // load_case_type
        
        [XmlElement("load_combination", Order = 18)]
        public List<LoadCombination> LoadCombinations = new List<LoadCombination>(); // load_combination_type

        [XmlElement("load_group_table", Order = 19)]
        public LoadGroupTable LoadGroupTable { get; set; } // load_group_table_type

        
        /// <summary>
        /// Get PointLoad, LineLoad, PressureLoad and SurfaceLoads from Loads.
        /// </summary>
        public List<FemDesign.GenericClasses.ILoadElement> GetLoads()
        {
            var objs = new List<FemDesign.GenericClasses.ILoadElement>();
            objs.AddRange(this.PointLoads);
            objs.AddRange(this.LineLoads);
            objs.AddRange(this.LineStressLoads);
            objs.AddRange(this.LineTemperatureLoads);
            objs.AddRange(this.PressureLoads);
            objs.AddRange(this.SurfaceLoads);
            objs.AddRange(this.SurfaceTemperatureLoads);
            objs.AddRange(this.FootfallAnalysisData);
            return objs;
        }

        /// <summary>
        /// Gets the <see cref="ModelGeneralLoadGroup">ModelGeneralLoadGroup</see> objects of the LoadGroupTable.
        /// </summary>
        /// <returns>List of <see cref="ModelGeneralLoadGroup">ModelGeneralLoadGroup</see> objects</returns>
        public List<ModelGeneralLoadGroup> GetLoadGroups()
        {
            List<ModelGeneralLoadGroup> loadGroups = new List<ModelGeneralLoadGroup>();

            if (LoadGroupTable == null) return loadGroups;

            foreach (ModelGeneralLoadGroup generalLoadGroup in LoadGroupTable.GeneralLoadGroups)
                loadGroups.Add(generalLoadGroup);
            return loadGroups;
 
        }    
    }
}