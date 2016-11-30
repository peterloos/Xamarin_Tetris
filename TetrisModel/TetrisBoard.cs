namespace AnotherTetrisModel
{
    using System;
    using System.Diagnostics;

    public delegate void BoardChangedHandler(ViewCellList cells);
    public delegate void LinesCompletedHandler(int lines);

    public class TetrisBoard : ITetrisBoard
    {
        public event BoardChangedHandler BoardChanged;
        public event LinesCompletedHandler LinesCompleted;

        private int numRows;
        private int numColumns;
        private TetrisCell[,] board;

        public TetrisBoard(int rows, int cols)
        {
            this.numRows = rows;
            this.numColumns = cols;
            this.AllocateBoard(rows, cols);
        }

        // properties
        public int NumRows
        {
            get { return this.numRows; }
        }

        public int NumColumns
        {
            get { return this.numColumns; }
        }

        // indexer
        public TetrisCell this[int row, int col]
        {
            get
            {
                return this.board[row, col];
            }

            set
            {
                this.board[row, col] = value;
            }
        }

        // public interface
        public void Clear()
        {
            // reset internal state of board
            for (int i = 0; i < this.numRows; i++)
            {
                for (int j = 0; j < this.numColumns; j++)
                {
                    this.board[i, j].Color = CellColor.LightGray;
                    this.board[i, j].State = CellState.Free;
                }
            }

            // update view 
            ViewCellList list = new ViewCellList();
            for (int i = 0; i < this.numRows; i++)
            {
                for (int j = 0; j < this.numColumns; j++)
                {
                    ViewCell cell = new ViewCell();
                    cell.Color = CellColor.LightGray;
                    cell.Point = new CellPoint() { X = j, Y = i };
                    list.Add(cell);
                }
            }
            this.OnBoardChanged(list);
        }

        public void PostChanges(ViewCellList list)
        {
            this.OnBoardChanged(list);
        }

        public bool IsBottomRowComplete()
        {
            return this.IsRowComplete(this.numRows - 1);
        }

        public void MoveNonEmptyRowsDown()
        {
            ViewCellList list = new ViewCellList();

            // compute number of complete rows - beginning from the bottom
            int completeRows = 0;
            for (int row = this.numRows - 1; row >= 0; row --)
            {
                if (this.IsRowComplete(row))
                {
                    completeRows++;
                }
                else
                {
                    break;
                }
            }

            // calculate number of rows to move
            int startRow = this.numRows - 1;
            while (!this.IsRowEmpty(startRow))
                startRow--;

            for (int i = this.numRows - 2; i >= startRow; i--)
            {
                this.CopySingleRow(list, i);
            }

            this.OnBoardChanged(list);
            this.OnLinesCompleted(completeRows);
        }

        // private helper methods
        private void AllocateBoard(int rows, int cols)
        {
            this.board = new TetrisCell[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    this.board[i, j] = new TetrisCell();
                }
            }
        }

        private bool IsRowComplete(int row)
        {
            bool isComplete = true;
            for (int j = 0; j < this.numColumns; j++)
            {
                if (this.board[row, j].State != CellState.Used)
                {
                    isComplete = false;
                    break;
                }
            }

            return isComplete;
        }

        private void CopySingleRow(ViewCellList list, int row)
        {
            for (int j = 0; j < this.numColumns; j++)
            {
                // create cell to update view
                ViewCell cell = new ViewCell();
                cell.Color = this.board[row, j].Color;
                cell.Point = new CellPoint() { X = j, Y = row + 1 };
                list.Add(cell);

                // finally copy block one row down ...
                // Note: TetrisCell is a 'struct', so '=' works fine -- 
                // In case of a reference type: Clone needed (deep copy) !!!
                this.board[row + 1, j] = this.board[row, j];
            }
        }

        private bool IsRowEmpty(int index)
        {
            bool isEmpty = true;
            for (int j = 0; j < this.numColumns; j++)
            {
                if (this.board[index, j].State == CellState.Used)
                {
                    isEmpty = false;
                    break;
                }
            }

            return isEmpty;
        }

        private void OnBoardChanged(ViewCellList list)
        {
            if (this.BoardChanged != null)
                this.BoardChanged.Invoke(list);
        }

        private void OnLinesCompleted(int lines)
        {
            if (this.LinesCompleted != null)
                this.LinesCompleted.Invoke(lines);
        }

        // private test tools
        public static void DumpBoard(TetrisCell[,] board, String indent)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                Debug.WriteLine("{0}", indent);
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Debug.WriteLine("{0}", board[i, j].State == CellState.Free ? "O" : "X");
                }
            }
        }
    }
}
