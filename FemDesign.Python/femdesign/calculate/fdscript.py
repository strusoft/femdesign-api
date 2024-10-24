## create a template for a class in python
import os
import xml.etree.ElementTree as ET
from command import Command

class FdscriptHeader:
    """
    Class to represent the header of a FEM-Design script file
    """
    def __init__(self, log_file : str, title : str = "FEM-Design API", version : int = 2400, module : str = "SFRAME"):
        self.title = title
        self.version = str(version)
        self.module = module
        self.logfile = os.path.abspath(log_file)


    def to_xml_element(self) -> ET.Element:
        """Convert the FdscriptHeader object to an xml element

        Returns:
            ET.Element: xml element representing the FdscriptHeader object
        """
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
    """
    Class to represent a FEM-Design script file
    """
    attributes = {
        "xmlns:xsi" : "http://www.w3.org/2001/XMLSchema-instance",
        "xsi:noNamespaceSchemaLocation" : "fdscript.xsd"
        }

    def __init__(self, log_file : str, commands : list[Command] ):
        self.fdscriptheader = FdscriptHeader(log_file)
        self.commands = commands

    def add_command(self, command : Command):
        """Add a command to the Fdscript object

        Args:
            command (Command): Command object to add to the Fdscript object
        """
        self.commands.append(command)

    def to_xml_element(self) -> ET.Element:
        """Convert the Fdscript object to an xml element

        Returns:
            ET.Element: xml element representing the Fdscript object
        """
        fdscript = ET.Element("fdscript")
        fdscript.attrib = self.attributes

        fdscript.append(self.fdscriptheader.to_xml_element())

        for command in self.commands:
            fdscript.append(command.to_xml_element())

        return fdscript
    
    def serialise_to_file(self, file_name : str):
        """Serialise the Fdscript object to a file

        Args:
            file_name (str): file name to save the fdscript to
        """
        fdscript = self.to_xml_element()

        tree = ET.ElementTree(fdscript)
        tree.write(file_name, encoding="utf-8")