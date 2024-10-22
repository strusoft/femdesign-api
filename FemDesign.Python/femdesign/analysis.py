from enum import Enum
import xml.etree.ElementTree as ET



class Stage:
    """
    Class to represent a stage in FEM-Design
    """
    class Method(Enum):
        """
        Enum to represent the calculation method of the construction stage
        """
        TRACKING = 0
        GHOST = 1

    def __init__(self, method : Method = Method.TRACKING, tda : bool = True, creepincrementlimit = 25.0):
        self.method = method.value
        self.tda = tda
        self.creepincrementlimit = creepincrementlimit

    def to_xml_element(self) -> ET.Element:
        """Convert the Stage object to an xml element

        Returns:
            ET.Element: xml element representing the Stage object
        """
        stage = ET.Element("stage")
        stage.attrib = {
            "ghost": str(self.method.value),
            "tda": str(int(self.tda)),
            "creepincrementlimit": str(self.creepincrementlimit)
        }
        return stage


class CombItem:
    def __init__(self, Calc : bool = True, NLE : bool = False, PL : bool = False, NLS : bool = False, Cr : bool = False, f2nd : bool = False, Im : float = 0, Amplitudo : float = 1.0, Waterlevel : float = 0, ImpfRqd = 0, StabRqd = 0):
        self.Calc = Calc
        self.NLE = NLE
        self.PL = PL
        self.NLS = NLS
        self.Cr = Cr
        self.f2nd = f2nd
        self.Im = Im
        self.Amplitudo = Amplitudo
        self.Waterlevel = Waterlevel

        self.ImpfRqd = ImpfRqd
        self.StabRqd = StabRqd


    @classmethod
    def StaticAnalysis(cls, Calc : bool = True, NLE : bool = False, PL : bool = False):
        return cls(Calc = Calc, NLE = NLE, PL = PL)
    
    @classmethod
    def NoCalculation(cls):
        return cls(Calc = False)

    def to_xml_element(self):
        comb_item = ET.Element("combitem")
        comb_item.attrib = {
            "Calc": str(int(self.Calc)),
            "NLE": str(int(self.NLE)),
            "PL": str(int(self.PL)),
            "NLS": str(int(self.NLS)),
            "Cr": str(int(self.Cr)),
            "f2nd": str(int(self.f2nd)),
            "Im": str(self.Im),
            "Amplitudo": str(self.Amplitudo),
            "Waterlevel": str(self.Waterlevel),

            "ImpfRqd": str(self.ImpfRqd),
            "StabRqd": str(self.StabRqd),
        }
        return comb_item


class Comb:
    def __init__(self, NLEmaxiter : int = 30, PLdefloadstep : int = 20, PLminloadstep : int = 2, PLmaxeqiter : int = 30, PlKeepLoadStep : bool = True, PlTolerance : int = 1, PlShellLayers : int = 10, PlShellCalcStr : bool = 1, NLSMohr : bool = True, NLSinitloadstep : int = 10, NLSminloadstep : int = 10, NLSactiveelemratio : int = 5, NLSplasticelemratio : int = 5, CRloadstep : int = 20, CRmaxiter : int = 30, CRstifferror : int = 2, combitems : list[CombItem] = None):
        self.NLEmaxiter = NLEmaxiter
        self.PLdefloadstep = PLdefloadstep
        self.PLminloadstep = PLminloadstep
        self.PLmaxeqiter = PLmaxeqiter
        self.PlKeepLoadStep = PlKeepLoadStep
        self.PlTolerance = PlTolerance
        self.PlShellLayers = PlShellLayers
        self.PlShellCalcStr = PlShellCalcStr
        self.NLSMohr = NLSMohr
        self.NLSinitloadstep = NLSinitloadstep
        self.NLSminloadstep = NLSminloadstep
        self.NLSactiveelemratio = NLSactiveelemratio
        self.NLSplasticelemratio = NLSplasticelemratio
        self.CRloadstep = CRloadstep
        self.CRmaxiter = CRmaxiter
        self.CRstifferror = CRstifferror
        self.combitems = combitems or []

    @classmethod
    def Default(cls):
        return cls()

    def to_xml_element(self) -> ET.Element:
        comb = ET.Element("comb")
        comb.attrib = {
            "NLEmaxiter": str(self.NLEmaxiter),
            "PLdefloadstep": str(self.PLdefloadstep),
            "PLminloadstep": str(self.PLminloadstep),
            "PLmaxeqiter": str(self.PLmaxeqiter),
            "PlKeepLoadStep": str(self.PlKeepLoadStep),
            "PlTolerance": str(self.PlTolerance),
            "PlShellLayers": str(self.PlShellLayers),
            "PlShellCalcStr": str(self.PlShellCalcStr),
            "NLSMohr": str(self.NLSMohr),
            "NLSinitloadstep": str(self.NLSinitloadstep),
            "NLSminloadstep": str(self.NLSminloadstep),
            "NLSactiveelemratio": str(self.NLSactiveelemratio),
            "NLSplasticelemratio": str(self.NLSplasticelemratio),
            "CRloadstep": str(self.CRloadstep),
            "CRmaxiter": str(self.CRmaxiter),
            "CRstifferror": str(self.CRstifferror)
        }

        for combitem in self.combitems:
            comb.append(combitem.to_xml_element())
        return comb



