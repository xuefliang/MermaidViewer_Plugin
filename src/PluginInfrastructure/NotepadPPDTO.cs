using System;

namespace MermaidViewer
{
    /// <summary>
    /// Holds information about Notepad++ state for DTOs
    /// </summary>
    public class NotepadPPDTO
    {
        private readonly IntPtr _nppHandle;
        private readonly IntPtr _scintillaMainHandle;
        private readonly IntPtr _scintillaSecondHandle;

        public NotepadPPDTO(IntPtr nppHandle, IntPtr scintillaMainHandle, IntPtr scintillaSecondHandle)
        {
            _nppHandle = nppHandle;
            _scintillaMainHandle = scintillaMainHandle;
            _scintillaSecondHandle = scintillaSecondHandle;
        }

        /// <summary>
        /// Gets the Notepad++ main window handle
        /// </summary>
        public IntPtr NppHandle
        {
            get { return _nppHandle; }
        }

        /// <summary>
        /// Gets the main scintilla window handle
        /// </summary>
        public IntPtr ScintillaMainHandle
        {
            get { return _scintillaMainHandle; }
        }

        /// <summary>
        /// Gets the secondary scintilla window handle
        /// </summary>
        public IntPtr ScintillaSecondHandle
        {
            get { return _scintillaSecondHandle; }
        }
    }
}
