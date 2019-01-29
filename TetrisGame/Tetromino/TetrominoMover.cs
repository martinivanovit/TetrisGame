using System.Windows.Media;

namespace TetrisGame
{
    public static class TetrominoMover
    {
        public static void Move(Tetromino tetromino, Brush[,] fieldMatrix, MoveContext context)
        {
            RemoveTetrominoFromFieldMatrix(tetromino, fieldMatrix);
            tetromino.Move(context);
            UpdateField(tetromino, fieldMatrix);
        }

        public static void Rotate(Tetromino tetromino, Brush[,] fieldMatrix, MoveContext context)
        {
            RemoveTetrominoFromFieldMatrix(tetromino, fieldMatrix);
            tetromino.Rotate(context);
            UpdateField(tetromino, fieldMatrix);
        }

        private static void RemoveTetrominoFromFieldMatrix(Tetromino tetromino, Brush[,] matrix)
        {
            for (int i = 0; i < tetromino.BodyPositions.Count; i++)
            {
                var position = tetromino.BodyPositions[i];
                matrix[position.Y, position.X] = null;
            }
        }

        internal static void UpdateField(Tetromino tetromino, Brush[,] fieldMatrix)
        {
            for (int i = 0; i < tetromino.BodyPositions.Count; i++)
            {
                var position = tetromino.BodyPositions[i];
                fieldMatrix[position.Y, position.X] = tetromino.Brush;
            }
        }

        public static MoveContext GetRotationContext(Tetromino tetromino, bool isCounterClockwise = false)
        {
            var newMatrix = GetRotatedMatrix(tetromino.BodyMatrix, isCounterClockwise);
            var rotatedPositions = Tetromino.GetPositions(tetromino.TopLeft, newMatrix);
            return new MoveContext(tetromino.TopLeft, rotatedPositions, newMatrix);
        }

        public static MoveContext GetMoveContext(Tetromino tetromino, MovementDirection direction, int speed)
        {
            PositionInfo newTopLeft = tetromino.TopLeft;
            if (direction == MovementDirection.Down)
            {
                newTopLeft = new PositionInfo(tetromino.TopLeft.X, tetromino.TopLeft.Y + speed);
            }
            else if (direction == MovementDirection.Left)
            {
                newTopLeft = new PositionInfo(tetromino.TopLeft.X - speed, tetromino.TopLeft.Y);
            }
            else if (direction == MovementDirection.Right)
            {
                newTopLeft = new PositionInfo(tetromino.TopLeft.X + speed, tetromino.TopLeft.Y);
            }

            var context = new MoveContext(newTopLeft, Tetromino.GetPositions(newTopLeft, tetromino.BodyMatrix), tetromino.BodyMatrix);
            return context;
        }

        public static bool[,] GetRotatedMatrix(bool[,] originalMatrix, bool isCounterClockwise)
        {
            if (isCounterClockwise)
            {
                return RotateMatrixCounterClockwise(originalMatrix);
            }
            else
            {
                return RotateMatrixClockwise(originalMatrix);
            }
        }

        private static bool[,] RotateMatrixCounterClockwise(bool[,] originalMatrix)
        {
            int rows = originalMatrix.GetLength(0);
            int columns = originalMatrix.GetLength(1);
            int nextRow = 0;
            bool[,] rotatedMatrix = new bool[columns, rows];

            for (int oldColumn = columns - 1; oldColumn >= 0; oldColumn--)
            {
                for (int oldRow = 0; oldRow < rows; oldRow++)
                {
                    rotatedMatrix[nextRow, oldRow] = originalMatrix[oldRow, oldColumn];
                }
                nextRow++;
            }
            return rotatedMatrix;
        }

        private static bool[,] RotateMatrixClockwise(bool[,] originalMatrix)
        {
            int columns = originalMatrix.GetLength(1);
            int rows = originalMatrix.GetLength(0);

            var rotatedMatrix = new bool[columns, rows];

            for (var oldColumn = 0; oldColumn < columns; ++oldColumn)
            {
                for (var oldRow = 0; oldRow < rows; ++oldRow)
                {
                    rotatedMatrix[oldColumn, oldRow] = originalMatrix[rows - oldRow - 1, oldColumn];
                }
            }
            return rotatedMatrix;
        }
    }
}
