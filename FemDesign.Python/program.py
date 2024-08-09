from fdpipe import FemDesignConnection, Verbosity
from fdscript import *
from analysis import *
from command import *

def main():
    fdscript_header = FdscriptHeader(r"C:\GitHub\femdesign-api\FemDesign.Python\example\x.log")

    cmd_open =  CmdOpen(r"C:\GitHub\femdesign-api\FemDesign.Python\example\simple_beam.str")
    
    cmd_resmode = CmdUser(User.RESMODE)

    comb = Comb()
    comb.combitems.append( CombItem.StaticAnalysis() )
    analysis = Analysis.StaticAnalysis(comb)
    cmd_analysis = CmdCalculation(analysis)

    cmd_save = CmdSave(r"C:\GitHub\femdesign-api\FemDesign.Python\example\simple_beam_out.str")


    # design = Design()
    # cmd_design = CmdCalculation(None, design)

    cmd_list_gen = CmdListGen(r"C:\GitHub\femdesign-api\FemDesign.Python\example\nodal_displacement.bsc",
                              r"C:\GitHub\femdesign-api\FemDesign.Python\example\nodal_displacement.csv")

    cmd_project = CmdProjDescr("Test project", "Test project description", "Test designer", "Test signature",
                               "Test comment", {"a": "a_txt", "b": "b_txt"}, read = False, reset = False)

    fdscript = Fdscript(fdscript_header, [cmd_open, cmd_project, cmd_resmode, cmd_analysis , cmd_list_gen, cmd_save])


    ## Usage example and unit test
    pipe = FemDesignConnection(fd_path= r"C:\Program Files\StruSoft\FEM-Design 23 Night Install\fd3dstruct.exe")
    try:
        pipe.SetVerbosity(Verbosity.SCRIPT_LOG_LINES)
        pipe.RunScript(
            fdscript,
            file_name="script",
        )
        pipe.Disconnect()
    except Exception as err:
        pipe.KillProgramIfExists()
        raise err


if __name__ == "__main__":
    main()