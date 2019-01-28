using System.Collections.Generic;
using System.Windows.Media;
using Tetris.Engine;

namespace Tetris
{
    public class Tetromino
    {       
        private TetrominoShapeType shapeType;
        private bool[,] bodyMatrix;
        private Brush brush;
        private PositionInfo topLeft;
        private List<PositionInfo> bodyPositions;

        public TetrominoShapeType ShapeType
        {
            get { return this.shapeType; }
        }

        public bool[,] BodyMatrix
        {
            get { return this.bodyMatrix; }
        }

        public Brush Brush
        {
            get { return this.brush; }
        }

        public PositionInfo TopLeft
        {
            get { return this.topLeft; }
        }

        public List<PositionInfo> BodyPositions
        {
            get { return this.bodyPositions; }
        }

        public Tetromino(PositionInfo topLeft, TetrominoShapeType shapeType, Brush color = null)
        {
            this.topLeft = topLeft;
            this.shapeType = shapeType;
            this.brush = color != null ? color : TetrominoBrushesCache.GetBrushByShapeType(shapeType);            
            this.bodyMatrix = TetrominoPositionMatricesCache.GetTetrominoMatrix(shapeType);
            this.bodyPositions = GetPositions(this.topLeft, this.bodyMatrix);            
        }

        public void Move(MovementDirection direction, int speed)
        {
            var moveContext = TetrominoMover.GetMoveContext(this, direction, speed);
            this.Move(moveContext);
        }

        public void Move(MoveContext moveContext)
        {
            this.topLeft = moveContext.TopLeft;
            this.bodyPositions = moveContext.Positions;
        }

        public void Rotate(MoveContext rotationContext)
        {
            this.bodyMatrix = rotationContext.BodyMatrix;
            this.bodyPositions = rotationContext.Positions;
        }

        public static List<PositionInfo> GetPositions(PositionInfo topLeft, bool[,] bodyMatrix)
        {
            List<PositionInfo> result = new List<PositionInfo>();
            for (int r = 0; r < bodyMatrix.GetLength(0); r++)
            {
                for (int c = 0; c < bodyMatrix.GetLength(1); c++)
                {
                    if (bodyMatrix[r, c])
                    {
                        result.Add(GetActualPositionByRelativePosition(topLeft, c, r));
                    }
                }
            }
            return result;
        }
       

        private static PositionInfo GetActualPositionByRelativePosition(PositionInfo topLeft, int relativeX, int relativeY)
        {
            int actualX = topLeft.X + relativeX;
            int actualY = topLeft.Y + relativeY;
            return new PositionInfo(actualX, actualY);
        }
    }
}
