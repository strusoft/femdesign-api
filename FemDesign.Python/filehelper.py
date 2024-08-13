import os
import pathlib
## define a static class to help with file operations
class OutputFileHelper:
    _scriptsDirectory = "scripts"
    _resultsDirectory = "results"
    _bscDirectory = "bsc"

    _logFileName = "logfile.log"
    _struxmlFileName = "model.struxml"
    _strFileName = "model.str"
    _docxFileName = "model.docx"

    _fdscriptFileExtension = ".fdscript"
    _bscFileExtension = ".bsc"
    _csvFileExtension = ".csv"

    _strFileExtension = ".str"
    _struxmlFileExtension = ".struxml"

    def GetLogFilePath(outputDirectory: str) -> str:
        if not os.path.exists(outputDirectory):
            os.makedirs(outputDirectory)
        return os.path.abspath( os.path.join(outputDirectory, OutputFileHelper._logFileName) )
    
    def GetFdscriptFilePath(outputDirectory: str, file_name: str = "script") -> str:
        dir = os.path.join(outputDirectory, OutputFileHelper._scriptsDirectory)
        if not os.path.exists(dir):
            os.makedirs(dir)

        path = os.path.abspath( os.path.join(dir, f"{file_name}" + OutputFileHelper._fdscriptFileExtension) )
        return path
    
    def GetBscFilePath(outputDirectory: str, file_name: str) -> str:
        dir = os.path.join(outputDirectory, OutputFileHelper._scriptsDirectory, OutputFileHelper._bscDirectory)
        if not os.path.exists(dir):
            os.makedirs(dir)

        path = os.path.abspath( os.path.join(dir, f"{file_name}" + OutputFileHelper._bscFileExtension) )
        return path