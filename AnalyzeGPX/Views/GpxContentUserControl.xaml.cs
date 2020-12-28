using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnalyzeGPX.Views
{
    /// <summary>
    /// UserControl for displaying GPX content
    /// </summary>
    public partial class GpxContentUserControl : UserControl
    {
        private Visibility mHScrollbarVisible = Visibility.Hidden;

        private GarminGpxFile gpxFile = new GarminGpxFile();
        public GarminGpxFile GpxFile { get => gpxFile; }

        public GpxContentUserControl()
        {
            InitializeComponent();
            this.DataContext = this.gpxFile;
        }

        private void ListBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // TODO: check how the track, route & waypoint listboxes interfere
            // Check if horizontal scrollbar is visible or not and adjust the height of the listbox view
            var scrollViewer = FindVisualChild < ScrollViewer >((ListBox)sender);
            var horizontalScrollBarVisibility = scrollViewer.ComputedHorizontalScrollBarVisibility;
            if (horizontalScrollBarVisibility != mHScrollbarVisible)
            {
                mHScrollbarVisible = horizontalScrollBarVisibility;
                if (horizontalScrollBarVisibility == Visibility.Visible)
                {
                    Height += 10;
                }
            }

        }

        #region Helpers
        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        #endregion

    }
}
