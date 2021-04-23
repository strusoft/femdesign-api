# https://strusoft.com/
import os
import shutil
from pathlib import Path

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

def readCsContent(csFilePath):
    with open(csFilePath, "r") as f:
        lines = f.readlines()
    return lines

def parseCsContentGH(csContent):
    content = ""
    dynamoRegion = False
    skipRegion = False
    skipLine = False
    for line in csContent:
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

def parseCsContentDyn(csContent):
    content = ""
    ghRegion = False
    skipRegion = False
    skipLine = False
    for line in csContent:
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

def parseCsContentDll(csContent):
    csContent = parseCsContentGH(csContent)
    csContent = csContent.splitlines(True)
    content = parseCsContentDyn(csContent)
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
        src = srcDir + "/" + _file
        dst = dstDir + "/" + _file
        shutil.copyfile(src, dst)

def wrapper(srcDir, destDir, parse):
    srcPaths = getCsFiles(srcDir)
    for srcPath in srcPaths:
        destPath = destDir + srcPath.split(srcDir)[-1]
        csContent = readCsContent(srcPath)

        # parse content
        if parse == "GH":
            content = parseCsContentGH(csContent)
        elif parse == "GH-regions":
            parseCsContentGHRegions(csContent)
        elif parse == "Dyn":
            content = parseCsContentDyn(csContent)
        elif parse == "DLL":
            content = parseCsContentDll(csContent)

        # write new file to destination path
        writeNewCsFile(content, destPath, True)

    # clear list
    srcPaths.clear()

def convertGH(srcDir, dstDir):
    wrapper(srcDir + "/src", dstDir, parse = "GH-regions")
    wrapper(srcDir + "/componentsGrasshopper", dstDir, parse = "GH")
    copyCommonFiles(srcDir, dstDir)

def convertDynamo(srcDir, dstDir):
    wrapper(srcDir + "/src", dstDir, parse = "Dyn")
    copyCommonFiles(srcDir, dstDir)

def convertDLL(srcDir, dstDir):
    wrapper(srcDir + "/src", dstDir, parse = "DLL")
    copyCommonFiles(srcDir, dstDir)

if __name__ == "__main__":
    print("Converting to separate projects")
    src_dir = "."
    dll_dir = os.path.abspath("core")
    gh_dir = os.path.abspath("grasshopper")
    dynamo_dir = os.path.abspath("dynamo")
    
    # convertDLL(src_dir, dll_dir)
    # print(f"➕ Core project at {dll_dir}")

    convertGH(src_dir, gh_dir)
    print(f"➕ Grasshopper project at {gh_dir}")

    # convertDynamo(src_dir, dynamo_dir)
    # print(f"➕ Dynamo project at {dynamo_dir}")
