using System.Windows.Media;

namespace Tetris
{
    public class GameField
    {
        public int Rows = 24;
        public int Columns = 10;
        public double CellWidth = 20;
        public double CellHeight = 20;
        public Brush[,] FieldMatrix { get; set; }
               
        public double Width
        {
            get { return this.CellWidth * this.Columns; }
        }

        public double Height
        {
            get { return this.CellHeight * this.Rows; }
        }

        public GameField()
        {
            this.FieldMatrix = new Brush[Rows, Columns];
        }

        public int ClearFullRows()
        {
            int removedRowsCount = 0;
            int rows = this.FieldMatrix.GetLength(0);

            for (int rowIndex = rows - 1; rowIndex >= 0; rowIndex--)
            {
                while (ShouldClearRow(rowIndex))
                {
                    removedRowsCount++;
                    this.ClearRow(rowIndex);
                    this.MoveRowsDown(rowIndex - 1, 1);
                }
            }
            return removedRowsCount;
        }

        private bool ShouldClearRow(int rowIndex)
        {
            int columns = this.FieldMatrix.GetLength(1);

            for (int columnIndex = 0; columnIndex < columns; columnIndex++)
            {
                if (this.FieldMatrix[rowIndex, columnIndex] == null)
                {
                    return false;
                }
            }
            return true;
        }

        private void ClearRow(int rowIndex)
        {
            int columns = this.FieldMatrix.GetLength(1);

            for (int columnIndex = 0; columnIndex < columns; columnIndex++)
            {
                this.FieldMatrix[rowIndex, columnIndex] = null;
            }
        }

        public void Clear()
        {
            int rows = this.FieldMatrix.GetLength(0);

            for (int rowIndex = rows - 1; rowIndex >= 0; rowIndex--)
            {
                this.ClearRow(rowIndex);
            }
        }

        private void MoveRowsDown(int startRowIndex, int rowIndexOffset)
        {
            int columns = this.FieldMatrix.GetLength(1);

            for (int rowIndex = startRowIndex; rowIndex >= 0; rowIndex--)
            {
                for (int columnIndex = 0; columnIndex < columns; columnIndex++)
                {
                    if (this.FieldMatrix[rowIndex, columnIndex] != null)
                    {
                        var color = this.FieldMatrix[rowIndex, columnIndex];
                        this.FieldMatrix[rowIndex, columnIndex] = null;
                        this.FieldMatrix[rowIndex + rowIndexOffset, columnIndex] = color;
                    }
                }
            }
        }
    }
}
