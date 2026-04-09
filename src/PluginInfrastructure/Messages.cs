using System;

namespace MermaidViewer
{
    /// <summary>
    /// Defines the different messages that Notepad++ can send to plugins
    /// </summary>
    public static class NppMessages
    {
        public const int NPPMSG_BASE = 0x40000;

        // Document management
        public const int NPPM_GETCURRENTLINE = NPPMSG_BASE + 24;
        public const int NPPM_GETCURRENTCOLUMN = NPPMSG_BASE + 25;
        public const int NPPM_GETNBOPENFILES = NPPMSG_BASE + 26;
        public const int NPPM_GETOPENFILENAMELIST = NPPMSG_BASE + 27;
        public const int NPPM_GETOPENFILENAMES = NPPMSG_BASE + 28;

        // Plugin management
        public const int NPPM_GETPLUGINSCONFIGDIR = NPPMSG_BASE + 23;
        public const int NPPM_PLUGINGETNAME = NPPMSG_BASE + 18;
        public const int NPPM_GETDLLIMPATH = NPPMSG_BASE + 17;

        // Notepad++ window
        public const int NPPM_GETNPPHANDLE = NPPMSG_BASE + 50;
        public const int NPPM_GETHWND = NPPMSG_BASE + 51;
        public const int NPPM_RELOADFILE = NPPMSG_BASE + 58;
        public const int NPPM_GETCURRENTBUFFERID = NPPMSG_BASE + 53;
        public const int NPPM_GETBUFFERIDFROMPOS = NPPMSG_BASE + 54;
        public const int NPPM_GETPOSFROMBUFFERID = NPPMSG_BASE + 55;
        public const int NPPM_SETBUFFERATTR = NPPMSG_BASE + 56;
        public const int NPPM_ASSIGNBUFFERKEYMAP = NPPMSG_BASE + 57;

        // Menu
        public const int NPPM_ADDTOOLBARICON = NPPMSG_BASE + 35;
        public const int NPPM_DMMREGAS = NPPMSG_BASE + 59;
        public const int NPPM_DMMREG = NPPMSG_BASE + 60;
        public const int NPPM_DMMUNREG = NPPMSG_BASE + 61;
        public const int NPPM_DMMSHOW = NPPMSG_BASE + 62;
        public const int NPPM_DMMHIDE = NPPMSG_BASE + 63;
        public const int NPPM_DMMINVOKEMENU = NPPMSG_BASE + 64;
        public const int NPPM_DMMGETPLUGINHWNDBYNAME = NPPMSG_BASE + 65;
        public const int NPPM_DMMVIEWOTHERTAB = NPPMSG_BASE + 66;

        // Scintilla
        public const int NPPM_SENDMSGTOEDITOR = NPPMSG_BASE + 68;
        public const int NPPM_LAUNCHFINDINFILESDLG = NPPMSG_BASE + 69;
        public const int NPPM_DMMUPDATEDIMENSION = NPPMSG_BASE + 70;
        public const int NPPM_SETMENUITEMCHECK = NPPMSG_BASE + 71;
        public const int NPPM_ADDMENUITEM = NPPMSG_BASE + 72;

        // Notepad++ general info
        public const int NPPM_GETMENUHANDLE = NPPMSG_BASE + 76;
        public const int NPPM_GETEDITORDEFAULTFOREGROUNDCOLOR = NPPMSG_BASE + 77;
        public const int NPPM_GETEDITORDEFAULTBACKGROUNDCOLOR = NPPMSG_BASE + 78;
        public const int NPPM_GETCURRENTLANGSEL = NPPMSG_BASE + 79;
        public const int NPPM_GETCURRENTSCINTILLA = NPPMSG_BASE + 80;
        public const int NPPM_GETCURRENTLINE = NPPMSG_BASE + 81;

        // File operations
        public const int NPPM_SAVEFILE = NPPMSG_BASE + 82;
        public const int NPPM_SAVECURRENTFILE = NPPMSG_BASE + 83;
        public const int NPPM_SAVEAS = NPPMSG_BASE + 84;

