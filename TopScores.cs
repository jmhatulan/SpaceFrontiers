using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game5
{
    internal class TopScores
    {
        //Class Properties
        private string[,] leaderboardData = new string[10, 2];
        private string[] textFileLines = new string[10];

        //Variables for Class Methods
        int tempNum;
        string tempString;
        string[] tempArray;

        public string[,] LeaderboardData
        {
            get { return leaderboardData; }
        }
        public void GetTopScores()
        //This method will modify the leaderboardData property based on the data from the text file.
        {
            //This will create the leaderboard file without content if the file doesn't exist.
            if (File.Exists("leaderboard.txt") == false)
            {
                StreamWriter sw = new StreamWriter("leaderboard.txt");
                sw.Close();
            }
            //This portion of the method will populate the tempArray with string data from the text file.
            using (StreamReader sr = new StreamReader("leaderboard.txt"))
            {
                string line; int i = 0;
                while ((line = sr.ReadLine()) != null)
                { textFileLines[i] = line; i++; }
            }

            //This portion of the method will transfer the data into a 2D array.
            for (int i = 0; i < 10; i++)
            {
                if (textFileLines[i] == null)
                {
                    leaderboardData[i, 0] = "--------";
                    leaderboardData[i, 1] = "00000";
                }
                else
                {
                    tempArray = textFileLines[i].Split(';');
                    leaderboardData[i, 0] = tempArray[0];
                    leaderboardData[i, 1] = tempArray[1];
                }
            }
        }
        public void CompareCurrentScore(string playerName, int score)
        //This method will compare the score acquired from the Survival Mode to the current Top Scores
        {
            //This portion of the method will compare current score to the scores from the leaderboardData.
            for (int i = 0; i < 10; i++)
            {
                if (score >= int.Parse(leaderboardData[i, 1]))
                { tempNum = i; UpdateScoreArrays(playerName, score); break; }
            }
        }
        public void UpdateScoreArrays(string playerName, int score)
        {
            //This will move lower scores to its next index.
            for (int i = 9; i > tempNum; i--)
            {
                leaderboardData[i, 0] = leaderboardData[(i - 1), 0];
                leaderboardData[i, 1] = leaderboardData[(i - 1), 1];
            }

            //This will encode the player name and acquired score on its appopriate place on the leaderboard.
            leaderboardData[tempNum, 0] = playerName;
            leaderboardData[tempNum, 1] = score.ToString();

            //This for loop will update the textFileLines array.
            for (int i = 0; i < 10; i++)
            {
                if ((leaderboardData[i, 0]) != "--------")
                {
                    tempString = (leaderboardData[i, 0] + ";" + leaderboardData[i, 1]);
                    textFileLines[i] = tempString;
                }
                else { break; }
            }
            UpdateScoreFile();
        }
        public void UpdateScoreFile()
        //This method will overwrite the text file if any changes occurs in the arrays.
        {
            using (StreamWriter sw = new StreamWriter("leaderboard.txt"))
            {
                for (int i = 0; i < 10; i++)
                {
                    if (textFileLines[i] == null)
                    { break; }
                    else
                    { sw.WriteLine(textFileLines[i]); }
                }
            }
        }
    }
}
