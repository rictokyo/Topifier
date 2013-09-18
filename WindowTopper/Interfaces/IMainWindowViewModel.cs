using System.Collections.Generic;
using Topifier.Structs;

namespace Topifier.Interfaces
{
    public interface IMainWindowViewModel
    {
        string AppWindowTitle { get; set; }
        IEnumerable<MyProcess> ProcessList { get; }
        MyProcess? SelectedProcess { get; set; }
    }
}