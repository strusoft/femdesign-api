#!/usr/bin/env python3

# --- Import and registrations ---
import subprocess
import xml.etree.ElementTree as ET
import xml.dom.minidom as minidom
import os
import uuid
import math
from datetime import datetime

# --- Databases dir ---
tree_mat = ET.parse('data\\materials.struxml')
tree_sec = ET.parse('data\\sections.struxml')

# --- Installation dir ---
FD_root = 'c:\\program files\\strusoft\\fem-design 18\\fd3dstruct'

# ---------------------------------------------------------------------
# ---------------------------------------------------------------------
# ---------------------------------------------------------------------

# --- Fixed variables (DO NOT CHANGE) ---
i = 0
nrcomb = 0
u_beam = 0
u_column = 0
u_truss = 0
u_plate = 0
u_wall = 0
u_edge = 0
u_cover = 0
u_support = 0
u_storey = 0
u_axis = 0
mat_handler = []
xml_annex = ''
xml_bars = []
xml_shells = []
xml_pointsupports = []
xml_linesupports = []
xml_surfacesupports = []
xml_loadcases = []
xml_loadcombs = []
xml_pointloads = []
xml_lineloads = []
xml_surfaceloads = []
xml_complexsection = []
orgUTC = '1970-01-01T00:00:00.000'

root_mat = tree_mat.getroot()
root_sec = tree_sec.getroot()
ET.register_namespace('', 'urn:strusoft')
dir_path = os.path.dirname(os.path.realpath(__file__))


class coord:
    def __init__(self, x=0, y=0, z=0):
        self.x = x
        self.y = y
        self.z = z


class XML:
    def __init__(self, filename='', batch='', export=''):
        self.filename = filename
        self.batch = batch
        self.export = export


class material:
    def __init__(self, type='', creep='', shrinkage=''):
        self.type = type
        self.creep = creep
        self.shrinkage = shrinkage


def ID(type):
    global u_beam, u_column, u_truss, u_cover, u_plate, u_wall, u_edge, u_support, u_axis
    if type == 'beam':
        u_beam = u_beam + 1
        return 'B.'+str(u_beam)
    if type =='column':
        u_column = u_column + 1
        return 'C.'+str(u_column)
    if type == 'truss':
        u_truss = u_truss + 1
        return 'T.'+str(u_truss)
    if type == 'cover':
        u_cover = u_cover + 1
        return 'CO.'+str(u_cover)
    if type == 'plate':
        u_plate = u_plate + 1
        return 'P.' +str(u_plate)
    if type == 'wall':
        u_wall = u_wall + 1
        return 'W.' +str(u_wall)
    if type == 'edge':
        u_edge = u_edge + 1
        return 'CE.' +str(u_edge)
    if type == 'support':
        u_support = u_support + 1
        return 'S.'+str(u_support)
    if type == 'axis':
        u_axis = u_axis + 1
        return str(u_axis)


def genGUID():
    return str(uuid.uuid4())


def genUTC():
    return datetime.utcnow().strftime("%Y-%m-%dT%H:%M:%S.000")


def nmz(v): # This function will normalize a vector
    normalized = [v[0]/math.sqrt(v[0]**2 + v[1]**2 + v[2]**2), v[1]/math.sqrt(v[0]**2 + v[1]**2 + v[2]**2), v[2]/math.sqrt(v[0]**2 + v[1]**2 + v[2]**2)]
    return normalized


def normal(point1, point2, rot): # This function will find the normal to a plane defined by three points, two input, one defined as z+1
    point3 = coord(point1.x, point1.y, point1.z + 1) # Create a new point on the plane
    # Define two vectors which defines the plane:
    v1 = [point2.x - point1.x, point2.y - point1.y, point2.z - point1.z]
    v2 = [point3.x - point1.x, point3.y - point1.y, point3.z - point1.z]
    # Find crossproduct of the two vectors = normal to the plane:
    n = [v1[1] * v2[2] - v1[2] * v2[1], -(v1[0] * v2[2] - v1[2] * v2[0]), v1[0] * v2[1] - v1[1] * v2[0]]
    if math.sqrt(n[0]**2 + n[1]**2 + n[2]**2) == 0:
        n = [0, -1, 0]
    # Calculate the heading:
    heading = [point2.x - point1.x, point2.y - point1.y, point2.z - point1.z]
    # Adjust normal with regard to rotation:
    y = math.cos(rot * math.pi/180)
    z = math.sin(rot * math.pi/180)
    local_z = [heading[1] * n[2] - heading[2] * n[1], heading[2] * n[0] - heading[0] * n[2], heading[0] * n[1] - heading[1] * n[0]]
    local_z_norm = nmz(local_z)
    n_norm = nmz(n)
    n_rot = []
    for l in range(len(local_z_norm)):
        n_rot.append(n_norm[l] * y + local_z_norm[l] * z)
    # Normalize the final normal vector:
    n_final = [-n_rot[0]/math.sqrt(n_rot[0]**2 + n_rot[1]**2 + n_rot[2]**2), -n_rot[1]/math.sqrt(n_rot[0]**2 + n_rot[1]**2 + n_rot[2]**2), -n_rot[2]/math.sqrt(n_rot[0]**2 + n_rot[1]**2 + n_rot[2]**2)]
    return n_final


def initiateModel(annex):
    global xml_annex, xml_root, xml_entities, xml_loads, xml_supports, xml_advanced, xml_sections
    xml_annex = annex
    xml_root = ET.Element('database', struxml_version='01.00.000', source_software='FEM-Design 18.00.002', start_time=orgUTC, end_time=genUTC(), guid=genGUID(), convertid='00000000-0000-0000-0000-000000000000', standard='EC', country=annex, xmlns='urn:strusoft')
    xml_entities = ET.SubElement(xml_root, 'entities')
    xml_sections = ET.SubElement(xml_root, 'sections')
    xml_materials = ET.SubElement(xml_root, 'materials')
    xml_loads = ET.SubElement(xml_entities, 'loads')
    xml_supports = ET.SubElement(xml_entities, 'supports')
    xml_advanced = ET.SubElement(xml_entities, 'advanced-fem')
    ET.SubElement(xml_root, 'end') # endtag


