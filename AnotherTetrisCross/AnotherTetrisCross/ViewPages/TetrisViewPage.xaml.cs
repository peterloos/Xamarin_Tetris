namespace AnotherTetrisCross.ViewPages
{
    using System;
    using AnotherTetrisCross.ViewModels;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public partial class TetrisViewPage : ContentPage
    {
        public TetrisViewPage()
        {
            InitializeComponent();

            Debug.WriteLine("==============================> TetrisViewPage c'tor");

            var viewModel = Locator.TetrisViewPageBindingContext;

            // don't want to break the MVVM pattern - poking delegate of
            // dialog method into view model (in this way the view model
            // is able to practice "inversion of control" for user interactions)
            Func<int, Task> lambdaDialog1 = async (score) =>
            {
                viewModel.EnterNewScore = await this.GameOverDialog(score);
            };
            viewModel.GameOverDialog = lambdaDialog1;

            Func<Task> lambdaDialog2 = async () =>
            {
                viewModel.ContinueCurrentGame = await this.GamePausedDialog();
            };
            viewModel.GamePausedDialog = lambdaDialog2;

            // bind box views with a corresponding 'board cell' of the view model
            for (int i = 0; i < viewModel.NumRows; i++)
            {
                for (int j = 0; j < viewModel.NumColumns; j++)
                {
                    BoxView boxView = new BoxView();
                    boxView.BindingContext =
                        (BoardCellModel)viewModel.BoardCells[j + i * viewModel.NumColumns];

                    boxView.SetBinding(
                        BoxView.BackgroundColorProperty,
                        new Binding("CellColor"));
                    this.TetrisGrid.Children.Add(boxView, j, i);

                    // adding tap recognition
                    TapGestureRecognizer recognizer = new TapGestureRecognizer();
                    recognizer.SetBinding(
                        TapGestureRecognizer.CommandProperty,
                        "TapCommand");
                    String parameters = String.Format ("Row={0}, Col={1}", i, j);
                    recognizer.CommandParameter = parameters;
                    boxView.GestureRecognizers.Add(recognizer);
                }
            }
        }

        // raises alert box to establish a dialog with the user
        private async Task<bool> GameOverDialog (int score)
        {
            String header = String.Format("Game over: {0} Points !", score);
            return await DisplayAlert(
                header, "Would you like to enter your Score?", "Yes", "No");
        }

        private async Task<bool> GamePausedDialog()
        {
            return await DisplayAlert(
                "", "The Game has been paused.", "Resume", "Exit");
        }
    }
}

