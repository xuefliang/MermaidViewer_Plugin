using System;
using System.IO;
using System.Windows.Forms;

namespace MermaidViewer.Forms
{
    /// <summary>
    /// Simple preview form for displaying rendered Mermaid diagrams
    /// </summary>
    public class PreviewForm : Form
    {
        private WebBrowser _webBrowser;
        private string _currentSvg;

        public PreviewForm()
        {
            InitializeComponent();
            Text = "Mermaid Viewer - Preview";
            Width = 800;
            Height = 600;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeComponent()
        {
            _webBrowser = new WebBrowser();
            _webBrowser.Dock = DockStyle.Fill;
            _webBrowser.ScriptErrorsSuppressed = true;

            Controls.Add(_webBrowser);

            // Handle browser document completed
            _webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // Ensure SVG is displayed at original size
            try
            {
                if (_webBrowser.Document != null && _webBrowser.Document.Body != null)
                {
                    _webBrowser.Document.Body.Style = "margin:0;padding:0;overflow:auto;background:#ffffff;";
                }
            }
            catch { }
        }

        /// <summary>
        /// Displays SVG content in the preview
        /// </summary>
        public void DisplaySvg(string svg)
        {
            _currentSvg = svg;

            if (string.IsNullOrEmpty(svg))
            {
                _webBrowser.DocumentText = @"<!DOCTYPE html>
<html>
<head><style>body{font-family:Arial;padding:20px;color:#666;}</style></head>
<body><p>No diagram to display</p></body>
</html>";
                return;
            }

            // Create HTML wrapper for SVG
            string html = @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"">
    <style>
        * { margin: 0; padding: 0; box-sizing: border-box; }
        body { 
            background: #ffffff; 
            padding: 10px; 
            overflow: auto;
            font-family: Arial, sans-serif;
        }
        svg { 
            display: block; 
            max-width: 100%; 
            height: auto;
        }
    </style>
</head>
<body>
" + svg + @"
</body>
</html>";

            _webBrowser.DocumentText = html;
        }
    }
}
