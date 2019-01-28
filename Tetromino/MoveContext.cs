using System.Collections.Generic;

namespace Tetris.Engine
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