def finish(filename):
    for x in xml_complexsection:
        xml_sections.remove(x)
        xml_sections.append(x)
    for li in [xml_bars, xml_shells]: # sorting loop
        for x in li:
            xml_entities.remove(x)
            xml_entities.append(x)
    xml_entities.remove(xml_loads)
    xml_entities.remove(xml_supports)
    xml_entities.remove(xml_advanced)
    xml_entities.append(xml_loads)
    xml_entities.append(xml_supports)
    xml_entities.append(xml_advanced)
    if u_storey > 0:
        xml_entities.remove(xml_storeys)
        xml_entities.append(xml_storeys)
    if u_axis > 0:
        xml_entities.remove(xml_axes)
        xml_entities.append(xml_axes)
    for li in [xml_pointsupports, xml_linesupports, xml_surfacesupports]: # Sorting loop
        for x in li:
            xml_supports.remove(x)
            xml_supports.append(x)
    for li in [xml_pointloads, xml_lineloads, xml_surfaceloads, xml_loadcases, xml_loadcombs]: # sorting loop
        for x in li:
            xml_loads.remove(x)
            xml_loads.append(x)
    xml_reparsed = minidom.parseString(ET.tostring(xml_root))
    xml_root_pretty = minidom.parseString(xml_reparsed.toprettyxml())
    xml_file_handler = open(filename, "w+")
    xml_root_pretty.writexml(xml_file_handler)
    xml_file_handler.close()


def addMaterial(i_material):
    matGUID = genGUID()
    if i_material.type in ('S235', 'S275', 'S355', 'S420', 'S450', 'S460'):
        nameMat = i_material.type[0] + " " + i_material.type[1:4]
        add_material = ET.SubElement(xml_root.find('materials'), 'material', guid=matGUID, last_change=orgUTC, action='added', standard='EC', country=xml_annex, name=nameMat)
        mat_add = root_mat.find(i_material.type)
    if i_material.type in ('C12/15', 'C16/20', 'C20/25', 'C25/30', 'C28/35', 'C30/37', 'C32/40', 'C35/45', 'C40/50', 'C45/55', 'C50/60', 'C54/65', 'C55/67', 'C58/70', 'C60/75', 'C70/85', 'C80/95', 'C90/105'):
        add_material = ET.SubElement(xml_root.find('materials'), 'material', guid=matGUID, last_change=orgUTC, action='added', standard='EC', country=xml_annex, name=i_material.type)
        mat_add = root_mat.find(i_material.type.replace('/', ''))
        mat_add[0].attrib['creep'] = str(i_material.creep)
        mat_add[0].attrib['creep_sls'] = str(i_material.creep)
        mat_add[0].attrib['shrinkage'] = str(i_material.shrinkage)
    if i_material.type in ('C14', 'C16', 'C18', 'C20', 'C22', 'C24', 'C27', 'C30', 'C35', 'C40', 'C45', 'C50', 'D18', 'D24', 'D30', 'D35', 'D40', 'D50', 'D60', 'D70', 'L40s', 'L40c'):
        add_material = ET.SubElement(xml_root.find('materials'), 'material', guid=matGUID, last_change=orgUTC, action='added', standard='EC', country=xml_annex, name=i_material.type)
        mat_add = root_mat.find(i_material.type)
    if i_material.type in ('GL 20h', 'GL 22h', 'GL 24h', 'GL 26h', 'GL 28h', 'GL 30h', 'GL 32h', 'GL 36h'):
        add_material = ET.SubElement(xml_root.find('materials'), 'material', guid=matGUID, last_change=orgUTC, action='added', standard='EC', country=xml_annex, name=i_material.type.replace(' ', '  '))
        mat_add = root_mat.find(i_material.type.replace(' ', ''))
    if i_material.type in ('GL 20c', 'GL 22c', 'GL 24c', 'GL 26c', 'GL 28c', 'GL 30c', 'GL 32c', 'GL 36c'):
        add_material = ET.SubElement(xml_root.find('materials'), 'material', guid=matGUID, last_change=orgUTC, action='added', standard='EC', country=xml_annex, name=i_material.type)
        mat_add = root_mat.find(i_material.type.replace(' ', ''))
    add_material.append(mat_add[0])
    return matGUID


def addSection(section, eccentricity):
    global i
    complexGUID = genGUID()
    i = i+1
    sec_add = root_sec.find(section)
    sec_name_find = sec_add.attrib['name']
    simpGUID = sec_add.attrib['guid']
    mat_handler.append([i, section, simpGUID])
    sec_name = sec_name_find.rstrip(', ').split(', ')
    add_section = ET.SubElement(xml_root.find('sections'), 'section', guid=simpGUID, last_change=orgUTC, action='added', name=(sec_name[0] + ', ' + sec_name[1] + ', ' + sec_name[2]), type='custom', fd_name_code=sec_name[0], fd_name_type=sec_name[1], fd_name_size=sec_name[2])
    add_section.append(sec_add[0])
    ET.SubElement(add_section, 'end')
    add_section_complex = ET.SubElement(xml_root.find('sections'), 'complex_section', guid=complexGUID, last_change=orgUTC, action='added')
    add_section_complex_child_start = ET.SubElement(add_section_complex, 'section', pos='0', guid=simpGUID)
    ET.SubElement(add_section_complex_child_start, 'ecc', x=str(0), y=str(eccentricity.y), z=str(eccentricity.z))
    ET.SubElement(add_section_complex_child_start, 'end')
    add_section_complex_child_end = ET.SubElement(add_section_complex, 'section', pos='1', guid=simpGUID)
    ET.SubElement(add_section_complex_child_end, 'ecc', x=str(0), y=str(eccentricity.y), z=str(eccentricity.z))
    ET.SubElement(add_section_complex_child_end, 'end')
    secFabric = 'False' # Set default value
    if add_section.attrib['fd_name_type'] in ('IPE', 'HE-A', 'HE-B', 'HE-M', 'I', 'LE', 'LU', 'T', 'TPS', 'U', 'UAP', 'UKB', 'UKC', 'UPE', 'UPE-Swedish', 'VKR', 'Z'):
        secFabric = 'rolled'
        add_section.attrib['fd-mat'] = '0'
    if add_section.attrib['fd_name_type'] in ('CHS', 'KCKR', 'KKR', 'VCKR'):
        secFabric = 'cold_worked'
        add_section.attrib['fd-mat'] = '1'
    if add_section.attrib['fd_name_type'] in ('D', 'DR'):
        secFabric = 'welded'
        add_section.attrib['fd-mat'] = '2'
    if sec_name[0] == 'Concrete sections':
        add_section.attrib['fd-mat'] = '3'
    if sec_name[0] == 'Timber sections':
        add_section.attrib['fd-mat'] = '4'
    xml_complexsection.append(add_section_complex)
    return [simpGUID, complexGUID, secFabric, eccentricity]


def openFD(filename):
    subprocess.Popen([FD_root, dir_path + '\\' + filename])


