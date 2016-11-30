using AnotherTetrisCross.Services;
using AnotherTetrisCross.ViewPages;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace AnotherTetrisCross
{
    public class App : Application
    {
        public App()
        {
            Debug.WriteLine("==============================> App c'tor");

            Locator.Init();

            ContentPage page = ServiceLocator.Current.GetInstance<TetrisViewPage>();
            NavigationPage navigationPage = new NavigationPage(page);
            NavigationService service = (NavigationService) ServiceLocator.Current.GetInstance<INavigationService>();
            service.Initialize(navigationPage);

            this.MainPage = navigationPage;
        }
    }
}
