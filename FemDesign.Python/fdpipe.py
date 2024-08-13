import subprocess
from datetime import datetime
from time import sleep

from filehelper import OutputFileHelper

from enum import Enum

import win32file
import win32pipe

from command import *
import analysis
from fdscript import Fdscript, FdscriptHeader

import os

"""
FEM - Design usage with pipe

To initiate :
1 : create a WIN32 named pipe for duplex mode, message oriented
1a : optional : create another pipe for back channel, named appending 'b'.
2 : launch FD with command line
/ p Name
passing the name you used at creation.FD will open it right at start and exit if can't
after successful open it listens to commands while the usual interface is active
you can combine it with the windowless / minimized mode to hide the window
it also attaches to the back channel pipe at this moment, if unable, all output is permanently disabled
3 : send commands through the pipe
4 : FD will exit if 'exit' command received or the pipe is closed on this end

FD only reads the main pipe and only writes the back channel(if supplied), allowing this end to never
read.While the pipe is duplexand can be used in both direction, if it gets clogged in
one direction(by not reading what the other end sends), the write can get blocked too.
The document recommends using another pipe for a back channel.
By default nothing is written to the back channel, you need to set output level or commands with implicit reply.
FD buffers all outgoing messages till they can be sent over, if this end is lazy to read it will not clog,
however they will accumulate in memory.



Messages are text in  9bit ANSI(codepage), limited to 4096 bytes

The command format is
[!]cmd[space][args]
there is no delimiter at the end, the pipe message counts.
FD reads the pipe immediatelyand puts the commands in a queue.The queue is processed when it's READYSTATE
for another command, finishing execution of the previous or a running script.
The !requests out - of bound execution.That is not suppoerted by very command and mainly
serves to manipulate the queue itself, verbosity or check the communicaiton is alive.

Commands:

exit
stop the FD process

detach
close the pipe and continue IN normal interface mode

clear[in | out]
flush the FD mesage queue  for the direction, both without parameters
has no Effect on what is already issued to the pipe

echo[txt]
write txt to output

stat
write queueand processing status to output

v[N]
set verbosity control (bits)
   1: enable basic output
   2: echo all INPUT commands
   4: FD log Lines
   8: script log lines
 *16: calculation window messages (except fortran)
 *32: progress window title

  echo and stat always cretes output, otherwise nothing is written aT V = 0
  * not yet supported

run[scriptfile]
execute script as from tools / run script menu
*Note: When using Unicode commands, add the suffix 'UTF8'. E.g.: "runUTF8 [scriptfile]".

cmd[command]
execute command as if typed into the command window -- No warranty!!!
*Note: When using Unicode commands, add the suffix 'UTF8'. E.g.: "cmdUTF8 [command]".

esc
press Escape during calculation to break it

"""


def GetElapsedTime(start_time):
    return (datetime.now() - start_time).total_seconds()