        // File info
        public const int NPPM_GETFULLCURRENTPATH = NPPMSG_BASE + 85;
        public const int NPPM_GETCURRENTDIRECTORY = NPPMSG_BASE + 86;
        public const int NPPM_GETFILENAME = NPPMSG_BASE + 87;
        public const int NPPM_GETNAMEPART = NPPMSG_BASE + 88;
        public const int NPPM_GETEXTPART = NPPMSG_BASE + 89;
        public const int NPPM_GETLANGFROMEXT = NPPMSG_BASE + 90;

        // Clipboard
        public const int NPPM_SETMENUITEMCHECK = NPPMSG_BASE + 91;

        // Settings
        public const int NPPM_HIDETOOLBAR = NPPMSG_BASE + 92;
        public const int NPPM_SHOWTOOLBAR = NPPMSG_BASE + 93;
        public const int NPPM_HIDEMENU = NPPMSG_BASE + 94;
        public const int NPPM_SHOWMENU = NPPMSG_BASE + 95;
        public const int NPPM_HIDESTATUSBAR = NPPMSG_BASE + 96;
        public const int NPPM_SHOWSTATUSBAR = NPPMSG_BASE + 97;
        public const int NPPM_HIDESCINTILLAMARGIN = NPPMSG_BASE + 98;
        public const int NPPM_SHOWSCINTILLAMARGIN = NPPMSG_BASE + 99;
        public const int NPPM_HIDESELECTIONMARGIN = NPPMSG_BASE + 100;
        public const int NPPM_SHOWSELECTIONMARGIN = NPPMSG_BASE + 101;
        public const int NPPM_LANGMODEVISIBLE = NPPMSG_BASE + 102;

        // Themes
        public const int NPPM_USetheme = NPPMSG_BASE + 103;

        // Windows
        public const int NPPM_WANTSUBSTITUTION = NPPMSG_BASE + 104;
        public const int NPPM_GETSUBSTITUTION = NPPMSG_BASE + 105;
        public const int NPPM_SETSUBSTITUTION = NPPMSG_BASE + 106;

        // Encoding
        public const int NPPM_GETCODEPAGE = NPPMSG_BASE + 107;

        // User settings
        public const int NPPM_GETUSERPREFERENCE = NPPMSG_BASE + 108;
        public const int NPPM_SETUSERPREFERECE = NPPMSG_BASE + 109;

        // Auto-completion
        public const int NPPM_AUTOCSET = NPPMSG_BASE + 110;
        public const int NPPM_AUTOCGET = NPPMSG_BASE + 111;

        // Files
        public const int NPPM_DOOPEN = NPPMSG_BASE + 112;
        public const int NPPM_GETFILESMAX = NPPMSG_BASE + 113;
        public const int NPPM_GETSESSIONFILES = NPPMSG_BASE + 114;
        public const int NPPM_SAVESESSION = NPPMSG_BASE + 115;
        public const int NPPM_OPENFILELISTSWITCHER = NPPMSG_BASE + 116;

        // Command line
        public const int NPPM_GETMAINSCINTILLA = NPPMSG_BASE + 117;
        public const int NPPM_DROPTYPE = NPPMSG_BASE + 118;

        // Printing
        public const int NPPM_ISOKEN = NPPMSG_BASE + 119;

        // Notifications
        public const int NPPM_SHUTDOWN = NPPMSG_BASE + 120;

        // Window style
        public const int NPPM_WINDOWFS = NPPMSG_BASE + 121;

        // Docking
        public const int NPPM_DMMASNOTEPAD = NPPMSG_BASE + 122;
        public const int NPPM_ISTABBARHIDDEN = NPPMSG_BASE + 123;
        public const int NPPM_SHOWTABBAR = NPPMSG_BASE + 124;
        public const int NPPM_ISTABBARALWAYSON = NPPMSG_BASE + 125;
        public const int NPPM_SETTABBARALWAYSON = NPPMSG_BASE + 126;

        // Splitter
        public const int NPPM_SETSPLITTER = NPPMSG_BASE + 127;

        // Undo
        public const int NPPM_CANREDO = NPPMSG_BASE + 128;
        public const int NPPM_CANUNDO = NPPMSG_BASE + 129;

        // Menu modifiers
        public const int NPPM_MENUCOMMAND = NPPMSG_BASE + 130;

        // Run macro
        public const int NPPM_RUNMACRO = NPPMSG_BASE + 131;

        // Editors
        public const int NPPM_NBMAINFILES = NPPMSG_BASE + 132;

