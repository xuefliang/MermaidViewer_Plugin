using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MermaidViewer
{
    /// <summary>
    /// Provides a base class for menu items
    /// </summary>
    public abstract class MenuItemBase
    {
        protected string _name;
        protected string _description;
        protected ShortcutKey _shortcut;

        /// <summary>
        /// Gets or sets the menu item name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the description (shown in status bar)
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Gets or sets the keyboard shortcut
        /// </summary>
        public ShortcutKey Shortcut
        {
            get { return _shortcut; }
            set { _shortcut = value; }
        }

        /// <summary>
        /// Creates a FuncItem structure for this menu item
        /// </summary>
        public FuncItem CreateFuncItem()
        {
            FuncItem item = new FuncItem();
            item._itemName = _name;
            item._pFuncItemProc = OnCommand;
            item._cmdID = 0; // Will be set by Notepad++
            item._init2Check = false;
            item._pShKey = _shortcut._key != 0 ? CreateShortcutKey() : IntPtr.Zero;
            return item;
        }

        /// <summary>
        /// Creates the shortcut key structure
        /// </summary>
        private IntPtr CreateShortcutKey()
        {
            IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(ShortcutKey)));
            Marshal.StructureToPtr(_shortcut, ptr, false);
            return ptr;
        }

        /// <summary>
        /// Called when the menu item is clicked
        /// </summary>
        public abstract void OnCommand();

        /// <summary>
        /// Creates a simple menu item without shortcut
        /// </summary>
        public static MenuItemBase Create(string name, string description, Action callback)
        {
            return new SimpleMenuItem(name, description, callback);
        }

        /// <summary>
        /// Creates a menu item with shortcut
        /// </summary>
        public static MenuItemBase Create(string name, string description, Action callback, bool ctrl, bool alt, bool shift, uint key)
        {
            return new SimpleMenuItem(name, description, callback, ctrl, alt, shift, key);
        }

        private class SimpleMenuItem : MenuItemBase
        {
            private readonly Action _callback;

            public SimpleMenuItem(string name, string description, Action callback)
            {
                _name = name;
                _description = description;
                _callback = callback;
            }

            public SimpleMenuItem(string name, string description, Action callback, bool ctrl, bool alt, bool shift, uint key)
            {
                _name = name;
                _description = description;
                _callback = callback;
                _shortcut = new ShortcutKey(ctrl, alt, shift, key);
            }

            public override void OnCommand()
            {
                _callback?.Invoke();
            }
        }
    }

    // ShortcutKey is defined in NativeDataStructs.cs
}