def runFD(analysis, save, close, design, filename, batchfile='', exportfile=''):
    run_root = ET.Element('fdscript')
    run_root.attrib['xmlns:xsi'] = 'http://www.w3.org/2001/XMLSchema-instance'
    run_root.attrib['xsi:noNamespaceSchemaLocation'] = 'fdscript.xsd'
    run_header = ET.SubElement(run_root, 'fdscriptheader')
    run_title = ET.SubElement(run_header, 'title')
    run_title.text = 'Generated script for FEM-Design'
    run_version = ET.SubElement(run_header, 'version')
    run_version.text = '1.0'
    run_module = ET.SubElement(run_header, 'module')
    run_module.text = 'SFRAME'
    run_log = ET.SubElement(run_header, 'logfile')
    run_log.text = dir_path
    run_cmdopen = ET.SubElement(run_root, 'cmdopen', command='; CXL CS2SHELL OPEN')
    run_filename = ET.SubElement(run_cmdopen, 'filename')
    run_filename.text = dir_path + '\\' + filename
    if design == 'steel':
        ET.SubElement(run_root, 'cmduser', command='; CXL $MODULE STEELDESIGN')
    elif design == 'timber':
        ET.SubElement(run_root, 'cmduser', command='; CXL $MODULE TIMBERDESIGN')
    elif design == 'RC':
        ET.SubElement(run_root, 'cmduser', command='; CXL $MODULE RCDESIGN')
    else:
        ET.SubElement(run_root, 'cmduser', command='; CXL $MODULE RESMODE')
    run_cmdcalc = ET.SubElement(run_root, 'cmdcalculation', command='; CXL $MODULE CALC')
    run_analysis = ET.SubElement(run_cmdcalc, 'analysis')
    if analysis == 'STAB':
        run_analysis.attrib['calcStab'] = '1'
    elif analysis == 'IMP':
        run_analysis.attrib['calcImpf'] = '1'
    else:
        run_analysis.attrib['calcCase'] = '1'
        run_analysis.attrib['calcComb'] = '1'
    if design == 'steel' or design == 'timber' or design == 'RC':
        run_analysis.attrib['calcDesign'] = '1'
    run_analysis.attrib['elemfine'] = '1'
    run_analysis.attrib['peaksmoothing'] = '1'
    run_comb = ET.SubElement(run_analysis, 'comb', NLEmaxiter='30', PLdefloadstep='20', PLminloadstep='2', PLmaxeqiter='30')
    for i_comb in range(nrcomb):
        run_combitem = ET.SubElement(run_comb, 'combitem')
        if analysis == 'NLE':
            run_combitem.attrib['NLE'] = '1'
        if analysis == 'NLE+PL':
            run_combitem.attrib['NLE'] = '1'
            run_combitem.attrib['PL'] = '1'
        if analysis == 'CR':
            run_combitem.attrib['NLE'] = '1'
            run_combitem.attrib['Cr'] = '1'
        if analysis == '2ND':
            run_combitem.attrib['NLE'] = '1'
            run_combitem.attrib['f2nd'] = '1'
    if batchfile != '':
        ET.SubElement(run_root, 'cmdlistgen', command='$ MODULECOM LISTGEN', bscfile=dir_path + '\\' + batchfile, outfile=dir_path + '\\' + exportfile)
    if save == True:
        run_cmdsave = ET.SubElement(run_root, 'cmdsave', command='; CXL CS2SHELL SAVE')
        run_save = ET.SubElement(run_cmdsave, 'filename')
        run_save.text = dir_path + '\\' + filename + '.str'
    if close != False:
        ET.SubElement(run_root, 'cmdendsession')
    run_reparsed = minidom.parseString(ET.tostring(run_root))
    run_root_pretty = minidom.parseString(run_reparsed.toprettyxml())
    run_file_handle = open("data\\run.fdscript", "w+")
    run_root_pretty.writexml(run_file_handle)
    run_file_handle.close()
    subprocess.run([FD_root, '/s', dir_path + "\\data\\run.fdscript"])


def addAxis(point1, point2):
    global xml_axes
    axisID = ID('axis')
    if u_axis == 1:
        xml_axes = ET.SubElement(xml_entities, 'axes')
    add_axis = ET.SubElement(xml_axes, 'axis', guid=genGUID(), last_change=genUTC(), action='added', id=axisID, id_is_letter='false', prefix='')
    ET.SubElement(add_axis, 'start_point', x=str(point1.x), y=str(point1.y))
    ET.SubElement(add_axis, 'end_point', x=str(point2.x), y=str(point2.y))


def addStorey(dimensionX, dimensionY, level):
    global xml_storeys, u_storey
    u_storey = u_storey + 1
    if u_storey == 1:
        xml_storeys = ET.SubElement(xml_entities, 'storeys')
    add_storey = ET.SubElement(xml_storeys, 'storey', guid=genGUID(), last_change=genUTC(), action='added', name=('Storey at ' + str(level) + ' m'), dimension_x=str(dimensionX), dimension_y=str(dimensionY))
    ET.SubElement(add_storey, 'origo', x='0', y='0', z=str(level))
    ET.SubElement(add_storey, 'direction', x='1', y='0')


def addBeam(section_INFO, material_ID, point1, point2, rotation, release):
    barName = ID('beam')
    local_y = normal(point1, point2, rotation)
    add_beam = ET.SubElement(xml_root.find('entities'), 'bar', name=barName, type='beam', guid=genGUID(), last_change=genUTC(), action='added')
    add_beam_child = ET.SubElement(add_beam, 'bar_part', guid=genGUID(), last_change=genUTC(), action='added', name=(barName + '.1'), complex_material=material_ID, complex_section=section_INFO[1], ecc_calc='true')
    if section_INFO[2] != 'False':
        add_beam_child.attrib['made'] = section_INFO[2]
    add_beam_grandchild = ET.SubElement(add_beam_child, 'curve', type='line')
    ET.SubElement(add_beam_grandchild, 'point', x=str(point1.x), y=str(point1.y), z=str(point1.z))
    ET.SubElement(add_beam_grandchild, 'point', x=str(point2.x), y=str(point2.y), z=str(point2.z))
    ET.SubElement(add_beam_child, 'local-y', x=str(local_y[0]), y=str(local_y[1]), z=str(local_y[2]))
    if release == 'hinged':
        ET.SubElement(add_beam_child, 'connectivity', m_x='true', m_y='true', m_z='true', r_x='true', r_y='false', r_z='false')
        ET.SubElement(add_beam_child, 'connectivity', m_x='true', m_y='true', m_z='true', r_x='true', r_y='false', r_z='false')
    else:
        ET.SubElement(add_beam_child, 'connectivity', m_x='true', m_y='true', m_z='true', r_x='true', r_y='true', r_z='true')
        ET.SubElement(add_beam_child, 'connectivity', m_x='true', m_y='true', m_z='true', r_x='true', r_y='true', r_z='true')
    add_beam_ecc = ET.SubElement(add_beam_child, 'eccentricity', use_default_physical_alignment='false')
    ET.SubElement(add_beam_ecc, 'analytical', x=str(0), y=str(section_INFO[3].y), z=str(section_INFO[3].z))
    ET.SubElement(add_beam_ecc, 'analytical', x=str(0), y=str(section_INFO[3].y), z=str(section_INFO[3].z))
    ET.SubElement(add_beam_ecc, 'physical', x=str(0), y=str(section_INFO[3].y), z=str(section_INFO[3].z))
    ET.SubElement(add_beam_ecc, 'physical', x=str(0), y=str(section_INFO[3].y), z=str(section_INFO[3].z))
    ET.SubElement(add_beam_child, 'end')
    ET.SubElement(add_beam, 'end')
    xml_bars.append(add_beam)


