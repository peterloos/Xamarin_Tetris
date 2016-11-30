namespace AnotherTetrisCross.ViewModels
{
    using System;
    using System.Diagnostics;
    using System.Windows.Input;
    
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Views;
    using Xamarin.Forms;

    public class AboutViewPageModel : ViewModelBase
    {
        private readonly INavigationService navigationService;

        public ICommand NavigateCommand { get; set; }

        private String displayText;

        public AboutViewPageModel(INavigationService navigationService)
        {
            if (navigationService == null)
                throw new ArgumentNullException("NavigationService not provided");

            Debug.WriteLine("==============================> AboutViewPageModel c'tor");

            this.navigationService = navigationService;

            this.displayText = "www.peterloos.de";

            // create commands
            this.NavigateCommand =
                new Command(() => { this.navigationService.GoBack(); });
        }

        public String AboutDisplayText
        {
            get
            {
                return this.displayText;
            }
        }

        public ICommand HyperlinkCommand
        {
            get
            {
                return new Command(() =>
                {
                    Uri peterloosUri =
                        new Uri ("http://www.peterloos.de", UriKind.Absolute);
                    Device.OpenUri(peterloosUri);
                });
            }
        }
    }
}
