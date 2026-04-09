using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MermaidViewer
{
    /// <summary>
    /// Provides access to Notepad++ gateway functionality
    /// </summary>
    public class NotepadPlusPlusGateway
    {
        private readonly IntPtr _nppHandle;

        public NotepadPlusPlusGateway(IntPtr nppHandle)
        {
            _nppHandle = nppHandle;
        }

        /// <summary>
        /// Gets the full path of the current file
        /// </summary>
        public string GetFullCurrentPath()
        {
            IntPtr ptr = Win32.SendMessage(_nppHandle, NppMessages.NPPM_GETFULLCURRENTPATH, IntPtr.Zero, IntPtr.Zero);
            return Marshal.PtrToStringUni(ptr);
        }

        /// <summary>
        /// Gets the current file name
        /// </summary>
        public string GetCurrentFileName()
        {
            IntPtr ptr = Win32.SendMessage(_nppHandle, NppMessages.NPPM_GETFILENAME, IntPtr.Zero, IntPtr.Zero);
            return Marshal.PtrToStringUni(ptr);
        }

        /// <summary>
        /// Gets the current directory
        /// </summary>
        public string GetCurrentDirectory()
        {
            IntPtr ptr = Win32.SendMessage(_nppHandle, NppMessages.NPPM_GETCURRENTDIRECTORY, IntPtr.Zero, IntPtr.Zero);
            return Marshal.PtrToStringUni(ptr);
        }

        /// <summary>
        /// Gets the file extension
        /// </summary>
        public string GetFileExtension()
        {
            IntPtr ptr = Win32.SendMessage(_nppHandle, NppMessages.NPPM_GETEXTPART, IntPtr.Zero, IntPtr.Zero);
            return Marshal.PtrToStringUni(ptr);
        }

        /// <summary>
        /// Gets the current buffer ID
        /// </summary>
        public int GetCurrentBufferId()
        {
            return (int)Win32.SendMessage(_nppHandle, NppMessages.NPPM_GETCURRENTBUFFERID, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Gets the current scintilla handle
        /// </summary>
        public IntPtr GetCurrentScintilla()
        {
            int which = (int)Win32.SendMessage(_nppHandle, NppMessages.NPPM_GETCURRENTSCINTILLA, IntPtr.Zero, IntPtr.Zero);
            return which == 0 ? PluginBase._nppData._scintillaMainHandle : PluginBase._nppData._scintillaSecondHandle;
        }

        /// <summary>
        /// Gets the main scintilla handle
        /// </summary>
        public IntPtr GetMainScintilla()
        {
            return PluginBase._nppData._scintillaMainHandle;
        }

        /// <summary>
        /// Gets the secondary scintilla handle
        /// </summary>
        public IntPtr GetSecondaryScintilla()
        {
            return PluginBase._nppData._scintillaSecondHandle;
        }

        /// <summary>
        /// Gets the NPP handle
        /// </summary>
        public IntPtr GetNppHandle()
        {
            return _nppHandle;
        }

        /// <summary>
        /// Saves the current file
        /// </summary>
        public void SaveCurrentFile()
        {
            Win32.SendMessage(_nppHandle, NppMessages.NPPM_SAVECURRENTFILE, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Saves a file with a new name
        /// </summary>
        public void SaveAs(string filePath)
        {
            Win32.SendMessage(_nppHandle, NppMessages.NPPM_SAVEAS, IntPtr.Zero, filePath);
        }

        /// <summary>
        /// Opens a file in Notepad++
        /// </summary>
        public void OpenFile(string filePath)
        {
            Win32.SendMessage(_nppHandle, NppMessages.NPPM_DOOPEN, IntPtr.Zero, filePath);
        }

        /// <summary>
        /// Reloads the current file
        /// </summary>
        public void ReloadCurrentFile()
        {
            Win32.SendMessage(_nppHandle, NppMessages.NPPM_RELOADFILE, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Sets the status bar text
        /// </summary>
        public void SetStatusBar(string message)
        {
            Win32.SendMessage(_nppHandle, NppMessages.NPPM_SETSTATUSBAR, (IntPtr)0, message);
        }

        /// <summary>
        /// Gets the plugin configuration directory
        /// </summary>
        public string GetPluginsConfigDir()
        {
            IntPtr ptr = Win32.SendMessage(_nppHandle, NppMessages.NPPM_GETPLUGINSCONFIGDIR, IntPtr.Zero, IntPtr.Zero);
            return Marshal.PtrToStringUni(ptr);
        }

        /// <summary>
        /// Shows a message box with Notepad++ as owner
        /// </summary>
        public DialogResult ShowMessage(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(message, title, buttons, icon);
        }

        /// <summary>
        /// Gets the number of open files
        /// </summary>
        public int GetNbOpenFiles()
        {
            return (int)Win32.SendMessage(_nppHandle, NppMessages.NPPM_GETNBOPENFILES, IntPtr.Zero, (IntPtr)0);
        }

        /// <summary>
        /// Gets the list of open file names
        /// </summary>
        public string[] GetOpenFileNames()
        {
            int nbOpen = GetNbOpenFiles();
            if (nbOpen == 0)
                return new string[0];

            IntPtr arrayPtr = Marshal.AllocCoTaskMem(nbOpen * IntPtr.Size);
            try
            {
                Win32.SendMessage(_nppHandle, NppMessages.NPPM_GETOPENFILENAMES, arrayPtr, (IntPtr)nbOpen);
                string[] result = new string[nbOpen];
                for (int i = 0; i < nbOpen; i++)
                {
                    IntPtr strPtr = Marshal.ReadIntPtr(arrayPtr, i * IntPtr.Size);
                    result[i] = Marshal.PtrToStringUni(strPtr);
                }
                return result;
            }
            finally
            {
                Marshal.FreeCoTaskMem(arrayPtr);
            }
        }

        /// <summary>
        /// Executes a menu command
        /// </summary>
        public void ExecuteMenuCommand(int commandId)
        {
            Win32.SendMessage(_nppHandle, NppMessages.NPPM_MENUCOMMAND, (IntPtr)commandId, IntPtr.Zero);
        }

        /// <summary>
        /// Gets the current language selection
        /// </summary>
        public int GetCurrentLangType()
        {
            return (int)Win32.SendMessage(_nppHandle, NppMessages.NPPM_GETCURRENTLANGSEL, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
