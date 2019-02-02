using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using TetrisApp;

namespace Tetris
{
    public partial class MainWindow : Window
    {
        private const double EndGameWindowHorizontalOffset = 6;
        private const double EndGameWindowVerticalOffset = 10;

        public MainWindow()
        {
            InitializeComponent();
            var model = new TetrisGameViewModel(this.renderSurface, this.nextShapeRenderSurface);
            model.GameEngine.EndGameAnimationCompleted += GameEngine_EndGameAnimationCompleted;
            this.DataContext = model;
        }

        private void GameEngine_EndGameAnimationCompleted(object sender, EventArgs e)
        {
        }
        
    }
}
