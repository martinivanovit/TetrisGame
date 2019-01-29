using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace TetrisGame
{
    public static class TetrominoGenerator
    {
        private static Random randomNumberGenerator = new Random();
        private static readonly TetrominoShapeType[] shapeTypes = Enum.GetValues(typeof(TetrominoShapeType)).OfType<TetrominoShapeType>().ToArray();
        
        public static Tetromino GenerateRandomTetromino(PositionInfo initialPosition)
        {
            TetrominoShapeType shapeType = shapeTypes[randomNumberGenerator.Next(0, shapeTypes.Length)];
            var tetromino = new Tetromino(initialPosition, shapeType);
            return tetromino;
        }
    }
}
