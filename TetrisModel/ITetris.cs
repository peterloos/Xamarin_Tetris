using System.ComponentModel;

namespace AnotherTetrisModel
{
    public interface ITetris : INotifyPropertyChanged
    {
        // general properties
        int NumRows { get; }
        int NumColumns { get; }
        GameState GameState { get; }
        ViewCellList BoardStateChanges { get; }

        // game commands
        void Start();
        void Stop();
        void Pause();
        void Continue();
        void Clear();

        // score management
        int Level { get; }
        int Lines { get; }
        int Score { get; }

        // tetrimino commands
        void DoAction(TetriminoAction action);
        bool DoAction(int row, int col);
    }
}