        // Status bar
        public const int NPPM_GETSTATUSBAR = NPPMSG_BASE + 133;
        public const int NPPM_SETSTATUSBAR = NPPMSG_BASE + 134;

        // Document
        public const int NPPM_GETCARETWIDTH = NPPMSG_BASE + 135;
        public const int NPPM_SETCARETWIDTH = NPPMSG_BASE + 136;

        // Notifications
        public const int NPPN_FIRST = 1024;
        public const int NPPN_PREPAREBOARD = NPPN_FIRST + 1;
        public const int NPPN_CANCELBOARD = NPPN_FIRST + 2;
        public const int NPPN_TABFIRST = NPPN_FIRST + 3;
        public const int NPPN_TABLAST = NPPN_FIRST + 4;
        public const int NPPN_TABCLOSED = NPPN_FIRST + 5;
        public const int NPPN_FILEBEFORECLOSE = NPPN_FIRST + 6;
        public const int NPPN_FILECLOSED = NPPN_FIRST + 7;
        public const int NPPN_FILEBEFOREOPEN = NPPN_FIRST + 8;
        public const int NPPN_FILEOPENED = NPPN_FIRST + 9;
        public const int NPPN_FILEBEFOREOPEN = NPPN_FIRST + 10;
        public const int NPPN_READONLYCHANGED = NPPN_FIRST + 11;
        public const int NPPN_DOCORDERCHANGED = NPPN_FIRST + 12;
        public const int NPPN_SNAPSHOTDIRTYFILELOADED = NPPN_FIRST + 13;
        public const int NPPN_BEFORESHUTDOWN = NPPN_FIRST + 14;
        public const int NPPN_CANCELSHUTDOWN = NPPN_FIRST + 15;
        public const int NPPN_FILEBEFORESAVE = NPPN_FIRST + 16;
        public const int NPPN_FILESAVED = NPPN_FIRST + 17;
        public const int NPPN_SHUTDOWN = NPPN_FIRST + 18;
        public const int NPPN_READY = NPPN_FIRST + 19;

        // Buffer update notifications
        public const int NPPN_BUFFERACTIVATED = NPPN_FIRST + 20;
        public const int NPPN_LANGCHANGED = NPPN_FIRST + 21;
        public const int NPPN_WORDSTYLESUPDATED = NPPN_FIRST + 22;
        public const int NPPN_SHORTCUTREMAPPED = NPPN_FIRST + 23;
        public const int NPPN_FILECHANGEDONDISK = NPPN_FIRST + 24;
        public const int NPPN_ONLINERETURN = NPPN_FIRST + 25;
        public const int NPPN_BEFORERUNNINGMACRO = NPPN_FIRST + 26;

        // Dockable window messages
        public const int DMM_BASE = 0x40000 + 2000;
        public const int DMM_CLOSE = DMM_BASE + 1;
        public const int DMM_DOCK = DMM_BASE + 2;
        public const int DMM_FLOAT = DMM_BASE + 3;
        public const int DMM_DOCKALL = DMM_BASE + 4;
        public const int DMM_FLOATALL = DMM_BASE + 5;
        public const int DMM_SENDMSG = DMM_BASE + 6;

        // Docking states
        public const int DOCKCONTROL_MAX = 16;
        public const int CONTNOL_DOCK = 0;
        public const int CONTNOL_FLOAT = 4;
        public const int CONTNOL_DOCKEDLEFT = 1;
        public const int CONTNOL_DOCKEDRIGHT = 2;
        public const int CONTNOL_DOCKEDTOP = 3;
        public const int CONTNOL_DOCKEDBOTTOM = 4;
        public const int CONTNOL_DOCKEDLEFTPLUS = 5;
        public const int CONTNOL_DOCKEDRIGHTPLUS = 6;
        public const int CONTNOL_DOCKEDTOPPLUS = 7;
        public const int CONTNOL_DOCKEDBOTTOMPLUS = 8;
        public const int CONTNOL_DOCKEDBASEMASK = 0xF;
        public const int CONTNOL_DOCKEDMASK = 0x10;
        public const int CONTNOL_HIDE = 0x100;
        public const int CONTNOL_SHOW = 0x200;
        public const int CONTNOL_CANCELPREVIOUS = 0x300;
    }
}
