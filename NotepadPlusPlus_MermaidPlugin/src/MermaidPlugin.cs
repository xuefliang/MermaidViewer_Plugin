using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using MermaidViewer.Forms;

namespace MermaidViewer
{
    /// <summary>
    /// Main plugin class for Mermaid Viewer
    /// </summary>
    public class MermaidPlugin
    {
        private static List<FuncItem> _funcItems = new List<FuncItem>();
        private static PreviewForm _previewForm;

        /// <summary>
        /// Initializes the plugin
        /// </summary>
        public void Initialize()
        {
            // Add menu items
            AddMenuItem("Preview Mermaid", ShowPreview);
            AddMenuItem("-", null);
            AddMenuItem("Export SVG", ExportSvg);
        }

        private void AddMenuItem(string name, Action callback)
        {
            FuncItem item = new FuncItem();
            item._itemName = name;
            item._pFuncItemProc = callback;
            item._cmdID = 0;
            item._init2Check = false;
            item._pShKey = IntPtr.Zero;
            _funcItems.Add(item);
        }

        /// <summary>
        /// Gets the command count
        /// </summary>
        public int GetCommandCount()
        {
            return _funcItems.Count;
        }

        /// <summary>
        /// Creates the FuncItem array for Notepad++
        /// </summary>
        public IntPtr CreateFuncArray()
        {
            int count = _funcItems.Count;
            int size = Marshal.SizeOf(typeof(FuncItem));
            IntPtr ptr = Marshal.AllocCoTaskMem(count * size);

            for (int i = 0; i < count; i++)
            {
                IntPtr itemPtr = new IntPtr(ptr.ToInt64() + i * size);
                Marshal.StructureToPtr(_funcItems[i], itemPtr, false);
            }

            return ptr;
        }

        /// <summary>
        /// Called when a command is executed
        /// </summary>
        public void Command(int index)
        {
            if (index >= 0 && index < _funcItems.Count)
            {
                var callback = _funcItems[index]._pFuncItemProc;
                if (callback != null)
                {
                    callback();
                }
            }
        }

        private void ShowPreview()
        {
            if (_previewForm == null || _previewForm.IsDisposed)
            {
                _previewForm = new PreviewForm();
            }
            _previewForm.Show();
            _previewForm.BringToFront();
            RefreshPreview();
        }

        private void RefreshPreview()
        {
            string text = GetCurrentText();
            string svg = MmdrRenderer.Render(text);
            _previewForm?.DisplaySvg(svg);
        }

        private void ExportSvg()
        {
            string text = GetCurrentText();
            string svg = MmdrRenderer.Render(text);

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "SVG Files|*.svg";
            dlg.DefaultExt = ".svg";
            dlg.FileName = "diagram";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(dlg.FileName, svg);
                MessageBox.Show("SVG exported successfully!", "Mermaid Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string GetCurrentText()
        {
            IntPtr scintilla = GetCurrentScintilla();
            int length = (int)Win32.SendMessage(scintilla, Win32.SCI_GETLENGTH, IntPtr.Zero, IntPtr.Zero);

            if (length <= 0)
                return string.Empty;

            length++; // Include null terminator
            StringBuilder sb = new StringBuilder(length);
            Win32.SendMessage(scintilla, Win32.SCI_GETTEXT, new IntPtr(length), sb);
            return sb.ToString();
        }

        private IntPtr GetCurrentScintilla()
        {
            // Return main scintilla for simplicity
            return PluginBase.NppData._scintillaMainHandle;
        }
    }
}
