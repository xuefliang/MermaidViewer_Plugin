using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MermaidViewer
{
    /// <summary>
    /// Gateway to Scintilla editor functionality
    /// </summary>
    public class ScintillaGateway
    {
        private readonly IntPtr _scintillaHandle;

        public ScintillaGateway(IntPtr scintillaHandle)
        {
            _scintillaHandle = scintillaHandle;
        }

        #region Scintilla Messages

        private const int SCI_GETLENGTH = 2006;
        private const int SCI_GETTEXT = 2001;
        private const int SCI_GETTEXTRANGE = 2162;
        private const int SCI_GETCURRENTLINE = 2165;
        private const int SCI_GETCURRENTPOS = 2008;
        private const int SCI_GETANCHOR = 2009;
        private const int SCI_GETSELTEXT = 2166;
        private const int SCI_GETSELECTIONSTART = 2145;
        private const int SCI_GETSELECTIONEND = 2146;
        private const int SCI_SETSELECTIONSTART = 2143;
        private const int SCI_SETSELECTIONEND = 2144;
        private const int SCI_GOTOLINE = 2024;
        private const int SCI_GOTOPOS = 2025;
        private const int SCI_GETLINE = 2153;
        private const int SCI_GETLINECOUNT = 2154;
        private const int SCI_SETTEXT = 2181;
        private const int SCI_CLEARALL = 2180;
        private const int SCI_SELECTALL = 2013;

        #endregion

        private IntPtr SendMessage(int msg, IntPtr wParam, IntPtr lParam)
        {
            return Win32.SendMessage(_scintillaHandle, (uint)msg, wParam, lParam);
        }

        private IntPtr SendMessage(int msg, IntPtr wParam, string lParam)
        {
            return Win32.SendMessage(_scintillaHandle, (uint)msg, wParam, lParam);
        }

        /// <summary>
        /// Gets the total length of the document
        /// </summary>
        public int GetLength()
        {
            return (int)SendMessage(SCI_GETLENGTH, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Gets the current cursor position
        /// </summary>
        public int GetCurrentPos()
        {
            return (int)SendMessage(SCI_GETCURRENTPOS, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Gets the anchor position
        /// </summary>
        public int GetAnchor()
        {
            return (int)SendMessage(SCI_GETANCHOR, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Gets the current line number
        /// </summary>
        public int GetCurrentLine()
        {
            return (int)SendMessage(SCI_GETCURRENTLINE, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Gets the total number of lines
        /// </summary>
        public int GetLineCount()
        {
            return (int)SendMessage(SCI_GETLINECOUNT, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Gets the selected text
        /// </summary>
        public string GetSelectedText()
        {
            int length = (int)SendMessage(SCI_GETSELTEXT, IntPtr.Zero, IntPtr.Zero);
            if (length <= 0) return string.Empty;

            StringBuilder sb = new StringBuilder(length + 1);
            SendMessage(SCI_GETSELTEXT, IntPtr.Zero, sb.ToString());
            return sb.ToString();
        }

        /// <summary>
        /// Gets all text in the document
        /// </summary>
        public string GetText()
        {
            int length = GetLength();
            if (length <= 0) return string.Empty;

            StringBuilder sb = new StringBuilder(length + 1);
            SendMessage(SCI_GETTEXT, (IntPtr)(length + 1), sb.ToString());
            return sb.ToString();
        }

        /// <summary>
        /// Gets text from a specific line
        /// </summary>
        public string GetLine(int lineNumber)
        {
            int length = (int)SendMessage(SCI_GETLINE, (IntPtr)lineNumber, IntPtr.Zero);
            if (length <= 0) return string.Empty;

            StringBuilder sb = new StringBuilder(length + 1);
            SendMessage(SCI_GETLINE, (IntPtr)lineNumber, sb.ToString());
            return sb.ToString().TrimEnd('\0', '\r', '\n');
        }

        /// <summary>
        /// Selects all text
        /// </summary>
        public void SelectAll()
        {
            SendMessage(SCI_SELECTALL, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
