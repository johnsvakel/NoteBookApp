using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoteBookSync;
using NoteApp;

namespace NoteAppUTProj
{
    [TestClass]
    public class NoteBookSyncUT
    {
        [TestMethod]
        //Test the data sync basics
        public void TestSyncFileSystemReplicasOneWay()
        {
            //@read configuration
            NoteBookSync.NoteBookFileSyncProvider objTest = new NoteBookFileSyncProvider();
            NoteAppPreference pref = new NoteAppPreference();
            bool bSync = objTest.SyncFileSystemReplicas(pref.GetLocalServer(), pref.GetServerInfo());
            Assert.AreEqual(bSync, true, "Sync not happened properly");
        }
        [TestMethod]
        public void TestDetectChangesOnFileSystemReplica()
        {
            NoteBookSync.NoteBookFileSyncProvider objTest = new NoteBookFileSyncProvider();
            NoteAppPreference pref = new NoteAppPreference();
            bool bSync = objTest.DetectChangesOnFileSystemReplica(pref.GetServerInfo());
        }
    }
}
