namespace AnotherTetrisCross.ViewModels
{
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using AnotherTetrisModel;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Views;
    using Xamarin.Forms;

    using ViewCell = AnotherTetrisModel.ViewCell;
    using System.ComponentModel;
    using System.Text.RegularExpressions;

    public class TetrisViewPageModel : ViewModelBase
    {
        private ITetris model;
        private IList<BoardCellModel> board;

        private readonly INavigationService navigationService;

        // score management
        private int level;
        private int lines;
        private int score;

        public bool EnterNewScore { set; get; }
        public bool ContinueCurrentGame { set; get; }

        // c'tor
        public TetrisViewPageModel(INavigationService navigationService)
        {
            if (navigationService == null)
                throw new ArgumentNullException("NavigationService not provided");

            Debug.WriteLine("==============================> TetrisViewModel c'tor");

            this.navigationService = navigationService;

            // empty function to prevent any null exceptions ...
            this.GameOverDialog = (n) => { return Task.Delay(0); };
            this.GamePausedDialog = () => { return Task.Delay(0); };

            this.model = new TetrisModelImpl();

            // initialize view model data repository
            this.board = new List<BoardCellModel>();
            for (int row = 0; row < this.model.NumRows; row++)
            {
                for (int col = 0; col < this.model.NumColumns; col++)
                {
                    BoardCellModel cell = new BoardCellModel(this, Color.Gray);
                    this.board.Add(cell);
                }
            }

            this.level = this.model.Level;
            this.lines = this.model.Lines;
            this.score = this.model.Score;

            // listen to changes of underlying model
            this.model.PropertyChanged += this.PropertyChangedEventHandler;
        }

        // properties
        public int NumColumns
        {
            get
            {
                return this.model.NumColumns;
            }
        }

        public int NumRows
        {
            get
            {
                return this.model.NumRows;
            }
        }

        public IList<BoardCellModel> BoardCells
        {
            get
            {
                return this.board;
            }
        }

        public int Level
        {
            get
            {
                return this.level;
            }

            set
            {
                if (this.level != value)
                {
                    this.level = value;

                    this.RaisePropertyChanged(() => Level);
                }
            }
        }

        public int Lines
        {
            get
            {
                return this.lines;
            }

            set
            {
                if (this.lines != value)
                {
                    this.lines = value;

                    this.RaisePropertyChanged(() => Lines);
                }
            }
        }

        public int Score
        {
            get
            {
                return this.score;
            }

            set
            {
                if (this.score != value)
                {
                    this.score = value;

                    this.RaisePropertyChanged(() => Score);
                }
            }
        }
        public Func<int, Task> GameOverDialog { set; get; }

        public Func<Task> GamePausedDialog { set; get; }

        // commands
        public ICommand NavigateSettingsCommand
        {
            get
            {
                return new Command(() => {
                    this.navigationService.NavigateTo(Locator.SettingsViewPage);
                });
            }
        }

        public ICommand NavigateHighScoresCommand
        {
            get
            {
                return new Command(() => {
                    this.navigationService.NavigateTo(Locator.HighScoresSummaryViewPage, null);
                });
            }
        }

        public ICommand NavigateAboutCommand
        {
            get
            {
                return new Command(() => {
                    this.navigationService.NavigateTo(Locator.AboutViewPage);
                });
            }
        }

        public ICommand StartCommand
        {
            get
            {
                return new Command(() =>
                {
                    this.model.Start();
                });
            }
        }

