using System;
using System.Windows.Media;
using System.Windows.Threading;

namespace TetrisGame
{
    public partial class GameEngine
    {
        public event EventHandler<EnginePropertyChangedArgs> GamePropertyChanged;
        public event EventHandler<EventArgs> GameEnded;

        private const int movementSpeed = 1;
        private const int frameRenderingInterval = 500;

        private GameField playfieldInfo;
        private GameRenderer renderer;
        private Tetromino fallingTetromino;
        private Tetromino nextTetromino;
        private DispatcherTimer frameRenderTimer;
        private int currentScore = 0;
        private int currentLevel = 0;
        private int clearedLinesCount = 0;
        private int highscore = 0;
        private GameState gameState = GameState.Ended;
        
        public int CurrentLevel
        {
            get { return this.currentLevel; }
            private set
            {
                this.currentLevel = value;
                this.RaisePropertyChangedEvent("CurrentLevel", this.currentLevel);
            }
        }

        public int CurrentScore
        {
            get { return this.currentScore; }
            private set
            {
                this.currentScore = value;
                this.RaisePropertyChangedEvent("CurrentScore", this.currentScore);
            }
        }

        public int Highscore
        {
            get { return this.highscore; }
            private set
            {
                this.highscore = value;
                this.RaisePropertyChangedEvent("Highscore", this.highscore);
            }
        }

        public int ClearedLinesCount
        {
            get { return this.clearedLinesCount; }
            private set
            {
                this.clearedLinesCount = value;
                this.RaisePropertyChangedEvent("ClearedLinesCount", this.clearedLinesCount);
            }
        }

        public GameEngine(GameField playfieldInfo, GameRenderer renderer)
        {
            this.playfieldInfo = playfieldInfo;
            this.renderer = renderer;
            this.frameRenderTimer = new DispatcherTimer();
            this.frameRenderTimer.Interval = TimeSpan.FromMilliseconds(frameRenderingInterval);
            this.frameRenderTimer.Tick += OnFrameRenderTimerTick;

            this.highscore = ScoringSystem.GetHighscore();

            this.InitializeEndGameAnimationWorker();

            this.endGameBlocksBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8FC461"));
            this.endGameBlocksBrush.Freeze();

            this.renderer.RenderGameField();
        }

        private void OnFrameRenderTimerTick(object sender, EventArgs e)
        {
            if (this.frameRenderTimer.IsEnabled && this.fallingTetromino != null)
            {
                this.MoveTetromino(MovementDirection.Down);
            }
        }

        private void DropNewTetromino()
        {
            if (this.TryDropNewTetromino())
            {
                TetrominoMover.UpdateField(this.fallingTetromino, this.playfieldInfo.FieldMatrix);

                this.renderer.RenderGameField();
                this.renderer.RenderNextShape(TetrominoPositionMatricesCache.GetTetrominoMatrix(this.nextTetromino.ShapeType), this.nextTetromino.Brush);
            }
            else
            {
                this.EndGame();
            }
        }

        public void StartNewGame()
        {
            if (this.endGameAnimationWorker.IsBusy)
            {
                this.endGameAnimationWorker.CancelAsync();
            }
            this.playfieldInfo.Clear();
            this.DropNewTetromino();
            this.gameState = GameState.Running;
            this.frameRenderTimer.Start();
        }

        public void PauseGame()
        {
            this.gameState = GameState.Paused;
        }

        public void ResumeGame()
        {
            this.gameState = GameState.Running;
        }

        public void EndGame()
        {
            if ((this.gameState == GameState.Running || this.gameState == GameState.Paused) && !this.endGameAnimationWorker.IsBusy)
            {
                this.frameRenderTimer.Stop();
                ScoringSystem.SaveHighscore(this.highscore);
                this.gameState = GameState.Ended;
                this.RaiseGameEndedEvent();
                this.endGameAnimationWorker.RunWorkerAsync();
            }
        }

        private bool TryDropNewTetromino()
        {
            Tetromino tetromino = this.nextTetromino != null ? this.nextTetromino : TetrominoGenerator.GenerateRandomTetromino(new PositionInfo(3, 0));
            this.nextTetromino = TetrominoGenerator.GenerateRandomTetromino(new PositionInfo(3, 0));
           
            var movingContext = TetrominoMover.GetMoveContext(tetromino, MovementDirection.Down, movementSpeed);
            if (!CollisionDetector.CollisionDetected(tetromino, this.playfieldInfo.FieldMatrix, movingContext.Positions))
            {
                this.fallingTetromino = tetromino;
                return true;
            }
            return false;
        }

        public void MoveTetromino(MovementDirection direction)
        {
            if (this.fallingTetromino != null && this.gameState == GameState.Running)
            {
                var movingContext = TetrominoMover.GetMoveContext(fallingTetromino, direction, movementSpeed);
                if (!CollisionDetector.CollisionDetected(this.fallingTetromino, this.playfieldInfo.FieldMatrix, movingContext.Positions))
                {
                    TetrominoMover.Move(this.fallingTetromino, this.playfieldInfo.FieldMatrix, movingContext);
                    this.renderer.RenderGameField();
                }
                else
                {
                    if (direction == MovementDirection.Down)
                    {
                        int clearedRowsCount = this.playfieldInfo.ClearFullRows();

                        this.ClearedLinesCount += clearedRowsCount;
                        if (this.clearedLinesCount >= ((this.currentLevel * 10) + 10))
                        {
                            this.CurrentLevel++;

                            int currentTimerInterval = this.frameRenderTimer.Interval.Milliseconds;
                            this.frameRenderTimer.Interval = TimeSpan.FromMilliseconds(currentTimerInterval - 50);
                        }
                        

                        this.AddToScore(ScoringSystem.GetScore(this.currentLevel, clearedRowsCount));
                        this.renderer.RenderGameField();
                        this.DropNewTetromino();
                    }
                }
            }
        }

        public void AddToScore(int score)
        {
            this.CurrentScore += score;
            if (this.currentScore > this.highscore)
            {
                this.Highscore = this.currentScore;
            }
            
        }

        public void RotateTetromino(bool isCounterClockwise = false)
        {
            if (this.fallingTetromino.ShapeType != TetrominoShapeType.O && this.gameState == GameState.Running)
            {
                MoveContext rotationContext = TetrominoMover.GetRotationContext(this.fallingTetromino, isCounterClockwise);

                if (!CollisionDetector.CollisionDetected(this.fallingTetromino, this.playfieldInfo.FieldMatrix, rotationContext.Positions))
                {
                    TetrominoMover.Rotate(this.fallingTetromino, this.playfieldInfo.FieldMatrix, rotationContext);
                    this.renderer.RenderGameField();
                }
            }
        }

        private void RaisePropertyChangedEvent(string propertyName, object newValue)
        {
            if (GamePropertyChanged != null)
            {
                this.GamePropertyChanged(this, new EnginePropertyChangedArgs(propertyName, newValue));
            }
        }

        private void RaiseGameEndedEvent()
        {
            if (GameEnded != null)
            {
                this.GameEnded(this, new EventArgs());
            }
        }
    }
}
