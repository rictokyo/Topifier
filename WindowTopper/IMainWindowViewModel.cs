using System.Collections.Generic;

namespace Topifier
{
    public interface IMainWindowViewModel
    {
        string AppWindowTitle { get; set; }
        IEnumerable<MyProcess> ProcessList { get; }
        MyProcess SelectedProcess { get; set; }
    }
}