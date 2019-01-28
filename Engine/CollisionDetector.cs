using System.Collections.Generic;
using System.Windows.Media;

namespace Tetris
{
    public static class CollisionDetector
    {
        internal static bool CollisionDetected(Tetromino tetromino, Brush[,] fieldMatrix, List<PositionInfo> updatedTetrominoPositions)
        {
            Brush[,] modifiedFieldMatrix = ExcludeFromFieldMatrix(fieldMatrix, tetromino.BodyPositions);
            int rows = modifiedFieldMatrix.GetLength(0);
            int cols = modifiedFieldMatrix.GetLength(1);

            foreach (PositionInfo position in updatedTetrominoPositions)
            {
                bool isOutsideMatrix = position.X < 0 || position.X >= cols ||  position.Y < 0 || position.Y >= rows;
                if (isOutsideMatrix || modifiedFieldMatrix[position.Y, position.X] != null)
                {
                    return true;
                }
            }
            return false;
        }

        internal static Brush[,] ExcludeFromFieldMatrix(Brush[,] fieldMatrix, List<PositionInfo> positions)
        {
            var matrix = CopyFieldMatrix(fieldMatrix);
            foreach (PositionInfo position in positions)
            {
                matrix[position.Y, position.X] = null;
            }
            return matrix;
        }

        internal static Brush[,] CopyFieldMatrix(Brush[,] fieldMatrix)
        {
            int rows = fieldMatrix.GetLength(0);
            int cols = fieldMatrix.GetLength(1);

            Brush[,] matrix = new Brush[rows, cols];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    matrix[r, c] = fieldMatrix[r, c];
                }
            }
            return matrix;
        }
    }
}
