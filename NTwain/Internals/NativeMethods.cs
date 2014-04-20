﻿using System;
using System.Runtime.InteropServices;

namespace NTwain.Internals
{
    static class NativeMethods
    {
        // should be unsafe native methods?

        #region mem stuff for twain 1.x

        [DllImport("kernel32", SetLastError = true, EntryPoint = "GlobalAlloc")]
        public static extern IntPtr WinGlobalAlloc(uint uFlags, UIntPtr dwBytes);

        [DllImport("kernel32", SetLastError = true, EntryPoint = "GlobalFree")]
        public static extern IntPtr WinGlobalFree(IntPtr hMem);

        [DllImport("kernel32", SetLastError = true, EntryPoint = "GlobalLock")]
        public static extern IntPtr WinGlobalLock(IntPtr handle);

        [DllImport("kernel32", SetLastError = true, EntryPoint = "GlobalUnlock")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WinGlobalUnlock(IntPtr handle);

        #endregion
    }
}
