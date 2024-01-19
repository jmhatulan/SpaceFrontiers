using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing.Text;
using System.Diagnostics;
using System.Media;
using Game5;

namespace Game5
{
    internal class SurvivalMode : Form
    // This class will serve as the Pre-Game Form for Survival Mode
    {
        // Windows Form Elements Declaration
        TextBox playerNameInput;
        Label playerNameLabel;
        Button continueButton;
        Button returnToMenu;

        //Variable Declaration
        private string playerName;

        GameplayLoop GL;
        public SurvivalMode()
        // Win Form Elements Formatting & Placement
        {
            this.Size = new Size(1280, 720);
            this.Text = "Survival Mode";
            this.BackColor = Color.Gray;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;

            playerNameInput = new TextBox()
            {
                Location = new Point(450, 300),
                Size = new Size(300, 50),
            };
            playerNameInput.KeyDown += new KeyEventHandler(EnterKeyDown);

            playerNameLabel = new Label()
            {
                Text = "Insert Player Name",
                Font = new Font("Arial", 20, FontStyle.Bold),
                Location = new Point(450, 250),
                Size = new Size(300, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            continueButton = new Button()
            {
                Text = "Continue",
                Location = new Point(450, 325),
                Size = new Size(300, 25),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            continueButton.Click += new EventHandler(ValidatePlayerName);

            returnToMenu = new Button()
            {
                Text = "Back to Menu",
                Location = new Point(5, 5),
                Size = new Size(100, 20),
            };
            returnToMenu.Click += new EventHandler(ExitSurvivalMode);

            this.KeyDown += new KeyEventHandler(ValidatePlayerName);

            //Add Controls
            this.Controls.Add(playerNameInput);
            this.Controls.Add(playerNameLabel);
            this.Controls.Add(continueButton);
            this.Controls.Add(returnToMenu);
        }
        private void EnterKeyDown(object sender, KeyEventArgs e)
        //This will allow the player to use enter key to submit player name input.
        {
            if (e.KeyCode == Keys.Enter)
            {
                ValidatePlayerName();
            }
        }
        private void ValidatePlayerName()
        // This will validate if the player name inputted only contains letters and has 12 or less characters.
        {
            string name;
            name = playerNameInput.Text;
            if (name == "")
            {
                DialogResult result = MessageBox.Show("Please input a player name.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (Regex.IsMatch(name, @"^[a-zA-Z]+$") == false)
            {
                DialogResult result = MessageBox.Show("Player name cannot contain numbers, spaces or special characters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (name.Length > 12)
            {
                DialogResult result = MessageBox.Show("Player name can only contain 12 or less characters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                playerName = name;

                // Gameplay Start
                this.Hide();
                this.Close();
                GL = new GameplayLoop(playerName);
                GL.ShowDialog();
            }
        }
        private void ValidatePlayerName(object sender, EventArgs e)
        // Overloaded method as an event receiver
        {
            string name;
            name = playerNameInput.Text;
            if (name == "")
            {
                DialogResult result = MessageBox.Show("Please input a player name.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (Regex.IsMatch(name, @"^[a-zA-Z]+$") == false)
            {
                DialogResult result = MessageBox.Show("Player name cannot contain numbers, spaces or special characters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (name.Length > 12)
            {
                DialogResult result = MessageBox.Show("Player name can only contain 12 or less characters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                playerName = name;

                // GameplayStart
                this.Hide();
                this.Close();
                GL = new GameplayLoop(playerName);
                GL.ShowDialog();
            }
        }
        public void ExitSurvivalMode()
        // This will exit survival mode and return to main menu
        {
            this.Hide();
            this.Close();
            MyForm mf = new MyForm();
            mf.Show();
        }
        public void ExitSurvivalMode(object sender, EventArgs e)
        // Overloaded method as an event receiver
        {
            this.Hide();
            this.Close();
            MyForm mf = new MyForm();
            mf.Show();
        }
    }
    internal class GameplayLoop : Form
    //This class is responsible for the gameplay of Survival Mode
    {
        //Variable Declaration
        private string currentPlayerName;
        private int currentPlayerScore = 0;

        //Object Declaration for Survival Mode Exclusive Mechanics
        Currency currency = new Currency();
        PlayerUpgrades pUpgrades = new PlayerUpgrades();
        TopScores tScores = new TopScores();

        // Win Forms for Survival Mode
        Label currScore;
        Label currScoreLabel;

        // Gameplay Code
        public Bitmap hp10 = new Bitmap(Path.Combine(Application.StartupPath, "HP-stateA.png"));
        public SoundPlayer survivalModeSoundPlayer = new SoundPlayer("Blue Archive - Endless Carnival.wav");
        public SoundPlayer gameOverSoundEffect = new SoundPlayer("Arcade game over sound effect!.wav");
        private Bitmap playerDeadBitmap = new Bitmap(Path.Combine(Application.StartupPath, "GameOverExplosion.gif"));

        private Bitmap medbutton = new Bitmap(Path.Combine(Application.StartupPath, "medkitbutton.png"));
        private Bitmap rapidbutton = new Bitmap(Path.Combine(Application.StartupPath, "rapidbutton.png"));
        private Bitmap boostSpeedbutton = new Bitmap(Path.Combine(Application.StartupPath, "boostspeedbutton.png"));

        private Bitmap meteoriteBitmap = new Bitmap(Path.Combine(Application.StartupPath, "meteor.png"));

        public Stopwatch gameStopWatch = new Stopwatch();

        protected List<Bullet> bulletList = new List<Bullet>();
        protected List<Enemy> enemiesList = new List<Enemy>();
        protected List<EnemyBullet> enemyBulletList = new List<EnemyBullet>();
        private List<EnemyBullet> bossBullets = new List<EnemyBullet>();

        private List<PictureBox> ObstacleList = new List<PictureBox>();

        protected Timer gameTimer;
        protected Enemy enemy = new Enemy();


        private bool quarterPointChoiceMade = false;
        private int tag = 1;

        private int type1MovementChoice = -1;
        private int type3Tag = 0;
        private Random rand = new Random();

        private bool gameOver = false;

        private long nextBulletAdd = 0;
        private long nextBulletMove = 0;
        private long nextAddEnemy = 0;
        private long nextAddEnemyBullet = 0;
        private long nextObstacleSpawn = 0;

        public int currentWave = 1;
        public int maxWave = 5;

        public int StageLevel { get; set; }
        public int EnemiesSpawned { get; set; }
        public int X { get; set; }

        public int newX = 1280;
        public int newY = 0;

        public int hp = 0;
        public bool goUp = true;
        public int mobSize = 60;

        private Enemy enemyCreated = null;
        private PlayerSKills playerSkills = new PlayerSKills();
        protected Player player;
        protected PlayerMovement playerMovement;

        public string selectedplayerSkill;
        public Button button1;
        public Button button2;
        public Button button3;
        private PictureBox hpBar = new PictureBox()
        {
            Size = new Size(150, 30),
            SizeMode = PictureBoxSizeMode.StretchImage,
            Location = new Point(0, 0),
        };
        long nextEnableButton1 = 0;
        long nextEnableButton2 = 0;
        long nextEnableButton3 = 0;
        long skillDuration1 = 0;
        long skillDuration2 = 0;
        long skillDuration3 = 0;
        long nextBossSpawn = 30000;
        long nextplayerHPdecrease = 5000;
        long nextIncreaseEnemyValue = 10000;

        private int totalEnemiesToSpawn = 5;

        private string[] enemyTypeArray = { "Type1", "Type2", "Type3" };
        private string[] bossArray = { "Boss", "Final Boss" };

        // Player Stat Modifier
        private int hpBuff;
        private int atkBuff;
        private int spdBuff;

        // Enemy Stat Modifier
        private int minDamage = 1;
        private int maxDamage = 2;

        private int minHp = 2;
        private int maxHP = 3;

        private int maxDamageMultiplier = 15;
        private int maxHpMultiplier = 15;
        public GameplayLoop(string playerName)
        {
            this.Size = new Size(1280, 720);
            this.Text = "Survival Mode";
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.MidnightBlue;
            currentPlayerName = playerName;
            // Gameplay Code
            using (StreamReader reader = new StreamReader("Skill1.txt"))
            {
                playerSkills.SelectedPlayerSkill = reader.ReadLine();
            }
            player = new Player();

            this.Controls.Add(player);
            playerMovement = new PlayerMovement(this);
            enemy = new Enemy();

            // Live Score
            currScore = new Label()
            {
                Size = new Size(100, 25),
                Location = new Point(1200, 5),
                Font = new Font("Arial", 15, FontStyle.Bold),
                AutoSize = true,
                Text = currentPlayerScore.ToString(),
                ForeColor = Color.White,
            };
            Controls.Add(currScore);
            currScoreLabel = new Label()
            {
                Size = new Size(100, 25),
                Location = new Point(1100, 5),
                Font = new Font("Arial", 15, FontStyle.Bold),
                AutoSize = true,
                Text = "Score:",
                ForeColor = Color.White,
            };
            Controls.Add(currScoreLabel);

            button1 = new Button
            {
                Text = "J",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Image = medbutton,
                Size = new Size(50, 50),
                Location = new Point(0, 31),
                BackColor = Color.White,
                TextAlign = ContentAlignment.TopRight,
            };
            button2 = new Button
            {
                Text = "K",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Image = rapidbutton,
                Size = new Size(50, 50),
                Location = new Point(50, 31),
                BackColor = Color.White,
                TextAlign = ContentAlignment.TopRight,
            };
            button3 = new Button
            {
                Text = "L",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Image = boostSpeedbutton,
                Size = new Size(50, 50),
                Location = new Point(100, 31),
                BackColor = Color.White,
                TextAlign = ContentAlignment.TopRight,
            };

            StartGameplayLoop();

            this.Controls.Add(button1);
            button1.Click += button1_Click;
            this.Controls.Add(button2);
            button2.Click += button2_Click;
            this.Controls.Add(button3);
            button3.Click += button3_Click;
            this.KeyDown += SkillKeyDownHandler;
            this.Controls.Add(hpBar);
            hpBar.BringToFront();

            survivalModeSoundPlayer.PlayLooping();
            Console.WriteLine("Button click event attached.");
            this.KeyPreview = true;

            gameStopWatch.Start();
            gameTimer = new Timer();
            gameTimer.Interval = 30; // edit to change speed of movement
            gameTimer.Tick += new EventHandler(MainGame_Tick);
            gameTimer.Start();
        }
        public void StartGameplayLoop()
        // This method will retrieve stat buff from their upgrades in the armory.
        {
            pUpgrades.RetrieveStatBuff();
            hpBuff = pUpgrades.HPBuff;
            atkBuff = pUpgrades.AtkBuff;
            spdBuff = pUpgrades.SpdBuff;

            player.MaxHP += hpBuff;
            player.BulletDamage += atkBuff;
            player.Speed += spdBuff;
            player.HP += hpBuff;
        }
        #region GAMEPLAY_RELATED CODE

        private void MainGame_Tick(object sender, EventArgs e)
        {
            Console.WriteLine(player.HP);
            hpBar.Image = playerSkills.UpdateHPBar(this, player.HP, player.MaxHP);
            playerMovement.MovePlayer(player, player.Speed, button1);
            if (gameStopWatch.ElapsedMilliseconds >= nextIncreaseEnemyValue)
            {
                // Enemy Stat Modifier
                if (minDamage <= maxDamageMultiplier)
                { minDamage += 5; }
                if (maxDamage <= maxDamageMultiplier)
                { maxDamage += 5; }
                if (minHp <= maxHpMultiplier)
                { minHp += 5; }
                if (maxHP <= maxHpMultiplier)
                { maxHP += 5; }
                nextIncreaseEnemyValue = gameStopWatch.ElapsedMilliseconds + 10000;
            }
            if (gameStopWatch.ElapsedMilliseconds >= nextplayerHPdecrease)
            {
                player.HP -= 10;    //HP Reduction Mechanic
                nextplayerHPdecrease = gameStopWatch.ElapsedMilliseconds + 5000;
            }

            if (gameStopWatch.ElapsedMilliseconds >= nextObstacleSpawn)
            {
                AddObstacle();
                nextObstacleSpawn = gameStopWatch.ElapsedMilliseconds + 800;
            }

            if (button1.Enabled == true)
            {
                nextEnableButton1 = gameStopWatch.ElapsedMilliseconds;
                skillDuration1 = gameStopWatch.ElapsedMilliseconds;
            }
            else
            {
                if (gameStopWatch.ElapsedMilliseconds > nextEnableButton1 + 5000)
                {
                    button1.Enabled = true;
                }
            }

            if (button2.Enabled == true)
            {
                nextEnableButton2 = gameStopWatch.ElapsedMilliseconds;
                skillDuration2 = gameStopWatch.ElapsedMilliseconds;
            }
            else
            {

                if (gameStopWatch.ElapsedMilliseconds > nextEnableButton2 + 10000)
                {
                    button2.Enabled = true;
                }

                if (gameStopWatch.ElapsedMilliseconds > skillDuration2 + 5000)
                {
                    player.AddBulletInterval = 300;
                }
            }

            if (button3.Enabled == true)
            {
                nextEnableButton3 = gameStopWatch.ElapsedMilliseconds;
                skillDuration3 = gameStopWatch.ElapsedMilliseconds;
            }
            else
            {

                if (gameStopWatch.ElapsedMilliseconds > nextEnableButton3 + 10000)
                {
                    button3.Enabled = true;
                }
                if (gameStopWatch.ElapsedMilliseconds > skillDuration3 + 5000)
                {
                    player.Speed = 20;
                }
            }

            if (gameStopWatch.ElapsedMilliseconds >= nextBulletAdd)
            {
                AddBullet();
                nextBulletAdd = gameStopWatch.ElapsedMilliseconds + player.AddBulletInterval;
            }
            if (gameStopWatch.ElapsedMilliseconds >= nextAddEnemyBullet)
            {
                AddEnemyBullet();
                nextAddEnemyBullet = gameStopWatch.ElapsedMilliseconds + 700;
            }
            if (gameStopWatch.ElapsedMilliseconds >= nextBulletMove)
            {
                BulletMovement();
                EnemyBulletMovement();
                nextBulletMove = gameStopWatch.ElapsedMilliseconds + 40;
            }
            CheckCollision();

            if (EnemiesSpawned < totalEnemiesToSpawn)
            {
                int randomEnemyNO = rand.Next(0, 3);
                int randomMovement;
                int randomX = rand.Next(600, 1280);
                int randomY = rand.Next(75, 650);
                int randomDamage = rand.Next(minDamage, maxDamage);
                int randomHP = rand.Next(minHp, maxHP);
                if (gameStopWatch.ElapsedMilliseconds >= nextAddEnemy)
                {
                    if (enemyTypeArray[randomEnemyNO] == "Type3")
                    {
                        randomMovement = rand.Next(1, 5);
                    }
                    else
                    {
                        randomMovement = rand.Next(1, 3);
                    }

                    enemyCreated = enemy.CreateEnemy(randomX, randomY, hp = randomHP, enemyTypeArray[randomEnemyNO], randomMovement, randomDamage, goUp, mobSize);
                    EnemiesSpawned++;
                    this.Controls.Add(enemyCreated.PictureBox);
                    enemiesList.Add(enemyCreated);
                    nextAddEnemy = gameStopWatch.ElapsedMilliseconds + 500;
                }
                if (gameStopWatch.ElapsedMilliseconds >= nextBossSpawn)
                {
                    randomEnemyNO = rand.Next(0, 2);
                    if (enemyTypeArray[randomEnemyNO] == "Boss")
                    {
                        randomMovement = rand.Next(1, 3);
                    }
                    else
                    {
                        randomMovement = 1;//Set to Joshua's Boss Movement
                    }

                    enemyCreated = enemy.CreateEnemy(1100, 400, hp = 100 + randomHP, bossArray[randomEnemyNO], randomMovement, randomDamage, goUp, 180);
                    nextBossSpawn = gameStopWatch.ElapsedMilliseconds + 30000;
                    EnemiesSpawned++;
                    this.Controls.Add(enemyCreated.PictureBox);
                    enemiesList.Add(enemyCreated);
                }
            }
            HandleGameOver();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            // Check the player's skill and perform the corresponding action

            player.HP = playerSkills.HealPlayer(player.HP, player.MaxHP);

            Console.WriteLine(player.HP);
            this.Focus();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;

            player.AddBulletInterval = playerSkills.StartRapidFire(player.AddBulletInterval);

            Console.WriteLine(player.HP);
            this.Focus();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            // Check the player's skill and perform the corresponding action
            player.Speed = playerSkills.ApplySpeedBoost(player.Speed);

            Console.WriteLine(player.HP);
            this.Focus();
        }
        private void SkillKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.J)
            {
                button1_Click(button1, EventArgs.Empty);
            }
            if (e.KeyCode == Keys.K)
            {
                button2_Click(button2, EventArgs.Empty);
            }
            if (e.KeyCode == Keys.L)
            {
                button3_Click(button3, EventArgs.Empty);
            }
        }
        protected void CheckCollision()
        {
            foreach (Enemy enemy in enemiesList.ToArray())
            {
                foreach (Bullet bullet in bulletList.ToArray())
                {
                    if (bullet.PictureBox.Bounds.IntersectsWith(enemy.PictureBox.Bounds))
                    {

                        bulletList.Remove(bullet);
                        bullet.PictureBox.Dispose();
                        enemy.HP -= player.BulletDamage;
                    }
                }
                if (enemy.Type == "Type1")
                {
                    enemy.Type1Movement(enemy.PictureBox, enemiesList);
                }
                else if (enemy.Type == "Type2")
                {
                    enemy.Type2Movement(enemy.PictureBox, enemiesList);
                }
                else if (enemy.Type == "Type3")
                {
                    enemy.Type3Movement(enemy.PictureBox, enemiesList);
                }
                else if (enemy.Type == "Boss")
                {
                    enemy.BossMovement(enemy.PictureBox, enemiesList);
                }
                if (enemy.PictureBox.Bounds.IntersectsWith(player.Bounds))
                {
                    player.HP -= 5;

                    if (enemy.Type != "Boss")
                    {
                        enemy.IsDead = true;
                    }
                }
                if (enemy.HP <= 0)
                {
                    enemy.IsDead = true;
                    this.Controls.Remove(enemy.PictureBox);
                    enemy.PictureBox.Dispose();
                    enemy.IsDisposed = true;
                }
                if (enemy.PictureBox.Left <= 100 || enemy.IsDead || enemy.PictureBox.Top <= 0)
                {
                    this.Controls.Remove(enemy.PictureBox);
                    enemy.PictureBox.Dispose();
                    enemy.IsDisposed = true;
                    EnemiesSpawned--;
                }
            }
            enemiesList.RemoveAll(enemy => enemy.IsDisposed);

            foreach (PictureBox obstacle in ObstacleList.ToArray())
            {
                foreach (Bullet bullet in bulletList.ToArray())
                {
                    if (bullet.PictureBox.Bounds.IntersectsWith(obstacle.Bounds))
                    {
                        ObstacleList.Remove(obstacle);
                        bulletList.Remove(bullet);


                        obstacle.Dispose();
                        this.Controls.Remove(bullet.PictureBox);
                        bullet.PictureBox.Dispose();

                    }
                }
                obstacle.Left -= 20;
                if (obstacle.Left <= 0)
                {
                    obstacle.Dispose();
                }
                if (obstacle.Bounds.IntersectsWith(player.Bounds))
                {
                    player.HP -= 5;
                    obstacle.Dispose();
                }
            }
        }
        public void AddBullet()
        {
            PictureBox bulletPictureBox = new PictureBox
            {
                Size = new Size(10, 10),
                BackColor = System.Drawing.Color.Red,
                Tag = "bullet",
                Location = new Point(player.Location.X + player.Width + 15, player.Location.Y + player.Height / 2)
            };
            //Add bullet to list + form
            Bullet bullet = new Bullet(bulletPictureBox, player.BulletDamage);
            bulletList.Add(bullet);
            this.Controls.Add(bulletPictureBox);
            bulletPictureBox.BringToFront();
        }
        public void BulletMovement()
        {
            //Loop through bullets List
            if (bulletList.Count >= 1)
            {
                for (int i = bulletList.Count - 1; i >= 0; i--)
                {
                    Bullet bullet = bulletList[i];
                    int newX = bullet.PictureBox.Location.X + player.BulletSpeed;
                    bullet.PictureBox.Location = new Point(newX, bullet.PictureBox.Location.Y);

                    // Loop for every enemies List to see if a bullet interacts with one
                    for (int j = enemiesList.Count - 1; j >= 0; j--)
                    {

                        Enemy enemy = enemiesList[j];
                        if (bullet.PictureBox.Bounds.IntersectsWith(enemy.PictureBox.Bounds))
                        {
                            this.Controls.Remove(bullet.PictureBox);
                            bulletList.Remove(bullet);
                            bullet.PictureBox.Dispose();

                            enemy.HP -= bullet.Damage; // NEEDS WORK bullet damage

                            if (enemy.HP <= 0)
                            {
                                HandleEnemyDeath(enemy);
                            }

                            break;
                        }
                    }

                    if (newX > 1280)
                    {
                        this.Controls.Remove(bullet.PictureBox);
                        bulletList.Remove(bullet);
                    }
                }
                foreach (Bullet bullet in bulletList)
                {
                    bullet.PictureBox.Left += 5;

                    //Check if the bullet is out of bounds
                    if (bullet.PictureBox.Right >= this.ClientSize.Width)
                    {
                        bullet.PictureBox.Dispose();
                    }
                }


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
        public void AddEnemyBullet()
        {
            bool goUp = true;

            //Loop each Class Enemy in enemies list & Looking for a enemy that has specif tag
            foreach (Enemy enemy in enemiesList)
            {
                if (enemy.Type == "Type2" || enemy.Type == "Type3")
                {
                    PictureBox bulletPictureBox = new PictureBox
                    {
                        Size = new Size(10, 10),
                        BackColor = Color.Gold,
                        Tag = "enemyBullet",
                        Location = new Point(enemy.PictureBox.Location.X - 10, enemy.PictureBox.Location.Y + enemy.PictureBox.Height / 2)
                    };

                    //Add enemyBullet to list + form
                    EnemyBullet enemyBullet = new EnemyBullet(bulletPictureBox, enemy.damage, goUp); // enemyBullet NEEDS Work! Change depending on type of enemy
                    enemyBulletList.Add(enemyBullet);
                    this.Controls.Add(bulletPictureBox);
                    bulletPictureBox.BringToFront();
                }
                if (enemy.Type == "Boss")
                {
                    int bossBulletPattern = rand.Next(1, 4);

                    //int bossBulletPattern = 3;
                    if (bossBulletPattern == 1)
                    {
                        PictureBox bossbulletPictureBox1 = new PictureBox
                        {
                            Size = new Size(10, 10),
                            BackColor = Color.Cyan,
                            Tag = "enemyBullet1",
                            Location = new Point(enemy.PictureBox.Location.X - 10, enemy.PictureBox.Location.Y + 60)
                        };

                        //Add enemyBullet to list + form
                        EnemyBullet enemyBullet = new EnemyBullet(bossbulletPictureBox1, enemy.damage, goUp); // enemyBullet NEEDS Work! Change depending on type of enemy
                        bossBullets.Add(enemyBullet);
                        this.Controls.Add(bossbulletPictureBox1);
                        bossbulletPictureBox1.BringToFront();

                        PictureBox bossbulletPictureBox2 = new PictureBox
                        {
                            Size = new Size(10, 10),
                            BackColor = System.Drawing.Color.Yellow,
                            Tag = "enemyBullet2",
                            Location = new Point(enemy.PictureBox.Location.X - 10, enemy.PictureBox.Location.Y + 100)
                        };

                        //Add enemyBullet to list + form
                        enemyBullet = new EnemyBullet(bossbulletPictureBox2, enemy.damage, goUp); // enemyBullet NEEDS Work! Change depending on type of enemy
                        bossBullets.Add(enemyBullet);
                        this.Controls.Add(bossbulletPictureBox2);
                        bossbulletPictureBox2.BringToFront();
                    }
                    else if (bossBulletPattern == 2)
                    {
                        PictureBox bossbulletPictureBox1 = new PictureBox
                        {
                            Size = new Size(10, 10),
                            BackColor = System.Drawing.Color.Yellow,
                            Tag = "enemyBullet3",
                            Location = new Point(enemy.PictureBox.Location.X - 10, enemy.PictureBox.Location.Y)
                        };

                        //Add enemyBullet to list + form
                        EnemyBullet enemyBullet = new EnemyBullet(bossbulletPictureBox1, enemy.damage, goUp); // enemyBullet NEEDS Work! Change depending on type of enemy
                        bossBullets.Add(enemyBullet);
                        this.Controls.Add(bossbulletPictureBox1);
                        bossbulletPictureBox1.BringToFront();
                    }

                    else if (bossBulletPattern == 3)
                    {
                        PictureBox bossbulletPictureBox1 = new PictureBox
                        {
                            Size = new Size(10, 10),
                            BackColor = System.Drawing.Color.Yellow,
                            Tag = "enemyBullet4",
                            Location = new Point(enemy.PictureBox.Location.X - 10, enemy.PictureBox.Location.Y + enemy.PictureBox.Height)
                        };

                        //Add enemyBullet to list + form
                        EnemyBullet enemyBullet = new EnemyBullet(bossbulletPictureBox1, enemy.damage, false); // enemyBullet NEEDS Work! Change depending on type of enemy
                        bossBullets.Add(enemyBullet);
                        this.Controls.Add(bossbulletPictureBox1);
                        bossbulletPictureBox1.BringToFront();
                    }
                }

            }

        }
        public void EnemyBulletMovement()
        {
            for (int i = enemyBulletList.Count - 1; i >= 0; i--)
            {
                EnemyBullet enemyBullet = enemyBulletList[i];
                int newX = enemyBullet.PictureBox.Location.X - 40; // NEEDS WORK? (20) Should be different depending on enemy type? 
                enemyBullet.PictureBox.Location = new Point(newX, enemyBullet.PictureBox.Location.Y);

                //Check if a enemybullet intersect with player Ship
                if (enemyBullet.PictureBox.Bounds.IntersectsWith(player.Bounds))
                {
                    //HPBar();
                    this.Controls.Remove(enemyBullet.PictureBox);
                    enemyBulletList.Remove(enemyBullet);
                    player.HP -= enemyBullet.Damage;
                }

                if (newX < 0)
                {
                    this.Controls.Remove(enemyBullet.PictureBox);
                    enemyBulletList.Remove(enemyBullet);
                    enemyBullet.PictureBox.Dispose();
                }
            }

            for (int i = bossBullets.Count - 1; i >= 0; i--)
            {
                EnemyBullet enemyBullet = bossBullets[i];
                int newX = enemyBullet.PictureBox.Location.X - 20;
                int newY = enemyBullet.PictureBox.Location.Y;
                if (enemyBullet.PictureBox.Tag == "enemyBullet1" || enemyBullet.PictureBox.Tag == "enemyBullet2")
                {
                    newX -= 20;
                }
                else if (enemyBullet.PictureBox.Tag == "enemyBullet3")
                {
                    if (enemyBullet.BulletGoUP == false)
                    {
                        newY = enemyBullet.PictureBox.Location.Y + 10;
                    }
                    else
                    {
                        newY = enemyBullet.PictureBox.Location.Y - 10;
                        if (enemyBullet.PictureBox.Top <= 20)
                        {
                            enemyBullet.BulletGoUP = false; ;
                        }
                    }

                }
                else if (enemyBullet.PictureBox.Tag == "enemyBullet4")
                {

                    if (enemyBullet.BulletGoUP == false)
                    {
                        newY = enemyBullet.PictureBox.Location.Y + 10;
                        if (newY >= 660)
                        {
                            enemyBullet.BulletGoUP = true;
                        }
                    }
                    else
                    {
                        newY = enemyBullet.PictureBox.Location.Y - 10;

                    }

                }

                enemyBullet.PictureBox.Location = new Point(newX, newY);

                //Check if a enemybullet intersect with player Ship
                if (enemyBullet.PictureBox.Bounds.IntersectsWith(player.Bounds))
                {
                    this.Controls.Remove(enemyBullet.PictureBox);
                    bossBullets.Remove(enemyBullet);
                    player.HP -= enemyBullet.Damage;
                    break;
                }

                if (newX < 0)
                {
                    this.Controls.Remove(enemyBullet.PictureBox);
                    bossBullets.Remove(enemyBullet);
                }
            }
        }
        public void HandleEnemyDeath(Enemy enemy)
        {
            if (enemy.HP <= 0)
            {
                player.HP += 10;
                if (player.HP > player.MaxHP)
                {
                    player.HP = player.MaxHP;
                }
                this.Controls.Remove(enemy.PictureBox);
                enemiesList.Remove(enemy);
                enemy.Dispose();
                EnemiesSpawned--;

                // This will incease player Score
                currentPlayerScore += 1000;
                currScore.Text = currentPlayerScore.ToString();
            }
        }

        public void StopAllTimers()
        {
            gameStopWatch.Stop();
            gameTimer.Stop();
        }
        public void HandleGameOver()
        {
            if (player.HP <= 0)
            {
                hpBar.Image = playerSkills.UpdateHPBar(this, player.HP, player.MaxHP);
                StopAllTimers();
                //Remove all bullets, enemy bullets, and enemies
                foreach (Bullet bullet in bulletList)
                {
                    this.Controls.Remove(bullet.PictureBox);
                }

                foreach (EnemyBullet enemyBullet in enemyBulletList)
                {
                    this.Controls.Remove(enemyBullet.PictureBox);
                }
                enemyBulletList.Clear();
                bossBullets.Clear();

                foreach (Enemy enemy in enemiesList)
                {
                    this.Controls.Remove(enemy.PictureBox);
                }
                enemiesList.Clear();

                player.Image = playerDeadBitmap;

                gameOverSoundEffect.PlaySync(); // Play the sound synchronously
                gameOverSoundEffect.Stop();
                survivalModeSoundPlayer.Stop();
                MessageBox.Show("Game Over");

                EndGameplayLoop();

                Hide();
                MyForm myform = new MyForm();
                myform.ShowDialog();
            }
        }

        public class Bullet
        {
            public PictureBox PictureBox { get; set; }
            public int Damage { get; set; }

            public Bullet(PictureBox pictureBox, int damage)
            {
                PictureBox = pictureBox;
                Damage = damage;
            }
        }
        public class EnemyBullet : Bullet
        {
            public bool BulletGoUP { get; set; }
            public EnemyBullet(PictureBox pictureBox, int damage, bool bulletGoUP) : base(pictureBox, damage)
            {
                PictureBox = pictureBox;
                Damage = damage;
                BulletGoUP = bulletGoUP;
            }
        }
        public void EndGameplayLoop()
        // This method will let the player earn currency & record their name and score.
        {
            currency.GetCurrency();
            currency.EarnCurrency(currentPlayerScore);
            tScores.GetTopScores();
            tScores.CompareCurrentScore(currentPlayerName, currentPlayerScore);
            currentPlayerScore = 0; // Reset of Variable
        }
        #endregion
    }
}
