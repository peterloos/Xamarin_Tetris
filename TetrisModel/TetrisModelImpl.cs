namespace AnotherTetrisModel
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Xamarin.Forms;

    public class TetrisModelImpl : TetrisModel
    {
        public static readonly int MaxTetriminos = 7;

        private static readonly int IntervalMSecs = 50;
        private static readonly TimeSpan TimerInterval = TimeSpan.FromMilliseconds(IntervalMSecs);

        private static readonly int MaxLevel = 10;
        private static readonly int MaxIntervalCounter = 12;

        private static readonly int LinesPerLevel = 1;

        private enum TetrisState { Idle, Normal, Accelerated, AtTop, AtBottom, GameOver }

        private enum TimerMode { Normal, Accelerated }

        private TetrisBoard board;

        private int testTetriCounter;  // just for testing !!!

        // current tetrimino
        private ITetrimino curr;

        private TetrisState state;
        private GameState gameState;

        private ViewCellList viewCellList;

        // score management
        private int level;
        private int lines;
        private int score;
        private int linesOfCurrentLevel;

        // event(s)
        public override event PropertyChangedEventHandler PropertyChanged;

        private Random random;

        // private timer tools
        private bool keepRecurring;
        private TimerMode timerMode;
        private int maxIntervalCounter;
        private int currentIntervalCounter;

        // c'tor
        public TetrisModelImpl()
        {
            Debug.WriteLine("c'tor TetrisModel");

            this.random = new Random();

            this.testTetriCounter = 0;

            this.board = new TetrisBoard(this.NumRows, this.NumColumns);
            this.board.BoardChanged += this.TetrisBoard_Changed;
            this.board.LinesCompleted += this.TetrisBoard_LinesCompleted;

            this.state = TetrisState.Idle;
            this.gameState = GameState.GameIdle;

            this.viewCellList = new ViewCellList();

            // initialize score management
            this.level = 1;
            this.lines = 0;
            this.score = 0;

            // setup timer
            this.timerMode = TimerMode.Normal;
            this.maxIntervalCounter = MaxIntervalCounter;
            this.currentIntervalCounter = 0;
        }

        // properties
        private TetrisState State
        {
            get
            {
                return this.state;
            }

            set
            {
                Debug.WriteLine("TetrisState => {0}", value);
                this.state = value;
            }
        }

        public override GameState GameState
        {
            get
            {
                return this.gameState;
            }

            protected set
            {
                if (this.gameState != value)
                {
                    Debug.WriteLine("GameState => {0}", value);
                    this.gameState = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public override ViewCellList BoardStateChanges
        {
            get
            {
                return this.viewCellList;
            }

            protected set
            {
                this.viewCellList = value;
                this.RaisePropertyChanged();
            }
        }

        // score management
        public override int Level
        {
            get
            {
                return this.level;
            }

            protected set
            {
                this.level = value;
                this.RaisePropertyChanged();
            }
        }

        public override int Lines
        {
            get
            {
                return this.lines;
            }

            protected set
            {
                this.lines = value;
                this.RaisePropertyChanged();
            }
        }

        public override int Score
        {
            get
            {
                return this.score;
            }

            protected set
            {
                this.score = value;
                this.RaisePropertyChanged();
            }
        }

        private void RaisePropertyChanged([CallerMemberName] String propName = "")
        {
            if (this.PropertyChanged != null)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(propName);
                this.PropertyChanged(this, args);
            }
        }

        // tetrimino management
        private ITetrimino AttachTetrimino()
        {
            // choose tetrimino by random
            int which = this.random.Next() % MaxTetriminos;

            // or do not :-) choose tetrimino by random (just for testing)
            which = this.testTetriCounter++;

            if (which % MaxTetriminos == 0)
                this.curr = new Tetrimino_I(this.board);
            else if (which % MaxTetriminos == 1)
                this.curr = new Tetrimino_J(this.board);
            else if (which % MaxTetriminos == 2)
                this.curr = new Tetrimino_L(this.board);
            else if (which % MaxTetriminos == 3)
                this.curr = new Tetrimino_O(this.board);
            else if (which % MaxTetriminos == 4)
                this.curr = new Tetrimino_S(this.board);
            else if (which % MaxTetriminos == 5)
                this.curr = new Tetrimino_T(this.board);
            else if (which % MaxTetriminos == 6)
                this.curr = new Tetrimino_Z(this.board);

            return this.curr;
        }

        private void DetachTetrimino()
        {
            this.curr = null;
        }

        // controller requests
        public override void DoAction(TetriminoAction action)
        {
            switch (action)
            {
                case TetriminoAction.Left:
                    this.DoActionMoveLeft();
                    break;
                case TetriminoAction.Right:
                    this.DoActionMoveRight();
                    break;
                case TetriminoAction.Rotate:
                    this.DoActionRotate();
                    break;
                case TetriminoAction.BeginAllWayDown:
                    this.DoBeginAllWayDown();
                    break;
            }
        }

        public override bool DoAction(int row, int col)
        {
            return this.curr.IsCoordinateWithin (row, col);
        }

        // timer tick method
        private bool TimerTetrisTick ()
        {
            if (!this.keepRecurring)
            {
                // should never occur ... but occurs
                return false;
            }

            if (!IsTimerReady())
                return this.keepRecurring;

            switch (this.State)
            {
                case TetrisState.AtTop:
                    this.DoActionSetToTop();
                    break;

                case TetrisState.Normal:
                case TetrisState.Accelerated:
                    this.DoActionMoveDown();
                    break;

                case TetrisState.AtBottom:
                    this.DoActionAtBottom();
                    break;

                case TetrisState.GameOver:

                    this.DoActionGameOver();
                    break;

                default:
                    Debug.WriteLine("TimerTetrisTick: Internal ERROR: Should never be reached");
                    break;
            }

            return this.keepRecurring;
        }

        private bool IsTimerReady()
        {
            if (this.currentIntervalCounter == 0)
            {
                this.currentIntervalCounter = (this.timerMode == TimerMode.Normal) ? this.maxIntervalCounter : 1;
                return true;
            }

            this.currentIntervalCounter--;

            return false;
        }

        // animation commands
        public override void Start()
        {
            if (this.GameState == GameState.GameIdle)
            {
                this.GameState = AnotherTetrisModel.GameState.GameRunning;

                this.board.Clear();
                this.State = TetrisState.AtTop;
                this.keepRecurring = true;

                TimeSpan interval = TimeSpan.FromMilliseconds(IntervalMSecs - 10 * this.level);
                Device.StartTimer(interval, this.TimerTetrisTick);
            }
            else
            {
                Debug.WriteLine("==> Ignoring StartRequest ... model is not IDLE");
            }
        }

        public override void Stop()
        {
            Debug.WriteLine("==> Model Stop");
            this.keepRecurring = false;
            this.GameState = GameState.GameOver;
        }

        public override void Pause()
        {
            Debug.WriteLine("==> Model Pause");

            if (this.GameState == GameState.GameRunning)
            {
                this.keepRecurring = false;
                this.GameState = GameState.GamePaused;
            }
            else
            {
                Debug.WriteLine("==> Ignoring StartRequest ... model is not RUNNING");
            }
        }

        public override void Continue()
        {
            this.GameState = GameState.GameRunning;

            this.keepRecurring = true;
            Device.StartTimer(TimerInterval, this.TimerTetrisTick);
        }

        public override void Clear()
        {
            this.GameState = GameState.GameIdle;

            this.Level = 1;
            this.Lines = 0;
            this.Score = 0;

            this.board.Clear();
        }

        // action methods
        private void DoActionSetToTop()
        {
            Debug.Assert(this.State == TetrisState.AtTop);

            // create new tetrimino
            this.curr = this.AttachTetrimino();

            if (this.curr.CanSetToTop())
            {
                this.curr.SetToTop();
                this.State = TetrisState.Normal;
            }
            else
            {
                this.State = TetrisState.GameOver;
            }
        }

        private void DoActionAtBottom()
        {
            // tetrimino has reached bottom of field, release it
            this.DetachTetrimino();

            // rearrange field, if possible
            while (this.board.IsBottomRowComplete())
            {
                this.board.MoveNonEmptyRowsDown();
            }

            // reset timer to regular interval (in case of any preceding 'DoBeginAllWayDown' actions)
            this.timerMode = TimerMode.Normal;  // decrease amount of active timer ticks

            // schedule next tetrimino 
            this.State = TetrisState.AtTop;
        }

        private void DoActionGameOver()
        {
            this.Stop();
        }

        private void DoActionMoveRight()
        {
            if (this.State != TetrisState.Normal)
                return;

            if (this.curr.CanMoveRight())
                this.curr.MoveRight();
        }

        private void DoActionMoveLeft()
        {
            if (this.State != TetrisState.Normal)
                return;

            if (this.curr.CanMoveLeft())
                this.curr.MoveLeft();
        }

        private void DoActionRotate()
        {
            if (this.State != TetrisState.Normal)
                return;

            if (this.curr.CanRotate())
                this.curr.Rotate();
        }

        private void DoActionMoveDown()
        {
            if (this.curr == null)
                return;

            Debug.Assert(this.State == TetrisState.Normal || this.State == TetrisState.Accelerated);

            if (this.curr.CanMoveDown())
            {
                this.curr.MoveDown();

                if (this.State == TetrisState.Accelerated)
                {
                    this.Score++;
                }
            }
            else
            {
                this.State = TetrisState.AtBottom;
            }
        }

        private void DoBeginAllWayDown()
        {
            if (this.State != TetrisState.Normal)
                return;

            // accelerate timer
            this.timerMode = TimerMode.Accelerated;  // increase amount of active timer ticks

            this.State = TetrisState.Accelerated;    // allow only 'MoveDown' actions from now on
        }

        // private helper methods
        private void TetrisBoard_Changed(ViewCellList list)
        {
            this.BoardStateChanges = list;
        }

        private void TetrisBoard_LinesCompleted(int lines)
        {
            // increment lines counter
            this.Lines += lines;

            // calculate points for complete lines
            int points;
            switch (lines)
            {
                case 1:
                    points = 40 * this.level;
                    break;
                case 2:
                    points = 100 * this.level;
                    break;
                case 3:
                    points = 300 * this.level;
                    break;
                case 4:
                    points = 1200 * this.level;
                    break;
                default:
                    Debug.WriteLine("LinesCompleted: Internal ERROR: Should never be reached");
                    points = 0;
                    break;
            }
            this.Score += points;

            // check, if next level is reached
            this.linesOfCurrentLevel += lines;
            if (this.linesOfCurrentLevel >= LinesPerLevel)
            {
                this.linesOfCurrentLevel = 0;

                // switch to next level (reducing interval counter of timer tick routine)
                if (this.Level < MaxLevel)
                {
                    this.Level++;

                    this.maxIntervalCounter--;
                }
            }
        }
    }
}
