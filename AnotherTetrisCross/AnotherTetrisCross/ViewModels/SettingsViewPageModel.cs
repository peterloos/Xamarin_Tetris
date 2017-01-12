namespace AnotherTetrisCross.ViewModels
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Views;
    using System;
    using System.Diagnostics;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class SettingsViewPageModel : ViewModelBase
    {
        private readonly INavigationService navigationService;

        private String displayText;

        public SettingsViewPageModel(INavigationService navigationService)
        {
            if (navigationService == null)
                throw new ArgumentNullException("NavigationService not provided");

            Debug.WriteLine("==============================> SettingsViewPageModel c'tor");

            this.navigationService = navigationService;

            this.displayText = "Settings Say Hello :-)";

            // create commands
            this.NavigateCommand = new Command(() => { this.navigationService.GoBack(); });
        }

        public ICommand NavigateCommand { get; set; }

        public String SettingsDisplayText
        {
            protected set
            {
                if (this.displayText != value)
                {
                    this.displayText = value;

                    this.RaisePropertyChanged(() => SettingsDisplayText);
                }
            }

            get
            {
                return this.displayText;
            }
        }
    }
}
