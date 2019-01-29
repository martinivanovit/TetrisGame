using System.Collections.Generic;

namespace TetrisGame
{
    public class MoveContext
    {
        public bool[,] BodyMatrix { get; set; }
        public List<PositionInfo> Positions { get; set; }
        public PositionInfo TopLeft { get; set; }

        public MoveContext(PositionInfo topLeft, List<PositionInfo> positions, bool[,] bodyMatrix)
        {   
            this.TopLeft = topLeft;
            this.Positions = positions;
            this.BodyMatrix = bodyMatrix;
        }
    }
}
