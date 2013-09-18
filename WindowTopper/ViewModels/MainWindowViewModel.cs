using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Topifier.Interfaces;
using Topifier.Structs;

namespace Topifier.ViewModels
{
    public class MainWindowViewModel : BaseMainWindowViewModel, IMainWindowViewModel
    {
        private string _appWindowTitle;
        private IEnumerable<MyProcess> _processList;
        private MyProcess? _selectedProcess;
        private ICommand _refreshProcessListCommand;
        private ICommand _setOnTopCommand;
        private ICommand _setOffTopCommand;
        private ICommand _bringToFrontCommand;
        private ICommand _updateTitleCommand;

        public MainWindowViewModel(IWindowHandler windowHandler)
        {
            Handler = windowHandler;

            RefreshProcessListCommand = new DelegateCommand(RefreshProcessList);
            SetOnTopCommand = new DelegateCommand(SetOnTop);
            SetOffTopCommand = new DelegateCommand(SetOffTop);
            BringToFrontCommand = new DelegateCommand(BringToFront);
            UpdateTitleCommand = new DelegateCommand(UpdateTitle);
        }

        private void UpdateTitle()
        {
            if (this.Handler != null)
            {
                var selectedProcess = this.SelectedProcess;
                if (selectedProcess != null)
                    this.Handler.UpdateTitle(selectedProcess.Value.ProcessHandle, AppWindowTitle);
            }
        }

        public ICommand BringToFrontCommand
        {
            get { return _bringToFrontCommand; }
            set
            {
                SetField(ref _bringToFrontCommand, value, () => BringToFrontCommand);
            }
        }

        public ICommand UpdateTitleCommand
        {
            get { return _updateTitleCommand; }
            set
            {
                SetField(ref _updateTitleCommand, value, () => UpdateTitleCommand);
            }
        }

        private void SetOnTop()
        {
            if (this.Handler != null)
            {
                var selectedProcess = this.SelectedProcess;
                if (selectedProcess != null) this.Handler.SetOnTop(selectedProcess.Value.ProcessHandle);
            }
        }

        private void BringToFront()
        {
            if (this.Handler != null)
            {
                var selectedProcess = this.SelectedProcess;
                if (selectedProcess != null) this.Handler.BringToFront(selectedProcess.Value.ProcessHandle);
            }
        }

        private void SetOffTop()
        {
            if (this.Handler != null)
            {
                var selectedProcess = this.SelectedProcess;
                if (selectedProcess != null) this.Handler.SetOffTop(selectedProcess.Value.ProcessHandle);
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
            set
            {
                SetField(ref _refreshProcessListCommand, value, () => RefreshProcessListCommand);
            }
        }

        public IWindowHandler Handler { get; set; }

        public string AppWindowTitle
        {
            get { return _appWindowTitle; }
            set
            {
                SetField(ref _appWindowTitle, value, () => AppWindowTitle);
            }
        }

        public IEnumerable<MyProcess> ProcessList
        {
            get { return _processList; }
            set
            {
                SetField(ref _processList, value, () => ProcessList);
            }
        }

        public MyProcess? SelectedProcess
        {
            get
            {
                return _selectedProcess;
            }
            set
            {
                if (_selectedProcess != null) AppWindowTitle = _selectedProcess.Value.ProcessWindowTitle;
                SetField(ref _selectedProcess, value, () => SelectedProcess);
            }
        }

        public ICommand SetOnTopCommand
        {
            get { return _setOnTopCommand; }
            set
            {
                SetField(ref _setOnTopCommand, value, () => SetOnTopCommand);
            }
        }

        public ICommand SetOffTopCommand
        {
            get { return _setOffTopCommand; }
            set
            {
                SetField(ref _setOffTopCommand, value, () => SetOffTopCommand);
            }
        }
    }
}