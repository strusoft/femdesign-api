using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("FemDesign API")]
[assembly: AssemblyDescription("The FEM-Design API")]


// Allow internal methods to be accessed from the unit tests.
#if DEBUG
[assembly: InternalsVisibleTo("FemDesign.Tests")]
#endif

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("00f74121-ff13-46da-bd54-1d81c62c4952")]
