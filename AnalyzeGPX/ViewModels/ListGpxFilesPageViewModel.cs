using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Diagnostics;

namespace AnalyzeGPX
{
    public class ListGpxFilesPageViewModel
    {
        private RelayCommand<GpxFiles> _selectedItemChangedCmd = null;
        public RelayCommand<GpxFiles> SelectedItemChangedCmd =>
            _selectedItemChangedCmd ?? (_selectedItemChangedCmd = new RelayCommand<GpxFiles>(SelectedItemChanged));

        private bool CanSelectedItemChanged() => true;
        private void SelectedItemChanged(GpxFiles gpxFiles)
        {
            //Construct filename of selected item
            string filename = gpxFiles.Path + Path.DirectorySeparatorChar + gpxFiles.Title;

            // Get Page for listing GPX content
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            if (mainWindow.gpxContentPage == null)
                mainWindow.gpxContentPage = new GpxContentPage();

            // Parse GPX file
            try
            {
                mainWindow.gpxContentPage.GpxContentUserControl.GpxFile.LoadTables(filename);
            }
            catch (System.Xml.XmlException ex)
            {
                ViewHelpers.ShowGPXErrorMessage(ex);
                return;
            }
            mainWindow.Main.Content = mainWindow.gpxContentPage;
            mainWindow.Title = MainWindow.WindowTitle + " - " + mainWindow.gpxContentPage.Title;
        }

        private RelayCommand<GpxFiles> _isSelected = null;
        public RelayCommand<GpxFiles> IsSelectedCmd =>
            _isSelected ?? (_isSelected = new RelayCommand<GpxFiles>(IsSelected));

        private bool CanIsSelected() => true;
        private void IsSelected(GpxFiles gpxFiles)
        {
            Debug.WriteLine("IsSelected Cmd");
        }

    }
}
