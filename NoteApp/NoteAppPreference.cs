using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteApp
{
    public class NoteAppPreference
    {
        private string strLocalCacheDir;
        private string strServerInfo; //Test Server
        public void SetLocalCacheDir(string strLocalCacheInfo)
        {
            strLocalCacheDir = strLocalCacheInfo;
        }
        public void SetServerInfo(string strServerInfoDir)
        {
            strServerInfo = strServerInfoDir;
        }
        public string GetLocalServer()
        {
            return strLocalCacheDir;
        }
        public string GetServerInfo()
        {
            return strServerInfo;
        }
    }
}
