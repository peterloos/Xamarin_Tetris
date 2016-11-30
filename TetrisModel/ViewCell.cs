namespace AnotherTetrisModel
{
    public class ViewCell
    {
        // properties
        public CellColor Color { get; set; }
        public CellPoint Point { get; set; }

        // c'tors
        public ViewCell()
        {
            this.Color = CellColor.LightGray;
            this.Point = new CellPoint() { X = 0, Y = 0 };
        }

        public ViewCell(CellColor color, CellPoint point)
        {
            this.Color = color;
            this.Point = point;
        }
    }
}