class Freq:
    """
    Class to represent the frequency analysis settings
    """
    class ShapeNormalization(Enum):
        """
        Enum to represent the normalization unit of the shape
        """
        MassMatrix = 0
        Unit = 1

    def __init__(self, num_shapes : int = 2, auto_iter : int = 0, max_sturm : int = 0, norm_unit : ShapeNormalization = ShapeNormalization.MassMatrix, x : bool = True, y : bool = True, z : bool = True, top : float = -0.01):
        self.Numshapes = num_shapes
        self.AutoIter = auto_iter
        self.MaxSturm = max_sturm
        self.NormUnit = norm_unit
        self.X = x
        self.Y = y
        self.Z = z
        self.top = top

    def to_xml_element(self) -> ET.Element:
        """ Convert the Freq object to an xml element

        Returns:
            ET.Element: xml element representing the Freq object
        """
        freq = ET.Element("freq")
        freq.attrib = {
            "Numshapes": str(self.Numshapes),
            "MaxSturm": str(self.MaxSturm),
            "NormUnit": str(self.NormUnit.value),
            "X": str(int(self.X)),
            "Y": str(int(self.Y)),
            "Z": str(int(self.Z)),
            "top": str(self.top),
            "AutoIter": str(self.AutoIter)
        }
        return freq

    @classmethod
    def Default(cls, num_shapes = 5, auto_iter = 0, max_sturm = 0, norm_unit  = ShapeNormalization.MassMatrix, x = True, y = True, z = True, top = -0.01):
        return cls(num_shapes, auto_iter, max_sturm, norm_unit, x, y, z, top)

class Footfall:
    def __init__(self, TopOfSubstructure : float = -0.01):
        self.TopOfSubstructure = TopOfSubstructure

    def to_xml_element(self):
        footfall = ET.Element("footfall")
        footfall.attrib = {
            "TopOfSubstructure": str(self.TopOfSubstructure)
        }
        return footfall


class ThGroundAcc:
    def __init__(self, flevelspectra=1, dts=0.20, tsend=5.0, q=1.0, facc=1, nres=5, tcend=20.0, method=0, alpha=0.000, beta=0.000, ksi=5.0):
        self.flevelspectra = flevelspectra
        self.dts = dts
        self.tsend = tsend
        self.q = q
        self.facc = facc
        self.nres = nres
        self.tcend = tcend
        self.method = method
        self.alpha = alpha
        self.beta = beta
        self.ksi = ksi

    def to_xml_element(self):
        thgroundacc = ET.Element("thgroundacc")
        thgroundacc.attrib = {
            "flevelspectra": str(self.flevelspectra),
            "dts": str(self.dts),
            "tsend": str(self.tsend),
            "q": str(self.q),
            "facc": str(self.facc),
            "nres": str(self.nres),
            "tcend": str(self.tcend),
            "method": str(self.method),
            "alpha": str(self.alpha),
            "beta": str(self.beta),
            "ksi": str(self.ksi)
        }
        return thgroundacc


