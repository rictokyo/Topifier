using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Topifier
{
    public class MainWindowViewModel : BaseMainWindowViewModel, IMainWindowViewModel
    {
        private string _appWindowTitle;
        private IEnumerable<MyProcess> _processList;
        private MyProcess _selectedProcess;
        private ICommand _refreshProcessListCommand;
        private ICommand _setOnTopCommand;
        private ICommand _setOffTopCommand;
        private ICommand _bringToFrontCommand;

        public MainWindowViewModel(IWindowHandler windowHandler)
        {
            Handler = windowHandler;

            RefreshProcessListCommand = new DelegateCommand(RefreshProcessList);
            SetOnTopCommand = new DelegateCommand(SetOnTop);
            SetOffTopCommand = new DelegateCommand(SetOffTop);
            BringToFrontCommand = new DelegateCommand(BringToFront);
        }

        public ICommand BringToFrontCommand
        {
            get { return _bringToFrontCommand; }
            set { _bringToFrontCommand = value; OnPropertyChanged(); }
        }

        private void SetOnTop()
        {
            if (this.Handler != null)
            {
                this.Handler.SetOnTop(this.SelectedProcess.ProcessHandle);
            }
        }

        private void BringToFront()
        {
            if (this.Handler != null)
            {
                this.Handler.BringToFront(this.SelectedProcess.ProcessHandle);
            }
        }

        private void SetOffTop()
        {
            if (this.Handler != null)
            {
                this.Handler.SetOffTop(this.SelectedProcess.ProcessHandle);
            }
        }

        private void RefreshProcessList()
        {
            if (this.Handler != null)
            {
                ProcessList = this.Handler.GetProcesses();
            }
        }

        public ICommand RefreshProcessListCommand
        {
            get { return _refreshProcessListCommand; }
            set { _refreshProcessListCommand = value; OnPropertyChanged(); }
        }

        public IWindowHandler Handler { get; set; }

        public string AppWindowTitle
        {
            get { return _appWindowTitle; }
            set
            {
                _appWindowTitle = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<MyProcess> ProcessList
        {
            get { return _processList; }
            set { _processList = value; OnPropertyChanged(); }
        }

        public MyProcess SelectedProcess
        {
            get
            {
                return _selectedProcess;
            }
            set
            {
                _selectedProcess = value;
                AppWindowTitle = _selectedProcess.ProcessWindowTitle;

                OnPropertyChanged();
            }
        }

        public ICommand SetOnTopCommand
        {
            get { return _setOnTopCommand; }
            set { _setOnTopCommand = value; OnPropertyChanged(); }
        }

        public ICommand SetOffTopCommand
        {
            get { return _setOffTopCommand; }
            set { _setOffTopCommand = value; OnPropertyChanged(); }
        }
    }
}