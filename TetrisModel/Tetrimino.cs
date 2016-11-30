namespace AnotherTetrisModel
{
    using System;
    using System.Diagnostics;

    public abstract class Tetrimino : ITetrimino
    {
        private static readonly int MaxDistance = 2;
        protected enum RotationAngle { Degrees_0, Degrees_90, Degrees_180, Degrees_270 }

        protected ITetrisBoard board;
        protected CellPoint anchorPoint;
        protected RotationAngle rotation;
        protected CellColor color;

        // c'tor
        public Tetrimino(ITetrisBoard board, CellColor color)
        {
            this.board = board;
            this.color = color;
            this.rotation = RotationAngle.Degrees_0;
        }

        // abstract interface
        public abstract bool CanSetToTop();
        public abstract bool CanMoveLeft();
        public abstract bool CanMoveRight();
        public abstract bool CanMoveDown();
        public abstract bool CanRotate();

        public bool IsCoordinateWithin(int row, int col)
        {

            if (Math.Abs(this.anchorPoint.X - col) <= MaxDistance && Math.Abs(this.anchorPoint.Y - row) <= MaxDistance)
                return true;

            return false;
        }

        // public interface (movement specific methods)
        public void SetToTop()
        {
            this.Set();

            ViewCellList list = new ViewCellList();
            this.UpdateModifiedCellList(list, this.color);
            this.board.PostChanges(list);
        }

        public void MoveLeft()
        {
            ViewCellList list = new ViewCellList();
            this.Delete();
            this.UpdateModifiedCellList(list, CellColor.LightGray);
            this.MoveAnchorLeft();
            this.Set();
            this.UpdateModifiedCellList(list, this.color);
            this.board.PostChanges(list);
        }

        public void MoveRight()
        {
            ViewCellList list = new ViewCellList();
            this.Delete();
            this.UpdateModifiedCellList(list, CellColor.LightGray);
            this.MoveAnchorRight();
            this.Set();
            this.UpdateModifiedCellList(list, this.color);
            this.board.PostChanges(list);
        }

        public bool MoveDown()
        {
            if (!this.CanMoveDown())
                return false;

            ViewCellList list = new ViewCellList();
            this.Delete();
            this.UpdateModifiedCellList(list, CellColor.LightGray);
            this.MoveAnchorDown();
            this.Set();
            this.UpdateModifiedCellList(list, this.color);
            this.board.PostChanges(list);

            return true;
        }

        public void Rotate()
        {
            ViewCellList list = new ViewCellList();
            this.Delete();
            this.UpdateModifiedCellList(list, CellColor.LightGray);
            this.RotateTetrimino();
            this.Set();
            this.UpdateModifiedCellList(list, this.color);
            this.board.PostChanges(list);
        }

        // public interface (board specific methods)
        public void Set()
        {
            this.Update(CellState.Used);
        }

        public void Delete()
        {
            this.Update(CellState.Free);
        }

        public abstract void Update(CellState state);
        public abstract void UpdateModifiedCellList(ViewCellList list, CellColor color);

        // private (protected) helper methods
        protected void MoveAnchorLeft()
        {
            this.anchorPoint.X--;
        }

        protected void MoveAnchorRight()
        {
            this.anchorPoint.X++;
        }

        protected void MoveAnchorDown()
        {
            this.anchorPoint.Y++;
        }

        protected void RotateTetrimino()
        {
            if (this.rotation == RotationAngle.Degrees_0)
            {
                this.rotation = RotationAngle.Degrees_90;
            }
            else if (this.rotation == RotationAngle.Degrees_90)
            {
                this.rotation = RotationAngle.Degrees_180;
            }
            else if (this.rotation == RotationAngle.Degrees_180)
            {
                this.rotation = RotationAngle.Degrees_270;
            }
            else if (this.rotation == RotationAngle.Degrees_270)
            {
                this.rotation = RotationAngle.Degrees_0;
            }
        }
    }
}
