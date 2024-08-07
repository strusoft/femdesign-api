## create a template for a class in python
import os
import xml.etree.ElementTree as ET
from command import Command

class FdscriptHeader:

    def __init__(self, title, version, module, log_file):
        self.title = title
        self.version = str(version)
        self.module = module
        self.logfile = log_file


    def to_xml_element(self) -> ET.Element:
        fdscript_header = ET.Element("fdscriptheader")
        
        title_elem = ET.SubElement(fdscript_header, "title")
        title_elem.text = self.title

        version_elem = ET.SubElement(fdscript_header, "version")
        version_elem.text = self.version

        module_elem = ET.SubElement(fdscript_header, "module")
        module_elem.text = self.module

        logfile_elem = ET.SubElement(fdscript_header, "logfile")
        logfile_elem.text = self.logfile


        return fdscript_header


class Fdscript:

    attributes = {
        "xmlns:xsi" : "http://www.w3.org/2001/XMLSchema-instance",
        "xsi:noNamespaceSchemaLocation" : "fdscript.xsd"
        }

    def __init__(self, fdscriptheader : FdscriptHeader, commands : list[Command] ):
        self.fdscriptheader = fdscriptheader
        self.commands = commands

    def add_command(self, command):
        self.commands.append(command)

    def to_xml_element(self) -> ET.Element:
        fdscript = ET.Element("fdscript")
        fdscript.attrib = self.attributes

        fdscript.append(self.fdscriptheader.to_xml_element())

        for command in self.commands:
            fdscript.append(command.to_xml_element())

        return fdscript
    
    def serialise_to_file(self, file_name):
        fdscript = self.to_xml_element()

        tree = ET.ElementTree(fdscript)
        tree.write(file_name)