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
        private const int SCI_CLEARDOCUMENTSTYLE = 2167;
        private const int SCI_GETSTYLEAT = 2010;
        private const int SCI_GETCHARAT = 2007;
        private const int SCI_GETMODIFY = 2159;
        private const int SCI_SETREADONLY = 2171;
        private const int SCI_SETSELECTION = 2160;
        private const int SCI_SELECTALL = 2013;
        private const int SCI_CANCEL = 2172;
        private const int SCI_UNDO = 2174;
        private const int SCI_REDO = 2175;
        private const int SCI_CANUNDO = 2176;
        private const int SCI_CANREDO = 2177;
        private const int SCI_MARKERADD = 2043;
        private const int SCI_MARKERDELETE = 2044;
        private const int SCI_MARKERDELETEALL = 2045;
        private const int SCI_MARKERGET = 2047;
        private const int SCI_MARKERNEXT = 2048;
        private const int SCI_MARKERPREV = 2049;
        private const int SCI_GETFIRSTVISIBLELINE = 2152;
        private const int SCI_LINESONSCREEN = 2370;
        private const int SCI_LINESCROLL = 2132;
        private const int SCI_SCROLLCARET = 2169;
        private const int SCI_REPLACESEL = 2170;
        private const int SCI_SETWORDCHARS = 2076;
        private const int SCI_GETWORDCHARS = 2077;
        private const int SCI_SETCHARSDEFAULT = 2440;
        private const int SCI_AUTOCSHOW = 2100;
        private const int SCI_AUTOCCANCEL = 2110;
        private const int SCI_AUTOCACTIVE = 2111;
        private const int SCI_AUTOCPOSSTART = 2112;
        private const int SCI_AUTOCCOMPLETE = 2113;
        private const int SCI_AUTOCSELECT = 2114;
        private const int SCI_AUTOCGETCURRENT = 2115;
        private const int SCI_AUTOCGETCURRENTSEPARATOR = 2116;
        private const int SCI_AUTOCSETSEPARATOR = 2117;
        private const int SCI_AUTOCSETFILLUPS = 2118;
        private const int SCI_AUTOCSETCHOOSESINGLE = 2119;
        private const int SCI_AUTOCGETCHOOSESINGLE = 2120;
        private const int SCI_AUTOCSETIGNORECASE = 2121;
        private const int SCI_AUTOCGETIGNORECASE = 2122;
        private const int SCI_USERLISTSHOW = 2113;
        private const int SCI_AUTOCSETLIST = 2124;
        private const int SCI_FINDTEXT = 2156;
        private const int SCI_SEARCHINTARGET = 2147;
        private const int SCI_GETTARGETSTART = 2190;
        private const int SCI_GETTARGETEND = 2191;
        private const int SCI_SETTARGETSTART = 2192;
        private const int SCI_SETTARGETEND = 2193;
        private const int SCI_GETSEARCHFLAGS = 2194;
        private const int SCI_SETSEARCHFLAGS = 2195;
        private const int SCI_CALLTIPSHOW = 2200;
        private const int SCI_CALLTIPCANCEL = 2201;
        private const int SCI_CALLTIPACTIVE = 2202;
        private const int SCI_CALLTIPPOSSTART = 2203;
        private const int SCI_CALLTIPSETHLT = 2204;
        private const int SCI_CALLTIPSETBACK = 2205;
        private const int SCI_CALLTIPSETFORE = 2206;
        private const int SCI_CALLTIPSETFOREHLT = 2207;
        private const int SCI_CALLTIPADD = 2208;
        private const int SCI_CALLTIPSETPOSSTART = 2209;
        private const int SCI_GETCURRENTPOS = 2008;
        private const int SCI_SETCURRENTPOS = 2184;
        private const int SCI_GETANCHOR = 2009;
        private const int SCI_SETANCHOR = 2185;
        private const int SCI_STARTSTYLING = 2064;
        private const int SCI_SETSTYLING = 2065;
        private const int SCI_SETSTYLINGEX = 2070;
        private const int SCI_SETCARETLINEBACK = 2134;
        private const int SCI_GETCARETLINEBACK = 2135;
        private const int CMD_REDO = 2183;
        private const int CMD_UNDO = 2178;

        #endregion

        private IntPtr SendMessage(int msg, IntPtr wParam, IntPtr lParam)
        {
            return Win32.SendMessage(_scintillaHandle, (uint)msg, wParam, lParam);
        }

        private IntPtr SendMessage(int msg, IntPtr wParam, string lParam)
        {
            return Win32.SendMessage(_scintillaHandle, (uint)msg, wParam, lParam);
        }

        private IntPtr SendMessage(int msg, IntPtr wParam, StringBuilder lParam)
        {
            return Win32.SendMessage(_scintillaHandle, (uint)msg, wParam, lParam);
        }

        #region Text Operations

        /// <summary>
        /// Gets the current text from the editor
        /// </summary>
        public string GetText()
        {
            int length = (int)SendMessage(SCI_GETLENGTH, IntPtr.Zero, IntPtr.Zero) + 1;
            StringBuilder sb = new StringBuilder(length);
            SendMessage(SCI_GETTEXT, (IntPtr)length, sb);
            return sb.ToString();
        }

        /// <summary>
        /// Gets the text in a specific range
        /// </summary>
        public string GetTextRange(int start, int end)
        {
            int length = end - start;
            if (length <= 0)
                return string.Empty;

            byte[] buffer = new byte[length + 1];
            IntPtr ptr = Marshal.AllocCoTaskMem(length + 1);
            try
            {
                Marshal.Copy(buffer, 0, ptr, length + 1);
                SendMessage(SCI_GETTEXTRANGE, new IntPtr(start), new IntPtr(end));
                Marshal.Copy(ptr, buffer, 0, length);
                return Encoding.UTF8.GetString(buffer, 0, length).TrimEnd('\0');
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr);
            }
        }

        /// <summary>
        /// Gets the length of the text
        /// </summary>
        public int GetLength()
        {
            return (int)SendMessage(SCI_GETLENGTH, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Sets the text
        /// </summary>
        public void SetText(string text)
        {
            SendMessage(SCI_SETTEXT, IntPtr.Zero, text);
        }

        /// <summary>
        /// Clears all text
        /// </summary>
        public void ClearAll()
        {
            SendMessage(SCI_CLEARALL, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Gets the selected text
        /// </summary>
        public string GetSelText()
        {
            int length = (int)SendMessage(SCI_GETSELTEXT, IntPtr.Zero, IntPtr.Zero);
            if (length == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder(length);
            SendMessage(SCI_GETSELTEXT, IntPtr.Zero, sb);
            return sb.ToString();
        }

        /// <summary>
        /// Replaces the selected text
        /// </summary>
        public void ReplaceSel(string text)
        {
            SendMessage(SCI_REPLACESEL, IntPtr.Zero, text);
        }

        #endregion

        #region Position and Navigation

        /// <summary>
        /// Gets the current line number
        /// </summary>
        public int GetCurrentLine()
        {
            return (int)SendMessage(SCI_GETCURRENTLINE, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Gets the current position
        /// </summary>
        public int GetCurrentPos()
        {
            return (int)SendMessage(SCI_GETCURRENTPOS, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Sets the current position
        /// </summary>
        public void SetCurrentPos(int pos)
        {
            SendMessage(SCI_SETCURRENTPOS, (IntPtr)pos, IntPtr.Zero);
        }

        /// <summary>
        /// Gets the anchor position
        /// </summary>
        public int GetAnchor()
        {
            return (int)SendMessage(SCI_GETANCHOR, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Sets the anchor position
        /// </summary>
        public void SetAnchor(int pos)
        {
            SendMessage(SCI_SETANCHOR, (IntPtr)pos, IntPtr.Zero);
        }

        /// <summary>
        /// Gets the selection start position
        /// </summary>
        public int GetSelectionStart()
        {
            return (int)SendMessage(SCI_GETSELECTIONSTART, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Gets the selection end position
        /// </summary>
        public int GetSelectionEnd()
        {
            return (int)SendMessage(SCI_GETSELECTIONEND, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Sets the selection range
        /// </summary>
        public void SetSelection(int start, int end)
        {
            SendMessage(SCI_SETSELECTIONSTART, (IntPtr)start, IntPtr.Zero);
            SendMessage(SCI_SETSELECTIONEND, (IntPtr)end, IntPtr.Zero);
        }

        /// <summary>
        /// Selects all text
        /// </summary>
        public void SelectAll()
        {
            SendMessage(SCI_SELECTALL, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Goes to a specific line
        /// </summary>
        public void GoToLine(int line)
        {
            SendMessage(SCI_GOTOLINE, (IntPtr)line, IntPtr.Zero);
        }

        /// <summary>
        /// Gets the number of lines
        /// </summary>
        public int GetLineCount()
        {
            return (int)SendMessage(SCI_GETLINECOUNT, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Gets a specific line
        /// </summary>
        public string GetLine(int line)
        {
            int length = (int)SendMessage(SCI_GETLINE, (IntPtr)line, IntPtr.Zero);
            if (length == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder(length + 1);
            SendMessage(SCI_GETLINE, (IntPtr)line, sb);
            return sb.ToString().TrimEnd('\0');
        }

        #endregion

        #region Editing Operations

        /// <summary>
        /// Undo last action
        /// </summary>
        public void Undo()
        {
            SendMessage(SCI_UNDO, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Redo last undone action
        /// </summary>
        public void Redo()
        {
            SendMessage(SCI_REDO, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Checks if undo is available
        /// </summary>
        public bool CanUndo()
        {
            return SendMessage(SCI_CANUNDO, IntPtr.Zero, IntPtr.Zero) != IntPtr.Zero;
        }

        /// <summary>
        /// Checks if redo is available
        /// </summary>
        public bool CanRedo()
        {
            return SendMessage(SCI_CANREDO, IntPtr.Zero, IntPtr.Zero) != IntPtr.Zero;
        }

        #endregion

        #region Markers

        /// <summary>
        /// Adds a marker to a line
        /// </summary>
        public void MarkerAdd(int line, int marker)
        {
            SendMessage(SCI_MARKERADD, (IntPtr)line, (IntPtr)marker);
        }

        /// <summary>
        /// Deletes a marker from a line
        /// </summary>
        public void MarkerDelete(int line, int marker)
        {
            SendMessage(SCI_MARKERDELETE, (IntPtr)line, (IntPtr)marker);
        }

        /// <summary>
        /// Deletes all markers
        /// </summary>
        public void MarkerDeleteAll(int marker)
        {
            SendMessage(SCI_MARKERDELETEALL, (IntPtr)marker, IntPtr.Zero);
        }

        #endregion

        #region Visibility

        /// <summary>
        /// Gets the first visible line
        /// </summary>
        public int GetFirstVisibleLine()
        {
            return (int)SendMessage(SCI_GETFIRSTVISIBLELINE, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Gets the number of lines on screen
        /// </summary>
        public int LinesOnScreen()
        {
            return (int)SendMessage(SCI_LINESONSCREEN, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Scrolls the view
        /// </summary>
        public void LineScroll(int columns, int lines)
        {
            SendMessage(SCI_LINESCROLL, (IntPtr)columns, (IntPtr)lines);
        }

        /// <summary>
        /// Scrolls to make the caret visible
        /// </summary>
        public void ScrollCaret()
        {
            SendMessage(SCI_SCROLLCARET, IntPtr.Zero, IntPtr.Zero);
        }

        #endregion

        #region Search

        /// <summary>
        /// Sets search flags
        /// </summary>
        public void SetSearchFlags(int flags)
        {
            SendMessage(SCI_SETSEARCHFLAGS, (IntPtr)flags, IntPtr.Zero);
        }

        /// <summary>
        /// Searches in target text
        /// </summary>
        public int SearchInTarget(string text)
        {
            return (int)SendMessage(SCI_SEARCHINTARGET, (IntPtr)text.Length, text);
        }

        /// <summary>
        /// Gets the target start position
        /// </summary>
        public int GetTargetStart()
        {
            return (int)SendMessage(SCI_GETTARGETSTART, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Sets the target start position
        /// </summary>
        public void SetTargetStart(int pos)
        {
            SendMessage(SCI_SETTARGETSTART, (IntPtr)pos, IntPtr.Zero);
        }

        /// <summary>
        /// Gets the target end position
        /// </summary>
        public int GetTargetEnd()
        {
            return (int)SendMessage(SCI_GETTARGETEND, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Sets the target end position
        /// </summary>
        public void SetTargetEnd(int pos)
        {
            SendMessage(SCI_SETTARGETEND, (IntPtr)pos, IntPtr.Zero);
        }

        #endregion

        #region Style

        /// <summary>
        /// Gets the style at a position
        /// </summary>
        public int GetStyleAt(int pos)
        {
            return (int)SendMessage(SCI_GETSTYLEAT, (IntPtr)pos, IntPtr.Zero);
        }

        /// <summary>
        /// Gets the character at a position
        /// </summary>
        public char GetCharAt(int pos)
        {
            return (char)SendMessage(SCI_GETCHARAT, (IntPtr)pos, IntPtr.Zero);
        }

        /// <summary>
        /// Sets the caret line background color
        /// </summary>
        public void SetCaretLineBack(int color)
        {
            SendMessage(SCI_SETCARETLINEBACK, (IntPtr)color, IntPtr.Zero);
        }

        #endregion

        /// <summary>
        /// Cancels any operation
        /// </summary>
        public void Cancel()
        {
            SendMessage(SCI_CANCEL, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Checks if text has been modified
        /// </summary>
        public bool GetModify()
        {
            return SendMessage(SCI_GETMODIFY, IntPtr.Zero, IntPtr.Zero) != IntPtr.Zero;
        }

        /// <summary>
        /// Sets read only mode
        /// </summary>
        public void SetReadOnly(bool readOnly)
        {
            SendMessage(SCI_SETREADONLY, readOnly ? (IntPtr)1 : IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Gets the editor handle
        /// </summary>
        public IntPtr GetHandle()
        {
            return _scintillaHandle;
        }
    }
}
