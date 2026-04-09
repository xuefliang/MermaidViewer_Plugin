using System;

namespace MermaidViewer
{
    /// <summary>
    /// Defines the different messages that Notepad++ can send to plugins
    /// </summary>
    public static class NppMessages
    {
        public const int NPPMSG_BASE = 0x40000;
        public const int NPPN_FIRST = 0x400000;

        // Document management
        public const int NPPM_GETCURRENTLINE = NPPMSG_BASE + 81;
        public const int NPPM_GETCURRENTCOLUMN = NPPMSG_BASE + 25;
        public const int NPPM_GETNBOPENFILES = NPPMSG_BASE + 26;
        public const int NPPM_GETOPENFILENAMELIST = NPPMSG_BASE + 27;
        public const int NPPM_GETOPENFILENAMES = NPPMSG_BASE + 28;

        // Plugin management
        public const int NPPM_GETPLUGINSCONFIGDIR = NPPMSG_BASE + 23;
        public const int NPPM_PLUGINGETNAME = NPPMSG_BASE + 18;

        // Notepad++ window
        public const int NPPM_GETNPPHANDLE = NPPMSG_BASE + 50;
        public const int NPPM_GETHWND = NPPMSG_BASE + 51;
        public const int NPPM_RELOADFILE = NPPMSG_BASE + 58;
        public const int NPPM_GETCURRENTBUFFERID = NPPMSG_BASE + 53;

        // Menu
        public const int NPPM_ADDTOOLBARICON = NPPMSG_BASE + 35;
        public const int NPPM_DMMREGAS = NPPMSG_BASE + 59;
        public const int NPPM_DMMREG = NPPMSG_BASE + 60;
        public const int NPPM_DMMUNREG = NPPMSG_BASE + 61;
        public const int NPPM_DMMSHOW = NPPMSG_BASE + 62;
        public const int NPPM_DMMHIDE = NPPMSG_BASE + 63;
        public const int NPPM_SETMENUITEMCHECK = NPPMSG_BASE + 71;
        public const int NPPM_ADDMENUITEM = NPPMSG_BASE + 72;

        // File operations
        public const int NPPM_GETFULLCURRENTPATH = NPPMSG_BASE + 85;
        public const int NPPM_GETCURRENTDIRECTORY = NPPMSG_BASE + 86;
        public const int NPPM_GETFILENAME = NPPMSG_BASE + 87;

        // Notifications
        public const int NPPN_READY = NPPN_FIRST + 1;
        public const int NPPN_TBMODIFICATION = NPPN_FIRST + 2;
        public const int NPPN_FILEBEFOREOPEN = NPPN_FIRST + 8;
        public const int NPPN_FILEOPENED = NPPN_FIRST + 9;
        public const int NPPN_FILEBEFORESAVE = NPPN_FIRST + 10;
        public const int NPPN_FILESAVED = NPPN_FIRST + 11;
        public const int NPPN_SHUTDOWN = NPPN_FIRST + 12;
        public const int NPPN_BUFFERACTIVATED = NPPN_FIRST + 13;
        public const int NPPN_LANGCHANGED = NPPN_FIRST + 14;
    }
}
