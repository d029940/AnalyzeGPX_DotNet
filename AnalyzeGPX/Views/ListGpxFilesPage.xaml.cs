using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace AnalyzeGPX
{
    /// <summary>
    /// Interaction logic for ListGpxFilesPage.xaml
    /// </summary>
    public partial class ListGpxFilesPage : Page
    {
        public ListGpxFilesPageViewModel ViewModel { get; set; } = new ListGpxFilesPageViewModel();

        GpxFiles root = new GpxFiles() { Title = "Drives" };
        const string gpxDirectory = "Garmin\\GPX";

        #region Start up and Page Appearance handling
        public ListGpxFilesPage()
        {
            InitializeComponent();
            // MARK: If top level root should be shown uncomment t´follwing line
            //trvMenu.Items.Add(root);
            trvMenu.ItemsSource = root.Items;
        }

        #endregion


        public void PopulateTreeView()
        {
            // TODO: Create Commands: Should go into another file
            root.Items.Clear();
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
                    root.Items.Add(tempGpx);
                    string[] gpxFileList = Directory.GetFiles(gpxDirectoryPath, "*.gpx");
                    foreach (var gpxFile in gpxFileList)
                    {
                        tempGpx.Items.Add(new GpxFiles()
                        {
                            Title = Path.GetFileName(gpxFile),
                            Path = Path.GetDirectoryName(gpxFile)
                        });
                    }
                }
            }
            if (root.Items.Count == 0)
                trvMenu.Visibility = Visibility.Hidden;
            else
                trvMenu.Visibility = Visibility.Visible;
        }

        // TODO: Create Commands???
        /// <summary>
        /// The item selected in a treeview, i.e. its title, is converted to a full filename
        /// This filename is considered as a GPX file and parsed. The content of this GPX file
        /// is displayed on <see cref="GpxContentPage"/> Page.
        /// </summary>
        /// <param name="sender">treeview</param>
        /// <param name="e">represents the selected treeview item</param>
        private void trvMenu_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // Remark about MVVM pattern
            // Only the construction of the GPX filename and its parsing is GUI-independent.
            // Displaying the result, i.e. the content is GUI dependant - in this case, it is a gpxContentPage

            //Construct filename of selected item
            string filename = ((GpxFiles)e.NewValue).Path + Path.DirectorySeparatorChar + ((GpxFiles)e.NewValue).Title;

            // Get Page for listing GPX content
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            if (mainWindow.gpxContentPage == null)
                mainWindow.gpxContentPage = new GpxContentPage();
            mainWindow.Main.Content = mainWindow.gpxContentPage;
            mainWindow.Title = MainWindow.WindowTitle + " - " + mainWindow.gpxContentPage.Title;

            // Parse GPX file
            try
            {
                mainWindow.gpxContentPage.GpxContentUserControl.GpxFile.LoadTables(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
