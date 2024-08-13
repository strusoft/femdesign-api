from command import *
from analysis import Analysis, Comb, Design
from fdpipe import FemDesignConnection
import pytest

def test_pipe():
    connection = FemDesignConnection(output_dir="test", minimized=True)
    assert connection.output_dir == os.path.join( os.getcwd(), "test" )

    connection._output_dir = None
    assert connection.output_dir == os.path.join( os.getcwd(), "FEM-Design API" )

    ## assert that connection.open() raises an error
    try:
        connection.Open("myModel.str")
    except Exception as e:
        assert isinstance(e, FileNotFoundError)
        assert str(e) == "File myModel.str not found"

    try:
        connection.Open("myModel.3dm")
    except Exception as e:
        assert isinstance(e, ValueError)
        assert str(e) == "File myModel.3dm must have extension .struxml or .str"


    connection.RunDesign(DesignModule.STEELDESIGN, design)

