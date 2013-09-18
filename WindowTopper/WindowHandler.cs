using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace Topifier
{
    public class UnSafeMethods
    {
        public delegate bool WindowEnumCallback(int hwnd, int lparam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(WindowEnumCallback lpEnumFunc, int lParam);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, [Out] StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern int SetWindowText(IntPtr hWnd, string text);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(int h);

        [DllImport("user32.dll")]
        public static extern void GetWindowText(int h, StringBuilder s, int nMaxCount);
    }

    public abstract class WindowHandler : DependencyObject, IWindowHandler
    {
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
            UnSafeMethods.ShowWindow(windowHandle, 9);
            SetWindow(windowHandle, HWND_TOP);
        }

        public void UpdateTitle(IntPtr windowHandle, string appWindowTitle)
        {
            UnSafeMethods.SetWindowText(windowHandle, appWindowTitle);
        }

        private void SetWindow(IntPtr windowHandle, IntPtr windowState)
        {
            UnSafeMethods.SetWindowPos(windowHandle, windowState, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
        }

        public IEnumerable<MyProcess> GetProcesses()
        {
            var processes = Process.GetProcesses();
            List<MyProcess> lalista = new List<MyProcess>();

            UnSafeMethods.EnumWindows((a, b) =>
                {
                    if (UnSafeMethods.IsWindowVisible(a))
                    {
                        var stringBuilder = new StringBuilder(255);
                        UnSafeMethods.GetWindowText(a, stringBuilder, stringBuilder.Capacity);
                        string processWindowTitle = stringBuilder.ToString();
                        
                        if (!string.IsNullOrEmpty(processWindowTitle))
                        {
                            lalista.Add(new MyProcess(processWindowTitle, (IntPtr)a));
                        }
                    }
                    return true;
                }, 0);

            return lalista;

            //foreach (var process in processes.Where(p => !string.IsNullOrEmpty(p.MainWindowTitle)))
            //{
            //    foreach (ProcessThread thread in process.Threads)
            //    {
            //    }

            //    yield return new MyProcess(process.MainWindowTitle, process.MainWindowHandle);
            //}
        }
    }
}