        public ICommand TapCommand
        {
            get
            {
                return new Command<String>((key) =>
                {
                    Match match = Regex.Match(key, "[0-9]+");

                    int row = -1;
                    int col = -1;

                    if (match.Success)
                    {
                        row = Int32.Parse(match.Value);
                        match = match.NextMatch();
                    }

                    if (match.Success)
                    {
                        col = Int32.Parse(match.Value);
                        match = match.NextMatch();
                    }

                    Debug.WriteLine("   Tapped at row = '{0}' and col = {1}", row, col);

                    // decide, which action should be triggered on behalf of the tapped event
                    if (this.model.GameState == GameState.GameRunning)
                    {
                        if (this.model.DoAction(row, col))
                        {
                            this.model.DoAction(TetriminoAction.Rotate);
                        }
                        else if (col <= 3)
                        {
                            this.model.DoAction(TetriminoAction.Left);
                        }
                        else if (col >= 6)
                        {
                            this.model.DoAction(TetriminoAction.Right);
                        }
                        else
                        {
                            Debug.WriteLine("... ignored tapped event at [{0},{1}]", row, col);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("... ignored tapped event at [{0},{1}]", row, col);
                    }
                });
            }
        }

        public ICommand PauseCommand
        {
            get
            {
                return new Command(() =>
                {
                    Debug.WriteLine("PauseCommand - TID = {0}",
                        (Task.CurrentId == null) ? "UI Thread" : Task.CurrentId.ToString());

                    this.model.Pause();
                });
            }
        }

        public ICommand DoActionCommand
        {
            get
            {
                return new Command<String>((key) =>
                {
                    switch (key)
                    {
                        case "Left":
                            this.model.DoAction(TetriminoAction.Left);
                            break;
                        case "Right":
                            this.model.DoAction(TetriminoAction.Right);
                            break;
                        case "Rotate":
                            this.model.DoAction(TetriminoAction.Rotate);
                            break;
                        case "Down":
                            this.model.DoAction(TetriminoAction.BeginAllWayDown); 
                            break;
                    }
                });
            }
        }

        // private helper methods
        private void Model_BoardChanged(ViewCellList list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                ViewCell viewCell = list[i];

                int index = viewCell.Point.Y * this.model.NumColumns + viewCell.Point.X;

                Color color =
                    (viewCell.Color == CellColor.LightGray) ? Color.Gray :
                    (viewCell.Color == CellColor.Magenta) ? Color.Navy :
                    (viewCell.Color == CellColor.Red) ? Color.Red :
                    (viewCell.Color == CellColor.Cyan) ? Color.Fuchsia :
                    (viewCell.Color == CellColor.Blue) ? Color.Blue :
                    (viewCell.Color == CellColor.Ocker) ? Color.Maroon :
                    (viewCell.Color == CellColor.Red) ? Color.Red :
                    (viewCell.Color == CellColor.Green) ? Color.Green :
                        Color.Yellow;

                this.board[index].CellColor = color;
            }
        }

        private async void PropertyChangedEventHandler(Object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("GameState"))
            {
                GameState state = this.model.GameState;

                await this.HandleGameStateChanged(state);
            }
            else if (e.PropertyName.Equals("BoardStateChanges"))
            {
                ViewCellList list = this.model.BoardStateChanges;

                this.Model_BoardChanged(list);
            }
            else if (e.PropertyName.Equals("Level"))
            {
                this.Level = this.model.Level;
            }
            else if (e.PropertyName.Equals("Lines"))
            {
                this.Lines = this.model.Lines;
            }
            else if (e.PropertyName.Equals("Score"))
            {
                this.Score = this.model.Score;
            }
        }

        private async Task HandleGameStateChanged(GameState state)
        {
            if (state == GameState.GamePaused)
            {
                await this.GamePausedDialog();

                if (!this.ContinueCurrentGame)
                {
                    this.model.Clear();
                }
                else
                {
                    this.model.Continue();
                }
            }
            else if (state == GameState.GameOver)
            {
                if (this.GameOverDialog != null)
                {
                    int score = this.model.Score;

                    await this.GameOverDialog(score);

                    this.model.Clear();

                    if (this.EnterNewScore)
                    {
                        this.navigationService.NavigateTo(Locator.HighScoresDialogViewPage, score);
                    }
                }
            }
        }
    }
}
