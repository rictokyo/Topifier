using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;

namespace Topifier
{
    public abstract class WindowHandler : DependencyObject, IWindowHandler
    {
        public abstract IEnumerable<MyProcess> GetProcesses();

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr ShowWindow(IntPtr hWnd, int nCmdShow);

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        private static readonly IntPtr HWND_TOP = new IntPtr(0);

        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_SHOWWINDOW = 0x0040;

        public void SetOnTop(IntPtr windowHandle)
        {
            SetWindow(windowHandle, HWND_TOPMOST);
        }

        public void SetOffTop(IntPtr windowHandle)
        {
            SetWindow(windowHandle, HWND_NOTOPMOST);
        }

        public void BringToFront(IntPtr windowHandle)
        {
            ShowWindow(windowHandle, 9);
            SetWindow(windowHandle, HWND_TOP);
        }

        private void SetWindow(IntPtr windowHandle, IntPtr windowState)
        {
            SetWindowPos(windowHandle, windowState, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
        }
    }
}