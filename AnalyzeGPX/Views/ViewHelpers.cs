﻿using System;
using System.Windows;
using System.Xml;
using System.Globalization;

namespace AnalyzeGPX
{
    /// <summary>
    /// This class contains helpers, which are view related, but where no XAML is used
    /// e.g. Dialogs, ...
    /// This class helps to move UI-related code out of the business logic
    /// </summary>
    public static class ViewHelpers
    {
        /// <summary>
        /// Shows a GPX error message (GPX is based on XML)
        /// </summary>
        /// <param name="ex">exception to show</param>
        internal static void ShowGPXErrorMessage(XmlException ex)
        {
             MessageBox.Show($"File: {new System.Uri(ex.SourceUri).LocalPath} {Environment.NewLine} {ex.Message}",
                App.ResourceManager.GetString("GPX-File - Format Error", CultureInfo.CurrentUICulture),
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