def addColumn(section_INFO, material_ID, point1, height, rotation, release):
    barName = ID('column')
    local_y = normal(point1, coord(point1.x, point1.y, point1.z + height), rotation)
    add_column = ET.SubElement(xml_root.find('entities'), 'bar', name=barName, type='column', guid=genGUID(), last_change=genUTC(), action='added')
    add_column_child = ET.SubElement(add_column, 'bar_part', guid=genGUID(), last_change=genUTC(), action='added', name=(barName + '.1'), complex_material=material_ID, complex_section=section_INFO[1], ecc_calc='true')
    if section_INFO[2] != 'False':
        add_column_child.attrib['made'] = section_INFO[2]
    add_column_grandchild = ET.SubElement(add_column_child, 'curve', type='line')
    ET.SubElement(add_column_grandchild, 'point', x=str(point1.x), y=str(point1.y), z=str(point1.z))
    ET.SubElement(add_column_grandchild, 'point', x=str(point1.x), y=str(point1.y), z=str(point1.z + height))
    ET.SubElement(add_column_child, 'local-y', x=str(local_y[0]), y=str(local_y[1]), z=str(local_y[2]))
    if release == 'hinged':
        ET.SubElement(add_column_child, 'connectivity', m_x='true', m_y='true', m_z='true', r_x='true', r_y='false', r_z='false')
        ET.SubElement(add_column_child, 'connectivity', m_x='true', m_y='true', m_z='true', r_x='true', r_y='false', r_z='false')
    else:
        ET.SubElement(add_column_child, 'connectivity', m_x='true', m_y='true', m_z='true', r_x='true', r_y='true', r_z='true')
        ET.SubElement(add_column_child, 'connectivity', m_x='true', m_y='true', m_z='true', r_x='true', r_y='true', r_z='true')
    add_column_ecc = ET.SubElement(add_column_child, 'eccentricity', use_default_physical_alignment='false')
    ET.SubElement(add_column_ecc, 'analytical', x=str(0), y=str(section_INFO[3].y), z=str(section_INFO[3].z))
    ET.SubElement(add_column_ecc, 'analytical', x=str(0), y=str(section_INFO[3].y), z=str(section_INFO[3].z))
    ET.SubElement(add_column_ecc, 'physical', x=str(0), y=str(section_INFO[3].y), z=str(section_INFO[3].z))
    ET.SubElement(add_column_ecc, 'physical', x=str(0), y=str(section_INFO[3].y), z=str(section_INFO[3].z))
    ET.SubElement(add_column_child, 'end')
    ET.SubElement(add_column, 'end')
    xml_bars.append(add_column)


def addTruss(section_INFO, material_ID, point1, point2):
    barName = ID('truss')
    local_y = normal(point1, point2, 0)
    add_truss = ET.SubElement(xml_root.find('entities'), 'bar', name=barName, type='truss', guid=genGUID(), last_change=genUTC(), action='added')
    add_truss_child = ET.SubElement(add_truss, 'bar_part', guid=genGUID(), last_change=genUTC(), action='added', name=(barName + '.1'), complex_material=material_ID, complex_section=section_INFO[0])
    if section_INFO[2] != 'False':
        add_truss_child.attrib['made'] = section_INFO[2]
    add_truss_grandchild = ET.SubElement(add_truss_child, 'curve', type='line')
    ET.SubElement(add_truss_grandchild, 'point', x=str(point1.x), y=str(point1.y), z=str(point1.z))
    ET.SubElement(add_truss_grandchild, 'point', x=str(point2.x), y=str(point2.y), z=str(point2.z))
    ET.SubElement(add_truss_child, 'local-y', x=str(local_y[0]), y=str(local_y[1]), z=str(local_y[2]))
    ET.SubElement(add_truss_child, 'end')
    ET.SubElement(add_truss, 'end')
    xml_bars.append(add_truss)


