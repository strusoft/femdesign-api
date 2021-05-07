import os, re, sys
from os import path, scandir
# sys.path.append(path.abspath("scripts/converLibrary"))
from convertLibrary.convertLibrary import wrapper, getCsFiles, readCsContent, writeNewCsFile


LINK = """    <Compile Include="$INCLUDE$">
      <Link>$LINK$</Link>
    </Compile>"""

def get_all_subdirectories(root):
    return [directory for directory, _, _ in os.walk(root)]


def get_folders(root):
    # list_subfolders_with_paths = [f.path for f in os.scandir(root) if f.is_dir()]
    list_subfolders_with_paths = [directory for directory, _, _ in os.walk(root)]
    return list_subfolders_with_paths

def write_links_to_csproj_file(csproj_file, links):
    lines = []
    with open(csproj_file, "r") as csproj:
        lines = csproj.readlines()
    insertIndex = len(lines) - 1

    new_lines = []
    with open(csproj_file, "w") as csproj:
        for filepath, includepath in links:
            file_link_xml = LINK.replace("$LINK$", filepath).replace("$INCLUDE$", includepath)
            new_lines.extend(file_link_xml.split("\n"))
        lines[insertIndex:insertIndex] = new_lines
        csproj.writelines(lines)

def main(csproj_file, source_dir, target_dir):
    EXCLUDE = re.compile(r"(.*\\\.)|(^\.)|(.\\)*bin|(.\\)*obj")
    
    source_subdirs = get_folders(source_dir)
    source_subdirs = [x for x in source_subdirs if re.search(EXCLUDE, x) is None]

    links = []
    for subdir in source_subdirs:
        source_files = os.scandir(subdir)
        for file in source_files:
            links.append((file.path, f"..\\{file.path}"))

    write_links_to_csproj_file(csproj_file, links)

def parseCsContent(csContent):
    content = ""
    skipLine = False
    dynamoRegion = False

    for line in csContent:
        if "#region dynamo" in line:
            dynamoRegion = True
        elif "#endregion" in line and dynamoRegion:
            dynamoRegion = False

        if ("#region" in line or
                "#endregion" in line or
                "using" in line or
                "IsVisibleInDynamoLibrary" in line or
                "IsLacingDisabled" in line or
                line.strip() == "{" or
                line.strip() == "}" or
                line.strip() == "" or
                "namespace" in line):
            skipLine = False
        elif "class" in line:
            skipLine = False
            line = line.replace("public class", "public partial class")
        else:
            skipLine = True

        if dynamoRegion:
            content += line
        elif skipLine:
            skipLine = False
        else:
            content += line
            skipLine = False
    return content

def generate_empty_classes(srcDir, destDir):
    srcPaths = getCsFiles(srcDir)
    for srcPath in srcPaths:
        csContent = readCsContent(srcPath)
    
        csContent = parseCsContent(csContent)
        csContent = csContent.splitlines(True)
        csContent = [line for line, nextline in zip(csContent, csContent[1:]) if not line == nextline == "\n"] + [csContent[-1]]

        content = "".join(csContent)
        # write new file to destination path
        destPath = destDir + srcPath.split(srcDir)[-1]
        writeNewCsFile(content, destPath, True)
    


if __name__ == "__main__":
    # print("🔧 Creating linkes to 'FemDesign.Core' files in 'FemDesign.Dynamo'")

    csproj_file = "FemDesign.Dynamo/FemDesign.Dynamo.csproj"
    core_dir = "FemDesign.Core"
    # dynamo_dir = "FemDesign.Dynamo"
    dynamo_dir = "TEST"
    
    # main(csproj_file, core_dir, dynamo_dir)

    # print("🏗  Extracting dynamo regions from source and copying dynamo project")
    src_dir = "./src"
    # wrapper(src_dir, dynamo_dir, parse = "Dyn-regions")

    # print("🎨 Generate empty classes with 'partial' and '[IsVisibleInDynamoLibrary(false)]'")
    # generate_empty_classes(src_dir, dynamo_dir + "/Dynamo")

    print("🔧 Creating csproj compile items")
    for file in getCsFiles("FemDesign.Dynamo/Dynamo"):
        filepath = file.split("FemDesign.Dynamo/")[-1]
        print(f"<Compile Include=\"{filepath}\" />")

