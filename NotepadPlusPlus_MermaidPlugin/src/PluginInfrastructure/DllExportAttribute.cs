using System;

namespace MermaidViewer
{
    /// <summary>
    /// Marks a method to be exported from the DLL
    /// This is a stub that works with post-build processing tools
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class DllExportAttribute : Attribute
    {
        public DllExportAttribute() { }
        public string ExportName { get; set; }
        public System.Runtime.InteropServices.CallingConvention CallingConvention { get; set; } 
            = System.Runtime.InteropServices.CallingConvention.StdCall;
    }
}
