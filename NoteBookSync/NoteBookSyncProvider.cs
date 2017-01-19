using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Synchronization.Files;
using System.Windows;
using System.IO;
using Microsoft.Synchronization;
using Microsoft.Synchronization.FeedSync;

namespace NoteBookSync
{
    public class NoteBookFileSyncProvider
    {
        public bool SyncFileSystemReplicas(string strLocalDir, string strServerDir)
        {
            bool bSync = false;
            FileSyncScopeFilter obj = new FileSyncScopeFilter();

            FileSyncOptions options = FileSyncOptions.CompareFileStreams;
            SyncFileSystemReplicasOneWay(strLocalDir, strServerDir, obj, options);
            return bSync;
        }
        public static void Main(string[] args)
        {
            if (args.Length < 2 ||
                string.IsNullOrEmpty(args[0]) || string.IsNullOrEmpty(args[1]) ||
                !Directory.Exists(args[0]) || !Directory.Exists(args[1]))
            {
                Console.WriteLine(
                  "Usage for Test: NoteBookSynProvider [valid directory path 1] [valid directory path 2]");
                return;
            }

            string replica1RootPath = args[0];
            string replica2RootPath = args[1];

            try
            {
                // Set options for the synchronization operation
                FileSyncOptions options = FileSyncOptions.ExplicitDetectChanges |
                    FileSyncOptions.RecycleDeletedFiles |
                    FileSyncOptions.RecyclePreviousFileOnUpdates |
                    FileSyncOptions.RecycleConflictLoserFiles;

                FileSyncScopeFilter filter = new FileSyncScopeFilter();
                filter.FileNameExcludes.Add("*.lnk"); // Exclude all *.lnk files

                // Explicitly detect changes on both replicas upfront, to avoid two change
                // detection passes for the two-way synchronization

                DetectChangesOnFileSystemReplica(
                        replica1RootPath, filter, options);
                DetectChangesOnFileSystemReplica(
                    replica2RootPath, filter, options);

                // Synchronization in both directions
                SyncFileSystemReplicasOneWay(replica1RootPath, replica2RootPath, null, options);
                SyncFileSystemReplicasOneWay(replica2RootPath, replica1RootPath, null, options);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException from File Synchronization Provider:\n" + e.ToString());
            }
        }

