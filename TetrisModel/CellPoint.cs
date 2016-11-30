namespace AnotherTetrisModel
{
    using System;
    public struct CellPoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public CellPoint(int x, int y)
            : this()
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(Object obj)
        {
            if (!(obj is CellPoint))
                return false;

            CellPoint point = (CellPoint)obj;
            return this.X == point.X && this.Y == point.Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override String ToString()
        {
            return String.Format("X={0}, Y={1}", this.X, this.Y);
        }
    }
}
