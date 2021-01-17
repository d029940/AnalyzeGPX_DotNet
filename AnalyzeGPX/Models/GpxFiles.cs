using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace AnalyzeGPX
{
    public class GpxFiles
    {
        const string gpxDirectory = "Garmin\\GPX";

        public GpxFiles()
        {
            this.Items = new ObservableCollection<GpxFiles>();
            //this.Items = new List<GpxFiles>();
        }
        public string Title { get; set; }
        public string Path { get; set; }

        // Children (i.e. drives with gpx files or gpx files themselves)
        public ObservableCollection<GpxFiles> Items { get; }
        //public List<GpxFiles> Items { get; }

        /// <summary>
        /// Reads from all attached devices and looks for a GPX files in \Garmin\GPX\ folders.
        /// It saves all GPX files in a tree structure
        /// There is no check whether the contents of a GPX file is valid.
        /// </summary>
        public void ReadDrives()
        {
            Items.Clear();
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                string gpxDirectoryPath = drive.Name + gpxDirectory;
                if (Directory.Exists(gpxDirectoryPath))
                {
                    GpxFiles tempGpx = new GpxFiles()
                    {
                        Title = (drive.Name + " " + drive.VolumeLabel),
                        Path = ""
                    };
                    Items.Add(tempGpx);
                    string[] gpxFileList = Directory.GetFiles(gpxDirectoryPath, "*.gpx");
                    foreach (var gpxFile in gpxFileList)
                    {
                        tempGpx.Items.Add(new GpxFiles()
                        {
                            Title = System.IO.Path.GetFileName(gpxFile),
                            Path = System.IO.Path.GetDirectoryName(gpxFile)
                        });
                    }

                    // TODO: Implement the Watcher correctly
                    // Watch directory for changes
                    using (FileSystemWatcher watcher = new FileSystemWatcher())
                    {
                        watcher.Path = gpxDirectoryPath;

                        // Watch for changes in LastAccess and LastWrite times, and
                        // the renaming of files or directories.
                        watcher.NotifyFilter = NotifyFilters.LastAccess 
                            | NotifyFilters.LastWrite 
                            | NotifyFilters.FileName 
                            | NotifyFilters.DirectoryName;

                        // Only watch GPX files.
                        watcher.Filter = "*.gpx";

                        // Add event handlers.
                        watcher.Changed += OnChanged;
                        watcher.Created += OnChanged;
                        watcher.Deleted += OnChanged;
                        watcher.Renamed += OnChanged;
                    }
                }
            }
        }

        public void DeleteGpxFile(string path)
        {
            // check for drive / device
            FileInfo fileInfo = new FileInfo(path);
            if ((fileInfo.Attributes & FileAttributes.Normal) != FileAttributes.Normal) return;

            string dirName = fileInfo.DirectoryName;

            foreach (var item in Items)
            {
                if (item.Path == dirName)
                {
                    var children = item.Items;

                    // check for path
                    GpxFiles gpxFile = null;
                    foreach (var file in children)
                    {
                        if (file.Path == path)
                        {
                            gpxFile = file;
                            break;
                        }
                    }
                    if (gpxFile == null) return;

                    // Remove path from internal table
                    children.Remove(gpxFile);
                    // delete path (file)
                    // TODO: go on here
                    fileInfo.Delete();
                    return;
                }
            }
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e) =>
            // Reload when a file is changed, created, or deleted.
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
    }
}
