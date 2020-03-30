using System;
using System.Windows;
using System.IO;
using Microsoft.Win32;

namespace AnalyzeGPX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; } = new MainWindowViewModel();


        public const string WindowTitle = "Analyze GPX";

        private WelcomePage startPage = new WelcomePage();
        private ListGpxFilesPage listGpxFilesPage;
        public GpxContentPage gpxContentPage { get; set; }

        #region Start up
        public MainWindow()
        {
            InitializeComponent();
            Main.Content = startPage;
            this.Title = WindowTitle + " - " + startPage.Title;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MinWidth = this.Width;
            this.MinHeight = this.Height;
            this.MaxWidth = SystemParameters.WorkArea.Width;
            this.MaxHeight = SystemParameters.WorkArea.Height;
        }
        #endregion


        #region Methods/Actions which must be replaced by commands

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "GPX files (*.gpx)|*.gpx";
            if (openFileDialog.ShowDialog() == true)
            {
                if (gpxContentPage == null)
                    gpxContentPage = new GpxContentPage();
                Main.Content = gpxContentPage;
                this.Title = WindowTitle + " - " + gpxContentPage.Title;
                try
                {
                     gpxContentPage.GpxContentUserControl.GpxFile.LoadTables(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void LoadGarminButton_Click(object sender, RoutedEventArgs e)
        {
            if (listGpxFilesPage == null)
            {
                listGpxFilesPage = new ListGpxFilesPage();
            }
            Main.Content = listGpxFilesPage;
            this.Title = WindowTitle + " - " + listGpxFilesPage.Title;

            listGpxFilesPage.PopulateTreeView();
        }

        #endregion
    }
}
