import os
import xml.etree.ElementTree as ET
from enum import Enum, auto
from abc import ABC, abstractmethod

from analysis import Analysis, Design

import uuid
import pathlib

class User(Enum):
    """Enum class to represent the different modules in FEM-Design
    """
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


class DesignModule(Enum):
    """Enum class to represent the different design modules in FEM-Design
    """
    RCDESIGN = auto()
    STEELDESIGN = auto()
    TIMBERDESIGN = auto()
    MASONRYDESIGN = auto()
    COMPOSITEDESIGN = auto()

    def to_user(self) -> User:
        """Convert the DesignModule to a User object

        Returns:
            User: User object corresponding to the DesignModule
        """
        mapping = {
            DesignModule.RCDESIGN: User.RCDESIGN,
            DesignModule.STEELDESIGN: User.STEELDESIGN,
            DesignModule.TIMBERDESIGN: User.TIMBERDESIGN,
            DesignModule.MASONRYDESIGN: User.MASONRYDESIGN,
            DesignModule.COMPOSITEDESIGN: User.COMPOSITEDESIGN
        }
        return mapping[self]


class Command(ABC):
    """Abstract class to represent a command in a FEM-Design script file
    """
    @abstractmethod
    def to_xml_element(self) -> ET.Element:
        pass


class CmdUser(Command):
    """CmdUser class to represent the fdscript 'cmduser' command
    """
    def __init__(self, module : User):
        self.module = module

    def to_xml_element(self) -> ET.Element:
        """convert the CmdUser object to an xml element

        Returns:
            ET.Element: xml element representing the CmdUser object
        """
        cmd_user = ET.Element("cmduser")
        cmd_user.attrib = {"command": f"; CXL $MODULE {self.module.name}"}
        
        return cmd_user
    
    @classmethod
    def ResMode(cls) -> Command:
        """Create a CmdUser object for the ResMode module

        Returns:
            Command: CmdUser object for the ResMode module
        """
        return cls(User.RESMODE)
    
    @classmethod
    def Load(cls) -> Command:
        """Create a CmdUser object for the Load module

        Returns:
            Command: CmdUser object for the Load module
        """
        return cls(User.LOADS)
    
    @classmethod
    def Mesh(cls) -> Command:
        """Create a CmdUser object for the Mesh module

        Returns:
            Command: CmdUser object for the Mesh module
        """
        return cls(User.MESH)
    
    @classmethod
    def FoundationDesign(cls) -> Command:
        """Create a CmdUser object for the FoundationDesign module

        Returns:
            Command: CmdUser object for the FoundationDesign module
        """
        return cls(User.FOUNDATIONDESIGN)
    
    @classmethod
    def RCDesign(cls) -> Command:
        """Create a CmdUser object for the RCDesign module

        Returns:
            Command: CmdUser object for the RCDesign module
        """
        return cls(User.RCDESIGN)
    
    @classmethod
    def SteelDesign(cls) -> Command:
        """Create a CmdUser object for the SteelDesign module

        Returns:
            Command: CmdUser object for the SteelDesign module
        """
        return cls(User.STEELDESIGN)
    
    @classmethod
    def TimberDesign(cls)-> Command:
        """Create a CmdUser object for the TimberDesign module

        Returns:
            Command: CmdUser object for the TimberDesign module
        """
        return cls(User.TIMBERDESIGN)
    
    @classmethod
    def MasonryDesign(cls) -> Command:
        """Create a CmdUser object for the MasonryDesign module

        Returns:
            Command: CmdUser object for the MasonryDesign module
        """
        return cls(User.MASONRYDESIGN)
    
    @classmethod
    def CompositeDesign(cls) -> Command:
        """Create a CmdUser object for the CompositeDesign module

        Returns:
            Command: CmdUser object for the CompositeDesign module
        """
        return cls(User.COMPOSITEDESIGN)
    
    @classmethod
    def PerformanceBasedDesign(cls) -> Command:
        """Create a CmdUser object for the PerformanceBasedDesign module

        Returns:
            Command: CmdUser object for the PerformanceBasedDesign module
        """
        return cls(User.PERFORMANCEBASEDDESIGN)

    @classmethod
    def _fromDesignModule(cls, module : DesignModule) -> Command:
        """Create a CmdUser object from a DesignModule

        Args:
            module (DesignModule): DesignModule to create the CmdUser object from

        Returns:
            Command: CmdUser object for the specified DesignModule
        """
        if module == DesignModule.RCDESIGN:
            return cls.RCDesign()
        if module == DesignModule.STEELDESIGN:
            return cls.SteelDesign()
        if module == DesignModule.TIMBERDESIGN:
            return cls.TimberDesign()
        if module == DesignModule.MASONRYDESIGN:
            return cls.MasonryDesign()
        if module == DesignModule.COMPOSITEDESIGN:
            return cls.CompositeDesign()


class CmdOpen(Command):
    """CmdOpen class to represent the fdscript 'cmdopen' command
    """
    def __init__(self, file_name : str):
        """Constructor for the CmdOpen class

        Args:
            file_name (str): path to the file to open
        """
        self.file_name = os.path.abspath(file_name)

    def to_xml_element(self) -> ET.Element:
        cmd_open = ET.Element("cmdopen")

        file_name_elem = ET.SubElement(cmd_open, "filename")
        file_name_elem.text = self.file_name

        attributes = {"command": "; CXL CS2SHELL OPEN"}
        cmd_open.attrib = attributes

        return cmd_open