class ThExForce:
    def __init__(self, nres=5, tcend=20.0, method=0, alpha=0.000, beta=0.000, ksi=5.0):
        self.nres = nres
        self.tcend = tcend
        self.method = method
        self.alpha = alpha
        self.beta = beta
        self.ksi = ksi

    def to_xml_element(self):
        thexforce = ET.Element("thexforce")
        thexforce.attrib = {
            "nres": str(self.nres),
            "tcend": str(self.tcend),
            "method": str(self.method),
            "alpha": str(self.alpha),
            "beta": str(self.beta),
            "ksi": str(self.ksi)
        }
        return thexforce


class PeriodicExc:
    def __init__(self, deltat=0.0100, tend=5.00, dampeningtype=0, alpha=0.000, beta=0.000, ksi=5.0):
        self.deltat = deltat
        self.tend = tend
        self.dampeningtype = dampeningtype
        self.alpha = alpha
        self.beta = beta
        self.ksi = ksi

    def to_xml_element(self):
        periodicexc = ET.Element("periodicexc")
        periodicexc.attrib = {
            "deltat": str(self.deltat),
            "tend": str(self.tend),
            "dampeningtype": str(self.dampeningtype),
            "alpha": str(self.alpha),
            "beta": str(self.beta),
            "ksi": str(self.ksi)
        }
        return periodicexc


class Design:
    def __init__(self, autodesign : bool = True, check : bool = True, load_combination : bool = True):
        self.autodesign = autodesign
        self.check = check
        self.load_combination = load_combination

    def to_xml_element(self):
        design = ET.Element("design")

        
        if self.load_combination:
            comb_element = ET.SubElement(design, "cmax")
        else:
            comb_element = ET.SubElement(design, "gmax")
        comb_element.text = ""

        autodesign_element = ET.SubElement(design, "autodesign")
        autodesign_element.text = str(self.autodesign).lower()

        check_element = ET.SubElement(design, "check")
        check_element.text = str(self.check).lower()
        return design

