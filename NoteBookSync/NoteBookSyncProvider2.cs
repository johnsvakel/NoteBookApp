using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Synchronization.Files;
using Microsoft.Synchronization;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using System.ComponentModel;
using NoteBookObj;

namespace NoteBookSync 
{
    public class NoteSyncProviderBase : INotifyPropertyChanged
    {
        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class NoteSyncProvider : NoteSyncProviderBase
    {
        private NoteAppPreference objPreference;
        public RelayCommand GetPath1Command { get; set; }
        public RelayCommand GetPath2Command { get; set; }
        public RelayCommand SyncCommand { get; set; }

        string path1;
        public string Path1
        {
            get
            {
                return path1;
            }
            set
            {
                if (path1 != value)
                {
                    path1 = value;
                    RaisePropertyChanged("Path1");
                }
            }
        }

        string path2;
        public string Path2
        {
            get
            {
                return path2;
            }
            set
            {
                if (path2 != value)
                {
                    path2 = value;
                    RaisePropertyChanged("Path2");
                }
            }
        }

        bool isSynchronising;
        public bool IsSynchronising
        {
            get
            {
                return isSynchronising;
            }
            set
            {
                if (isSynchronising != value)
                {
                    isSynchronising = value;
                    RaisePropertyChanged("IsSynchronising");
                }
            }
        }

        ObservableCollection<string> log;
        public ObservableCollection<string> Log
        {
            get
            {
                return log;
            }
            set
            {
                if (log != value)
                {
                    log = value;
                    RaisePropertyChanged("Log");
                }
            }
        }

        ObservableCollection<FileInfo> path1Files;
        public ObservableCollection<FileInfo> Path1Files
        {
            get
            {
                return path1Files;
            }
            set
            {
                if (path1Files != value)
                {
                    path1Files = value;
                    RaisePropertyChanged("Path1Files");
                }
            }
        }

        ObservableCollection<FileInfo> path2Files;
        public ObservableCollection<FileInfo> Path2Files
        {
            get
            {
                return path2Files;
            }
            set
            {
                if (path2Files != value)
                {
                    path2Files = value;
                    RaisePropertyChanged("Path2Files");
                }
            }
        }

        const string FOLDER1 = "Local";
        const string FOLDER2 = "Destination";

        public NoteSyncProvider()
        {
            //SetUpDummyFolders();
            //ReloadFileLists();

            GetPath1Command = new RelayCommand(GetPath1);
            GetPath2Command = new RelayCommand(GetPath2);
            SyncCommand = new RelayCommand(TrySync);

            Mediator.Register("update", AddUpdate); // Listener for change events
        }

        private void GetPath1(object param)
        {
            Path1 = objPreference.GetLocalServer();          
        }

        private void GetPath2(object param)
        {
            Path2 = objPreference.GetServerInfo();           
        }

        public void TrySync(object param)
        {
            if (string.IsNullOrEmpty(Path1) || string.IsNullOrEmpty(path2) || !Directory.Exists(path1) || !Directory.Exists(Path2))
            {
                MessageBox.Show("Please supply two valid file paths");
                return;
            }

            IsSynchronising = true;
            // Put sync on background thread
            Task.Factory.StartNew(() => { DoSync(); }).ContinueWith(_ => { IsSynchronising = false; });

        }

        void AddUpdate(object param)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                Log.Add(param as string);
            }));
        }

        private void ReloadFileLists()
        {
            var di = new DirectoryInfo(FOLDER1);
            Path1Files = new ObservableCollection<FileInfo>(di.GetFiles());
            di = new DirectoryInfo(FOLDER2);
            Path2Files = new ObservableCollection<FileInfo>(di.GetFiles());
        }

        private void SetUpDummyFolders()
        {
            DirectoryInfo directory1 = new DirectoryInfo(FOLDER1);
            DirectoryInfo directory2 = new DirectoryInfo(FOLDER2);

            if (!directory1.Exists)
                Directory.CreateDirectory(FOLDER1);
            if (!directory2.Exists)
                Directory.CreateDirectory(FOLDER2);

            //clear out old files
            foreach (System.IO.FileInfo file in directory1.GetFiles()) file.Delete();
            foreach (System.IO.FileInfo file in directory2.GetFiles()) file.Delete();

            int fileNumber = 1;
            for (var a = 0; a < 5; a++)
                File.Create(System.IO.Path.Combine(directory1.ToString(), "File_" + fileNumber++ + ".txt"));
            for (var a = 0; a < 5; a++)
                File.Create(System.IO.Path.Combine(directory2.ToString(), "File_" + fileNumber++ + ".txt"));
            File.Create(System.IO.Path.Combine(directory2.ToString(), "File_" + fileNumber++ + ".pete"));

            Path1 = directory1.FullName;
            Path2 = directory2.FullName;
        }
        public void SetPreference(NoteAppPreference objPref)
        {
            objPreference = objPref;
            path1 = objPreference.GetLocalServer();
            path2 = objPreference.GetServerInfo();
        }
        public void DoSync()
        {
            try
            {
                Log = new ObservableCollection<string>(); //reset log

                FileSyncOptions options = FileSyncOptions.ExplicitDetectChanges 
                    | FileSyncOptions.RecycleConflictLoserFiles 
                    | FileSyncOptions.RecycleDeletedFiles
                    | FileSyncOptions.RecyclePreviousFileOnUpdates;

                FileSyncScopeFilter filter = new FileSyncScopeFilter();
                filter.FileNameExcludes.Add("*.metadata"); //?

                // Avoid two change detection passes for the two-way sync
                FindFileSystemReplicaChanges(Path1, filter, options);
                FindFileSystemReplicaChanges(Path2, filter, options);

                // Sync both ways
                OneWaySyncFileSystemReplicas(Path1, Path2, null, options);
                OneWaySyncFileSystemReplicas(path2, Path1, null, options);

               // ReloadFileLists();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
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
                    Mediator.NotifyColleagues("update", "File created: " + args.NewFilePath);
                    break;
                case ChangeType.Delete:
                    Mediator.NotifyColleagues("update", "Deleted File: " + args.OldFilePath);
                    break;
                case ChangeType.Update:
                    Mediator.NotifyColleagues("update", "Overwrote file: " + args.OldFilePath);
                    break;
                case ChangeType.Rename:
                    Mediator.NotifyColleagues("update", "Renamed file: " + args.OldFilePath + " to " + args.NewFilePath);
                    break;
            }
        }

        public static void OnSkippedChange(object sender, SkippedChangeEventArgs args)
        {
            Mediator.NotifyColleagues("update", "Error! Skipped file: " + args.ChangeType.ToString().ToUpper() + " for "
                + (!string.IsNullOrEmpty(args.CurrentFilePath) ? args.CurrentFilePath : args.NewFilePath));

            if (args.Exception != null)
                Mediator.NotifyColleagues("update", "Error: " + args.Exception.Message);
        }
    }
}
