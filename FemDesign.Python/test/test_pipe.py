from command import *
from analysis import Analysis, Comb, Design
from fdpipe import FemDesignConnection

def test_pipe():
    connection = FemDesignConnection(output_dir="test", minimized=True)
    assert connection.output_dir == os.path.join( os.getcwd(), "test" )

    connection = FemDesignConnection(minimized=True)
    assert connection.output_dir == os.path.join( os.getcwd(), "FEM-Design API" )

