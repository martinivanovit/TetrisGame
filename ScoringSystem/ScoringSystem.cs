using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tetris
{
    public static class ScoringSystem
    {
        private static List<int> initialScoresPerNumberOfLines = new List<int>() { 40, 100, 300, 1200 };

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
            using (StreamReader reader = new StreamReader("../../Resources/HighscoreData.txt"))
            {
                string highscoreString = reader.ReadToEnd().Trim();               
                int.TryParse(highscoreString, out highscore);
            }
            return highscore;
        }

        public static void SaveHighscore(int highscore)
        {
            using (StreamWriter writer = new StreamWriter("../../Resources/HighscoreData.txt", false))
            {
                writer.WriteLine(highscore);
            }
        }
    }
}
