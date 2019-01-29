using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TetrisGame
{
    public static class ScoringSystem
    {
        private const string HighscoreFileName = "../../HighscoreData.txt";

        private static List<int> initialScoresPerNumberOfLines = new List<int>() { 40, 100, 300, 1200 };

        static ScoringSystem()
        {
            if (!File.Exists(HighscoreFileName))
            {
                var file = File.Create(HighscoreFileName);
                file.Close();
            }
        }

        public static int GetScore(int level, int numberOfLines)
        {
            if (numberOfLines < 1 || level < 0)
            {
                return 0;
            }

            int initialScore = numberOfLines <= initialScoresPerNumberOfLines.Count ? 
                initialScoresPerNumberOfLines[numberOfLines - 1] : 
                initialScoresPerNumberOfLines.Last();

            return initialScore * (level + 1);
        }

        public static int GetHighscore()
        {
            int highscore = 0;
            
            using (StreamReader reader = new StreamReader(HighscoreFileName))
            {
                string highscoreString = reader.ReadToEnd().Trim();               
                int.TryParse(highscoreString, out highscore);
            }
            return highscore;
        }

        public static void SaveHighscore(int highscore)
        {
            using (StreamWriter writer = new StreamWriter(HighscoreFileName, false))
            {
                writer.WriteLine(highscore);
            }
        }
    }
}
