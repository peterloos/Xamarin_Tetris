namespace AnotherTetrisModel
{
    public struct TetrisCell
    {
        // properties
        public CellState State { get; set; }
        public CellColor Color { get; set; }

        public TetrisCell(CellState state, CellColor color)
            : this()
        {
            this.State = state;
            this.Color = color;
        }
    }
}
