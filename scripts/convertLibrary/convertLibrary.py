import os
import shutil
from pathlib import Path

# add this script as a pre-build event to Grasshopper components solution!

def getCsFiles(dir, lst = []):
    """
    Find cs-files in directory and sub-directories of directory.
    """
    obj = os.scandir(dir)
    for entry in obj:
        if entry.is_dir():
            lst = getCsFiles(entry.path, lst) #recursion call
        elif entry.is_file():
            assert Path(entry.path).suffix == ".cs"
            lst.append(entry.path)
    return lst

def parseCsFileGH(csFile):
    with open(csFile, "r") as f:
        lines = f.readlines()
    content = ""
    dynamoRegion = False
    skipRegion = False
    skipLine = False
    for line in lines:
        if "#region dynamo" in line:
            dynamoRegion = True
            skipRegion = True
        elif "#endregion" in line and dynamoRegion:
            dynamoRegion = False
            skipRegion = False
            skipLine = True
        if "IsVisibleInDynamoLibrary" in line or "IsLacingDisabled" in line:
            skipLine = True
        if skipRegion:
            pass
        elif skipLine:
            skipLine = False
        else:
            content += line
            skipLine = False
    return content

def parseCsFileDynamo(csFile):
    with open(csFile, "r") as f:
        lines = f.readlines()

    content = ""
    ghRegion = False
    skipRegion = False
    skipLine = False
    for line in lines:
        if "#region grasshopper" in line:
            ghRegion = True
            skipRegion = True
        elif "#endregion" in line and ghRegion:
            ghRegion = False
            skipRegion = False
            skipLine = True
        if skipRegion:
            pass
        elif skipLine:
            skipLine = False
        else:
            content += line
            skipLine = False
    return content

def writeNewCsFile(content, destPath, overWrite = False):
    if overWrite:
        param = "w"
    else:
        param = "x"

    try:
        f = open(destPath, param)
        f.write(content)
        f.close

    except FileNotFoundError:
        makeDir(destPath)
        writeNewCsFile(content, destPath, overWrite)

    return None

def makeDir(destPath):
    try:
        _destDir = destPath.split(os.path.basename(destPath))[0]
    except ValueError:
        _destDir = destPath.split(os.path.basename(os.path.dirname(destPath)))[0]

    if not os.path.isdir(_destDir):
        try:
            os.mkdir(_destDir)
        except:
            makeDir(_destDir)

def copyCommonFiles(srcDir, dstDir):
    files = ["LICENSE", "README.md"]
    for _file in files:
        src = srcDir + "\\" + _file
        dst = dstDir + "\\" + _file
        shutil.copyfile(src, dst)

def wrapper(srcDir, destDir, parseGH):
    srcPaths = getCsFiles(srcDir)
    for srcPath in srcPaths:
        destPath = destDir + srcPath.split(srcDir)[-1]
        if parseGH:
            content = parseCsFileGH(srcPath)
        if not parseGH:
            content = parseCsFileDynamo(srcPath)
        writeNewCsFile(content, destPath, True)
    srcPaths.clear()

def convertGH(srcDir, dstDir):
    wrapper(srcDir + "\\src", dstDir + "\\src", parseGH = True)
    wrapper(srcDir + "\\componentsGrasshopper", dstDir + "\\componentsGrasshopper", parseGH = True)
    copyCommonFiles(srcDir, dstDir)

def convertDynamo(srcDir, dstDir):
    wrapper(srcDir +"\\src", dstDir + "\\src", parseGH = False)
    copyCommonFiles(srcDir, dstDir)

if __name__ == "__main__":
    pass
