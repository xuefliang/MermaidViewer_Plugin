using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using MermaidViewer.Rendering;

namespace MermaidViewer.Forms
{
    /// <summary>
    /// Preview form for displaying rendered Mermaid diagrams
    /// </summary>
    public partial class MermaidPreviewForm : UserControl
    {
        private MermaidRenderer _renderer;
        private string _currentSvg;
        private double _zoomLevel = 1.0;
        private Point _panOffset = new Point(0, 0);
        private bool _isPanning = false;
        private Point _lastMousePos;
        private int _currentDiagramIndex = 0;
        private int _totalDiagrams = 1;
        private string _statusMessage = "";
        private bool _isDarkMode;

        public event EventHandler RefreshRequested;
        public event EventHandler ExportSvgRequested;
        public event EventHandler ExportPngRequested;

        /// <summary>
        /// Gets or sets whether dark mode is enabled
        /// </summary>
        public bool IsDarkMode
        {
            get { return _isDarkMode; }
            set
            {
                _isDarkMode = value;
                UpdateColors();
            }
        }

        /// <summary>
        /// Gets the current SVG content
        /// </summary>
        public string CurrentSvg => _currentSvg;

        /// <summary>
        /// Gets the current status message
        /// </summary>
        public string StatusMessage => _statusMessage;

        public MermaidPreviewForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);

