namespace AnotherTetrisModel
{
    public interface ITetrimino
    {
        // predicates
        bool CanSetToTop();
        bool CanMoveLeft();
        bool CanMoveRight();
        bool CanMoveDown();
        bool CanRotate();
        bool IsCoordinateWithin (int row, int col);

        // movement specific methods
        void SetToTop();
        void MoveLeft();
        void MoveRight();
        bool MoveDown();
        void Rotate();

        // board specific methods
        void Set();
        void Delete();
    }
}
