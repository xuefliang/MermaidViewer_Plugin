using System;

namespace MermaidViewer
{
    /// <summary>
    /// Marks a method as an unmanaged export for Notepad++ plugin API
    /// Note: This requires RGiesecke.DllExport NuGet package at build time
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class DllExportAttribute : Attribute
    {
        public DllExportAttribute()
        {
        }

        public DllExportAttribute(string exportName)
        {
            ExportName = exportName;
        }

        public string ExportName { get; set; }
        public CallingConvention CallingConvention { get; set; } = CallingConvention.StdCall;
    }

    public enum CallingConvention
    {
        Cdecl,
        StdCall,
        FastCall,
        ThisCall
    }
}
