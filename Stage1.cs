using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game5
{
    internal class Stage1 : Form
    {
        private PlayerSKills playerSkills = new PlayerSKills();
        public Bitmap hp10 = new Bitmap(Path.Combine(Application.StartupPath, "HP-stateA.png"));
        protected Player player;
        protected PlayerMovement playerMovement;
        public SoundPlayer gameOverSoundEffect = new SoundPlayer("Arcade game over sound effect!.wav");
        public SoundPlayer stage1SoundPlayer = new SoundPlayer("Space Invaders - Space Invaders.wav");
        private Bitmap playerDeadBitmap = new Bitmap(Path.Combine(Application.StartupPath, "GameOverExplosion.gif"));
        private Bitmap meteoriteBitmap = new Bitmap(Path.Combine(Application.StartupPath, "meteor.png"));

        List<PictureBox> ObstacleList = new List<PictureBox>();

        public Stopwatch gameStopWatch = new Stopwatch();
        Timer mainTimer;

        private PictureBox hpBar = new PictureBox()
        {
            Size = new Size(150, 30),
            SizeMode = PictureBoxSizeMode.StretchImage,
            Location = new Point(0, 0),
        };


        public Stage1()
        {
            Text = "Stage 1";
            CreateMenu();
            menuForm.Visible = false;
            this.KeyUp += new KeyEventHandler(KeyUpEvent);

            Size = new Size(1280, 720);
            StartPosition = FormStartPosition.CenterScreen;
            player = new Player();
            gameStopWatch.Start();
            BackColor = Color.MidnightBlue;
            
            this.Controls.Add(player);
            playerMovement = new PlayerMovement(this);
            stage1SoundPlayer.PlayLooping();

            this.Controls.Add(hpBar);
            hpBar.BringToFront();



            mainTimer = new Timer();
            mainTimer.Interval = 20;
            mainTimer.Tick += new EventHandler(MainGame_Tick);
            mainTimer.Start();

        }
        long nextObstacleSpawn = 0;
        private void MainGame_Tick(object sender, EventArgs e)
        {
            hpBar.Image = playerSkills.UpdateHPBar(this, player.HP, player.MaxHP);
            playerMovement.MovePlayer(player, player.Speed);

            if (gameStopWatch.ElapsedMilliseconds >= nextObstacleSpawn)
            {
                AddObstacle();
                nextObstacleSpawn = gameStopWatch.ElapsedMilliseconds + 200;
            }

            if (gameStopWatch.ElapsedMilliseconds >= 30000)
            {
                HandleAllWaveOver();
            }
            CheckCollision();
            HandleGameOver();
        }

        protected void CheckCollision()
        {
            foreach (PictureBox obstacle in ObstacleList.ToArray())
            {
                obstacle.Left -= 20;
                if (obstacle.Left <= 0)
                {
                    ObstacleList.Remove(obstacle);
                    obstacle.Dispose();
                }
                if (obstacle.Bounds.IntersectsWith(player.Bounds))
                {
                    ObstacleList.Remove(obstacle);
                    obstacle.Dispose();
                    player.HP -= 5;
                }
            }
        }
        public void HandleAllWaveOver()
        {
            StopAllTimers();
            //Remove all bullets, enemy bullets, and enemies         
            MessageBox.Show("Stage Cleared!");
            stage1SoundPlayer.Stop();
            Hide();
            AnotherForm anotherForm = new AnotherForm();
            anotherForm.ShowDialog();
        }

        public void StopAllTimers()
        {
            gameStopWatch.Stop();
            mainTimer.Stop();
        }


        public void HandleGameOver()
        {
            if (player.HP <= 0)
            {
                hpBar.Image = playerSkills.UpdateHPBar(this, player.HP, player.MaxHP);
                StopAllTimers();
                player.Image = playerDeadBitmap;
                gameOverSoundEffect.PlaySync(); // Play the sound synchronously
                gameOverSoundEffect.Stop();
                MessageBox.Show("Game Over");
                Hide();
                AnotherForm anotherForm = new AnotherForm();
                anotherForm.ShowDialog();
                stage1SoundPlayer.Stop();
            }
        }


        private void AddObstacle()
        {
            PictureBox obstaclePictureBox = new PictureBox
            {
                Size = new Size(20, 20),
                //BackColor = Color.Blue,
                Tag = "obstacle",
                Image = meteoriteBitmap,
                SizeMode = PictureBoxSizeMode.StretchImage,
            };

            Random random = new Random();
            int obstacleY = random.Next(0, (int)(this.ClientSize.Height - obstaclePictureBox.Height));
            obstaclePictureBox.Location = new Point((int)(this.ClientSize.Width - obstaclePictureBox.Width), obstacleY);

            ObstacleList.Add(obstaclePictureBox);
            this.Controls.Add(obstaclePictureBox);
        }

        public bool GamePaused = false;
        public Panel menuForm = new Panel();
        private void KeyUpEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && GamePaused == false)
            {
                OpenMenu();
            }
            else if (e.KeyCode == Keys.Escape && GamePaused == true)
            {
                CloseMenu();
            }
        }

        private void CreateMenu()
        {
            menuForm.Text = "Menu";
            menuForm.Size = new Size(1280, 720);
            menuForm.BackColor = Color.Gray;
            menuForm.Location = new Point(0, 0);
            this.Controls.Add(menuForm);

            Button resumeButton = new Button();
            resumeButton.Text = "Resume";
            resumeButton.Font = new Font("Arial", 10, FontStyle.Bold);
            resumeButton.Size = new Size(200, 100);
            resumeButton.Click += CloseMenu;
            resumeButton.Location = new Point(540, 210);
            resumeButton.BackColor = Color.White;
            menuForm.Controls.Add(resumeButton);

            Button menuButton = new Button();
            menuButton.Text = "Main Menu";
            menuButton.Font = new Font("Arial", 10, FontStyle.Bold);
            menuButton.Size = new Size(200, 100);
            menuButton.Click += MainMenu;
            menuButton.Location = new Point(540, 310);
            menuButton.BackColor = Color.White;
            menuForm.Controls.Add(menuButton);


        }

        public void OpenMenu()
        {
            GamePaused = true;
            mainTimer.Enabled = false;
            menuForm.Visible = true;
            menuForm.Enabled = true;
            menuForm.BringToFront();
            gameStopWatch.Stop();
        }
        public void CloseMenu()
        {
            GamePaused = false;
            mainTimer.Enabled = true;
            menuForm.Enabled = false;
            menuForm.Visible = false;
            gameStopWatch.Start();
            this.Focus();
        }
        public void CloseMenu(object sender, EventArgs e)
        {
            CloseMenu();
        }
        public void MainMenu(object sender, EventArgs e)
        {
            StopAllTimers();
            //Remove all bullets, enemy bullets, and enemies

            stage1SoundPlayer.Stop();

            Hide();
            AnotherForm anotherForm = new AnotherForm();
            anotherForm.ShowDialog();
        }
    }

}