def addPlate(material_ID, thickness, release, point1, point2):
    shellName = ID('plate')
    add_plate = ET.SubElement(xml_root.find('entities'), 'slab', name=shellName, guid=genGUID(), last_change=genUTC(), action='added', type='plate')
    add_plate_child = ET.SubElement(add_plate, 'slab_part', guid=genGUID(), last_change=genUTC(), action='added', name=(shellName + '.1'), complex_material=material_ID, alignment='center', align_offset='0', ortho_alfa='0', ortho_ratio='1')
    add_plate_grandchild = ET.SubElement(add_plate_child, 'contour')
    add_plate_greatgrandchild1 = ET.SubElement(add_plate_grandchild, 'edge', type='line')
    ET.SubElement(add_plate_greatgrandchild1, 'point', x=str(point1.x), y=str(point1.y), z=str(point1.z))
    ET.SubElement(add_plate_greatgrandchild1, 'point', x=str(point2.x), y=str(point1.y), z=str(point2.z))
    if release == 'hinged':
        edgeName = ID('edge')
        add_hinge = ET.SubElement(add_plate_greatgrandchild1, 'edge_connection', guid=genGUID(), last_change=genUTC(), action='added', name=edgeName, moving_local='true', joined_start_point='true', joined_end_point='true')
        add_hinge_child = ET.SubElement(add_hinge, 'rigidity')
        ET.SubElement(add_hinge_child, 'motions', x_neg='10000000', x_pos='10000000', y_neg='10000000', y_pos='10000000', z_neg='10000000', z_pos='10000000')
        ET.SubElement(add_hinge_child, 'rotations', x_neg='0', x_pos='0', y_neg='0', y_pos='0', z_neg='0', z_pos='0')
    add_plate_greatgrandchild2 = ET.SubElement(add_plate_grandchild, 'edge', type='line')
    ET.SubElement(add_plate_greatgrandchild2, 'point', x=str(point2.x), y=str(point1.y), z=str(point2.z))
    ET.SubElement(add_plate_greatgrandchild2, 'point', x=str(point2.x), y=str(point2.y), z=str(point2.z))
    if release == 'hinged':
        edgeName = ID('edge')
        add_hinge = ET.SubElement(add_plate_greatgrandchild2, 'edge_connection', guid=genGUID(), last_change=genUTC(), action='added', name=edgeName, moving_local='true', joined_start_point='true', joined_end_point='true')
        add_hinge_child = ET.SubElement(add_hinge, 'rigidity')
        ET.SubElement(add_hinge_child, 'motions', x_neg='10000000', x_pos='10000000', y_neg='10000000', y_pos='10000000', z_neg='10000000', z_pos='10000000')
        ET.SubElement(add_hinge_child, 'rotations', x_neg='0', x_pos='0', y_neg='0', y_pos='0', z_neg='0', z_pos='0')
    add_plate_greatgrandchild3 = ET.SubElement(add_plate_grandchild, 'edge', type='line')
    ET.SubElement(add_plate_greatgrandchild3, 'point', x=str(point2.x), y=str(point2.y), z=str(point2.z))
    ET.SubElement(add_plate_greatgrandchild3, 'point', x=str(point1.x), y=str(point2.y), z=str(point1.z))
    if release == 'hinged':
        edgeName = ID('edge')
        add_hinge = ET.SubElement(add_plate_greatgrandchild3, 'edge_connection', guid=genGUID(), last_change=genUTC(), action='added', name=edgeName, moving_local='true', joined_start_point='true', joined_end_point='true')
        add_hinge_child = ET.SubElement(add_hinge, 'rigidity')
        ET.SubElement(add_hinge_child, 'motions', x_neg='10000000', x_pos='10000000', y_neg='10000000', y_pos='10000000', z_neg='10000000', z_pos='10000000')
        ET.SubElement(add_hinge_child, 'rotations', x_neg='0', x_pos='0', y_neg='0', y_pos='0', z_neg='0', z_pos='0')
    add_plate_greatgrandchild4 = ET.SubElement(add_plate_grandchild, 'edge', type='line')
    ET.SubElement(add_plate_greatgrandchild4, 'point', x=str(point1.x), y=str(point2.y), z=str(point1.z))
    ET.SubElement(add_plate_greatgrandchild4, 'point', x=str(point1.x), y=str(point1.y), z=str(point1.z))
    if release == 'hinged':
        edgeName = ID('edge')
        add_hinge = ET.SubElement(add_plate_greatgrandchild4, 'edge_connection', guid=genGUID(), last_change=genUTC(), action='added', name=edgeName, moving_local='true', joined_start_point='true', joined_end_point='true')
        add_hinge_child = ET.SubElement(add_hinge, 'rigidity')
        ET.SubElement(add_hinge_child, 'motions', x_neg='10000000', x_pos='10000000', y_neg='10000000', y_pos='10000000', z_neg='10000000', z_pos='10000000')
        ET.SubElement(add_hinge_child, 'rotations', x_neg='0', x_pos='0', y_neg='0', y_pos='0', z_neg='0', z_pos='0')
    ET.SubElement(add_plate_child, 'thickness', x='0', y='0', z='0', val=str(thickness))
    ET.SubElement(add_plate_child, 'local_pos', x=str(point1.x), y=str(point1.y), z=str(point1.z)) # The position of the local coordinate system should be any point inside the plate, in this case in the first point
    ET.SubElement(add_plate_child, 'local_x', x='1', y='0', z='0') # The direction of the local system is hard coded to be the same as the global coordinate system
    ET.SubElement(add_plate_child, 'local_y', x='0', y='1', z='0') # The direction of the local system is hard coded to be the same as the global coordinate system
    ET.SubElement(add_plate_child, 'end')
    ET.SubElement(add_plate, 'end')
    xml_shells.append(add_plate)


def addPlateComplex(material_ID, thickness, release, *points):
    shellName = ID('plate')
    add_plate = ET.SubElement(xml_root.find('entities'), 'slab', name=shellName, guid=genGUID(), last_change=genUTC(), action='added', type='plate')
    add_plate_child = ET.SubElement(add_plate, 'slab_part', guid=genGUID(), last_change=genUTC(), action='added', name=(shellName + '.1'), complex_material=material_ID, alignment='center', align_offset='0', ortho_alfa='0', ortho_ratio='1')
    add_plate_grandchild = ET.SubElement(add_plate_child, 'contour')
    for j in range(len(points)):
        add_plate_greatgrandchild = ET.SubElement(add_plate_grandchild, 'edge', type='line')
        ET.SubElement(add_plate_greatgrandchild, 'point', x=str(points[j].x), y=str(points[j].y), z=str(points[j].z))
        if j == len(points)-1:
            ET.SubElement(add_plate_greatgrandchild, 'point', x=str(points[0].x), y=str(points[0].y), z=str(points[0].z))
        elif j != len(points)-1:
            ET.SubElement(add_plate_greatgrandchild, 'point', x=str(points[j+1].x), y=str(points[j+1].y), z=str(points[j+1].z))
        if release == 'hinged':
            edgeName = ID('edge')
            add_hinge = ET.SubElement(add_plate_greatgrandchild, 'edge_connection', guid=genGUID(), last_change=genUTC(), action='added', name=edgeName, moving_local='true', joined_start_point='true', joined_end_point='true')
            add_hinge_child = ET.SubElement(add_hinge, 'rigidity')
            ET.SubElement(add_hinge_child, 'motions', x_neg='10000000', x_pos='10000000', y_neg='10000000', y_pos='10000000', z_neg='10000000', z_pos='10000000')
            ET.SubElement(add_hinge_child, 'rotations', x_neg='0', x_pos='0', y_neg='0', y_pos='0', z_neg='0', z_pos='0')
    ET.SubElement(add_plate_child, 'thickness', x='0', y='0', z='0', val=str(thickness))
    ET.SubElement(add_plate_child, 'local_pos', x=str(points[0].x), y=str(points[0].y), z=str(points[0].z)) # The position of the local coordinate system should be any point inside the plate, in this case in the first point
    ET.SubElement(add_plate_child, 'local_x', x='1', y='0', z='0') # The direction of the local system is hard coded to be the same as the global coordinate system
    ET.SubElement(add_plate_child, 'local_y', x='0', y='1', z='0') # The direction of the local system is hard coded to be the same as the global coordinate system
    ET.SubElement(add_plate_child, 'end')
    ET.SubElement(add_plate, 'end')
    xml_shells.append(add_plate)


