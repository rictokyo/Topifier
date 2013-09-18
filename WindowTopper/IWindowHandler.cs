using System;
using System.Collections.Generic;

namespace Topifier
{
    public interface IWindowHandler
    {
        IEnumerable<MyProcess> GetProcesses();
        void SetOnTop(IntPtr windowHandle);
        void SetOffTop(IntPtr windowHandle);
        void BringToFront(IntPtr windowHandle);
        void UpdateTitle(IntPtr windowHandle, string appWindowTitle);
    }
}