using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.RegularExpressions;

namespace MermaidViewer.Rendering
{
    /// <summary>
    /// A lightweight SVG renderer that converts SVG to Bitmap
    /// This is a simplified implementation for basic SVG rendering
    /// For full SVG support, consider using Svg.dll or System.Drawing.Common
    /// </summary>
    public class SimpleSvgRenderer
    {
        /// <summary>
        /// Renders SVG content to a bitmap
        /// </summary>
        public static Bitmap RenderSvgToBitmap(string svgContent, float scale = 1.0f)
        {
            if (string.IsNullOrEmpty(svgContent))
                return new Bitmap(1, 1);

            try
            {
                // Parse SVG dimensions
                float width = 800;
                float height = 600;

                // Extract width
                var widthMatch = Regex.Match(svgContent, @"width\s*=\s*[""']?([\d.]+)", RegexOptions.IgnoreCase);
                if (widthMatch.Success)
                {
                    float.TryParse(widthMatch.Groups[1].Value, out width);
                }

                // Extract height
                var heightMatch = Regex.Match(svgContent, @"height\s*=\s*[""']?([\d.]+)", RegexOptions.IgnoreCase);
                if (heightMatch.Success)
                {
                    float.TryParse(heightMatch.Groups[1].Value, out height);
                }

                // Extract viewBox as fallback
                var viewBoxMatch = Regex.Match(svgContent, @"viewBox\s*=\s*[""']?\s*[\d.]+\s+[\d.]+\s+([\d.]+)\s+([\d.]+)", RegexOptions.IgnoreCase);
                if (viewBoxMatch.Success && widthMatch.Success == false)
                {
                    float.TryParse(viewBoxMatch.Groups[1].Value, out width);
                    float.TryParse(viewBoxMatch.Groups[2].Value, out height);
                }

                // Create bitmap with scale
                int bmpWidth = (int)(width * scale);
                int bmpHeight = (int)(height * scale);
                bmpWidth = Math.Max(bmpWidth, 1);
                bmpHeight = Math.Max(bmpHeight, 1);

                Bitmap bitmap = new Bitmap(bmpWidth, bmpHeight);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.Clear(Color.White);

                    // Render basic SVG elements
                    RenderSvgElements(g, svgContent, bmpWidth, bmpHeight);
                }

                return bitmap;
            }
            catch (Exception)
            {
                // Return a placeholder bitmap on error
                Bitmap bitmap = new Bitmap(400, 300);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);
                    using (Font font = new Font("Arial", 12))
                    using (Brush brush = new SolidBrush(Color.Red))
                    {
                        g.DrawString("SVG rendering error", font, brush, 10, 10);
                    }
                }
                return bitmap;
            }
        }

        private static void RenderSvgElements(Graphics g, string svgContent, int width, int height)
        {
            // This is a simplified SVG renderer
            // For production use, consider using the Svg.dll library

            // Extract and render basic shapes
            RenderRectangles(g, svgContent, width, height);
            RenderCircles(g, svgContent, width, height);
            RenderLines(g, svgContent, width, height);
            RenderPaths(g, svgContent, width, height);
            RenderText(g, svgContent, width, height);
        }

        private static void RenderRectangles(Graphics g, string svg, int canvasWidth, int canvasHeight)
        {
            var matches = Regex.Matches(svg, @"<rect[^>]*>", RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                string rect = match.Value;

                float x = 0, y = 0, w = 100, h = 100;
                string fill = "#000000";
                float strokeWidth = 0;
                string stroke = "none";

                // Parse attributes
                var xMatch = Regex.Match(rect, @"x\s*=\s*[""']?([\d.]+)");
                if (xMatch.Success) float.TryParse(xMatch.Groups[1].Value, out x);

                var yMatch = Regex.Match(rect, @"y\s*=\s*[""']?([\d.]+)");
                if (yMatch.Success) float.TryParse(yMatch.Groups[1].Value, out y);

                var wMatch = Regex.Match(rect, @"width\s*=\s*[""']?([\d.]+)");
                if (wMatch.Success) float.TryParse(wMatch.Groups[1].Value, out w);

                var hMatch = Regex.Match(rect, @"height\s*=\s*[""']?([\d.]+)");
                if (hMatch.Success) float.TryParse(hMatch.Groups[1].Value, out h);

                var fillMatch = Regex.Match(rect, @"fill\s*=\s*[""']?([#\w]+)");
                if (fillMatch.Success) fill = fillMatch.Groups[1].Value;

                var strokeMatch = Regex.Match(rect, @"stroke\s*=\s*[""']?([#\w]+)");
                if (strokeMatch.Success) stroke = strokeMatch.Groups[1].Value;

                var swMatch = Regex.Match(rect, @"stroke-width\s*=\s*[""']?([\d.]+)");
                if (swMatch.Success) float.TryParse(swMatch.Groups[1].Value, out strokeWidth);

                // Scale to canvas
                float scaleX = canvasWidth / 800f;
                float scaleY = canvasHeight / 600f;

                RectangleF rectF = new RectangleF(x * scaleX, y * scaleY, w * scaleX, h * scaleY);

                using (Brush brush = CreateBrush(fill, rectF))
                {
                    g.FillRectangle(brush, rectF);
                }

                if (stroke != "none" && strokeWidth > 0)
                {
                    using (Pen pen = new Pen(ParseColor(stroke), strokeWidth * scaleX))
                    {
                        g.DrawRectangle(pen, rectF.X, rectF.Y, rectF.Width, rectF.Height);
                    }
                }
            }
        }

        private static void RenderCircles(Graphics g, string svg, int canvasWidth, int canvasHeight)
        {
            var matches = Regex.Matches(svg, @"<circle[^>]*>", RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                string circle = match.Value;

                float cx = 50, cy = 50, r = 30;
                string fill = "#000000";
                string stroke = "none";
                float strokeWidth = 0;

                var cxMatch = Regex.Match(circle, @"cx\s*=\s*[""']?([\d.]+)");
                if (cxMatch.Success) float.TryParse(cxMatch.Groups[1].Value, out cx);

                var cyMatch = Regex.Match(circle, @"cy\s*=\s*[""']?([\d.]+)");
                if (cyMatch.Success) float.TryParse(cyMatch.Groups[1].Value, out cy);

                var rMatch = Regex.Match(circle, @"r\s*=\s*[""']?([\d.]+)");
                if (rMatch.Success) float.TryParse(rMatch.Groups[1].Value, out r);

                var fillMatch = Regex.Match(circle, @"fill\s*=\s*[""']?([#\w]+)");
                if (fillMatch.Success) fill = fillMatch.Groups[1].Value;

                var strokeMatch = Regex.Match(circle, @"stroke\s*=\s*[""']?([#\w]+)");
                if (strokeMatch.Success) stroke = strokeMatch.Groups[1].Value;

                var swMatch = Regex.Match(circle, @"stroke-width\s*=\s*[""']?([\d.]+)");
                if (swMatch.Success) float.TryParse(swMatch.Groups[1].Value, out strokeWidth);

                float scaleX = canvasWidth / 800f;
                float scaleY = canvasHeight / 600f;

                float scaledR = r * Math.Min(scaleX, scaleY);

                using (Brush brush = CreateBrush(fill, RectangleF.Empty))
                {
                    g.FillEllipse(brush, (cx - r) * scaleX, (cy - r) * scaleY, scaledR * 2, scaledR * 2);
                }

                if (stroke != "none" && strokeWidth > 0)
                {
                    using (Pen pen = new Pen(ParseColor(stroke), strokeWidth * scaleX))
                    {
                        g.DrawEllipse(pen, (cx - r) * scaleX, (cy - r) * scaleY, scaledR * 2, scaledR * 2);
                    }
                }
            }
        }

        private static void RenderLines(Graphics g, string svg, int canvasWidth, int canvasHeight)
        {
            var matches = Regex.Matches(svg, @"<line[^>]*>", RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                string line = match.Value;

                float x1 = 0, y1 = 0, x2 = 100, y2 = 100;
                string stroke = "#000000";
                float strokeWidth = 1;

                var x1Match = Regex.Match(line, @"x1\s*=\s*[""']?([\d.]+)");
                if (x1Match.Success) float.TryParse(x1Match.Groups[1].Value, out x1);

                var y1Match = Regex.Match(line, @"y1\s*=\s*[""']?([\d.]+)");
                if (y1Match.Success) float.TryParse(y1Match.Groups[1].Value, out y1);

                var x2Match = Regex.Match(line, @"x2\s*=\s*[""']?([\d.]+)");
                if (x2Match.Success) float.TryParse(x2Match.Groups[1].Value, out x2);

                var y2Match = Regex.Match(line, @"y2\s*=\s*[""']?([\d.]+)");
                if (y2Match.Success) float.TryParse(y2Match.Groups[1].Value, out y2);

                var strokeMatch = Regex.Match(line, @"stroke\s*=\s*[""']?([#\w]+)");
                if (strokeMatch.Success) stroke = strokeMatch.Groups[1].Value;

                var swMatch = Regex.Match(line, @"stroke-width\s*=\s*[""']?([\d.]+)");
                if (swMatch.Success) float.TryParse(swMatch.Groups[1].Value, out strokeWidth);

                float scaleX = canvasWidth / 800f;
                float scaleY = canvasHeight / 600f;

                using (Pen pen = new Pen(ParseColor(stroke), strokeWidth * scaleX))
                {
                    g.DrawLine(pen, x1 * scaleX, y1 * scaleY, x2 * scaleX, y2 * scaleY);
                }
            }
        }

        private static void RenderPaths(Graphics g, string svg, int canvasWidth, int canvasHeight)
        {
            var matches = Regex.Matches(svg, @"<path[^>]*>", RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                string path = match.Value;

                var dMatch = Regex.Match(path, @"d\s*=\s*[""']([^""']+)");
                if (!dMatch.Success) continue;

                string d = dMatch.Groups[1].Value;
                string fill = "none";
                string stroke = "#000000";
                float strokeWidth = 1;

                var fillMatch = Regex.Match(path, @"fill\s*=\s*[""']?([#\w]+)");
                if (fillMatch.Success) fill = fillMatch.Groups[1].Value;

                var strokeMatch = Regex.Match(path, @"stroke\s*=\s*[""']?([#\w]+)");
                if (strokeMatch.Success) stroke = strokeMatch.Groups[1].Value;

                var swMatch = Regex.Match(path, @"stroke-width\s*=\s*[""']?([\d.]+)");
                if (swMatch.Success) float.TryParse(swMatch.Groups[1].Value, out strokeWidth);

                // Simple path parsing for basic shapes
                using (GraphicsPath gp = new GraphicsPath())
                {
                    float scaleX = canvasWidth / 800f;
                    float scaleY = canvasHeight / 600f;

                    // Parse path commands (simplified)
                    var commands = Regex.Matches(d, @"([MmLlHhVvCcSsQqTtAaZz])([^MmLlHhVvCcSsQqTtAaZz]*)");
                    float currentX = 0, currentY = 0;

                    foreach (Match cmd in commands)
                    {
                        char command = cmd.Groups[1].Value[0];
                        string args = cmd.Groups[2].Value.Trim();
                        string[] parts = Regex.Split(args, @"[\s,]+");
                        parts = Array.FindAll(parts, s => !string.IsNullOrEmpty(s));

                        switch (command)
                        {
                            case 'M': // Move to
                                if (parts.Length >= 2)
                                {
                                    currentX = float.Parse(parts[0]) * scaleX;
                                    currentY = float.Parse(parts[1]) * scaleY;
                                    gp.StartFigure();
                                }
                                break;

                            case 'L': // Line to
                                if (parts.Length >= 2)
                                {
                                    float newX = float.Parse(parts[0]) * scaleX;
                                    float newY = float.Parse(parts[1]) * scaleY;
                                    gp.AddLine(currentX, currentY, newX, newY);
                                    currentX = newX;
                                    currentY = newY;
                                }
                                break;

                            case 'H': // Horizontal line
                                if (parts.Length >= 1)
                                {
                                    float newX = float.Parse(parts[0]) * scaleX;
                                    gp.AddLine(currentX, currentY, newX, currentY);
                                    currentX = newX;
                                }
                                break;

                            case 'V': // Vertical line
                                if (parts.Length >= 1)
                                {
                                    float newY = float.Parse(parts[0]) * scaleY;
                                    gp.AddLine(currentX, currentY, currentX, newY);
                                    currentY = newY;
                                }
                                break;

                            case 'C': // Cubic bezier
                                if (parts.Length >= 6)
                                {
                                    float x1 = float.Parse(parts[0]) * scaleX;
                                    float y1 = float.Parse(parts[1]) * scaleY;
                                    float x2 = float.Parse(parts[2]) * scaleX;
                                    float y2 = float.Parse(parts[3]) * scaleY;
                                    float x3 = float.Parse(parts[4]) * scaleX;
                                    float y3 = float.Parse(parts[5]) * scaleY;
                                    gp.AddBezier(currentX, currentY, x1, y1, x2, y2, x3, y3);
                                    currentX = x3;
                                    currentY = y3;
                                }
                                break;

                            case 'Z':
                                gp.CloseFigure();
                                break;
                        }
                    }

                    if (fill != "none")
                    {
                        using (Brush brush = CreateBrush(fill, RectangleF.Empty))
                        {
                            g.FillPath(brush, gp);
                        }
                    }

                    if (stroke != "none")
                    {
                        using (Pen pen = new Pen(ParseColor(stroke), strokeWidth * scaleX))
                        {
                            g.DrawPath(pen, gp);
                        }
                    }
                }
            }
        }

        private static void RenderText(Graphics g, string svg, int canvasWidth, int canvasHeight)
        {
            var matches = Regex.Matches(svg, @"<text[^>]*>([^<]*)</text>", RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                string text = match.Value;
                string content = match.Groups[1].Value;

                float x = 0, y = 20;
                string fill = "#000000";
                float fontSize = 14;
                string fontFamily = "Arial";

                var xMatch = Regex.Match(text, @"x\s*=\s*[""']?([\d.]+)");
                if (xMatch.Success) float.TryParse(xMatch.Groups[1].Value, out x);

                var yMatch = Regex.Match(text, @"y\s*=\s*[""']?([\d.]+)");
                if (yMatch.Success) float.TryParse(yMatch.Groups[1].Value, out y);

                var fillMatch = Regex.Match(text, @"fill\s*=\s*[""']?([#\w]+)");
                if (fillMatch.Success) fill = fillMatch.Groups[1].Value;

                var fsMatch = Regex.Match(text, @"font-size\s*=\s*[""']?([\d.]+)");
                if (fsMatch.Success) float.TryParse(fsMatch.Groups[1].Value, out fontSize);

                var ffMatch = Regex.Match(text, @"font-family\s*=\s*[""']?([^""']+)");
                if (ffMatch.Success) fontFamily = ffMatch.Groups[1].Value.Trim();

                float scaleX = canvasWidth / 800f;
                float scaleY = canvasHeight / 600f;

                using (Font font = new Font(fontFamily, fontSize * scaleX))
                using (Brush brush = new SolidBrush(ParseColor(fill)))
                {
                    g.DrawString(content, font, brush, x * scaleX, y * scaleY - fontSize * scaleY);
                }
            }
        }

        private static Brush CreateBrush(string color, RectangleF bounds)
        {
            if (color == "none")
                return new SolidBrush(Color.Transparent);

            Color c = ParseColor(color);
            return new SolidBrush(c);
        }

        private static Color ParseColor(string color)
        {
            if (string.IsNullOrEmpty(color))
                return Color.Black;

            color = color.Trim();

            // Handle color names
            switch (color.ToLower())
            {
                case "red": return Color.Red;
                case "green": return Color.Green;
                case "blue": return Color.Blue;
                case "yellow": return Color.Yellow;
                case "orange": return Color.Orange;
                case "purple": return Color.Purple;
                case "pink": return Color.Pink;
                case "brown": return Color.Brown;
                case "gray":
                case "grey": return Color.Gray;
                case "black": return Color.Black;
                case "white": return Color.White;
                case "transparent": return Color.Transparent;
                default:
                    break;
            }

            // Handle hex colors
            if (color.StartsWith("#"))
            {
                try
                {
                    if (color.Length == 4) // Short form #RGB
                    {
                        int r = Convert.ToInt32(color.Substring(1, 1) + color.Substring(1, 1), 16);
                        int g = Convert.ToInt32(color.Substring(2, 1) + color.Substring(2, 1), 16);
                        int b = Convert.ToInt32(color.Substring(3, 1) + color.Substring(3, 1), 16);
                        return Color.FromArgb(r, g, b);
                    }
                    else if (color.Length == 7) // #RRGGBB
                    {
                        int r = Convert.ToInt32(color.Substring(1, 2), 16);
                        int g = Convert.ToInt32(color.Substring(3, 2), 16);
                        int b = Convert.ToInt32(color.Substring(5, 2), 16);
                        return Color.FromArgb(r, g, b);
                    }
                    else if (color.Length == 9) // #RRGGBBAA
                    {
                        int r = Convert.ToInt32(color.Substring(1, 2), 16);
                        int g = Convert.ToInt32(color.Substring(3, 2), 16);
                        int b = Convert.ToInt32(color.Substring(5, 2), 16);
                        int a = Convert.ToInt32(color.Substring(7, 2), 16);
                        return Color.FromArgb(a, r, g, b);
                    }
                }
                catch
                {
                    return Color.Black;
                }
            }

            return Color.Black;
        }
    }
}
