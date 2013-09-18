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