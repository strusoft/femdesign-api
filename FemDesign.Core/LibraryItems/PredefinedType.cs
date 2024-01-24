// https://strusoft.com/

using FemDesign.Materials;
using StruSoft.Interop.StruXml.Data;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;


namespace FemDesign.LibraryItems
{
    [System.Serializable]
    public partial class PointConnectionTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType2> PredefinedTypes { get; set; }
    }

    [System.Serializable]
    public partial class PointSupportGroupTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType2> PredefinedTypes { get; set; }
    }

    [System.Serializable]
    public partial class LineConnectionTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType3> PredefinedTypes { get; set; }
    }

    [System.Serializable]
    public partial class LineSupportGroupTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType2> PredefinedTypes { get; set; }
    }

    [System.Serializable]
    public partial class SurfaceConnectionTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType1> PredefinedTypes { get; set; }
    }

    [System.Serializable]
    public partial class SurfaceSupportTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType1> PredefinedTypes { get; set; }
    }


    [System.Serializable]
    public partial class VehicleTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<StruSoft.Interop.StruXml.Data.Vehicle_lib_type> PredefinedTypes { get; set; }

    }

    /// <summary>
    /// Section database.
    /// </summary>
    [System.Serializable]
    [XmlRoot("database", Namespace = "urn:strusoft")]
    public partial class VehicleDatabase
    {
        [XmlElement("vehicle_types")]
        public LibraryItems.VehicleTypes VehicleTypes { get; set; }

        public static List<StruSoft.Interop.StruXml.Data.Vehicle_lib_type> DeserializeFromResource()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(VehicleDatabase));

            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (string resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName.EndsWith("vehicles.struxml"))
                {
                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        TextReader reader = new StreamReader(stream);
                        object obj = deserializer.Deserialize(reader);
                        VehicleDatabase vehicleDatabase = (VehicleDatabase)obj;
                        reader.Close();

                        return vehicleDatabase.VehicleTypes.PredefinedTypes;
                    }
                }
            }
            throw new System.ArgumentException("Vehicle library resource not in assembly! Was project compiled without embedded resource?");
        }

        public static List<StruSoft.Interop.StruXml.Data.Vehicle_lib_type> DeserializeFromFilePath(string filePath)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(VehicleDatabase));
            TextReader reader = new StreamReader(filePath);
            object obj = deserializer.Deserialize(reader);
            VehicleDatabase vehicleDatabase = (VehicleDatabase)obj;
            reader.Close();
            return vehicleDatabase.VehicleTypes.PredefinedTypes;
        }


    }

}