        public bool DetectChangesOnFileSystemReplica(string replicaRootPath)
        {
            FileSyncScopeFilter filter = new FileSyncScopeFilter();
            FileSyncOptions options = new FileSyncOptions();
            DetectChangesOnFileSystemReplica(replicaRootPath, filter, options);
            return true;
        }
        public static void DetectChangesOnFileSystemReplica(
                string replicaRootPath,
                FileSyncScopeFilter filter, FileSyncOptions options)
        {
            FileSyncProvider provider = null;

            try
            {
                provider = new FileSyncProvider(replicaRootPath, filter, options);
                //provider = new FileSyncProvider(replicaRootPath, replicaRootPath, filter, options);
                provider.DetectChanges();
            }
            finally
            {
                // Release resources
                if (provider != null)
                    provider.Dispose();
            }
        }
        /*
         * 
        static void Main(string[] args)
        {
            string myDocsPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string Dir1 = Path.Combine(myDocsPath, "Dir1");
            string Dir2 = Path.Combine(myDocsPath, "Dir2");
            string metadataPath = Path.Combine(myDocsPath, "Metadata");
            string tempDir = Path.Combine(myDocsPath, "Cache");
            string trashDir = Path.Combine(myDocsPath, "Trash");
            FileSyncScopeFilter filter = new FileSyncScopeFilter();
            filter.FileNameExcludes.Add("*.metadata");
            FileSyncOptions options = FileSyncOptions.None;

            //DetectChanges
            DetectChangesOnFileSystemReplica(Dir1, filter, options, Dir1, "filesync.metadata", tempDir, trashDir);
            DetectChangesOnFileSystemReplica(Dir2, filter, options, Dir2, "filesync.metadata", tempDir, trashDir);

            //SyncChanges Both Ways
            SyncFileSystemReplicasOneWay(Dir1, Dir2, filter, options, Dir1, "filesync.metadata", tempDir, trashDir);
            SyncFileSystemReplicasOneWay(Dir2, Dir1, filter, options, Dir2, "filesync.metadata", tempDir, trashDir);
        }

        public static void DetectChangesOnFileSystemReplica(string replicaRootPath, FileSyncScopeFilter filter, FileSyncOptions options, string metadataPath, string metadataFile, string tempDir, string trashDir)
        {
            FileSyncProvider provider = null;
            try
            {
                provider = new FileSyncProvider(replicaRootPath, filter, options, metadataPath, metadataFile, tempDir, trashDir);
                provider.DetectChanges();
            }
            finally
            {
                // Release resources
                if (provider != null)
                    provider.Dispose();
            }
        }
        public static void SyncFileSystemReplicasOneWay(string sourceReplicaRootPath, string destinationReplicaRootPath, FileSyncScopeFilter filter, FileSyncOptions options, string metadataPath, string metadataFile, string tempDir, string trashDir)
        {
            FileSyncProvider sourceProvider = null;
            FileSyncProvider destinationProvider = null;
            try
            {
                sourceProvider = new FileSyncProvider(sourceReplicaRootPath, filter, options, metadataPath, metadataFile, tempDir, trashDir);
                destinationProvider = new FileSyncProvider(destinationReplicaRootPath, filter, options, metadataPath, metadataFile, tempDir, trashDir);

                destinationProvider.AppliedChange += new EventHandler<AppliedChangeEventArgs>(OnAppliedChange);
                destinationProvider.SkippedChange += new EventHandler<SkippedChangeEventArgs>(OnSkippedChange);

                SyncOrchestrator agent = new SyncOrchestrator();
                agent.LocalProvider = sourceProvider;
                agent.RemoteProvider = destinationProvider;
                agent.Direction = SyncDirectionOrder.Upload; // Sync source to destination
                Console.WriteLine("Synchronizing changes to replica: " + destinationProvider.RootDirectoryPath);
                agent.Synchronize();
            }
            finally
            {
                // Release resources
                if (sourceProvider != null) sourceProvider.Dispose();
                if (destinationProvider != null) destinationProvider.Dispose();
            }
        }
         */
        public static bool SyncFileSystemReplicasOneWay(string sourceReplicaRootPath, string destinationReplicaRootPath, FileSyncScopeFilter filter, FileSyncOptions options, string metadataPath1, string metadataFile1, string metadataPath2, string metadataFile2, string tempDir, string trashDir)
        {
            bool bSync = false;
            FileSyncProvider sourceProvider = null;
            FileSyncProvider destinationProvider = null;
            sourceProvider = new FileSyncProvider(sourceReplicaRootPath, filter, options, metadataPath1, metadataFile1, tempDir, trashDir);
            destinationProvider = new FileSyncProvider(destinationReplicaRootPath, filter, options, metadataPath2, metadataFile2, tempDir, trashDir);
            return bSync;
        }
        public static bool SyncFileSystemReplicasOneWay(
                string sourceReplicaRootPath, string destinationReplicaRootPath,
                FileSyncScopeFilter filter, FileSyncOptions options)
        {
            bool bSync = false;
            FileSyncProvider sourceProvider = null;
            FileSyncProvider destinationProvider = null;

            options = FileSyncOptions.ExplicitDetectChanges
                   | FileSyncOptions.RecycleConflictLoserFiles
                   | FileSyncOptions.RecycleDeletedFiles
                   | FileSyncOptions.RecyclePreviousFileOnUpdates;
            filter.FileNameExcludes.Add("*.pete");

            try
            {
                // Avoid two change detection passes for the two-way sync
                FindFileSystemReplicaChanges(sourceReplicaRootPath, filter, options);
                FindFileSystemReplicaChanges(destinationReplicaRootPath, filter, options);

                OneWaySyncFileSystemReplicas(sourceReplicaRootPath, destinationReplicaRootPath, null, options);
                OneWaySyncFileSystemReplicas(destinationReplicaRootPath, sourceReplicaRootPath, null, options);

                //sourceProvider = new FileSyncProvider(
                //    sourceReplicaRootPath, filter, options);
                //destinationProvider = new FileSyncProvider(
                //    destinationReplicaRootPath, filter, options);

                //destinationProvider.AppliedChange +=
                //    new EventHandler<AppliedChangeEventArgs>(OnAppliedChange);
                //destinationProvider.SkippedChange +=
                //    new EventHandler<SkippedChangeEventArgs>(OnSkippedChange);

                //Microsoft.Synchronization.SyncOrchestrator agent = new SyncOrchestrator();
                //agent.LocalProvider = sourceProvider;
                //agent.RemoteProvider = destinationProvider;
                //agent.Direction = SyncDirectionOrder.Upload; // Sync source to destination

                //Console.WriteLine("Synchronizing changes to replica: " +
                //    destinationProvider.RootDirectoryPath);
                //agent.Synchronize();
                bSync = true;
            }
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);  
            //}
            finally
            {
                // Release resources
                if (sourceProvider != null) sourceProvider.Dispose();
                if (destinationProvider != null) destinationProvider.Dispose();
            }
            return bSync;
        }
        public static void FindFileSystemReplicaChanges(string replicaRootPath, FileSyncScopeFilter filter, FileSyncOptions options)
        {
            FileSyncProvider provider = null;

            try
            {
                provider = new FileSyncProvider(replicaRootPath, filter, options);
                provider.DetectChanges();
            }
            finally
            {
                if (provider != null)
                    provider.Dispose();
            }
        }

