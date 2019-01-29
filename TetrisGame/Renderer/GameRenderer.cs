using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TetrisGame
{
    public class GameRenderer
    {
        private GameField gameSurfaceInfo;
        private Grid gameSurfaceVisual;
        private Grid nextShapeSurfaceVisual;

        internal Dispatcher Dispatcher
        {
            get
            {
                return this.gameSurfaceVisual != null ? this.gameSurfaceVisual.Dispatcher : null;
            }
        }

        public GameRenderer(Grid renderSurfaceVisual, Grid nextPieceSurfaceVisual, GameField renderSurfaceInfo)
        {
            this.gameSurfaceInfo = renderSurfaceInfo;
            this.nextShapeSurfaceVisual = nextPieceSurfaceVisual;
            this.gameSurfaceVisual = renderSurfaceVisual;

            SetupGameSurfaceVisual(this.gameSurfaceVisual, 
                this.gameSurfaceInfo.Rows, this.gameSurfaceInfo.Columns,
                this.gameSurfaceInfo.Width, this.gameSurfaceInfo.Height,
                this.gameSurfaceInfo.CellHeight, this.gameSurfaceInfo.CellWidth);
        }

        public void RenderGameField()
        {
            this.gameSurfaceVisual.Children.Clear();
            this.RenderGridLines();
            this.RenderShapes();
        }

        private void RenderShapes()
        {
            int rows = this.gameSurfaceInfo.FieldMatrix.GetLength(0);
            int columns = this.gameSurfaceInfo.FieldMatrix.GetLength(1);

            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < columns; columnIndex++)
                {
                    var color = this.gameSurfaceInfo.FieldMatrix[rowIndex, columnIndex];
                    if (color != null)
                    {
                        this.RenderBlock(columnIndex, rowIndex, color);
                    }

                }
            }
        }

        public void RenderBlock(int columnIndex, int rowIndex, Brush brush)
        {
            FrameworkElement block = CreateBlockVisual(columnIndex, rowIndex, brush);
            this.gameSurfaceVisual.Children.Add(block);
        }

        private void RenderGridLines()
        {
            double cellWidth = this.gameSurfaceInfo.CellWidth;
            double cellHeight = this.gameSurfaceInfo.CellHeight;
            var gridLinesPanel = new Canvas();
            Brush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E8EC"));
            for (double linePosition = cellWidth; linePosition <= this.gameSurfaceInfo.Width - cellWidth; linePosition += cellWidth)
            {
                var verticalLine = new Line();
                verticalLine.X1 = linePosition;
                verticalLine.X2 = linePosition;
                verticalLine.Y1 = 0;
                verticalLine.Y2 = this.gameSurfaceInfo.Height;
                verticalLine.Stroke = brush;
                verticalLine.StrokeThickness = 1;
                gridLinesPanel.Children.Add(verticalLine);
            }

            for (double linePosition = cellHeight; linePosition <= this.gameSurfaceInfo.Height - cellHeight; linePosition += cellHeight)
            {
                var horizontalLine = new Line();
                horizontalLine.X1 = 0;
                horizontalLine.X2 = this.gameSurfaceInfo.Width;
                horizontalLine.Y1 = linePosition;
                horizontalLine.Y2 = linePosition;
                horizontalLine.Stroke = brush;
                horizontalLine.StrokeThickness = 1;
                gridLinesPanel.Children.Add(horizontalLine);
            }

            this.gameSurfaceVisual.Children.Insert(0, gridLinesPanel);
        }        

        public void RenderNextShape(bool[,] shapeBodyMatrix, Brush brush)
        {
            this.nextShapeSurfaceVisual.Children.Clear();

            int rows = shapeBodyMatrix.GetLength(0);
            int columns = shapeBodyMatrix.GetLength(1);
            double width = this.gameSurfaceInfo.CellWidth * columns;
            double height = this.gameSurfaceInfo.CellHeight * rows;

            SetupGameSurfaceVisual(this.nextShapeSurfaceVisual, rows, columns, width, height,
               this.gameSurfaceInfo.CellWidth, this.gameSurfaceInfo.CellHeight);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (shapeBodyMatrix[row, col])
                    {
                        this.nextShapeSurfaceVisual.Children.Add(CreateBlockVisual(col, row, brush));
                    }
                }
            }
        }

        internal void ClearGameField()
        {
            
        }

        private static FrameworkElement CreateBlockVisual(int x, int y, Brush brush)
        {
            Border visual = new Border();
            visual.Background = brush;
            visual.BorderBrush = Brushes.Black;
            visual.BorderThickness = new Thickness(1);
            Grid.SetColumn(visual, x);
            Grid.SetRow(visual, y);
            return visual;
        }

        private static void SetupGameSurfaceVisual(Grid visual, int rows, int columns, double totalWidth,
            double totalHeight, double cellWidth, double cellHeight)
        {
            visual.Width = totalWidth;
            visual.Height = totalHeight;

            for (int row = 0; row < rows; row++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(cellHeight);
                visual.RowDefinitions.Add(rowDefinition);
            }

            for (int column = 0; column < columns; column++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = new GridLength(cellWidth);
                visual.ColumnDefinitions.Add(columnDefinition);
            }
        }
    }
}
