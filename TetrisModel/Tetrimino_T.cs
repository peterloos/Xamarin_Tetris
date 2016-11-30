namespace AnotherTetrisModel
{
    using System.Diagnostics;

    public class Tetrimino_T : Tetrimino
    {
        public Tetrimino_T(ITetrisBoard board)
            : base(board, CellColor.Magenta)
        {
            this.anchorPoint = new CellPoint() { X = 5, Y = 1 };
        }

        // predicates
        public override bool CanSetToTop()
        {
            Debug.Assert(this.rotation == RotationAngle.Degrees_0);

            if (this.board[this.anchorPoint.Y, this.anchorPoint.X - 1].State == CellState.Used ||
                this.board[this.anchorPoint.Y, this.anchorPoint.X].State == CellState.Used ||
                this.board[this.anchorPoint.Y + 1, this.anchorPoint.X].State == CellState.Used ||
                this.board[this.anchorPoint.Y, this.anchorPoint.X + 1].State == CellState.Used)
                return false;

            return true;
        }

        public override bool CanMoveLeft()
        {
            // check fields to the left of the tetrimino
            switch (this.rotation)
            {
                case RotationAngle.Degrees_0:
                    if (this.anchorPoint.X == 1)
                        return false;
                    if (this.board[this.anchorPoint.Y, this.anchorPoint.X - 2].State == CellState.Used ||
                        this.board[this.anchorPoint.Y + 1, this.anchorPoint.X - 1].State == CellState.Used)
                        return false;
                    break;

                case RotationAngle.Degrees_90:
                    if (this.anchorPoint.X == 1)
                        return false;
                    if (this.board[this.anchorPoint.Y - 1, this.anchorPoint.X - 1].State == CellState.Used ||
                        this.board[this.anchorPoint.Y, this.anchorPoint.X - 2].State == CellState.Used ||
                        this.board[this.anchorPoint.Y + 1, this.anchorPoint.X - 1].State == CellState.Used)
                        return false;
                    break;

                case RotationAngle.Degrees_180:
                    if (this.anchorPoint.X == 1)
                        return false;
                    if (this.board[this.anchorPoint.Y - 1, this.anchorPoint.X - 1].State == CellState.Used ||
                        this.board[this.anchorPoint.Y, this.anchorPoint.X - 2].State == CellState.Used)
                        return false;
                    break;

                case RotationAngle.Degrees_270:
                    if (this.anchorPoint.X == 0)
                        return false;
                    if (this.board[this.anchorPoint.Y - 1, this.anchorPoint.X - 1].State == CellState.Used ||
                        this.board[this.anchorPoint.Y, this.anchorPoint.X - 1].State == CellState.Used ||
                        this.board[this.anchorPoint.Y + 1, this.anchorPoint.X - 1].State == CellState.Used)
                        return false;
                    break;
            }

            return true;
        }

        public override bool CanMoveRight()
        {
            // check fields to the right of the tetrimino
            switch (this.rotation)
            {
                case RotationAngle.Degrees_0:
                    if (this.anchorPoint.X == this.board.NumColumns - 2)
                        return false;
                    if (this.board[this.anchorPoint.Y, this.anchorPoint.X + 2].State == CellState.Used ||
                        this.board[this.anchorPoint.Y + 1, this.anchorPoint.X + 1].State == CellState.Used)
                        return false;
                    break;

                case RotationAngle.Degrees_90:
                    if (this.anchorPoint.X == this.board.NumColumns - 1)
                        return false;
                    if (this.board[this.anchorPoint.Y - 1, this.anchorPoint.X + 1].State == CellState.Used ||
                        this.board[this.anchorPoint.Y, this.anchorPoint.X + 1].State == CellState.Used ||
                        this.board[this.anchorPoint.Y + 1, this.anchorPoint.X + 1].State == CellState.Used)
                        return false;
                    break;

                case RotationAngle.Degrees_180:
                    if (this.anchorPoint.X == this.board.NumColumns - 2)
                        return false;
                    if (this.board[this.anchorPoint.Y - 1, this.anchorPoint.X + 1].State == CellState.Used ||
                        this.board[this.anchorPoint.Y, this.anchorPoint.X + 2].State == CellState.Used)
                        return false;
                    break;

                case RotationAngle.Degrees_270:
                    if (this.anchorPoint.X == this.board.NumColumns - 2)
                        return false;
                    if (this.board[this.anchorPoint.Y - 1, this.anchorPoint.X + 1].State == CellState.Used ||
                        this.board[this.anchorPoint.Y, this.anchorPoint.X + 2].State == CellState.Used ||
                        this.board[this.anchorPoint.Y + 1, this.anchorPoint.X + 1].State == CellState.Used)
                        return false;
                    break;
            }

            return true;
        }

        public override bool CanMoveDown()
        {
            // check for bottom line & check fields below tetrimino
            switch (this.rotation)
            {
                case RotationAngle.Degrees_0:
                    if (this.anchorPoint.Y >= this.board.NumRows - 2)
                        return false;
                    if (this.board[this.anchorPoint.Y + 1, this.anchorPoint.X - 1].State == CellState.Used ||
                        this.board[this.anchorPoint.Y + 2, this.anchorPoint.X].State == CellState.Used ||
                        this.board[this.anchorPoint.Y + 1, this.anchorPoint.X + 1].State == CellState.Used)
                        return false;
                    break;

                case RotationAngle.Degrees_90:
                    if (this.anchorPoint.Y >= this.board.NumRows - 2)
                        return false;
                    if (this.board[this.anchorPoint.Y + 1, this.anchorPoint.X - 1].State == CellState.Used ||
                        this.board[this.anchorPoint.Y + 2, this.anchorPoint.X].State == CellState.Used)
                        return false;
                    break;

                case RotationAngle.Degrees_180:
                    if (this.anchorPoint.Y >= this.board.NumRows - 1)
                        return false;
                    if (this.board[this.anchorPoint.Y + 1, this.anchorPoint.X - 1].State == CellState.Used ||
                        this.board[this.anchorPoint.Y + 1, this.anchorPoint.X].State == CellState.Used ||
                        this.board[this.anchorPoint.Y + 1, this.anchorPoint.X + 1].State == CellState.Used)
                        return false;
                    break;

                case RotationAngle.Degrees_270:
                    if (this.anchorPoint.Y >= this.board.NumRows - 2)
                        return false;
                    if (this.board[this.anchorPoint.Y + 2, this.anchorPoint.X].State == CellState.Used ||
                        this.board[this.anchorPoint.Y + 1, this.anchorPoint.X + 1].State == CellState.Used)
                        return false;
                    break;
            }

            return true;
        }

        public override bool CanRotate()
        {
            if (this.anchorPoint.X == 0 || this.anchorPoint.X == this.board.NumColumns - 1)
                return false;

            switch (this.rotation)
            {
                case RotationAngle.Degrees_0:
                    if (this.anchorPoint.Y >= this.board.NumRows - 2)
                        return false;
                    if (this.board[this.anchorPoint.Y - 1, this.anchorPoint.X].State == CellState.Used)
                        return false;
                    break;

                case RotationAngle.Degrees_90:
                    if (this.anchorPoint.Y >= this.board.NumRows - 2)
                        return false;
                    if (this.board[this.anchorPoint.Y, this.anchorPoint.X + 1].State == CellState.Used)
                        return false;
                    break;

                case RotationAngle.Degrees_180:
                    if (this.anchorPoint.Y >= this.board.NumRows - 2)
                        return false;
                    if (this.board[this.anchorPoint.Y + 1, this.anchorPoint.X].State == CellState.Used)
                        return false;
                    break;

                case RotationAngle.Degrees_270:
                    if (this.anchorPoint.Y >= this.board.NumRows - 2)
                        return false;
                    if (this.board[this.anchorPoint.Y, this.anchorPoint.X - 1].State == CellState.Used)
                        return false;
                    break;
            }

            return true;
        }

        // board specific methods
        public override void Update(CellState state)
        {
            CellColor color = (state == CellState.Free) ? CellColor.LightGray : this.color;
            TetrisCell cell = new TetrisCell() { Color = color, State = state };

            // update model
            if (this.rotation == RotationAngle.Degrees_0)
            {
                this.board[this.anchorPoint.Y, this.anchorPoint.X - 1] = cell;
                this.board[this.anchorPoint.Y, this.anchorPoint.X] = cell;
                this.board[this.anchorPoint.Y + 1, this.anchorPoint.X] = cell;
                this.board[this.anchorPoint.Y, this.anchorPoint.X + 1] = cell;
            }
            else if (this.rotation == RotationAngle.Degrees_90)
            {
                this.board[this.anchorPoint.Y - 1, this.anchorPoint.X] = cell;
                this.board[this.anchorPoint.Y, this.anchorPoint.X - 1] = cell;
                this.board[this.anchorPoint.Y, this.anchorPoint.X] = cell;
                this.board[this.anchorPoint.Y + 1, this.anchorPoint.X] = cell;
            }
            else if (this.rotation == RotationAngle.Degrees_180)
            {
                this.board[this.anchorPoint.Y, this.anchorPoint.X - 1] = cell;
                this.board[this.anchorPoint.Y, this.anchorPoint.X] = cell;
                this.board[this.anchorPoint.Y - 1, this.anchorPoint.X] = cell;
                this.board[this.anchorPoint.Y, this.anchorPoint.X + 1] = cell;
            }
            else if (this.rotation == RotationAngle.Degrees_270)
            {
                this.board[this.anchorPoint.Y - 1, this.anchorPoint.X] = cell;
                this.board[this.anchorPoint.Y, this.anchorPoint.X] = cell;
                this.board[this.anchorPoint.Y, this.anchorPoint.X + 1] = cell;
                this.board[this.anchorPoint.Y + 1, this.anchorPoint.X] = cell;
            }
        }

        public override void UpdateModifiedCellList(ViewCellList list, CellColor color)
        {
            if (this.rotation == RotationAngle.Degrees_0)
            {
                list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X - 1, Y = this.anchorPoint.Y }));
                list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X, Y = this.anchorPoint.Y + 1 }));
                list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X, Y = this.anchorPoint.Y }));
                list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X + 1, Y = this.anchorPoint.Y }));
            }
            else if (this.rotation == RotationAngle.Degrees_90)
            {
                list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X, Y = this.anchorPoint.Y - 1 }));
                list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X - 1, Y = this.anchorPoint.Y }));
                list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X, Y = this.anchorPoint.Y }));
                list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X, Y = this.anchorPoint.Y + 1 }));
            }
            else if (this.rotation == RotationAngle.Degrees_180)
            {
                list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X - 1, Y = this.anchorPoint.Y }));
                list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X, Y = this.anchorPoint.Y }));
                list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X, Y = this.anchorPoint.Y - 1 }));
                list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X + 1, Y = this.anchorPoint.Y }));
            }
            else if (this.rotation == RotationAngle.Degrees_270)
            {
                list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X, Y = this.anchorPoint.Y - 1 }));
                list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X, Y = this.anchorPoint.Y }));
                list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X + 1, Y = this.anchorPoint.Y }));
                list.Add(new ViewCell(color, new CellPoint() { X = this.anchorPoint.X, Y = this.anchorPoint.Y + 1 }));
            }
        }
    }
}
