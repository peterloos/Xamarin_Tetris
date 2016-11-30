namespace AnotherTetrisModel
{
    public interface ITetrisBoard
    {
        event BoardChangedHandler BoardChanged;
        event LinesCompletedHandler LinesCompleted;

        // properties
        int NumRows { get; }
        int NumColumns { get; }

        // indexer
        TetrisCell this[int row, int col] { get; set; }

        // methods
        void Clear();
        void PostChanges(ViewCellList list);
        bool IsBottomRowComplete();
        void MoveNonEmptyRowsDown();
    }
}
