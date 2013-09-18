using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

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

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        static extern uint GetClassLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        static extern IntPtr GetClassLong64(IntPtr hWnd, int nIndex);

        /// <summary>
        /// 64 bit version maybe loses significant 64-bit specific information
        /// </summary>
        static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
                return new IntPtr((long)GetClassLong32(hWnd, nIndex));
            else
                return GetClassLong64(hWnd, nIndex);
        }


        static readonly uint WM_GETICON = 0x007f;
        static readonly IntPtr ICON_SMALL2 = new IntPtr(2);
        static readonly IntPtr IDI_APPLICATION = new IntPtr(0x7F00);
        static readonly int GCL_HICON = -14;

        public static Bitmap GetSmallWindowIcon(IntPtr hWnd)
        {
            try
            {
                IntPtr hIcon = default(IntPtr);

                hIcon = SendMessage(hWnd, WM_GETICON, ICON_SMALL2, IntPtr.Zero);

                if (hIcon == IntPtr.Zero)
                    hIcon = GetClassLongPtr(hWnd, GCL_HICON);

                if (hIcon == IntPtr.Zero)
                    hIcon = LoadIcon(IntPtr.Zero, (IntPtr)0x7F00/*IDI_APPLICATION*/);

                if (hIcon != IntPtr.Zero)
                    return new Bitmap(Icon.FromHandle(hIcon).ToBitmap(), 16, 16);
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
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
                            var mainWindowHandle = (IntPtr) a;

                            var img = UnSafeMethods.GetSmallWindowIcon(mainWindowHandle);
                            img.MakeTransparent();
                            var bitmap = new BitmapImage();
                            
                            bitmap.BeginInit();
                            var memoryStream = new MemoryStream();

                            // Save to a memory stream...
                            img.Save(memoryStream, ImageFormat.Png);
                            // Rewind the stream...
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            bitmap.StreamSource = memoryStream;
                            bitmap.EndInit();

                            lalista.Add(new MyProcess(processWindowTitle, mainWindowHandle, bitmap));
                        }
                    }
                    return true;
                }, 0);

            return lalista;

            //foreach (var process in processes.Where(p=> !string.IsNullOrEmpty(p.MainWindowTitle)))
            //{
            //    //var sb = new StringBuilder(255);
            //    //UnSafeMethods.GetWindowText(process.MainWindowHandle, sb, sb.Capacity);
            //    yield return new MyProcess(process.MainWindowTitle, process.MainWindowHandle);
            //}
        }
    }
}