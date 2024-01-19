using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game5
{
    internal class Program
    {
        static void Main()
        {
            MyForm myForm = new MyForm();
            while (!myForm.IsDisposed)
            {
                Application.Run(myForm);
            }
        }
    }

    public class MyForm : Form
    {
        private Button btnStory;
        private Button btnEndless;
        private Button btnExit;
        private Button btnLeaderboard;
        private Button btnArmory;
        private string hex2 = "#32CD32";
        private Label welcomeLabel;

        public SoundPlayer menuSoundPlayer = new SoundPlayer("Terraria Music - Day.wav");
        private Bitmap menuBG = new Bitmap(Path.Combine(Application.StartupPath, "spaceBG.gif"));

        public MyForm()
        {
            InitializeComponents();
            Size = new Size(1280, 720);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Space Frontier";
            BackgroundImage = menuBG;
        }
        #region MAIN_MENU_FORM

        private void InitializeComponents()
        {
            Color lime = ColorTranslator.FromHtml(hex2);

            // Create the Story button
            btnStory = new Button()
            {
                Text = "Start",
                Location = new Point(480, 350),
                Size = new Size(300, 50),
            };
            btnStory.Click += new EventHandler(btnStart_Click);

            // Create Endless Mode button
            btnEndless = new Button()
            {
                Text = "Survival Mode",
                Location = new Point(480, 400),
                Size = new Size(300, 50),
            };
            btnEndless.Click += new EventHandler(btnEndless_Click);

            // Create Armory button
            btnArmory = new Button()
            {
                Text = "Armory",
                Location = new Point(480, 450),
                Size = new Size(300, 50),
            };
            btnArmory.Click += new EventHandler(btnArmory_Click);

            // Create Leaderboard button
            btnLeaderboard = new Button()
            {
                Text = "Leaderboard",
                Location = new Point(480, 500),
                Size = new Size(300, 50),
            };
            btnLeaderboard.Click += new EventHandler(btnLeaderboard_Click);
            // Create the Exit button
            btnExit = new Button()
            {
                Text = "Exit",
                Location = new Point(480, 550),
                Size = new Size(300, 50),
            };

            btnExit.Click += new EventHandler(btnExit_Click);
            // Add buttons to the form
            Controls.Add(btnStory);
            Controls.Add(btnEndless);
            Controls.Add(btnArmory);
            Controls.Add(btnLeaderboard);
            Controls.Add(btnExit);

            welcomeLabel = new Label()
            {
                Text = "Space Frontier",
                Location = new Point(470, 200),
                ForeColor = Color.LightSteelBlue,
                Font = new Font("Tahoma", 30, FontStyle.Bold),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
            };

            Controls.Add(welcomeLabel);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // Hide the current form
            Hide();

            // Show the new form with 6 buttons
            ShowAnotherForm();
            
        }

        private void btnEndless_Click(object sender, EventArgs e)
        {
            Hide();
            SurvivalMode sMode = new SurvivalMode();
            sMode.ShowDialog();

        }

        private void btnArmory_Click(object sender, EventArgs e)
        {
            ArmoryWinForm armory = new ArmoryWinForm();
            armory.Show();
            Hide();
        }
        private void btnLeaderboard_Click(object sender, EventArgs e)
        {
            Leaderboard leaderboard = new Leaderboard();
            leaderboard.Show();
            Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ShowAnotherForm()
        {
            // Create an instance of the new form
            AnotherForm anotherForm = new AnotherForm();

            // Show the new form
            anotherForm.Show();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Check if the form is being closed by the user
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Display a confirmation dialog
                DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    // If "No" is clicked, cancel the form closing
                    e.Cancel = true;
                }
            }
        }
        #endregion
    }

    public class AnotherForm : Form
    {
        private Button btn1;
        private Button btn2;
        private Button btn3;
        private Button btn4;
        private Button btn5;
        private Button btn6;
        private Button returnToMenu;

        private Label selectStage;
        private Label lbl1;
        private Label lbl2;
        private Label lbl3;
        private Label lbl4;
        private Label lbl5;
        private Label lbl6;


        private Bitmap bg = new Bitmap(Path.Combine(Application.StartupPath, "selectskillBG.jpg"));
        
        private string hex2 = "#32CD32";
        string currentStory;
        private string hex = "#301934";
        
        StoryWinForm story;
        public AnotherForm()
        {
            InitializeComponents();
            Size = new Size(1280, 720);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Stage Select";
            BackgroundImage = bg;
            
        }

        private void InitializeComponents()
        {
            Color darkPurple = ColorTranslator.FromHtml(hex);
            if (!(File.Exists("CurrentStory.txt")))
            {
                using (StreamWriter writer = new StreamWriter("CurrentStory.txt"))
                {
                    writer.WriteLine(1);
                };
            }
            try
            {
                using (StreamReader read = new StreamReader("CurrentStory.txt"))
                {
                    currentStory = read.ReadLine();
                }

                Color lime = ColorTranslator.FromHtml(hex2);

                btn1 = new Button()
                {
                    Text = "1",
                    Location = new Point(70, 120),
                    Size = new Size(170, 400),
                    ForeColor = Color.White,
                    BackColor = darkPurple,
                };
                Controls.Add(btn1);

                btn2 = new Button()
                {
                    Text = "2",
                    Location = new Point(260, 120),
                    Size = new Size(170, 400),
                    ForeColor = Color.White,
                    BackColor = darkPurple,
                    Enabled = false
                };
                Controls.Add(btn2);



                btn3 = new Button()
                {
                    Text = "3",
                    Location = new Point(450, 120),
                    Size = new Size(170, 400),
                    ForeColor = Color.White,
                    BackColor = darkPurple,
                    Enabled = false
                };
                Controls.Add(btn3);

                btn4 = new Button()
                {
                    Text = "4",
                    Location = new Point(640, 120),
                    Size = new Size(170, 400),
                    ForeColor = Color.White,
                    BackColor = darkPurple,
                    Enabled = false
                };
                Controls.Add(btn4);

                btn5 = new Button()
                {
                    Text = "5",
                    Location = new Point(830, 120),
                    Size = new Size(170, 400),
                    ForeColor = Color.White,
                    BackColor = darkPurple,
                    Enabled = false
                };
                Controls.Add(btn5);

                btn6 = new Button()
                {
                    Text = "6",
                    Location = new Point(1020, 120),
                    Size = new Size(170, 400),
                    ForeColor = Color.White,
                    BackColor = darkPurple,
                    Enabled = false
                };
                Controls.Add(btn6);

                returnToMenu = new Button()
                {
                    Text = "Back to Menu",
                    Location = new Point(5, 5),
                    Size = new Size(100, 20),
                };
                Controls.Add(returnToMenu);

                selectStage = new Label()
                {
                    Text = "Select Stage",
                    Location = new Point(500, 50),
                    ForeColor = lime,
                    BackColor = Color.Transparent,
                    Font = new Font("Tahoma", 30, FontStyle.Bold),
                    AutoSize = true,
                };
                Controls.Add(selectStage);

                lbl1 = new Label()
                {
                    Text = "Stage 1",
                    Location = new Point(100, 530),
                    ForeColor = lime,
                    BackColor = Color.Transparent,
                    Font = new Font("Tahoma", 20, FontStyle.Bold),
                    AutoSize = true,
                };
                Controls.Add(lbl1);

                lbl2 = new Label()
                {
                    Text = "Stage 2",
                    Location = new Point(290, 530),
                    ForeColor = lime,
                    BackColor = Color.Transparent,
                    Font = new Font("Tahoma", 20, FontStyle.Bold),
                    AutoSize = true,
                };
                Controls.Add(lbl2);

                lbl3 = new Label()
                {
                    Text = "Stage 3",
                    Location = new Point(480, 530),
                    ForeColor = lime,
                    BackColor = Color.Transparent,
                    Font = new Font("Tahoma", 20, FontStyle.Bold),
                    AutoSize = true,
                };
                Controls.Add(lbl3);

                lbl4 = new Label()
                {
                    Text = "Stage 4",
                    Location = new Point(670, 530),
                    ForeColor = lime,
                    BackColor = Color.Transparent,
                    Font = new Font("Tahoma", 20, FontStyle.Bold),
                    AutoSize = true,
                };
                Controls.Add(lbl4);

                lbl5 = new Label()
                {
                    Text = "Stage 5",
                    Location = new Point(860, 530),
                    ForeColor = lime,
                    BackColor = Color.Transparent,
                    Font = new Font("Tahoma", 20, FontStyle.Bold),
                    AutoSize = true,
                };
                Controls.Add(lbl5);

                lbl6 = new Label()
                {
                    Text = "Stage 6",
                    Location = new Point(1050, 530),
                    ForeColor = lime,
                    BackColor = Color.Transparent,
                    Font = new Font("Tahoma", 20, FontStyle.Bold),
                    AutoSize = true,
                };
                Controls.Add(lbl6);


                btn1.Click += new EventHandler(btn1_Click);
                btn2.Click += new EventHandler(btn2_Click);
                btn3.Click += new EventHandler(btn3_Click);
                btn4.Click += new EventHandler(btn4_Click);
                btn5.Click += new EventHandler(btn5_Click);
                btn6.Click += new EventHandler(btn6_Click);
                returnToMenu.Click += new EventHandler(ExitStageSelection);


                switch (currentStory)
                {
                    case "2":
                        btn2.Enabled = true;
                        break;

                    case "3":
                        btn2.Enabled = true;
                        btn3.Enabled = true;
                        break;

                    case "4":
                        btn2.Enabled = true;
                        btn3.Enabled = true;
                        btn4.Enabled = true;
                        break;

                    case "5":
                        btn2.Enabled = true;
                        btn3.Enabled = true;
                        btn4.Enabled = true;
                        btn5.Enabled = true;
                        break;

                    case "6":
                        btn2.Enabled = true;
                        btn3.Enabled = true;
                        btn4.Enabled = true;
                        btn5.Enabled = true;
                        btn6.Enabled = true;
                        break;
                }
            }
            catch (FileNotFoundException)
            {

            }
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            string text = "";
            Hide();
            using (StreamReader reader = new StreamReader("CurrentStory.txt"))
            {
                text = reader.ReadLine();
            };

            if (text == "1")
            {
                using (StreamWriter writer = new StreamWriter("CurrentStory.txt"))
                {
                    writer.WriteLine(2);
                };
            }

            story = new StoryWinForm("sm-stage1.txt");
            story.ShowDialog();

            Stage1 stage1 = new Stage1();
            stage1.ShowDialog();
        }
        private void btn2_Click(object sender, EventArgs e)
        {
            string text = "";
            Hide();
            using (StreamReader reader = new StreamReader("CurrentStory.txt"))
            {
                text = reader.ReadLine();
            };

            if (text == "2")
            {
                using (StreamWriter writer = new StreamWriter("CurrentStory.txt"))
                {
                    writer.WriteLine(3);
                };
            }

            story = new StoryWinForm("sm-stage2.txt");
            story.ShowDialog();

            Stage2 stage2 = new Stage2();
            stage2.ShowDialog();
        }
        private void btn3_Click(object sender, EventArgs e)
        {
            string text = "";
            Hide();
            using (StreamReader reader = new StreamReader("CurrentStory.txt"))
            {
                text = reader.ReadLine();
            };

            if (text == "3")
            {
                using (StreamWriter writer = new StreamWriter("CurrentStory.txt"))
                {
                    writer.WriteLine(4);
                };
            }

            story = new StoryWinForm("sm-stage3.txt");
            story.ShowDialog();

            Stage3 stage3 = new Stage3();
            stage3.ShowDialog();
        }
        private void btn4_Click(object sender, EventArgs e)
        {
            string text = "";
            Hide();
            using (StreamReader reader = new StreamReader("CurrentStory.txt"))
            {
                text = reader.ReadLine();
            };

            if (text == "4")
            {
                using (StreamWriter writer = new StreamWriter("CurrentStory.txt"))
                {
                    writer.WriteLine(5);
                };
            }

            story = new StoryWinForm("sm-stage4.txt");
            story.ShowDialog();

            Stage4 stage4 = new Stage4();
            stage4.ShowDialog();
        }
        private void btn5_Click(object sender, EventArgs e)
        {
            string text = "";
            Hide();
            using (StreamReader reader = new StreamReader("CurrentStory.txt"))
            {
                text = reader.ReadLine();
            };

            if (text == "5")
            {
                using (StreamWriter writer = new StreamWriter("CurrentStory.txt"))
                {
                    writer.WriteLine(6);
                };
            }

            story = new StoryWinForm("sm-stage5.txt");
            story.ShowDialog();

            Stage5 stage5 = new Stage5();
            stage5.ShowDialog();
        }
        private void btn6_Click(object sender, EventArgs e)
        {
            Hide();

            story = new StoryWinForm("sm-stage6.txt");
            story.ShowDialog();

            Stage6 stage6 = new Stage6();
            stage6.ShowDialog();
        }
        private void ExitStageSelection(object sender, EventArgs e)
        {
            Hide();
            Close();
            MyForm mf = new MyForm();
            mf.Show();
        }
    }
}