        public static void OneWaySyncFileSystemReplicas(string sourceReplicaRootPath, string destinationReplicaRootPath, FileSyncScopeFilter filter, FileSyncOptions options)
        {
            FileSyncProvider path1Provider = null;
            FileSyncProvider path2Provider = null;

            try
            {
                path1Provider = new FileSyncProvider(sourceReplicaRootPath, filter, options);
                path2Provider = new FileSyncProvider(destinationReplicaRootPath, filter, options);

                path2Provider.SkippedChange += OnSkippedChange;
                path2Provider.AppliedChange += OnAppliedChange;

                SyncOrchestrator manager = new SyncOrchestrator();
                manager.LocalProvider = path1Provider;
                manager.RemoteProvider = path2Provider;
                manager.Direction = SyncDirectionOrder.Upload;
                manager.Synchronize();
            }
            finally
            {
                if (path1Provider != null)
                    path1Provider.Dispose();
                if (path2Provider != null)
                    path2Provider.Dispose();
            }
        }
        public static void OnAppliedChange(object sender, AppliedChangeEventArgs args)
        {
            switch (args.ChangeType)
            {
                case ChangeType.Create:
                    Console.WriteLine("-- Applied CREATE for file " + args.NewFilePath);
                    break;
                case ChangeType.Delete:
                    Console.WriteLine("-- Applied DELETE for file " + args.OldFilePath);
                    break;
                case ChangeType.Update:
                    Console.WriteLine("-- Applied update for file " + args.OldFilePath);
                    break;
                case ChangeType.Rename:
                    Console.WriteLine("-- Applied RENAME for file " + args.OldFilePath +
                                      " as " + args.NewFilePath);
                    break;
            }
        }

        public static void OnSkippedChange(object sender, SkippedChangeEventArgs args)
        {
            Console.WriteLine("-- Skipped applying " + args.ChangeType.ToString().ToUpper()
                  + " for " + (!string.IsNullOrEmpty(args.CurrentFilePath) ?
                                args.CurrentFilePath : args.NewFilePath) + " due to error");

            if (args.Exception != null)
                Console.WriteLine("   [" + args.Exception.Message + "]");
        }

    }
}
