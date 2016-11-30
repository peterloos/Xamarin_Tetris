using AnotherTetrisCross.ViewPages;
using AnotherTetrisCross.Services;
using AnotherTetrisCross.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AnotherTetrisCross
{
    public class Locator
    {
        public const String TetrisViewPage = "TetrisViewPage";
        public const String SettingsViewPage = "SettingsViewPage";
        public const String HighScoresDialogViewPage = "HighScoresDialogViewPage";
        public const String HighScoresSummaryViewPage = "HighScoresSummaryViewPage";
        public const String AboutViewPage = "AboutViewPage";

        static Locator()
        {
            Debug.WriteLine("==============================> Locator static c'tor");
        }

        public static void Init()
        {
            Debug.WriteLine("==============================> Locator Init");

            SimpleIoc.Default.Reset();
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // view models
            SimpleIoc.Default.Register<TetrisViewPageModel>();
            SimpleIoc.Default.Register<SettingsViewPageModel>();
            SimpleIoc.Default.Register<HighScoresViewModel>();
            SimpleIoc.Default.Register<AboutViewPageModel>();

            // navigation
            NavigationService navigationService = new NavigationService();
            navigationService.Configure(Locator.TetrisViewPage, typeof(TetrisViewPage));
            navigationService.Configure(Locator.SettingsViewPage, typeof(SettingsViewPage));
            navigationService.Configure(Locator.HighScoresDialogViewPage, typeof(HighScoresDialogViewPage));
            navigationService.Configure(Locator.HighScoresSummaryViewPage, typeof(HighScoresSummaryViewPage));
            navigationService.Configure(Locator.AboutViewPage, typeof(AboutViewPage));
            SimpleIoc.Default.Register<INavigationService>(() => navigationService);

            // views
            SimpleIoc.Default.Register<TetrisViewPage>();
            SimpleIoc.Default.Register<SettingsViewPage>();
            SimpleIoc.Default.Register<HighScoresDialogViewPage>();
            SimpleIoc.Default.Register<HighScoresSummaryViewPage>();
            SimpleIoc.Default.Register<AboutViewPage>();
        }

        public static TetrisViewPageModel TetrisViewPageBindingContext
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TetrisViewPageModel>();
            }
        }

        public static SettingsViewPageModel SettingsViewPageBindingContext
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsViewPageModel>();
            }
        }

        public static HighScoresViewModel HighScoresBindingContext
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HighScoresViewModel>();
            }
        }

        public static AboutViewPageModel AboutViewPageBindingContext
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AboutViewPageModel>();
            }
        } 
    }
}
