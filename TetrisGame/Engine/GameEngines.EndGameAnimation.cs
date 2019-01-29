using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Media;

namespace TetrisGame
{
    public partial class GameEngine 
    {
        private Brush endGameBlocksBrush;
        private BackgroundWorker endGameAnimationWorker;
        public event EventHandler<EventArgs> EndGameAnimationCompleted;

        private void InitializeEndGameAnimationWorker()
        {
            this.endGameAnimationWorker = new BackgroundWorker();
            this.endGameAnimationWorker.WorkerSupportsCancellation = true;
            this.endGameAnimationWorker.DoWork += EndGameAnimationWorker_DoWork;
            this.endGameAnimationWorker.RunWorkerCompleted += EndGameAnimationWorker_RunWorkerCompleted;
        }

        private void EndGameAnimationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int rows = this.playfieldInfo.FieldMatrix.GetLength(0);
            int columns = this.playfieldInfo.FieldMatrix.GetLength(1);

            for (int i = rows - 1; i >= 0; i--)
            {
                for (int k = 0; k < columns; k++)
                {
                    if (this.endGameAnimationWorker.CancellationPending)
                    {
                        this.playfieldInfo.Clear();
                        e.Cancel = true;
                        return;
                    }

                    int row = i;
                    int col = k;
                    Thread.Sleep(8);

                    this.renderer.Dispatcher.BeginInvoke(new Action(() => {
                        if (this.gameState == GameState.Ended)
                        {
                            this.renderer.RenderBlock(col, row, this.endGameBlocksBrush);
                        }
                    }));
                }
            }
        }

        private void EndGameAnimationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.playfieldInfo.Clear();
            this.RaiseEndGameAnimationCompletedEvent();
        }

        private void RaiseEndGameAnimationCompletedEvent()
        {
            if (EndGameAnimationCompleted != null)
            {
                this.EndGameAnimationCompleted(this, new EventArgs());
            }
        }
    }
}