class _FdConnect:

    def __exit__(self, exc_type, exc_value, traceback):
        self.Detach()
        self.ClosePipe()

    def __init__(self, pipe_name="FdPipe1"):
        """
        Creating fd pipe
            One fd connection for the life of the fem design until closure.
            It could be more than one with given uniqe pipe_name.
        """
        self.pipe_name = pipe_name
        self.pipe_send = self._CreatePipe(pipe_name)
        self.pipe_read = self._CreatePipe(f"{pipe_name}b")
        self.start_time = None
        self._log_message_history = []

    @staticmethod
    def _CreatePipe(name):
        return win32pipe.CreateNamedPipe(
            r"\\.\pipe\%s" % name,
            win32pipe.PIPE_ACCESS_DUPLEX,
            win32pipe.PIPE_TYPE_MESSAGE
            | win32pipe.PIPE_READMODE_MESSAGE
            | win32pipe.PIPE_NOWAIT,
            win32pipe.PIPE_UNLIMITED_INSTANCES,
            4096,
            4096,
            0,
            None,
        )

    def _ConnectPipe(self, timeout=60):
        start_time = datetime.now()
        while GetElapsedTime(start_time) <= timeout:
            try:
                win32pipe.ConnectNamedPipe(self.pipe_send, None)
                win32pipe.ConnectNamedPipe(self.pipe_read, None)
                return
            except:
                sleep(0.5)
        raise TimeoutError(f"Program not connected in {timeout} second")

    def Start(self, fd_path, timeout=60):
        """
        Start and connect of fem-design with specified path
        """
        self.process = subprocess.Popen([fd_path, "/p", self.pipe_name])
        self._ConnectPipe(timeout=timeout)

    def _Read(self):
        """
        Read one message if exists
        """
        try:
            return win32file.ReadFile(self.pipe_read, 4096)[1].decode()
        except Exception as err:
            if err.winerror == 232: # Error text (The pipe is being closed.) Pipe is empty
                return None
            elif err.winerror == 109: # Error text (The pipe has been ended.) Conecction lost
                raise AssertionError("Connection lost")
            else:
                raise err

    def ReadAll(self):
        """
        Read all existing message
        """
        results = []
        while True:
            result = self._Read()
            if result == None:
                return "\n".join(results)
            results.append(result)
            self._log_message_history.append(result)

    def GetLogMessageHistory(self):
        """
        Return all collected message
        """
        return self._log_message_history

    def Send(self, message):
        """
        Send one message
        """
        try:
            win32file.WriteFile(self.pipe_send, message.encode())
        except Exception as err:
            if err.winerror == 232: # Error text (The pipe is being closed.) Connection lost
                raise AssertionError("Connection lost")
            else:
                raise err

    def SendAndReceive(self, message, timeout=None):
        """
        Complete workflow of send message and receive answer until timeout
        """
        self.Send(message)
        start_time = self.start_time or datetime.now()
        while timeout == None or GetElapsedTime(start_time) <= timeout:
            result = self.ReadAll()
            if result:
                return result
            sleep(0.5)
        raise TimeoutError(f"Program not responding after {timeout} second")

    def Stat(self, timeout=None):
        """
        Return queueand processing status
        """
        return self.SendAndReceive("stat", timeout=timeout)

    def LogLevel(self, n):
        """
        Set log level
            verbosity control (bits)
               1: enable basic output
               2: echo all INPUT commands
               4: FD log Lines
               8: script log lines
             *16: calculation window messages (except fortran)
             *32: progress window title

              echo and stat always cretes output, otherwise nothing is written aT V = 0
              * not yet supported
        """
        return self.Send(f"v {n}")

    def RunScript(self, path, timeout=None):
        """
        Execute script as from tools / run script menu
        """
        self.Send(f"runUTF8 {path}")
        self.start_time = datetime.now()
        while timeout == None or GetElapsedTime(self.start_time) <= timeout:
            sleep(1)
            if "script idle" in self.Stat(timeout=timeout):
                self.start_time = None
                return
        raise TimeoutError(f"Too long script run time after {timeout} second")

    def Cmd(self, cmd_text):
        """
        Execute command as if typed into the command window -- No warranty!!!
        """
        self.Send(f"cmdUTF8 {cmd_text}")

    def Esc(self):
        """
        Press Escape during calculation to break it
        """
        self.Send("esc")

    def Exit(self):
        """
        Close fem-design
        """
        self.Send("exit")

    def Detach(self):
        """
        Disconnect from pipe and close it. You can't reattach to fem-design.
        It must be outside of KillProgramIfExists scope.
        """
        self.Send("detach")
        sleep(2)
        self.ClosePipe()

    def ClosePipe(self):
        """
        Regular closing of fd pipe
        """
        win32file.CloseHandle(self.pipe_send)
        win32file.CloseHandle(self.pipe_read)

    def KillProgramIfExists(self):
        """
        Kill program if exists
        """
        if not (self.process.poll()):
            try:
                self.process.kill()
            except WindowsError:
                print("Program gone in meantime")


class Verbosity(Enum):
    BASIC = 1
    ECHO_ALL_INPUT_COMMANDS = 2
    FD_LOG_LINES = 4
    SCRIPT_LOG_LINES = 8
    CALCULATION_WINDOW_MESSAGES = 16
    PROGRESS_WINDOW_TITLE = 32


## define a private class


