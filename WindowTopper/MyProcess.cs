using System;

namespace Topifier
{
    /// <summary>
    /// The my process.
    /// </summary>
    public struct MyProcess
    {
        public MyProcess(string processWindowTitle, IntPtr mainWindowHandle)
            : this()
        {
            ProcessWindowTitle = processWindowTitle;
            ProcessHandle = mainWindowHandle;
        }

        public override string ToString()
        {
            return ProcessWindowTitle;
        }

        public IntPtr ProcessHandle { get; set; }
        public string ProcessWindowTitle { get; set; }
    }
}