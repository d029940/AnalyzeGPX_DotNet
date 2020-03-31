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
        }
        public string Title { get; set; }
        public string Path { get; set; }

        // Children (i.e. drives with gpx files or gpx files themselves)
        public ObservableCollection<GpxFiles> Items { get; }


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
                }
            }
        }
    }
}
