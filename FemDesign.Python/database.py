import xml.etree.ElementTree as ET


namespace = {'': 'urn:strusoft'}

class Database:
    def __init__(self, filename : str):
        tree = ET.parse(filename)
        self._root = tree.getroot()

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