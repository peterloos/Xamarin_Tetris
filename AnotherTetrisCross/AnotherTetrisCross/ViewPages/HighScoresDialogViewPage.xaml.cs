using AnotherTetrisCross.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AnotherTetrisCross.ViewPages
{
    public partial class HighScoresDialogViewPage : ContentPage
    {
        public HighScoresDialogViewPage()
        {
            Debug.WriteLine("==============================> HighScoresViewPage standard c'tor");
            InitializeComponent();
        }

        [PreferredConstructor]
        // public HighScoresViewPage(Object parameter)
        public HighScoresDialogViewPage(int parameter)
        {
            Debug.WriteLine("==============================> HighScoresViewPage c'tor (parameter)");

            // testing - to be removed ...

            int score = (int) parameter;
            Debug.WriteLine("=========> Got score value {0}", parameter);

            InitializeComponent();

            var viewModel = Locator.HighScoresBindingContext;
            viewModel.LastScore = parameter;
        }
    }
}
