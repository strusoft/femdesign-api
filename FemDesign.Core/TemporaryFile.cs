using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign
{
    public class TemporaryFile : IDisposable
    {
        public string FilePath;

        public TemporaryFile(string extension = null)
        {
            if (string.IsNullOrEmpty(extension))
                extension = ".tmp";
            else if (extension.Contains("."))
                extension = Path.GetExtension(extension);
            else 
                extension = "." + extension;

            FilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + extension);
        }
        public void Dispose()
        {
            File.Delete(FilePath);
        }
    }
}
