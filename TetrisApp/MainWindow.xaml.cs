using System.Windows;

namespace Tetris
{
    public partial class MainWindow : Window
    {   
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new TetrisGameViewModel(this.renderSurface, this.nextShapeRenderSurface);
        }
    }
}