def addWall(material_ID, thickness, release, point1, point2):
    shellName = ID('wall')
    add_wall = ET.SubElement(xml_root.find('entities'), 'slab', name=shellName, guid=genGUID(), last_change=genUTC(), action='added', type='wall')
    add_wall_child = ET.SubElement(add_wall, 'slab_part', guid=genGUID(), last_change=genUTC(), action='added', name=(shellName + '.1'), complex_material=material_ID, alignment='center', align_offset='0', ortho_alfa='0', ortho_ratio='1')
    add_wall_grandchild = ET.SubElement(add_wall_child, 'contour')
    add_wall_greatgrandchild1 = ET.SubElement(add_wall_grandchild, 'edge', type='line')
    ET.SubElement(add_wall_greatgrandchild1, 'point', x=str(point1.x), y=str(point1.y), z=str(point1.z))
    ET.SubElement(add_wall_greatgrandchild1, 'point', x=str(point2.x), y=str(point2.y), z=str(point1.z))
    if release == 'hinged':
        edgeName = ID('edge')
        add_hinge = ET.SubElement(add_wall_greatgrandchild1, 'edge_connection', guid=genGUID(), last_change=genUTC(), action='added', name=edgeName, moving_local='true', joined_start_point='true', joined_end_point='true')
        add_hinge_child = ET.SubElement(add_hinge, 'rigidity')
        ET.SubElement(add_hinge_child, 'motions', x_neg='10000000', x_pos='10000000', y_neg='10000000', y_pos='10000000', z_neg='10000000', z_pos='10000000')
        ET.SubElement(add_hinge_child, 'rotations', x_neg='0', x_pos='0', y_neg='0', y_pos='0', z_neg='0', z_pos='0')
    add_wall_greatgrandchild2 = ET.SubElement(add_wall_grandchild, 'edge', type='line')
    ET.SubElement(add_wall_greatgrandchild2, 'point', x=str(point2.x), y=str(point2.y), z=str(point1.z))
    ET.SubElement(add_wall_greatgrandchild2, 'point', x=str(point2.x), y=str(point2.y), z=str(point2.z))
    if release == 'hinged':
        edgeName = ID('edge')
        add_hinge = ET.SubElement(add_wall_greatgrandchild2, 'edge_connection', guid=genGUID(), last_change=genUTC(), action='added', name=edgeName, moving_local='true', joined_start_point='true', joined_end_point='true')
        add_hinge_child = ET.SubElement(add_hinge, 'rigidity')
        ET.SubElement(add_hinge_child, 'motions', x_neg='10000000', x_pos='10000000', y_neg='10000000', y_pos='10000000', z_neg='10000000', z_pos='10000000')
        ET.SubElement(add_hinge_child, 'rotations', x_neg='0', x_pos='0', y_neg='0', y_pos='0', z_neg='0', z_pos='0')
    add_wall_greatgrandchild3 = ET.SubElement(add_wall_grandchild, 'edge', type='line')
    ET.SubElement(add_wall_greatgrandchild3, 'point', x=str(point2.x), y=str(point2.y), z=str(point2.z))
    ET.SubElement(add_wall_greatgrandchild3, 'point', x=str(point1.x), y=str(point1.y), z=str(point2.z))
    if release == 'hinged':
        edgeName = ID('edge')
        add_hinge = ET.SubElement(add_wall_greatgrandchild3, 'edge_connection', guid=genGUID(), last_change=genUTC(), action='added', name=edgeName, moving_local='true', joined_start_point='true', joined_end_point='true')
        add_hinge_child = ET.SubElement(add_hinge, 'rigidity')
        ET.SubElement(add_hinge_child, 'motions', x_neg='10000000', x_pos='10000000', y_neg='10000000', y_pos='10000000', z_neg='10000000', z_pos='10000000')
        ET.SubElement(add_hinge_child, 'rotations', x_neg='0', x_pos='0', y_neg='0', y_pos='0', z_neg='0', z_pos='0')
    add_wall_greatgrandchild4 = ET.SubElement(add_wall_grandchild, 'edge', type='line')
    ET.SubElement(add_wall_greatgrandchild4, 'point', x=str(point1.x), y=str(point1.y), z=str(point2.z))
    ET.SubElement(add_wall_greatgrandchild4, 'point', x=str(point1.x), y=str(point1.y), z=str(point1.z))
    if release == 'hinged':
        edgeName = ID('edge')
        add_hinge = ET.SubElement(add_wall_greatgrandchild4, 'edge_connection', guid=genGUID(), last_change=genUTC(), action='added', name=edgeName, moving_local='true', joined_start_point='true', joined_end_point='true')
        add_hinge_child = ET.SubElement(add_hinge, 'rigidity')
        ET.SubElement(add_hinge_child, 'motions', x_neg='10000000', x_pos='10000000', y_neg='10000000', y_pos='10000000', z_neg='10000000', z_pos='10000000')
        ET.SubElement(add_hinge_child, 'rotations', x_neg='0', x_pos='0', y_neg='0', y_pos='0', z_neg='0', z_pos='0')
    ET.SubElement(add_wall_child, 'thickness', x='0', y='0', z='0', val=str(thickness))
    ET.SubElement(add_wall_child, 'local_pos', x=str(point1.x), y=str(point1.y), z=str(point1.z)) # The position of the local coordinate system should be any point inside the plate, in this case in the first point
    local_y = normal(point2, point1, 0)
    ET.SubElement(add_wall_child, 'local_x', x=str(local_y[0]), y=str(local_y[1]), z=str(local_y[2]))
    ET.SubElement(add_wall_child, 'local_y', x=str(0), y=str(0), z=str(1))
    ET.SubElement(add_wall_child, 'end')
    ET.SubElement(add_wall, 'end')
    xml_shells.append(add_wall)


def addPointSupport(point, type):
    supportName = ID('support')
    add_pointsupport_root = xml_root.find('entities')
    add_pointsupport = ET.SubElement(add_pointsupport_root[1], 'point_support', guid=genGUID(), last_change=genUTC(), action='added', name=supportName)
    add_pointsupport_child = ET.SubElement(add_pointsupport, 'group')
    ET.SubElement(add_pointsupport_child, 'local_x', x='1', y='0', z='0')
    ET.SubElement(add_pointsupport_child, 'local_y', x='0', y='1', z='0')
    add_pointsupport_grandchild = ET.SubElement(add_pointsupport_child, 'rigidity')
    if type == 'fixed':
        ET.SubElement(add_pointsupport_grandchild, 'motions', x_neg='10000000000', x_pos='10000000000', y_neg='10000000000', y_pos='10000000000', z_neg='10000000000', z_pos='10000000000')
        ET.SubElement(add_pointsupport_grandchild, 'rotations', x_neg='10000000000', x_pos='10000000000', y_neg='10000000000', y_pos='10000000000', z_neg='10000000000', z_pos='10000000000')
    else:
        ET.SubElement(add_pointsupport_grandchild, 'motions', x_neg='10000000000', x_pos='10000000000', y_neg='10000000000', y_pos='10000000000', z_neg='10000000000', z_pos='10000000000')
        ET.SubElement(add_pointsupport_grandchild, 'rotations', x_neg='0', x_pos='0', y_neg='0', y_pos='0', z_neg='0', z_pos='0')
    ET.SubElement(add_pointsupport, 'position', x=str(point.x), y=str(point.y), z=str(point.z))
    xml_pointsupports.append(add_pointsupport)


