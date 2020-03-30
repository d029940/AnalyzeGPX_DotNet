using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace AnalyzeGPX
{
    /// <summary>
    /// Represents a Garmin GPX file
    /// </summary>
    public class GarminGpxFile
    {
        #region Public Properties

        /// <summary>
        /// Currently only Tracks, Routes and Waypoints (POIs) are taken into account
        /// </summary>
        public enum ContentType
        {
            trk,    // Track
            rte,    // Route
            wpt     // Waypoint
        }

        /// <summary>
        /// These properties are bound in the GUI
        /// </summary>
        public ObservableCollection<string> Tracks { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> Routes { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> Waypoints { get; } = new ObservableCollection<string>();

        #endregion

        #region Private members

        private string filename;

        /// <summary>
        /// For XML parsing
        /// </summary>
        private XElement gpx;
        private XNamespace defaultNS;

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes an object meant to represent a Garmn GPX file
        /// </summary>
        public GarminGpxFile()
        {
        }

        /// <summary>
        /// Loads a garmin GPX file in the gpxFile object <see cref="GarminGpxFile"/>
        /// </summary>
        /// <param name="filename">The full path of the GPX file to read</param>
        /// <exception cref="FileFormatException">If the GPX file does not conform to XML format</exception>
        public void LoadTables(string filename)
        {
            try
            {
                this.Load(filename);
            }
            catch (FileFormatException)
            {
                throw;
            }
            this.GetAllTypes();
        }

        /// <summary>
        /// Loads a GPX file which needs then to be parsed with <see cref="GetAllTypes()"/>
        /// </summary>
        /// <param name="filename">The full path of the GPX file to read</param>
        /// <exception cref="FileFormatException">If the GPX file does not conform to XML format</exception>
        public void Load(string filename)
        {
            this.filename = filename;
            Tracks.Clear();
            Routes.Clear();
            Waypoints.Clear();

            gpx = XElement.Load(filename);
            if (gpx == null)
                throw new FileFormatException();
            defaultNS = gpx.GetDefaultNamespace();
        }

        /// <summary>
        /// Find all occurences as there are taken into account according to the <see cref="ContentType"/>
        /// </summary>
        public void GetAllTypes()
        {
            var types = Enum.GetValues(typeof(ContentType)).Cast<ContentType>();
            foreach (var type in types)
            {
                GetContentType(type);
            }
        }

        #endregion

        #region Private Methods for parsing the XML file

        /// <summary>
        /// Find all occurences of a specifc "type" in the GPX XML file
        /// </summary>
        /// <param name="type"><see cref="ContentType"/></param>
        public void GetContentType(ContentType type)
        {
            IEnumerable<string> elements = from t in gpx.Descendants(defaultNS + type.ToString())
                                           select t.Element(defaultNS + "name").Value;
            foreach (var element in elements)
            {
                switch (type)
                {
                    case ContentType.trk:
                        Tracks.Add(element);
                        break;
                    case ContentType.rte:
                        Routes.Add(element);
                        break;
                    case ContentType.wpt:
                        Waypoints.Add(element);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
    }
}
