import os
import xml.etree.ElementTree as ET
from enum import Enum, auto
from abc import ABC, abstractmethod
from analysis import Analysis, Design

class User(Enum):
    STRUCT = auto()
    LOADS = auto()
    MESH = auto()
    RESMODE = auto()
    FOUNDATIONDESIGN = auto()
    RCDESIGN = auto()
    STEELDESIGN = auto()
    TIMBERDESIGN = auto()
    MASONRYDESIGN = auto()
    COMPOSITEDESIGN = auto()
    PERFORMANCEBASEDDESIGN = auto()


class Command(ABC):

    @abstractmethod
    def to_xml_element(self) -> ET.Element:
        pass


class CmdUser(Command):
    def __init__(self, module : User):
        self.module = module

    def to_xml_element(self) -> ET.Element:
        cmd_user = ET.Element("cmduser")
        cmd_user.attrib = {"command": f"; CXL $MODULE {self.module.name}"}
        
        return cmd_user


class CmdOpen(Command):
    def __init__(self, file_name : str):
        self.file_name = file_name

    def to_xml_element(self) -> ET.Element:
        cmd_open = ET.Element("cmdopen")

        file_name_elem = ET.SubElement(cmd_open, "filename")
        file_name_elem.text = self.file_name

        attributes = {"command": "; CXL CS2SHELL OPEN"}
        cmd_open.attrib = attributes

        return cmd_open


class CmdCalculation(Command):
    def __init__(self, analysis : Analysis, design : Design = None):
        self.analysis = analysis
        self.design = design


    def to_xml_element(self) -> ET.Element:
        cmd_calculation = ET.Element("cmdcalculation")

        attributes = { "command": "; CXL $MODULE CALC"}
        cmd_calculation.attrib = attributes

        if self.analysis:
            cmd_calculation.append(self.analysis.to_xml_element())
        if self.design:
            cmd_calculation.append(self.design.to_xml_element())

        return cmd_calculation


class CmdSave(Command):
    def __init__(self, file_name : str):
        self.file_name = file_name

    def to_xml_element(self) -> ET.Element:
        cmd_save = ET.Element("cmdsave")

        file_name_elem = ET.SubElement(cmd_save, "filename")
        file_name_elem.text = self.file_name

        attributes = {"command": "; CXL CS2SHELL SAVE"}
        cmd_save.attrib = attributes

        return cmd_save


class CmdEndSession(Command):
    def __init__(self):
        pass

    def to_xml_element(self) -> ET.Element:
        cmd_end_session = ET.Element("cmdendsession")
        return cmd_end_session


class CmdProjDescr(Command):
    def __init__(self, project : str, description : str, designer : str, signature : str, comment : str, items : dict, read : bool = False, reset : bool = False):
        self.project = project
        self.description = description
        self.designer = designer
        self.signature = signature
        self.comment = comment
        self.items = items
        self.read = read
        self.reset = reset

    def to_xml_element(self) -> ET.Element:
        cmd_proj_descr = ET.Element("cmdprojdescr")

        attributes = {
            "command": "$ MODULECOM PROJDESCR",
            "szProject": self.project,
            "szDescription": self.description,
            "szDesigner": self.designer,
            "szSignature": self.signature,
            "szComment": self.comment,
            "read": str(int(self.read)),
            "reset": str(int(self.reset))
        }

        ## join two dictionaries
        cmd_proj_descr.attrib = attributes

        for key, value in self.items.items():
            item = ET.SubElement(cmd_proj_descr, "item")
            item.attrib = {"id": key, "txt": value}

        return cmd_proj_descr