namespace AnotherTetrisModel
{
    using System.Collections.Generic;

    public class ViewCellList
    {
        private List<ViewCell> cells;

        public ViewCellList()
        {
            this.cells = new List<ViewCell>();
        }

        public int Length
        {
            get
            {
                return this.cells.Count;
            }
        }

        public ViewCell this[int index]
        {
            get
            {
                return this.cells[index];
            }
        }

        public void Add(ViewCell cell)
        {
            // is location of this cell already present
            for (int i = 0; i < this.cells.Count; i++)
            {
                ViewCell tmp = this.cells[i];
                if (tmp.Point.Equals(cell.Point))
                {
                    // replace this point with new cell (old one is obsolete)
                    this.cells[i] = cell;
                    return;
                }
            }

            // cell not found, just add it
            this.cells.Add(cell);
        }
    }
}
