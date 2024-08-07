import fdpipe
from fdpipe import FemDesignConnection, Verbosity
from fdscript import *
from analysis import *
from command import *

def main():
    ## Usage example and unit test
    print("Unit test of connection with sample script")
    pipe = fdpipe._FdConnect()
    try:
        pipe.Start(fd_path=r"..\..\2300\DEBUG64\fd3dstruct.exe") ## path of fd3dstruct.exe
        pipe.LogLevel(15)
        print("Send end receive test with echo:")
        print(pipe.SendAndReceive("echo Ping"))
        print(pipe.SendAndReceive("echo Pong"))
        print("Run examples script:")
        pipe.RunScript(
            r"d:\proj\test\fdscript_test_files\fdscript_results\2024-04-09-16-43-41\verification_example_1.1.fdscript", ## path of sample fdscript
            timeout=20,
        )
        pipe.Exit()
        pipe.ClosePipe()
    except Exception as err:
        pipe.KillProgramIfExists()
        raise err
    finally:
        pipe.ClosePipe()


def test():
    fdscript_header = FdscriptHeader("FEM-Design example script", 2300, "SFRAME", r"C:\GitHub\femdesign-api\FemDesign.Python\temp\x.log")

    cmd_open =  CmdOpen(r"C:\GitHub\femdesign-api\FemDesign.Python\temp\model.str")
    cmd_save = CmdSave(r"C:\GitHub\femdesign-api\FemDesign.Python\temp\model_saves.str")
    cmd_resmode = CmdUser(User.RESMODE)

    analysis = Analysis.StaticAnalysis(Comb())

    #cmd_analysis = CmdCalculation(analysis)

    design = Design()
    cmd_design = CmdCalculation(None, design)

    cmd_project = CmdProjDescr("Test project", "Test project description", "Test designer", "Test signature",
                               "Test comment", {"a": "a_txt", "b": "b_txt"}, read = False, reset = False)

    cmd_project_read = CmdProjDescr("Test project", "Test project description", "Test designer", "Test signature",
                               "Test comment", {"a": "a_txt", "b": "b_txt"}, read = True, reset = False)

    fdscript = Fdscript(fdscript_header, [cmd_open, cmd_project, cmd_save, cmd_project_read])


    ## Usage example and unit test
    pipe = FemDesignConnection(fd_path= r"C:\Program Files\StruSoft\FEM-Design 23 Night Install\fd3dstruct.exe")
    try:
        pipe.SetVerbosity(Verbosity.SCRIPT_LOG_LINES)
        pipe.RunScript(
            fdscript,
            file_name="script",
        )
        pipe.Detach()
    except Exception as err:
        pipe.KillProgramIfExists()
        raise err


if __name__ == "__main__":
    #main()
    test()