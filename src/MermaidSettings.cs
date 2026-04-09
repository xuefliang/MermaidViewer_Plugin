using System;
using System.IO;
using System.Xml.Linq;

namespace MermaidViewer
{
    /// <summary>
    /// Settings for the Mermaid Viewer plugin
    /// </summary>
    public class MermaidSettings
    {
        public bool AutoRefresh { get; set; } = true;
        public bool DarkMode { get; set; } = false;
        public int RefreshDelayMs { get; set; } = 500;
        public double DefaultPngScale { get; set; } = 2.0;
        public string DefaultExportPath { get; set; } = "";
        public string MmdrPath { get; set; } = "";
        public bool FollowNotepadDarkMode { get; set; } = true;

        /// <summary>
        /// Loads settings from XML file
        /// </summary>
        public static MermaidSettings Load(string filePath)
        {
            var settings = new MermaidSettings();

            if (File.Exists(filePath))
            {
                try
                {
                    var doc = XDocument.Load(filePath);
                    var root = doc.Root;

                    if (root != null)
                    {
                        bool.TryParse(root.Element("AutoRefresh")?.Value, out var autoRefresh);
                        settings.AutoRefresh = autoRefresh;

                        bool.TryParse(root.Element("DarkMode")?.Value, out var darkMode);
                        settings.DarkMode = darkMode;

                        int.TryParse(root.Element("RefreshDelayMs")?.Value, out var refreshDelay);
                        settings.RefreshDelayMs = refreshDelay > 0 ? refreshDelay : 500;

                        double.TryParse(root.Element("DefaultPngScale")?.Value, out var pngScale);
                        settings.DefaultPngScale = pngScale > 0 ? pngScale : 2.0;

                        settings.DefaultExportPath = root.Element("DefaultExportPath")?.Value ?? "";
                        settings.MmdrPath = root.Element("MmdrPath")?.Value ?? "";

                        bool.TryParse(root.Element("FollowNotepadDarkMode")?.Value, out var followDarkMode);
                        settings.FollowNotepadDarkMode = followDarkMode;
                    }
                }
                catch { }
            }

            return settings;
        }

        /// <summary>
        /// Saves settings to XML file
        /// </summary>
        public void Save(string filePath)
        {
            try
            {
                var dir = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var doc = new XDocument(
                    new XElement("Settings",
                        new XElement("AutoRefresh", AutoRefresh),
                        new XElement("DarkMode", DarkMode),
                        new XElement("RefreshDelayMs", RefreshDelayMs),
                        new XElement("DefaultPngScale", DefaultPngScale),
                        new XElement("DefaultExportPath", DefaultExportPath ?? ""),
                        new XElement("MmdrPath", MmdrPath ?? ""),
                        new XElement("FollowNotepadDarkMode", FollowNotepadDarkMode)
                    )
                );

                doc.Save(filePath);
            }
            catch { }
        }

        /// <summary>
        /// Creates a deep copy of the settings
        /// </summary>
        public MermaidSettings Clone()
        {
            return new MermaidSettings
            {
                AutoRefresh = AutoRefresh,
                DarkMode = DarkMode,
                RefreshDelayMs = RefreshDelayMs,
                DefaultPngScale = DefaultPngScale,
                DefaultExportPath = DefaultExportPath,
                MmdrPath = MmdrPath,
                FollowNotepadDarkMode = FollowNotepadDarkMode
            };
        }
    }
}