class FemDesignConnection(_FdConnect):
    def __init__(self,
                 fd_path : str = r"C:\Program Files\StruSoft\FEM-Design 23\fd3dstruct.exe",
                 pipe_name : str ="FdPipe1",
                 verbose : Verbosity = Verbosity.SCRIPT_LOG_LINES,
                 output_dir : str = None,
                 minimized : bool = False,):
        super().__init__(pipe_name)

        self._output_dir = output_dir

        os.environ["FD_NOLOGO"] = "1"

        if minimized:
            os.environ["FD_NOGUI"] = "1"
        
        self.Start(fd_path)
        self.LogLevel(verbose)

    @property
    def output_dir(self):
        if self._output_dir == None:
            return os.path.join(os.getcwd(), "FEM-Design API")
        else:
            return os.path.abspath(self._output_dir)
    
    @output_dir.setter
    def output_dir(self, value):
        self._output_dir = os.path.abspath(value)
        if not os.path.exists(value):
            os.makedirs(os.path.abspath(value))

    def RunScript(self, fdscript : Fdscript, file_name : str = "script"):
        """

        Args:
            fdscript (Fdscript): fdscript object to run
            file_name (str, optional): file name to save the script. Defaults to "script".
        """
        path = OutputFileHelper.GetFdscriptFilePath(self.output_dir, file_name)


        fdscript.serialise_to_file(path)
        super().RunScript(path)

    def RunAnalysis(self, analysis : analysis.Analysis):
        """Run analysis

        Args:
            analysis (analysis.Analysis): analysis object
        """
        log = OutputFileHelper.GetLogFilePath(self.output_dir)

        fdscript = Fdscript(log, [CmdUser.ResMode(), CmdCalculation(analysis)])
        self.RunScript(fdscript, "analysis")

    def RunDesign(self, designMode : DesignModule  ,design : analysis.Design):
        """Run design

        Args:
            designMode (DesignModule): design module
            design (analysis.Design): design object
        """
        log = OutputFileHelper.GetLogFilePath(self.output_dir)

        fdscript = Fdscript(log, [CmdUser(designMode.to_user()), CmdCalculation(design)])
        self.RunScript(fdscript, "design")
    
    def SetProjectDescription(self, project_name : str = "", project_description : str = "", designer : str = "", signature : str = "", comment : str = "", item : dict = None, read : bool = False, reset : bool = False):
        """Set project description

        Args:
            project_name (str): project name
            project_description (str): project description
            designer (str): designer
            signature (str): signature
            comment (str): comment
            additional_info (dict): define key-value user data. Defaults to None.
            read (bool, optional): read the project settings. Value will be store in the clipboard. Defaults to False.
            reset (bool, optional): reset the project settings. Defaults to False.
        """
        log = OutputFileHelper.GetLogFilePath(self.output_dir)

        cmd_project = CmdProjDescr(project_name, project_description, designer, signature, comment, item, read, reset)
        fdscript = Fdscript(log, [cmd_project])
        self.RunScript(fdscript, "project_description")

    def Save(self, file_name : str):
        """Save the project

        Args:
            file_name (str): name of the file to save
        """
        log = OutputFileHelper.GetLogFilePath(self.output_dir)

        cmd_save = CmdSave(file_name)

        fdscript = Fdscript(log, [cmd_save])
        self.RunScript(fdscript, "save")

    def Open(self, file_name : str):
        """Open a model from file

        Args:
            file_name (str): file path to open

        Raises:
            ValueError: extension must be .struxml or .str
            FileNotFoundError: file not found
        """
        log = OutputFileHelper.GetLogFilePath(self.output_dir)

        if not file_name.endswith(".struxml") and not file_name.endswith(".str"):
            raise ValueError(f"File {file_name} must have extension .struxml or .str")
        if not os.path.exists(file_name):
            raise FileNotFoundError(f"File {file_name} not found")

        cmd_open = CmdOpen(file_name)
        fdscript = Fdscript(log, [cmd_open])
        self.RunScript(fdscript, "open")

    def SetVerbosity(self, verbosity : Verbosity):
        super().LogLevel(verbosity.value)

    def GenerateListTables(self, bsc_file : str, csv_file : str = None):
        log = OutputFileHelper.GetLogFilePath(self.output_dir)

        cmd_results = CmdListGen(bsc_file, csv_file)
        fdscript = Fdscript(log, [cmd_results])
        self.RunScript(fdscript, "generate_list_tables")
        

    ## it does not work
    def Disconnect(self):
        super().Detach()
        win32pipe.DisconnectNamedPipe(self.pipe_send)
