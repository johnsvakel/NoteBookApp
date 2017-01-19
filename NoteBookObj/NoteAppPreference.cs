using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteBookObj;

namespace NoteBookObj
{
    public class NoteAppPreference
    {
        private string strLocalCacheDir;
        private string strServerInfo; //Test Server
        private string strFileName;
        private string strBackName;
        private const String NOTES_FILENAME = "notes.bin";
        private const String NOTES_BACKUP_FILENAME = "notes.bak";
        private NoteBookDirSettings noteBookDirObj = new NoteBookDirSettings();
        public void SetLocalCacheDir(string strLocalCacheInfo)
        {
            strLocalCacheDir = strLocalCacheInfo;
            SetLoaclDirCahceName(strLocalCacheDir);
            SetBackupPathName(strLocalCacheDir);
            noteBookDirObj.ValidateDirLocal(strLocalCacheDir);
        }
        public void SetServerInfo(string strServerInfoDir)
        {
            strServerInfo = strServerInfoDir;
            noteBookDirObj.ValidateDirSrerver(strServerInfoDir);
          
        }
        public string GetLocalServer()
        {
            return strLocalCacheDir;
        }
        public string GetServerInfo()
        {
            return strServerInfo;
        }
        private void SetLoaclDirCahceName(string strLocalDir)
        {
            strFileName = strLocalDir + "\\" + NOTES_FILENAME;           
        }
        private void SetBackupPathName(string strLocalDir)
        {
            strBackName = strLocalDir + "\\" + NOTES_BACKUP_FILENAME;          
        }
        public string GetLoaclDirCahceName()
        {
            return strFileName;
        }
        public string  GetBackupPathName()
        {
            return strBackName;
        }        
    }
}