def addLineSupport(point1, point2, type):
    supportName = ID('support')
    add_linesupport_root = xml_root.find('entities')
    add_linesupport = ET.SubElement(add_linesupport_root[1], 'line_support', guid=genGUID(), last_change=genUTC(), action='added', name=supportName)
    add_linesupport_child = ET.SubElement(add_linesupport, 'group')
    local_y = normal(point1, point2, 0)
    ET.SubElement(add_linesupport_child, 'local_x', x='0', y='0', z='1')
    ET.SubElement(add_linesupport_child, 'local_y', x=str(local_y[0]), y=str(local_y[1]), z=str(local_y[2]))
    add_linesupport_grandchild = ET.SubElement(add_linesupport_child, 'rigidity')
    if type == 'fixed':
        ET.SubElement(add_linesupport_grandchild, 'motions', x_neg='10000000000', x_pos='10000000000', y_neg='10000000000', y_pos='10000000000', z_neg='10000000000', z_pos='10000000000')
        ET.SubElement(add_linesupport_grandchild, 'rotations', x_neg='10000000000', x_pos='10000000000', y_neg='10000000000', y_pos='10000000000', z_neg='10000000000', z_pos='10000000000')
    else:
        ET.SubElement(add_linesupport_grandchild, 'motions', x_neg='10000000000', x_pos='10000000000', y_neg='10000000000', y_pos='10000000000', z_neg='10000000000', z_pos='10000000000')
        ET.SubElement(add_linesupport_grandchild, 'rotations', x_neg='0', x_pos='0', y_neg='0', y_pos='0', z_neg='0', z_pos='0')
    add_linesupport_edge = ET.SubElement(add_linesupport, 'edge', type='line')
    ET.SubElement(add_linesupport_edge, 'point', x=str(point1.x), y=str(point1.y), z=str(point1.z))
    ET.SubElement(add_linesupport_edge, 'point', x=str(point2.x), y=str(point2.y), z=str(point2.z))
    ET.SubElement(add_linesupport_edge, 'normal', x=str(local_y[0]), y=str(local_y[1]), z=str(local_y[2]))
    ET.SubElement(add_linesupport, 'normal', x='0', y='0', z='1')
    xml_linesupports.append(add_linesupport)


def addSurfaceSupport(point1, point2):
    supportName = ID('support')
    add_surfacesupport_root = xml_root.find('entities')
    add_surfacesupport = ET.SubElement(add_surfacesupport_root[1], 'surface_support', guid=genGUID(), last_change=genUTC(), action='added', name=supportName)
    add_surfacesupport_child = ET.SubElement(add_surfacesupport, 'region')
    add_surfacesupport_grandchild = ET.SubElement(add_surfacesupport_child, 'contour')
    add_surfacesupport_greatgrandchild1 = ET.SubElement(add_surfacesupport_grandchild, 'edge', type='line')
    ET.SubElement(add_surfacesupport_greatgrandchild1, 'point', x=str(point1.x), y=str(point1.y), z=str(point1.z))
    ET.SubElement(add_surfacesupport_greatgrandchild1, 'point', x=str(point2.x), y=str(point1.y), z=str(point2.z))
    add_surfacesupport_greatgrandchild2 = ET.SubElement(add_surfacesupport_grandchild, 'edge', type='line')
    ET.SubElement(add_surfacesupport_greatgrandchild2, 'point', x=str(point2.x), y=str(point1.y), z=str(point2.z))
    ET.SubElement(add_surfacesupport_greatgrandchild2, 'point', x=str(point2.x), y=str(point2.y), z=str(point2.z))
    add_surfacesupport_greatgrandchild3 = ET.SubElement(add_surfacesupport_grandchild, 'edge', type='line')
    ET.SubElement(add_surfacesupport_greatgrandchild3, 'point', x=str(point2.x), y=str(point2.y), z=str(point2.z))
    ET.SubElement(add_surfacesupport_greatgrandchild3, 'point', x=str(point1.x), y=str(point2.y), z=str(point1.z))
    add_surfacesupport_greatgrandchild4 = ET.SubElement(add_surfacesupport_grandchild, 'edge', type='line')
    ET.SubElement(add_surfacesupport_greatgrandchild4, 'point', x=str(point1.x), y=str(point2.y), z=str(point1.z))
    ET.SubElement(add_surfacesupport_greatgrandchild4, 'point', x=str(point1.x), y=str(point1.y), z=str(point1.z))
    add_surfacesupport_motion = ET.SubElement(add_surfacesupport, 'rigidity')
    ET.SubElement(add_surfacesupport_motion, 'motions', x_neg='10000000000', x_pos='10000000000', y_neg='10000000000', y_pos='10000000000', z_neg='10000000000', z_pos='10000000000')
    xml_surfacesupports.append(add_surfacesupport)


def addCover(point1, point2):
    coverName = ID('cover')
    add_cover_root = xml_root.find('entities')
    add_cover = ET.SubElement(add_cover_root[2], 'cover', guid=genGUID(), last_change=genUTC(), action='added', name=coverName)
    add_cover_child = ET.SubElement(add_cover, 'region')
    add_cover_grandchild = ET.SubElement(add_cover_child, 'contour')
    add_cover_greatgrandchild1 = ET.SubElement(add_cover_grandchild, 'edge', type='line')
    ET.SubElement(add_cover_greatgrandchild1, 'point', x=str(point1.x), y=str(point1.y), z=str(point1.z))
    ET.SubElement(add_cover_greatgrandchild1, 'point', x=str(point2.x), y=str(point1.y), z=str(point2.z))
    add_cover_greatgrandchild2 = ET.SubElement(add_cover_grandchild, 'edge', type='line')
    ET.SubElement(add_cover_greatgrandchild2, 'point', x=str(point2.x), y=str(point1.y), z=str(point2.z))
    ET.SubElement(add_cover_greatgrandchild2, 'point', x=str(point2.x), y=str(point2.y), z=str(point2.z))
    add_cover_greatgrandchild3 = ET.SubElement(add_cover_grandchild, 'edge', type='line')
    ET.SubElement(add_cover_greatgrandchild3, 'point', x=str(point2.x), y=str(point2.y), z=str(point2.z))
    ET.SubElement(add_cover_greatgrandchild3, 'point', x=str(point1.x), y=str(point2.y), z=str(point1.z))
    add_cover_greatgrandchild4 = ET.SubElement(add_cover_grandchild, 'edge', type='line')
    ET.SubElement(add_cover_greatgrandchild4, 'point', x=str(point1.x), y=str(point2.y), z=str(point1.z))
    ET.SubElement(add_cover_greatgrandchild4, 'point', x=str(point1.x), y=str(point1.y), z=str(point1.z))


def addPointLoad(force, direction, loadCase, point):
    add_pointload_root = xml_root.find('entities')
    add_pointload = ET.SubElement(add_pointload_root[0], 'point_load', load_case=loadCase, guid=genGUID(), last_change=genUTC(), action='added', load_type='force')
    ET.SubElement(add_pointload, 'direction', x=str(direction.x), y=str(direction.y), z=str(direction.z))
    ET.SubElement(add_pointload, 'load', x=str(point.x), y=str(point.y), z=str(point.z), val=str(force))
    xml_pointloads.append(add_pointload)