            // Initialize renderer
            string mmdrPath = GetMmdrPath();
            _renderer = new MermaidRenderer(mmdrPath);
        }

        private string GetMmdrPath()
        {
            // Try plugin directory first
            string pluginDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string mmdrPath = Path.Combine(pluginDir, "mmdr.exe");

            if (File.Exists(mmdrPath))
                return mmdrPath;

            // Try tools subdirectory
            mmdrPath = Path.Combine(pluginDir, "tools", "mmdr", "mmdr.exe");
            if (File.Exists(mmdrPath))
                return mmdrPath;

            // Fallback to PATH
            return "mmdr.exe";
        }

        private void UpdateColors()
        {
            if (_isDarkMode)
            {
                BackColor = Color.FromArgb(45, 45, 48);
                ForeColor = Color.White;
            }
            else
            {
                BackColor = SystemColors.Window;
                ForeColor = SystemColors.WindowText;
            }
            Invalidate();
        }

        /// <summary>
        /// Renders Mermaid content asynchronously
        /// </summary>
        public async Task RenderAsync(string mermaidContent)
        {
            if (string.IsNullOrWhiteSpace(mermaidContent))
            {
                _currentSvg = null;
                _statusMessage = "No content to render";
                Invalidate();
                return;
            }

            try
            {
                // Extract the current diagram
                string diagram = MermaidRenderer.ExtractDiagram(mermaidContent, _currentDiagramIndex);
                _totalDiagrams = MermaidRenderer.CountDiagrams(mermaidContent);

                _statusMessage = $"Rendering diagram {_currentDiagramIndex + 1} of {_totalDiagrams}...";
                Invalidate();

                // Render to SVG
                _currentSvg = await _renderer.RenderToSvgAsync(diagram);

                if (_currentSvg != null)
                {
                    _statusMessage = $"Rendered successfully ({_currentSvg.Length} bytes)";
                    _zoomLevel = 1.0;
                    _panOffset = new Point(0, 0);
                }
                else
                {
                    _statusMessage = $"Error: {_renderer.LastError ?? "Unknown error"}";
                }
            }
            catch (Exception ex)
            {
                _currentSvg = null;
                _statusMessage = $"Exception: {ex.Message}";
            }

            Invalidate();
        }

        /// <summary>
        /// Navigates to the next diagram
        /// </summary>
        public void NextDiagram()
        {
            if (_currentDiagramIndex < _totalDiagrams - 1)
            {
                _currentDiagramIndex++;
                RefreshRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Navigates to the previous diagram
        /// </summary>
        public void PreviousDiagram()
        {
            if (_currentDiagramIndex > 0)
            {
                _currentDiagramIndex--;
                RefreshRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Zooms in
        /// </summary>
        public void ZoomIn()
        {
            _zoomLevel = Math.Min(_zoomLevel * 1.25, 5.0);
            Invalidate();
        }

        /// <summary>
        /// Zooms out
        /// </summary>
        public void ZoomOut()
        {
            _zoomLevel = Math.Max(_zoomLevel / 1.25, 0.1);
            Invalidate();
        }

        /// <summary>
        /// Resets zoom and pan
        /// </summary>
        public void ResetView()
        {
            _zoomLevel = 1.0;
            _panOffset = new Point(0, 0);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.Clear(_isDarkMode ? Color.FromArgb(45, 45, 48) : SystemColors.Window);

            if (string.IsNullOrEmpty(_currentSvg))
            {
                // Draw placeholder
                using (Font font = new Font("Segoe UI", 10))
                using (Brush brush = new SolidBrush(_isDarkMode ? Color.Gray : SystemColors.GrayText))
                {
                    string message = string.IsNullOrEmpty(_statusMessage) 
                        ? "Open a Mermaid file or paste diagram code,\nthen press Refresh to preview"
                        : _statusMessage;
                    
                    var size = g.MeasureString(message, font);
                    g.DrawString(message, font, brush, 
                        (Width - size.Width) / 2, 
                        (Height - size.Height) / 2);
                }
                return;
            }

            try
            {
                // Render SVG to bitmap using our simple renderer
                using (var bitmap = SimpleSvgRenderer.RenderSvgToBitmap(_currentSvg, (float)_zoomLevel))
                {
                    // Calculate center position
                    int drawX = (Width - bitmap.Width) / 2 + _panOffset.X;
                    int drawY = (Height - bitmap.Height) / 2 + _panOffset.Y;

                    g.DrawImage(bitmap, drawX, drawY);
                }
            }
            catch (Exception ex)
            {
                using (Brush brush = new SolidBrush(Color.Red))
                {
                    g.DrawString($"Error rendering SVG:\n{ex.Message}", 
                        new Font("Consolas", 9), brush, 10, 10);
                }
            }

            // Draw status bar
            DrawStatusBar(g);

            // Draw navigation buttons if multiple diagrams
            if (_totalDiagrams > 1)
            {
                DrawNavigation(g);
            }

            // Draw zoom indicator
            DrawZoomIndicator(g);
        }

        private void DrawStatusBar(Graphics g)
        {
            int barHeight = 24;
            Rectangle statusBarRect = new Rectangle(0, Height - barHeight, Width, barHeight);

            using (Brush bgBrush = new SolidBrush(_isDarkMode ? Color.FromArgb(63, 63, 70) : SystemColors.Control))
            using (Brush textBrush = new SolidBrush(_isDarkMode ? Color.White : SystemColors.WindowText))
            using (Font font = new Font("Segoe UI", 9))
            {
                g.FillRectangle(bgBrush, statusBarRect);
                g.DrawLine(new Pen(_isDarkMode ? Color.FromArgb(80, 80, 80) : SystemColors.GrayText), 
                    0, statusBarRect.Top, Width, statusBarRect.Top);

                // Draw status message
                string status = _statusMessage;
                if (_totalDiagrams > 1)
                    status += $" (Diagram {_currentDiagramIndex + 1}/{_totalDiagrams})";
                
                g.DrawString(status, font, textBrush, 8, statusBarRect.Top + 4);
            }
        }

        private void DrawNavigation(Graphics g)
        {
            int buttonWidth = 32;
            int buttonHeight = 32;
            int margin = 5;

            // Previous button
            Rectangle prevBtn = new Rectangle(margin, margin, buttonWidth, buttonHeight);
            bool prevEnabled = _currentDiagramIndex > 0;
            
            using (Brush btnBrush = new SolidBrush(_isDarkMode ? Color.FromArgb(63, 63, 70) : SystemColors.Control))
            using (Brush arrowBrush = new SolidBrush(prevEnabled ? (_isDarkMode ? Color.White : SystemColors.WindowText) : SystemColors.GrayText))
            {
                g.FillRectangle(btnBrush, prevBtn);
                g.FillPolygon(arrowBrush, new Point[] {
                    new Point(prevBtn.Right - 12, prevBtn.Top + prevBtn.Height / 2),
                    new Point(prevBtn.Left + 8, prevBtn.Top + 8),
                    new Point(prevBtn.Left + 8, prevBtn.Bottom - 8)
                });
            }

            // Next button
            Rectangle nextBtn = new Rectangle(margin + buttonWidth + margin, margin, buttonWidth, buttonHeight);
            bool nextEnabled = _currentDiagramIndex < _totalDiagrams - 1;
            
            using (Brush btnBrush = new SolidBrush(_isDarkMode ? Color.FromArgb(63, 63, 70) : SystemColors.Control))
            using (Brush arrowBrush = new SolidBrush(nextEnabled ? (_isDarkMode ? Color.White : SystemColors.WindowText) : SystemColors.GrayText))
            {
                g.FillRectangle(btnBrush, nextBtn);
                g.FillPolygon(arrowBrush, new Point[] {
                    new Point(nextBtn.Left + 12, nextBtn.Top + nextBtn.Height / 2),
                    new Point(nextBtn.Right - 8, nextBtn.Top + 8),
                    new Point(nextBtn.Right - 8, nextBtn.Bottom - 8)
                });
            }
        }

        private void DrawZoomIndicator(Graphics g)
        {
            using (Font font = new Font("Segoe UI", 9))
            using (Brush brush = new SolidBrush(_isDarkMode ? Color.FromArgb(180, 180, 180) : SystemColors.GrayText))
            {
                string zoomText = $"{(_zoomLevel * 100):F0}%";
                var size = g.MeasureString(zoomText, font);
                g.DrawString(zoomText, font, brush, Width - size.Width - 10, 10);
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (Control.ModifierKeys == Keys.Control)
            {
                if (e.Delta > 0)
                    ZoomIn();
                else
                    ZoomOut();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                _isPanning = true;
                _lastMousePos = e.Location;
                Cursor = Cursors.Hand;
            }
            else if (e.Button == MouseButtons.Right)
            {
                ShowContextMenu(e.Location);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_isPanning)
            {
                _panOffset.X += e.X - _lastMousePos.X;
                _panOffset.Y += e.Y - _lastMousePos.Y;
                _lastMousePos = e.Location;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_isPanning)
            {
                _isPanning = false;
                Cursor = Cursors.Default;
            }
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            ResetView();
        }

        private void ShowContextMenu(Point location)
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            menu.Items.Add("Refresh", null, (s, e) => RefreshRequested?.Invoke(this, EventArgs.Empty));
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Zoom In", null, (s, e) => ZoomIn());
            menu.Items.Add("Zoom Out", null, (s, e) => ZoomOut());
            menu.Items.Add("Reset View", null, (s, e) => ResetView());
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Export as SVG...", null, (s, e) => ExportSvgRequested?.Invoke(this, EventArgs.Empty));
            menu.Items.Add("Export as PNG...", null, (s, e) => ExportPngRequested?.Invoke(this, EventArgs.Empty));

            menu.Show(this, location);
        }

        /// <summary>
        /// Exports the current diagram to SVG file
        /// </summary>
        public async Task ExportToSvgAsync(string filePath)
        {
            if (string.IsNullOrEmpty(_currentSvg))
                return;

            await Task.Run(() => File.WriteAllText(filePath, _currentSvg));
        }

        /// <summary>
        /// Exports the current diagram to PNG file
        /// </summary>
        public async Task ExportToPngAsync(string filePath, double scale = 2.0)
        {
            if (string.IsNullOrEmpty(_currentSvg))
                return;

            byte[] pngData = await _renderer.RenderToPngAsync(_currentSvg, scale);
            if (pngData != null)
            {
                await Task.Run(() => File.WriteAllBytes(filePath, pngData));
            }
        }
    }
}
