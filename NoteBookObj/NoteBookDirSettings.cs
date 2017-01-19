using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NoteBookObj
{
    public class NoteBookDirSettings
    {
        public void ValidateDirLocal(string strLocalDir)
        {
            bool exists = System.IO.Directory.Exists(strLocalDir);
            if (!exists)
                System.IO.Directory.CreateDirectory(strLocalDir);
        }
        public void ValidateDirSrerver(string strServerDir)
        {            
            bool exists = System.IO.Directory.Exists(strServerDir);
            if (!exists)
                System.IO.Directory.CreateDirectory(strServerDir);
        }
    }
}