class CmdCalculation(Command):
    """CmdCalculation class to represent the fdscript 'cmdcalculation' command
    """
    def __init__(self, analysis : Analysis, design : Design = None):
        """Constructor for the CmdCalculation class

        Args:
            analysis (Analysis): Analysis object to be included in the calculation
            design (Design, optional): Design object to be included in the calculation.
        """
        self.analysis = analysis
        self.design = design


    def to_xml_element(self) -> ET.Element:
        """Convert the CmdCalculation object to an xml element

        Returns:
            ET.Element: xml element representing the CmdCalculation object
        """
        cmd_calculation = ET.Element("cmdcalculation")

        attributes = { "command": "; CXL $MODULE CALC"}
        cmd_calculation.attrib = attributes

        if self.analysis:
            cmd_calculation.append(self.analysis.to_xml_element())
        if self.design:
            cmd_calculation.append(self.design.to_xml_element())

        return cmd_calculation


class CmdSave(Command):
    """CmdSave class to represent the fdscript 'cmdsave' command
    """
    def __init__(self, file_name : str):
        """Constructor for the CmdSave class

        Args:
            file_name (str): path to the file to save
        """

        if not file_name.endswith(".str") and not file_name.endswith(".struxml"):
            raise ValueError("file_name must have suffix .str or .struxml")
        
        if not os.path.exists(os.path.dirname(os.path.abspath(file_name))):
            os.makedirs(os.path.dirname(os.path.abspath(file_name)))

        self.file_name = os.path.abspath(file_name)

    def to_xml_element(self) -> ET.Element:
        """Convert the CmdSave object to an xml element

        Returns:
            ET.Element: xml element representing the CmdSave object
        """
        cmd_save = ET.Element("cmdsave")

        file_name_elem = ET.SubElement(cmd_save, "filename")
        file_name_elem.text = self.file_name

        attributes = {"command": "; CXL CS2SHELL SAVE"}
        cmd_save.attrib = attributes

        return cmd_save


class CmdEndSession(Command):
    """CmdEndSession class to represent the fdscript 'cmdendsession' command
    """
    def __init__(self):
        """"""
        pass

    def to_xml_element(self) -> ET.Element:
        """convert the CmdEndSession object to an xml element

        Returns:
            ET.Element: xml element representing the CmdEndSession object
        """
        cmd_end_session = ET.Element("cmdendsession")
        return cmd_end_session


class CmdProjDescr(Command):
    """class to represent the fdscript cmdprojdescr command
    """
    def __init__(self, project : str, description : str, designer : str, signature : str, comment : str, items : dict = None, read : bool = False, reset : bool = False):
        """Constructor for the CmdProjDescr class

        Args:
            project (str): project name
            description (str): description
            designer (str): designer
            signature (str): signature
            comment (str): comment
            items (dict, optional): define key-value user data. Defaults to None.
            read (bool, optional): read the project settings. Value will be store in the clipboard. Defaults to False.
            reset (bool, optional): reset the project settings. Defaults to False.
        """
        self.project = project
        self.description = description
        self.designer = designer
        self.signature = signature
        self.comment = comment
        self.items = items
        self.read = read
        self.reset = reset

    def to_xml_element(self) -> ET.Element:
        """Convert the CmdProjDescr object to an xml element

        Returns:
            ET.Element: xml element representing the CmdProjDescr object
        """
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

        if self.items:
            for key, value in self.items.items():
                item = ET.SubElement(cmd_proj_descr, "item")
                item.attrib = {"id": key, "txt": value}

        return cmd_proj_descr


class CmdListGen:
    """Class to represent the fdscript 'cmdlistgen' command
    """
    def __init__(self, bscfile : str, outfile : str = None, guids : list[uuid.UUID] = None, regional : bool = True, fillcells : bool = True, headers : bool = True):
        """Constructor for the CmdListGen class

        Args:
            bscfile (str): path to the bsc file
            outfile (str, optional): path to the output file. Defaults to None.
            guids (list[uuid.UUID], optional): list of element part guids to include in the output. Defaults to None.
            regional (bool, optional):
            fillcells (bool, optional):
            headers (bool, optional):
        """
        if not bscfile.endswith(".bsc"):
            raise ValueError("bscfile must have suffix .bsc")

        if outfile and not outfile.endswith(".csv"):
            raise ValueError("outfile must have suffix .csv")

        if not outfile:
            outfile = pathlib.Path(self.bscfile).with_suffix(".csv")

        if not os.path.exists(os.path.dirname(os.path.abspath(outfile))):
            os.makedirs(os.path.dirname(os.path.abspath(outfile)))

        self.bscfile = os.path.abspath(bscfile)
        self.outfile = os.path.abspath(outfile)
        self.regional = regional
        self.fillcells = fillcells
        self.headers = headers
        self.guids = guids

    def to_xml_element(self) -> ET.Element:
        """convert the CmdListGen object to an xml element

        Returns:
            ET.Element: xml element representing the CmdListGen object
        """
        cmd_listgen = ET.Element("cmdlistgen")

        attributes = {
            "command": "$ MODULECOM LISTGEN",
            "bscfile": self.bscfile,
            "outfile": self.outfile,
            "regional": str(int(self.regional)),
            "fillcells": str(int(self.fillcells)),
            "headers": str(int(self.headers)),
        }

        cmd_listgen.attrib = attributes

        if self.guids:
            for guid in self.guids:
                guid_elem = ET.SubElement(cmd_listgen, "GUID")
                guid_elem.text = str(guid)

        return cmd_listgen