def addLineLoad(force, direction, loadCase, point1, point2):
    add_lineload_root = xml_root.find('entities')
    add_lineload = ET.SubElement(add_lineload_root[0], 'line_load', load_case=loadCase, guid=genGUID(), last_change=genUTC(), action='added', load_dir='constant', load_projection='false', load_type='force')
    add_lineload_child = ET.SubElement(add_lineload, 'edge', type='line')
    ET.SubElement(add_lineload_child, 'point', x=str(point1.x), y=str(point1.y), z=str(point1.z))
    ET.SubElement(add_lineload_child, 'point', x=str(point2.x), y=str(point2.y), z=str(point2.z))
    normalx = normal(point1, point2, 0)
    ET.SubElement(add_lineload_child, 'normal', x=str(normalx[0]), y=str(normalx[1]), z=str(normalx[2]))
    ET.SubElement(add_lineload, 'direction', x=str(direction.x), y=str(direction.y), z=str(direction.z))
    ET.SubElement(add_lineload, 'normal', x=str(normalx[0]), y=str(normalx[1]), z=str(normalx[2]))
    ET.SubElement(add_lineload, 'load', x=str(point1.x), y=str(point1.y), z=str(point1.z), val=str(force))
    ET.SubElement(add_lineload, 'load', x=str(point2.x), y=str(point2.y), z=str(point2.z), val=str(force))
    xml_lineloads.append(add_lineload)


def addSurfaceLoad(force, direction, loadCase, point1, point2):
    add_surfaceload_root = xml_root.find('entities')
    add_surfaceload = ET.SubElement(add_surfaceload_root[0], 'surface_load', load_case=loadCase, guid=genGUID(), last_change=genUTC(), action='added', load_projection='false', load_type='force')
    add_surfaceload_child = ET.SubElement(add_surfaceload, 'region')
    add_surfaceload_grandchild = ET.SubElement(add_surfaceload_child, 'contour')
    add_surfaceload_greatgrandchild1 = ET.SubElement(add_surfaceload_grandchild, 'edge', type='line')
    ET.SubElement(add_surfaceload_greatgrandchild1, 'point', x=str(point1.x), y=str(point1.y), z=str(point1.z))
    ET.SubElement(add_surfaceload_greatgrandchild1, 'point', x=str(point2.x), y=str(point1.y), z=str(point2.z))
    add_surfaceload_greatgrandchild2 = ET.SubElement(add_surfaceload_grandchild, 'edge', type='line')
    ET.SubElement(add_surfaceload_greatgrandchild2, 'point', x=str(point2.x), y=str(point1.y), z=str(point2.z))
    ET.SubElement(add_surfaceload_greatgrandchild2, 'point', x=str(point2.x), y=str(point2.y), z=str(point2.z))
    add_surfaceload_greatgrandchild3 = ET.SubElement(add_surfaceload_grandchild, 'edge', type='line')
    ET.SubElement(add_surfaceload_greatgrandchild3, 'point', x=str(point2.x), y=str(point2.y), z=str(point2.z))
    ET.SubElement(add_surfaceload_greatgrandchild3, 'point', x=str(point1.x), y=str(point2.y), z=str(point1.z))
    add_surfaceload_greatgrandchild4 = ET.SubElement(add_surfaceload_grandchild, 'edge', type='line')
    ET.SubElement(add_surfaceload_greatgrandchild4, 'point', x=str(point1.x), y=str(point2.y), z=str(point1.z))
    ET.SubElement(add_surfaceload_greatgrandchild4, 'point', x=str(point1.x), y=str(point1.y), z=str(point1.z))
    ET.SubElement(add_surfaceload, 'direction', x=str(direction.x), y=str(direction.y), z=str(direction.z))
    ET.SubElement(add_surfaceload, 'load', x=str(point1.x), y=str(point1.y), z=str(point1.z), val=str(force))
    xml_surfaceloads.append(add_surfaceload)


def addSurfaceLoadComplex(force, direction, loadCase, *points):
    add_surfaceload_root = xml_root.find('entities')
    add_surfaceload = ET.SubElement(add_surfaceload_root[0], 'surface_load', load_case=loadCase, guid=genGUID(), last_change=genUTC(), action='added', load_projection='false', load_type='force')
    add_surfaceload_child = ET.SubElement(add_surfaceload, 'region')
    add_surfaceload_grandchild = ET.SubElement(add_surfaceload_child, 'contour')
    for j in range(len(points)):
        add_surfaceload_greatgrandchild = ET.SubElement(add_surfaceload_grandchild, 'edge', type='line')
        ET.SubElement(add_surfaceload_greatgrandchild, 'point', x=str(points[j].x), y=str(points[j].y), z=str(points[j].z))
        if j == len(points)-1:
            ET.SubElement(add_surfaceload_greatgrandchild, 'point', x=str(points[0].x), y=str(points[0].y), z=str(points[0].z))
        elif j != len(points)-1:
            ET.SubElement(add_surfaceload_greatgrandchild, 'point', x=str(points[j+1].x), y=str(points[j+1].y), z=str(points[j+1].z))
    ET.SubElement(add_surfaceload, 'direction', x=str(direction.x), y=str(direction.y), z=str(direction.z))
    ET.SubElement(add_surfaceload, 'load', x=str(points[0].x), y=str(points[0].y), z=str(points[0].z), val=str(force))
    xml_surfaceloads.append(add_surfaceload)


def addLoadCase(name, selfWeight):
    global xml_loadcases
    loadCaseID = genGUID()
    add_loadcase_root = xml_root.find('entities')
    add_loadcase = ET.SubElement(add_loadcase_root[0], 'load_case', duration_class='permanent', guid=loadCaseID, last_change=genUTC(), action='added', name=name)
    if selfWeight == True:
        add_loadcase.attrib['type'] = 'dead_load'
    else:
        add_loadcase.attrib['type'] = 'static'
    xml_loadcases.append(add_loadcase)
    return loadCaseID


def addLoadComb(nameLoadComb, type, loadCaseList, gammaLoadCases):
    global xml_loadcombs, nrcomb
    nrcomb = nrcomb + 1
    add_loadcase_root = xml_root.find('entities')
    add_loadcomb = ET.SubElement(add_loadcase_root[0], 'load_combination', guid=genGUID(), last_change=genUTC(), action='added', name=nameLoadComb)
    if type == 'U':
        add_loadcomb.attrib['type'] = 'ultimate_ordinary'
    if type == 'Ua':
        add_loadcomb.attrib['type'] = 'ultimate_accidental'
    if type == 'Us':
        add_loadcomb.attrib['type'] = 'ultimate_seismic'
    if type == 'Sq':
        add_loadcomb.attrib['type'] = 'serviceability_quasi_permanent'
    if type == 'Sf':
        add_loadcomb.attrib['type'] = 'serviceability_frequent'
    if type == 'Sc':
        add_loadcomb.attrib['type'] = 'serviceability_characteristic'
    for i_LC in range(len(loadCaseList)):
        ET.SubElement(add_loadcomb, 'load_case', guid=loadCaseList[i_LC], gamma=str(gammaLoadCases[i_LC]))
    xml_loadcombs.append(add_loadcomb)