using System.Collections.Generic;
using System.Windows.Media;

namespace TetrisGame
{
    public static class TetrominoBrushesCache
    {
        private static Dictionary<TetrominoShapeType, SolidColorBrush> shapeTypeToBrushDict = new Dictionary<TetrominoShapeType, SolidColorBrush>()
        {
            [TetrominoShapeType.J] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D23734")),
            [TetrominoShapeType.L] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A73B4")),
            [TetrominoShapeType.S] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#92832C")),
            [TetrominoShapeType.Z] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1D9079")),
            [TetrominoShapeType.T] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#446524")),
            [TetrominoShapeType.O] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#76B03E")),
            [TetrominoShapeType.I] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5A68A1")),
        };

        static TetrominoBrushesCache()
        {
            foreach (var item in shapeTypeToBrushDict)
            {
                item.Value.Freeze();
            }
        }

        public static Brush GetBrushByShapeType(TetrominoShapeType shapeType)
        {
           return shapeTypeToBrushDict[shapeType];
        }
    }
}
