using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using System.Windows;

namespace AnalyzeGPX
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Application-wide resource manager
        public static ResourceManager ResourceManager { get; } = new ResourceManager(typeof(Resources));
    }
}
