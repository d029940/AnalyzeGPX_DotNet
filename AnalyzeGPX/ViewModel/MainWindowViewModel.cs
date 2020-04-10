using System;
using System.Collections.Generic;
using System.Text;

namespace AnalyzeGPX
{
    public class MainWindowViewModel
    {
        private RelayCommand _exitCmd = null;
        public RelayCommand ExitCmd =>
            _exitCmd ?? (_exitCmd = new RelayCommand(Exit));

        private bool CanExit() => true;
        private void Exit()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
