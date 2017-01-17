using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;

namespace NoteBookSync
{
    public class NoteBookBackGroudSync
    {
        private string strLocalServerInfo;
        private string strServerInfo;
        private System.ComponentModel.BackgroundWorker backgroundWorkerThread;
        public void NoteBookBackGroudSyncCreate(string strLocalServer, string strServer)
        {
            strLocalServerInfo = strLocalServer;
            strServerInfo = strServer;
            this.backgroundWorkerThread = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            backgroundWorkerThread.RunWorkerAsync();
        }
        public void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            SyncDataToOrFromCloud();
        }
        private void SyncDataToOrFromCloud()
        {
            NoteBookFileSyncProvider syncProvider = new NoteBookFileSyncProvider();
            syncProvider.SyncFileSystemReplicas(strLocalServerInfo, strServerInfo);
        }
    }
}
