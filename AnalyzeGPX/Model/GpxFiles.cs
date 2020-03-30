using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AnalyzeGPX
{
    public class GpxFiles
    {
        public GpxFiles()
        {
            this.Items = new ObservableCollection<GpxFiles>();
        }
        public string Title { get; set; }
        public string Path { get; set; }

        // Children (i.e. drives with gpx files or gpx files themselves)
        public ObservableCollection<GpxFiles> Items { get; set; }
    }
}
