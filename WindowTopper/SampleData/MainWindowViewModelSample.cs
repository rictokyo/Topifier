using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Topifier.Interfaces;
using Topifier.Structs;

namespace Topifier.SampleData
{
    public class MainWindowViewModelSample : IMainWindowViewModel
    {
        private static readonly BitmapImage sampleIcon = new BitmapImage(new Uri("http://icons.iconarchive.com/icons/google/chrome/16/Google-Chrome-icon.png"));

        static MyProcess myProcess = new MyProcess("Google Chrome", (IntPtr)2, sampleIcon);

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
               new MyProcess("Task Manager", default(IntPtr), sampleIcon), myProcess, new MyProcess("Internet Explorer", default(IntPtr), sampleIcon)
            };
            }
        }

        public MyProcess? SelectedProcess { get { return myProcess; } set { throw new NotImplementedException(); } }
    }
}