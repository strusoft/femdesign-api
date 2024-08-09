from command import *
from analysis import Analysis, Comb, Design

def test_design():
    xmlDesign = Design(True, True, True).to_xml_element()

    assert xmlDesign.tag == "design"
    assert xmlDesign.attrib == {}
    assert xmlDesign.text == None

    assert xmlDesign.find("autodesign") != None
    assert xmlDesign.find("autodesign").text == "true"
    assert xmlDesign.find("check").text == "true"
    assert xmlDesign.find("gmax") == None
    assert xmlDesign.find("cmax") != None


    xmlDesign = Design(False, False, False).to_xml_element()

    assert xmlDesign.tag == "design"
    assert xmlDesign.attrib == {}
    assert xmlDesign.text == None

    assert xmlDesign.find("autodesign") != None
    assert xmlDesign.find("autodesign").text == "false"
    assert xmlDesign.find("check").text == "false"

    assert xmlDesign.find("gmax") != None
    assert xmlDesign.find("cmax") == None
