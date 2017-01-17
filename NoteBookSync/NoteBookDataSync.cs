using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Synchronization.Files;
using Microsoft.Synchronization;
using Microsoft.Synchronization.FeedSync;

namespace NoteBookSync
{
    public class NoteBookDataSync
    {
        public static bool SyncFileSystemReplicasOneWay(
        string sourceReplicaRootPath, string destinationReplicaRootPath,
        FileSyncScopeFilter filter, FileSyncOptions options)
        {
            bool bSync = false;
            FileSyncProvider sourceProvider = null;
            FileSyncProvider destinationProvider = null;

            try
            {
                sourceProvider = new FileSyncProvider(
                    sourceReplicaRootPath, filter, options);
                destinationProvider = new FileSyncProvider(
                    destinationReplicaRootPath, filter, options);

                destinationProvider.AppliedChange +=
                    new EventHandler<AppliedChangeEventArgs>(NoteBookFileSyncProvider.OnAppliedChange);
                destinationProvider.SkippedChange +=
                    new EventHandler<SkippedChangeEventArgs>(NoteBookFileSyncProvider.OnSkippedChange);

                SyncOrchestrator agent = new SyncOrchestrator();
                agent.LocalProvider = sourceProvider;
                agent.RemoteProvider = destinationProvider;
                agent.Direction = SyncDirectionOrder.Upload; // Synchronize source to destination

                Console.WriteLine("Synchronizing changes to replica: " +
                    destinationProvider.RootDirectoryPath);
                agent.Synchronize();
                bSync = true;
            }
            finally
            {
                // Release resources
                if (sourceProvider != null) sourceProvider.Dispose();
                if (destinationProvider != null) destinationProvider.Dispose();
            }
            return bSync;
        }
    }
}
