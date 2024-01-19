using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game5
{
    internal class Leaderboard : Form
    {
        //Windows Form Elements Declaration
        Label top1_Score = new Label();
        Label top2_Score = new Label();
        Label top3_Score = new Label();
        Label top4_Score = new Label();
        Label top5_Score = new Label();
        Label top6_Score = new Label();
        Label top7_Score = new Label();
        Label top8_Score = new Label();
        Label top9_Score = new Label();
        Label top10_Score = new Label();

        Label top1_Name = new Label();
        Label top2_Name = new Label();
        Label top3_Name = new Label();
        Label top4_Name = new Label();
        Label top5_Name = new Label();
        Label top6_Name = new Label();
        Label top7_Name = new Label();
        Label top8_Name = new Label();
        Label top9_Name = new Label();
        Label top10_Name = new Label();

        Label heading_Main = new Label();
        Label heading_Score = new Label();
        Label heading_Name = new Label();

        Button exitButton = new Button();

        public Leaderboard()
        //Label Formatting & Placement of Windows Form Elements
        {
            this.Size = new Size(1280, 720);
            this.Text = "Leaderboard";
            this.BackColor = Color.Gray;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;

            top1_Score.Location = new Point(715, 150);
            top2_Score.Location = new Point(715, 200);
            top3_Score.Location = new Point(715, 250);
            top4_Score.Location = new Point(715, 300);
            top5_Score.Location = new Point(715, 350);
            top6_Score.Location = new Point(715, 400);
            top7_Score.Location = new Point(715, 450);
            top8_Score.Location = new Point(715, 500);
            top9_Score.Location = new Point(715, 550);
            top10_Score.Location = new Point(715, 600);

            top1_Score.Font = new Font("Arial", 12);
            top2_Score.Font = new Font("Arial", 12);
            top3_Score.Font = new Font("Arial", 12);
            top4_Score.Font = new Font("Arial", 12);
            top5_Score.Font = new Font("Arial", 12);
            top6_Score.Font = new Font("Arial", 12);
            top7_Score.Font = new Font("Arial", 12);
            top8_Score.Font = new Font("Arial", 12);
            top9_Score.Font = new Font("Arial", 12);
            top10_Score.Font = new Font("Arial", 12);

            top1_Score.TextAlign = ContentAlignment.MiddleCenter;
            top2_Score.TextAlign = ContentAlignment.MiddleCenter;
            top3_Score.TextAlign = ContentAlignment.MiddleCenter;
            top4_Score.TextAlign = ContentAlignment.MiddleCenter;
            top5_Score.TextAlign = ContentAlignment.MiddleCenter;
            top6_Score.TextAlign = ContentAlignment.MiddleCenter;
            top7_Score.TextAlign = ContentAlignment.MiddleCenter;
            top8_Score.TextAlign = ContentAlignment.MiddleCenter;
            top9_Score.TextAlign = ContentAlignment.MiddleCenter;
            top10_Score.TextAlign = ContentAlignment.MiddleCenter;

            this.Controls.Add(top1_Score);
            this.Controls.Add(top2_Score);
            this.Controls.Add(top3_Score);
            this.Controls.Add(top4_Score);
            this.Controls.Add(top5_Score);
            this.Controls.Add(top6_Score);
            this.Controls.Add(top7_Score);
            this.Controls.Add(top8_Score);
            this.Controls.Add(top9_Score);
            this.Controls.Add(top10_Score);

            top1_Name.Location = new Point(450, 150);
            top2_Name.Location = new Point(450, 200);
            top3_Name.Location = new Point(450, 250);
            top4_Name.Location = new Point(450, 300);
            top5_Name.Location = new Point(450, 350);
            top6_Name.Location = new Point(450, 400);
            top7_Name.Location = new Point(450, 450);
            top8_Name.Location = new Point(450, 500);
            top9_Name.Location = new Point(450, 550);
            top10_Name.Location = new Point(450, 600);

            top1_Name.TextAlign = ContentAlignment.MiddleCenter;
            top2_Name.TextAlign = ContentAlignment.MiddleCenter;
            top3_Name.TextAlign = ContentAlignment.MiddleCenter;
            top4_Name.TextAlign = ContentAlignment.MiddleCenter;
            top5_Name.TextAlign = ContentAlignment.MiddleCenter;
            top6_Name.TextAlign = ContentAlignment.MiddleCenter;
            top7_Name.TextAlign = ContentAlignment.MiddleCenter;
            top8_Name.TextAlign = ContentAlignment.MiddleCenter;
            top9_Name.TextAlign = ContentAlignment.MiddleCenter;
            top10_Name.TextAlign = ContentAlignment.MiddleCenter;

            top1_Name.Font = new Font("Arial", 12);
            top2_Name.Font = new Font("Arial", 12);
            top3_Name.Font = new Font("Arial", 12);
            top4_Name.Font = new Font("Arial", 12);
            top5_Name.Font = new Font("Arial", 12);
            top6_Name.Font = new Font("Arial", 12);
            top7_Name.Font = new Font("Arial", 12);
            top8_Name.Font = new Font("Arial", 12);
            top9_Name.Font = new Font("Arial", 12);
            top10_Name.Font = new Font("Arial", 12);

            this.Controls.Add(top1_Name);
            this.Controls.Add(top2_Name);
            this.Controls.Add(top3_Name);
            this.Controls.Add(top4_Name);
            this.Controls.Add(top5_Name);
            this.Controls.Add(top6_Name);
            this.Controls.Add(top7_Name);
            this.Controls.Add(top8_Name);
            this.Controls.Add(top9_Name);
            this.Controls.Add(top10_Name);

            heading_Main.Location = new Point(535, 25);
            heading_Name.Location = new Point(425, 100);
            heading_Score.Location = new Point(700, 100);

            heading_Main.TextAlign = ContentAlignment.MiddleCenter;
            heading_Name.TextAlign = ContentAlignment.MiddleCenter;
            heading_Score.TextAlign = ContentAlignment.MiddleCenter;

            heading_Main.Size = new Size(200, 25);
            heading_Name.Size = new Size(150, 25);
            heading_Score.Size = new Size(150, 25);

            heading_Main.Text = "Leaderboard";
            heading_Name.Text = "Player Names";
            heading_Score.Text = "Player Scores";

            heading_Main.Font = new Font("Arial", 20, FontStyle.Bold);
            heading_Name.Font = new Font("Arial", 15, FontStyle.Bold);
            heading_Score.Font = new Font("Arial", 15, FontStyle.Bold);

            this.Controls.Add(heading_Main);
            this.Controls.Add(heading_Name);
            this.Controls.Add(heading_Score);

            exitButton.Location = new Point(5, 5);
            exitButton.Size = new Size(100, 20);
            exitButton.Text = "Back to Menu";
            exitButton.Click += (ExitLeaderBoard);
            this.Controls.Add(exitButton);

            PopulateLeaderboard();
        }
        public void PopulateLeaderboard()
        //This class method will change the text written on the labels based on the Top Scores.
        {
            TopScores ts = new TopScores();
            ts.GetTopScores();
            top1_Name.Text = ts.LeaderboardData[0, 0];
            top1_Score.Text = ts.LeaderboardData[0, 1];
            top2_Name.Text = ts.LeaderboardData[1, 0];
            top2_Score.Text = ts.LeaderboardData[1, 1];
            top3_Name.Text = ts.LeaderboardData[2, 0];
            top3_Score.Text = ts.LeaderboardData[2, 1];
            top4_Name.Text = ts.LeaderboardData[3, 0];
            top4_Score.Text = ts.LeaderboardData[3, 1];
            top5_Name.Text = ts.LeaderboardData[4, 0];
            top5_Score.Text = ts.LeaderboardData[4, 1];
            top6_Name.Text = ts.LeaderboardData[5, 0];
            top6_Score.Text = ts.LeaderboardData[5, 1];
            top7_Name.Text = ts.LeaderboardData[6, 0];
            top7_Score.Text = ts.LeaderboardData[6, 1];
            top8_Name.Text = ts.LeaderboardData[7, 0];
            top8_Score.Text = ts.LeaderboardData[7, 1];
            top9_Name.Text = ts.LeaderboardData[8, 0];
            top9_Score.Text = ts.LeaderboardData[8, 1];
            top10_Name.Text = ts.LeaderboardData[9, 0];
            top10_Score.Text = ts.LeaderboardData[9, 1];
        }
        public void ExitLeaderBoard(object sender, EventArgs e)
        //This will execute once the exit button is clicked and will close the leaderboard form.
        {
            this.Hide();
            this.Close();
            MyForm mf = new MyForm();
            mf.Show();
        }
    }
}
