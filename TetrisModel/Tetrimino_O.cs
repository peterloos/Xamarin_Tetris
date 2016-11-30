namespace AnotherTetrisModel
{
    using System.Diagnostics;

    public class Tetrimino_O : Tetrimino
    {
        public Tetrimino_O(ITetrisBoard board)
            : base(board, CellColor.Yellow)
        {
            this.anchorPoint = new CellPoint() { X = 5, Y = 1 };
        }

        // predicates
        public override bool CanSetToTop()
        {
            Debug.Assert(this.rotation == RotationAngle.Degrees_0, "Initial rotation should be 0 degrees!");

            if (this.board[this.anchorPoint.Y, this.anchorPoint.X].State == CellState.Used ||
                this.board[this.anchorPoint.Y, this.anchorPoint.X + 1].State == CellState.Used ||
                this.board[this.anchorPoint.Y + 1, this.anchorPoint.X].State == CellState.Used ||
                this.board[this.anchorPoint.Y + 1, this.anchorPoint.X + 1].State == CellState.Used)
                return false;

            return true;
        }

        public override bool CanMoveLeft()
        {
            if (this.anchorPoint.X == 0)
                return false;
            if (this.board[this.anchorPoint.Y, this.anchorPoint.X - 1].State == CellState.Used ||
                this.board[this.anchorPoint.Y + 1, this.anchorPoint.X - 1].State == CellState.Used)
                return false;
            return true;
        }

        public override bool CanMoveRight()
        {
            if (this.anchorPoint.X >= this.board.NumColumns - 2)
                return false;
            if (this.board[this.anchorPoint.Y, this.anchorPoint.X + 2].State == CellState.Used ||
                this.board[this.anchorPoint.Y + 1, this.anchorPoint.X + 2].State == CellState.Used)
                return false;
            return true;
        }

        public override bool CanMoveDown()
        {
            if (this.anchorPoint.Y >= this.board.NumRows - 2)
                return false;
            if (this.board[this.anchorPoint.Y + 2, this.anchorPoint.X].State == CellState.Used ||
                this.board[this.anchorPoint.Y + 2, this.anchorPoint.X + 1].State == CellState.Used)
                return false;
            return true;
        }

        public override bool CanRotate()
        {
            return true;
        }

        // board specific methods
        public override void Update(CellState state)
        {
            CellColor color = (state == CellState.Free) ? CellColor.LightGray : this.color;
            TetrisCell cell = new TetrisCell() { Color = color, State = state };

            // update model
            this.board[this.anchorPoint.Y, this.anchorPoint.X] = cell;
            this.board[this.anchorPoint.Y, this.anchorPoint.X + 1] = cell;
            this.board[this.anchorPoint.Y + 1, this.anchorPoint.X] = cell;
            this.board[this.anchorPoint.Y + 1, this.anchorPoint.X + 1] = cell;
        }

        public override void UpdateModifiedCellList(ViewCellList list, CellColor color)
        {
            list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X, Y = this.anchorPoint.Y }));
            list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X + 1, Y = this.anchorPoint.Y }));
            list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X, Y = this.anchorPoint.Y + 1 }));
            list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X + 1, Y = this.anchorPoint.Y + 1 }));
        }
    }
}
