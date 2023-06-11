using InitialProject.Application.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.GuestOne
{
    public class StatusBarViewModel : ViewModelBase
    {
        private string _currentLanguage;
        public string CurrentLanguage
        {
            get { return _currentLanguage; }
            set
            {
                _currentLanguage = value;
                OnPropertyChanged();
            }
        }
        public ICommand ChangeLanguageCommand { get; }
        public StatusBarViewModel()
        {
            CurrentLanguage = TranslationSource.Instance.CurrentCulture.Name;
            ChangeLanguageCommand = new ExecuteMethodCommand(Execute_SwitchLanguageCommand);
        }
        private void Execute_SwitchLanguageCommand()
        {
            App app = (App)System.Windows.Application.Current;
            if (CurrentLanguage.Equals("en-US"))
            {
                CurrentLanguage = "sr-Latn";
            }
            else
            {
                CurrentLanguage = "en-US";
            }
            app.ChangeLanguage(CurrentLanguage);
        }
    }
}
