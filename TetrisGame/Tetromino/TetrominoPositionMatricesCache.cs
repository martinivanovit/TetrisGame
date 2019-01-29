using System.Collections.Generic;

namespace TetrisGame
{
    public static class TetrominoPositionMatricesCache
    {
        private static Dictionary<TetrominoShapeType, bool[,]> shapeTypeToMatrixDict = new Dictionary<TetrominoShapeType, bool[,]>()
        {
            [TetrominoShapeType.J] = new bool[3, 3]
            {
                { true, false, false },
                { true, true, true },
                { false, false, false },
            },
            [TetrominoShapeType.L] = new bool[3, 3]
            {
                { false, false, false },
                { true, true, true },
                { true, false, false },
            },
            [TetrominoShapeType.S] = new bool[3, 3]
            {
                { false, true, true },
                { true, true, false },
                { false, false, false },
            },
            [TetrominoShapeType.Z] = new bool[3, 3]
            {
                { true, true, false },
                { false, true, true },
                { false, false, false },
            },
            [TetrominoShapeType.T] = new bool[3, 3]
            {
                { false, true, false },
                { true, true, true },
                { false, false, false },
            },
            [TetrominoShapeType.O] = new bool[3, 4]
            {
                { false, true, true, false },
                { false, true, true, false },
                { false, false, false, false },
            },
            [TetrominoShapeType.I] = new bool[4, 4]
            {
                { false, false, false, false },
                { true, true, true, true },
                { false, false, false, false },
                { false, false, false, false },
            },
        };

        public static bool[,] GetTetrominoMatrix(TetrominoShapeType shapeType)
        {
            return shapeTypeToMatrixDict[shapeType];
        } 
    }
}
