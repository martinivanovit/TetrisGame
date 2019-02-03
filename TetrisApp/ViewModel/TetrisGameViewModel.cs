using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using TetrisGame;

namespace Tetris
{
    public class TetrisGameViewModel : INotifyPropertyChanged
    {
        private bool? isGameRunning = false;
        private GameEngine gameEngine;
        
        private int lines;
        private int score;
        private int highscore;
        private int level;
        private bool isEndGameInfoVisible = false;

        public ICommand StartGameCommand { get; set; }
        public ICommand EndGameCommand { get; set; }
        public ICommand MoveCommand { get; set; }
        public ICommand RotateCommand { get; set; }
        public ICommand PauseGameCommand { get; set; }
        public ICommand ResumeGameCommand { get; set; }
        public ICommand HideEndGameInfoCommand { get; set; }
       
        public int Score
        {
            get { return this.score; }
            set
            {
                if (this.score != value)
                {
                    this.score = value;
                    this.NotifyPropertyChanged("Score");
                }
            }
        }

        public int Highscore
        {
            get { return this.highscore; }
            set
            {
                if (this.highscore != value)
                {
                    this.highscore = value;
                    this.NotifyPropertyChanged("Highscore");
                }
            }
        }

        public int Lines
        {
            get { return this.lines; }
            set
            {
                if (this.lines != value)
                {
                    this.lines = value;
                    this.NotifyPropertyChanged("Lines");
                }
            }
        }

        public int Level
        {
            get { return this.level; }
            set
            {
                if (this.level != value)
                {
                    this.level = value;
                    this.NotifyPropertyChanged("Level");
                }
            }
        }

        public GameEngine GameEngine
        {
            get { return this.gameEngine;  }
        }
               
        public bool IsEndGameInfoVisible
        {
            get { return isEndGameInfoVisible; }
            set
            {
                if (this.isEndGameInfoVisible != value)
                {
                    this.isEndGameInfoVisible = value;
                    this.NotifyPropertyChanged("IsEndGameInfoVisible");
                }
            }
        }
        
        public TetrisGameViewModel(Grid gameRenderSurface, Grid nextShapeRenderSurface)
        {
            var playfield = new GameField();
            var renderer = new GameRenderer(gameRenderSurface, nextShapeRenderSurface, playfield);
            this.gameEngine = new GameEngine(playfield, renderer);
            this.gameEngine.GamePropertyChanged += GameEngine_PropertyChanged;
            this.gameEngine.GameEnded += GameEngine_GameEnded;
            this.gameEngine.EndGameAnimationCompleted += GameEngine_EndGameAnimationCompleted;

            this.StartGameCommand = new RelayCommand(OnStartGame, OnCanStartGame);
            this.EndGameCommand = new RelayCommand(OnEndGame, OnCanEndGame);
            this.PauseGameCommand = new RelayCommand(OnPause, OnCanPauseGame);
            this.ResumeGameCommand = new RelayCommand(OnResume, OnCanResumeGame);
            this.MoveCommand = new RelayCommand(OnMove);
            this.RotateCommand = new RelayCommand(OnRotate);
            this.HideEndGameInfoCommand = new RelayCommand(OnHideEndGameInfo);

            this.Highscore = ScoringSystem.GetHighscore();
        }

        private void GameEngine_EndGameAnimationCompleted(object sender, EventArgs e)
        {
            if (this.isGameRunning == false)
            {
                this.IsEndGameInfoVisible = true;
            }
        }

        private void GameEngine_GameEnded(object sender, EventArgs e)
        {
            this.isGameRunning = false;
            CommandManager.InvalidateRequerySuggested();
        }

        private bool OnCanResumeGame(object arg)
        {
            return !this.isGameRunning.HasValue;
        }

        private bool OnCanPauseGame(object arg)
        {
            return this.isGameRunning.HasValue && this.isGameRunning.Value;
        }

        private void OnResume(object obj)
        {
            this.isGameRunning = true;
            this.gameEngine.ResumeGame();
        }

        private void OnPause(object obj)
        {
            this.isGameRunning = null;
            this.gameEngine.PauseGame();

        }

        private void OnRotate(object obj)
        {
            bool isCounterClockwise = (bool)obj;
            this.gameEngine.RotateTetromino(isCounterClockwise);
        }

        private void OnMove(object obj)
        {
            MovementDirection direcion = (MovementDirection)obj;
            this.gameEngine.MoveTetromino(direcion);

            if (direcion == MovementDirection.Down)
            {
                this.gameEngine.AddToScore(1);
            }
        }

        private bool OnCanEndGame(object arg)
        {
            return !this.isGameRunning.HasValue || (this.isGameRunning.HasValue && this.isGameRunning.Value);
        }

        private void OnEndGame(object obj)
        {
            this.isGameRunning = false;
            this.gameEngine.EndGame();
        }

        private bool OnCanStartGame(object arg)
        {
            return this.isGameRunning.HasValue && !this.isGameRunning.Value;
        }

        private void OnStartGame(object obj)
        {
            this.isGameRunning = true;
            this.IsEndGameInfoVisible = false;
            this.gameEngine.StartNewGame();
        }

        private void OnHideEndGameInfo(object obj)
        {
            this.IsEndGameInfoVisible = false;
        }

        private void GameEngine_PropertyChanged(object sender, EnginePropertyChangedArgs e)
        {
            if (e.PropertyName == "CurrentScore")
            {
                this.Score = (int)e.NewValue;
            }
            else if (e.PropertyName == "Highscore")
            {
                this.Highscore = (int)e.NewValue;
            }
            else if (e.PropertyName == "ClearedLinesCount")
            {
                this.Lines = (int)e.NewValue;
            }
            else if (e.PropertyName == "CurrentLevel")
            {
                this.Level = (int)e.NewValue;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
