namespace AnotherTetrisModel
{
    using System.ComponentModel;

    public abstract class TetrisModel : ITetris
    {
        private static readonly int Columns = 10;
        private static readonly int Rows = 20;

        // event(s)
        public abstract event PropertyChangedEventHandler PropertyChanged;

        // properties
        public int NumRows
        {
            get { return Rows; }
        }

        public int NumColumns
        {
            get { return Columns; }
        }

        public abstract GameState GameState { get; protected set; }
        public abstract ViewCellList BoardStateChanges { get; protected set; }

        // score management
        public abstract int Level { get; protected set; }
        public abstract int Lines { get; protected set; }
        public abstract int Score { get; protected set; }

        // public interface
        public abstract void DoAction(TetriminoAction action);
        public abstract bool DoAction(int row, int col);
        public abstract void Start();
        public abstract void Stop();
        public abstract void Pause();
        public abstract void Continue();
        public abstract void Clear();
    }
}