class Analysis:
    def __init__(self,
                 calcCase : bool = False,
                 calcComb : bool = False,
                 calcGmax : bool = False,
                 calcStage : bool = False,
                 calcImpf : bool = False,
                 calcStab : bool = False,
                 calcFreq : bool = False,
                 calcSeis : bool = False,
                 calcFootfall : bool = False,
                 calcMovingLoad : bool = False,
                 calcThGrounAcc : bool = False,
                 calcThExforce : bool = False,
                 calcPeriodicExc : bool = False,
                 calcStoreyFreq : bool = False,
                 calcBedding : bool = False,
                 calcDesign : bool = False,
                 elemfine : bool = True,
                 diaphragm : bool = False,
                 peaksmoothings : bool = False,
                 comb : Comb = None,
                 stage : Stage = None,
                 freq : Freq = None,
                 footfall : Footfall = None,
                 #bedding=None,
                 thgroundacc : ThGroundAcc = None,
                 thexforce : ThExForce = None,
                 periodicexc : PeriodicExc = None):
        self.calcCase = calcCase
        self.calcStage = calcStage
        self.calcImpf = calcImpf
        self.calcComb = calcComb
        self.calcGmax = calcGmax
        self.calcStab = calcStab
        self.calcFreq = calcFreq
        self.calcSeis = calcSeis
        self.calcFootfall = calcFootfall
        self.calcMovingLoad = calcMovingLoad
        self.calcBedding = calcBedding
        self.calcThGrounAcc = calcThGrounAcc
        self.calcThExforce = calcThExforce
        self.calcPeriodicExc = calcPeriodicExc
        self.calcStoreyFreq = calcStoreyFreq
        self.calcDesign = calcDesign

        self.elemfine = elemfine
        self.diaphragm = diaphragm
        self.peaksmoothings = peaksmoothings
        
        self.stage = stage
        self.comb = comb
        self.freq = freq
        self.footfall = footfall
        #self.bedding = bedding
        self.thgroundacc = thgroundacc
        self.thexforce = thexforce
        self.periodicexc = periodicexc

    def to_xml_element(self):
        analysis = ET.Element("analysis")

        analysis.attrib = {
            "calcCase":         str(int(self.calcCase)),
            "calcComb":         str(int(self.calcComb)),
            "calcGmax":         str(int(self.calcGmax)),
            "calcStage":        str(int(self.calcStage)),
            "calcImpf":         str(int(self.calcImpf)),
            "calcStab":         str(int(self.calcStab)),
            "calcFreq":         str(int(self.calcFreq)),
            "calcSeis":         str(int(self.calcSeis)),
            "calcFootfall":     str(int(self.calcFootfall)),
            "calcMovingLoad":   str(int(self.calcMovingLoad)),
            "calcThGrounAcc":   str(int(self.calcThGrounAcc)),
            "calcThExforce":    str(int(self.calcThExforce)),
            "calcPeriodicExc":  str(int(self.calcPeriodicExc)),
            "calcStoreyFreq":   str(int(self.calcStoreyFreq)),
            "calcBedding":      str(int(self.calcBedding)),
            "calcDesign":       str(int(self.calcDesign)),

            "elemfine":         str(int(self.elemfine)),
            "diaphragm":        str(int(self.diaphragm)),
            "peaksmoothings":   str(int(self.peaksmoothings))
        }

        if self.comb:
            analysis.append(self.comb.to_xml_element())
        if self.stage:
            analysis.append(self.stage.to_xml_element())
        if self.freq:
            analysis.append(self.freq.to_xml_element())
        if self.footfall:
            analysis.append(self.footfall.to_xml_element())
        if self.thgroundacc:
            analysis.append(self.thgroundacc.to_xml_element())
        if self.thexforce:
            analysis.append(self.thexforce.to_xml_element())
        if self.periodicexc:
            analysis.append(self.periodicexc.to_xml_element())
        # if self.bedding:
        #     analysis.append(self.bedding.to_xml_element())

        return analysis

    @classmethod
    def StaticAnalysis(cls, comb : Comb = Comb.Default(), calcComb : bool = True):
        return cls(calcCase = True, calcComb = calcComb, comb = comb)
    
    @classmethod
    def FrequencyAnalysis(cls, num_shapes : int = 5, auto_iter : int = 0, max_sturm : int = 0, norm_unit : Freq.ShapeNormalization  = Freq.ShapeNormalization.MassMatrix, x : bool = True, y : bool = True, z : bool = True, top : bool = -0.01):
        freq = Freq(num_shapes, auto_iter, max_sturm, norm_unit, x, y, z, top)
        return cls(calcFreq = True, freq = freq)
    
    @classmethod
    def FootfallAnalysis(cls, footfall : Footfall, calcFootfall : bool = True):
        return cls(calcFootfall = calcFootfall, footfall = footfall)

# class Bedding:
#     def __init__(self, Ldcomb="a", Meshprep=0, Stiff_X=0.5, Stiff_Y=0.5):
#         self.Ldcomb = Ldcomb
#         self.Meshprep = Meshprep
#         self.Stiff_X = Stiff_X
#         self.Stiff_Y = Stiff_Y

#     def to_xml_element(self):
#         bedding = ET.Element("bedding")
#         bedding.attrib = {
#             "Ldcomb": str(self.Ldcomb),
#             "Meshprep": str(self.Meshprep),
#             "Stiff_X": str(self.Stiff_X),
#             "Stiff_Y": str(self.Stiff_Y)
#         }
#         return bedding