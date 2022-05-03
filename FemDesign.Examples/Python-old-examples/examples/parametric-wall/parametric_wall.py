#!/usr/bin/env python3
import xml.etree.ElementTree as et
et.register_namespace('', 'urn:strusoft')

# -------------- INPUT --------------
windowX = 2
windowY = 2
windowW = 1
connectionCapacity = 20
wallLength = 8
wallHeight = 5
doorPos = 4
# -----------------------------------

model_file = 'C:\\temp\\strusoft\\wall\\wall.struxml'

tree = et.parse(model_file)
root = tree.getroot()
contour_wall = root[0][0][0][0]     # This will shortening the path to the contour of the wall
contour_window = root[0][0][0][1]   # This will shortening the path to the contour of the window
connection_pos = root[0][1]         # This will shortening the path to the connection position coordinates

# -------------- WALL GEOMETRY --------------
contour_wall[0][1].attrib['x'] = str(doorPos)

contour_wall[1][0].attrib['x'] = str(doorPos)
contour_wall[1][1].attrib['x'] = str(doorPos)
contour_wall[1][1].attrib['z'] = str(2)

contour_wall[2][0].attrib['x'] = str(doorPos)
contour_wall[2][0].attrib['z'] = str(2)
contour_wall[2][1].attrib['x'] = str(doorPos+1)
contour_wall[2][1].attrib['z'] = str(2)

contour_wall[3][0].attrib['x'] = str(doorPos+1)
contour_wall[3][0].attrib['z'] = str(2)
contour_wall[3][1].attrib['x'] = str(doorPos+1)

contour_wall[4][0].attrib['x'] = str(doorPos+1)
contour_wall[4][1].attrib['x'] = str(wallLength)

contour_wall[5][0].attrib['x'] = str(wallLength)
contour_wall[5][1].attrib['x'] = str(wallLength)
contour_wall[5][1].attrib['z'] = str(wallHeight)

contour_wall[6][0].attrib['x'] = str(wallLength)
contour_wall[6][0].attrib['z'] = str(wallHeight)
contour_wall[6][1].attrib['z'] = str(wallHeight)

contour_wall[7][0].attrib['z'] = str(wallHeight)

# -------------- WINDOW GEOMETRY --------------
contour_window[0][0].attrib['x'] = str(windowX+windowW)
contour_window[0][0].attrib['z'] = str(windowY)
contour_window[0][1].attrib['x'] = str(windowX)
contour_window[0][1].attrib['z'] = str(windowY)

contour_window[1][0].attrib['x'] = str(windowX)
contour_window[1][0].attrib['z'] = str(windowY)
contour_window[1][1].attrib['x'] = str(windowX)
contour_window[1][1].attrib['z'] = str(windowY+windowW)

contour_window[2][0].attrib['x'] = str(windowX)
contour_window[2][0].attrib['z'] = str(windowY+windowW)
contour_window[2][1].attrib['x'] = str(windowX+windowW)
contour_window[2][1].attrib['z'] = str(windowY+windowW)

contour_window[3][0].attrib['x'] = str(windowX+windowW)
contour_window[3][0].attrib['z'] = str(windowY+windowW)
contour_window[3][1].attrib['x'] = str(windowX+windowW)
contour_window[3][1].attrib['z'] = str(windowY)

# -------------- CONNECTION POSITION --------------
connection_pos[0][0].attrib['x'] = str(wallLength-0.2)
connection_pos[0][1].attrib['x'] = str(wallLength+0.2)

connection_pos[1][0].attrib['x'] = str(wallLength-0.2)
connection_pos[1][0].attrib['z'] = str(wallHeight-0.8)
connection_pos[1][1].attrib['x'] = str(wallLength+0.2)
connection_pos[1][1].attrib['z'] = str(wallHeight-0.8)

connection_pos[2][0].attrib['z'] = str(wallHeight-0.2)
connection_pos[2][1].attrib['z'] = str(wallHeight+0.2)

connection_pos[3][0].attrib['x'] = str(wallLength-1)
connection_pos[3][0].attrib['z'] = str(wallHeight-0.2)
connection_pos[3][1].attrib['x'] = str(wallLength-1)
connection_pos[3][1].attrib['z'] = str(wallHeight+0.2)

# -------------- PLASTIC LIMIT FORCE --------------
for limit in root.iter('{urn:strusoft}plastic_limit_forces'):
    limit.attrib['x_pos'] = str(connectionCapacity)

tree.write(model_file)