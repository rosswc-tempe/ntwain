﻿using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NTwain
{
    /// <summary>
    /// Provides methods for managing memory on data exchanged with twain sources.
    /// This should only be used after the DSM has been opened in <see cref="TwainSession"/>
    /// via <see cref="TwainSession.OpenManager"/>.
    /// </summary>
    public class MemoryManager
    {
        /// <summary>
        /// Gets the global <see cref="MemoryManager"/> instance.
        /// </summary>
        public static readonly MemoryManager Global = new MemoryManager();

        private MemoryManager() { }

        /// <summary>
        /// Updates the entry point used by TWAIN.
        /// </summary>
        /// <param name="entryPoint">The entry point.</param>
        internal void UpdateEntryPoint(TWEntryPoint entryPoint)
        {
            _twain2Entry = entryPoint;
        }

        TWEntryPoint _twain2Entry;

        /// <summary>
        /// Function to allocate memory. Calls to this must be coupled with <see cref="MemFree"/> later.
        /// </summary>
        /// <param name="size">The size in bytes.</param>
        /// <returns>Handle to the allocated memory.</returns>
        public IntPtr MemAllocate(uint size)
        {
            if (_twain2Entry != null && _twain2Entry.AllocateFunction != null)
            {
                return _twain2Entry.AllocateFunction(size);
            }
            else
            {
                // 0x0040 is GPTR
                return GlobalAlloc(0x0040, new UIntPtr(size));
            }
        }

        /// <summary>
        /// Function to free memory. 
        /// </summary>
        /// <param name="handle">The handle from <see cref="MemAllocate"/>.</param>
        public void MemFree(IntPtr handle)
        {
            if (_twain2Entry != null && _twain2Entry.FreeFunction != null)
            {
                _twain2Entry.FreeFunction(handle);
            }
            else
            {
                GlobalFree(handle);
            }
        }

        /// <summary>
        /// Function to lock some memory. Calls to this must be coupled with <see cref="MemUnlock"/> later.
        /// </summary>
        /// <param name="handle">The handle to allocated memory.</param>
        /// <returns>Handle to the lock.</returns>
        public IntPtr MemLock(IntPtr handle)
        {
            if (_twain2Entry != null && _twain2Entry.LockFunction != null)
            {
                return _twain2Entry.LockFunction(handle);
            }
            else
            {
                return GlobalLock(handle);
            }
        }

        /// <summary>
        /// Function to unlock a previously locked memory region.
        /// </summary>
        /// <param name="handle">The handle from <see cref="MemLock"/>.</param>
        public void MemUnlock(IntPtr handle)
        {
            if (_twain2Entry != null && _twain2Entry.UnlockFunction != null)
            {
                _twain2Entry.UnlockFunction(handle);
            }
            else
            {
                GlobalUnlock(handle);
            }
        }

        #region old mem stuff for twain 1.x

        [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

        [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr GlobalFree(IntPtr hMem);

        [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr GlobalLock(IntPtr handle);

        [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GlobalUnlock(IntPtr handle);

        #endregion
    }
}
