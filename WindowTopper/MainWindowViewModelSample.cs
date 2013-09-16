using System;
using System.Collections.Generic;

namespace Topifier
{
    public class MainWindowViewModelSample : IMainWindowViewModel
    {
        static MyProcess myProcess = new MyProcess("Google Chrome", (IntPtr)2);

        public string AppWindowTitle
        {
            get { return "Task Manager"; }
            set { throw new NotImplementedException(); }
        }

        public IEnumerable<MyProcess> ProcessList
        {
            get
            {
                return new[]
            {
               new MyProcess("Task Manager", default(IntPtr)), myProcess, new MyProcess("Internet Explorer", default(IntPtr))
            };
            }
        }

        public MyProcess SelectedProcess { get { return myProcess; } set { throw new NotImplementedException(); } }
    }
}