import xml.etree.ElementTree as ET
import uuid

namespace = {'': 'urn:strusoft'}

class Database:
    def __init__(self, country):
        self.struxml_version = "01.00.000"
        self.source_software = f"FEM-Design API SDK {self.get_version()}"
        self.start_time = "1970-01-01T00:00:00.000"
        self.end_time = datetime.datetime.utcnow().strftime("%Y-%m-%dT%H:%M:%S.%f")[:-3]
        self.guid = str(uuid.uuid4())
        self.convert_id = "00000000-0000-0000-0000-000000000000"
        self.standard = "EC"
        self.country = country
        self.end = ""

    def get_version(self):
        return "0.1.0"

    @property
    def eurocode(self):
        return self._root.attrib["standard"]
    
    @property
    def country(self):
        return self._root.attrib["country"]

    @property
    def source_software(self):
        return self._root.attrib["source_software"]
    
    @property
    def entities(self):
        return self._root.findall(".//entities", namespace)
    
    @property
    def sections(self):
        return self._root.findall(".//sections", namespace)

    @property
    def materials(self):
        return self._root.findall(".//materials", namespace)

    @property
    def bars(self):
        return self._root.findall(".//bar", namespace)
    
    def serialise_to_xml(self):
        return ET.tostring(self._root, encoding="UTF-8")
    
        #     private void Initialize(Country country)
        # {
        #     this.StruxmlVersion = "01.00.000";
        #     this.SourceSoftware = $"FEM-Design API SDK {Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
        #     this.StartTime = "1970-01-01T00:00:00.000";
        #     this.EndTime = System.DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
        #     this.Guid = System.Guid.NewGuid();
        #     this.ConvertId = "00000000-0000-0000-0000-000000000000";
        #     this.Standard = "EC";
        #     this.Country = country;
        #     this.End = "";