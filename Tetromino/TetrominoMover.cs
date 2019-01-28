using System.Windows.Media;
using Tetris.Engine;

namespace Tetris
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

        public static MoveContext GetRotationContext(Tetromino tetromino)
        {
            var newMatrix = GetRotatedMatrix(tetromino.BodyMatrix);
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

        public static bool[,] GetRotatedMatrix(bool[,] originalMatrix)
        {
            int m = originalMatrix.GetLength(0);
            int n = originalMatrix.GetLength(1);
            int j = 0;
            int p = 0;
            int q = 0;
            int i = m - 1;
            bool[,] rotatedArr = new bool[m, n];

            //for (int i = m-1; i >= 0; i--)
            for (int k = 0; k < m; k++)
            {
                while (i >= 0)
                {
                    rotatedArr[p, q] = originalMatrix[i, j];
                    q++;
                    i--;
                }
                j++;
                i = m - 1;
                q = 0;
                p++;
            }
            return rotatedArr;
        }
    }
}
