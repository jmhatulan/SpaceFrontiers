using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using static Game5.Stage3;
using static Game5.InitializeStage;
using System.Media;
using System.Runtime.Remoting.Messaging;
using System.IO;
using static Game5.Stage4;
using System.Diagnostics.Eventing.Reader;

namespace Game5
{
    internal class Stage4 : Form
    {
        public SoundPlayer stage4SoundPlayer = new SoundPlayer("HoloCure - Holo Office.wav");
        public SoundPlayer gameOverSoundEffect = new SoundPlayer("Arcade game over sound effect!.wav");
        public Bitmap hp10 = new Bitmap(Path.Combine(Application.StartupPath, "HP-stateA.png"));
        private Bitmap playerDeadBitmap = new Bitmap(Path.Combine(Application.StartupPath, "GameOverExplosion.gif"));
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
        private int totalEnemiesToSpawn = 0;
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

        private PictureBox hpBar = new PictureBox()
        {
            Size = new Size(150, 30),
            SizeMode = PictureBoxSizeMode.StretchImage,
            Location = new Point(0, 0),
        };

        long nextEnableButton = 0;
        long skillDuration = 0;



        //A-5 , T1-5, TI-3, TI-5

        public Stage4()
        {
            Text = "Stage 4";

            CreateMenu();
            menuForm.Visible = false;
            this.KeyUp += new KeyEventHandler(KeyUpEvent);

            using (StreamReader reader = new StreamReader("Skill1.txt"))
            {
                playerSkills.SelectedPlayerSkill = reader.ReadLine();
            }
          

            Size = new Size(1280, 720);
            StartPosition = FormStartPosition.CenterScreen;
            player = new Player();
            BackColor = Color.MidnightBlue;
            this.Controls.Add(player);
            StartWave(currentWave);
            playerMovement = new PlayerMovement(this);
            enemy = new Enemy();

            button1 = playerSkills.CreateSkill1Button(this, button1);
            this.Controls.Add(button1);
            button1.Click += button1_Click;

            this.KeyDown += SkillKeyDownHandler;

            this.Controls.Add(hpBar);
            hpBar.BringToFront();

            stage4SoundPlayer.PlayLooping();
            Console.WriteLine("Button click event attached.");
            this.KeyPreview = true;

            gameStopWatch.Start();
            gameTimer = new Timer();
            gameTimer.Interval = 30; // edit to change speed of movement
            gameTimer.Tick += new EventHandler(MainGame_Tick);
            gameTimer.Start();


        }

        private void MainGame_Tick(object sender, EventArgs e)
        {
            //Console.WriteLine(enemiesList.Count);
            Console.WriteLine(player.HP);
            
            hpBar.Image = playerSkills.UpdateHPBar(this, player.HP, player.MaxHP);
             
            playerMovement.MovePlayer(player, player.Speed, button1);
            if (gameStopWatch.ElapsedMilliseconds >= nextObstacleSpawn)
            {
                AddObstacle();
                nextObstacleSpawn = gameStopWatch.ElapsedMilliseconds + 500;
            }

            if (button1.Enabled == true)
            {
                nextEnableButton = gameStopWatch.ElapsedMilliseconds;
                skillDuration = gameStopWatch.ElapsedMilliseconds;
            }
            else
            {
                if (playerSkills.SelectedPlayerSkill == "Heal")
                {
                    if (gameStopWatch.ElapsedMilliseconds > nextEnableButton + 5000)
                    {
                        button1.Enabled = true;
                    }

                }
                else
                {
                    if (gameStopWatch.ElapsedMilliseconds > nextEnableButton + 10000)
                    {
                        button1.Enabled = true;
                    }

                }

                if (gameStopWatch.ElapsedMilliseconds > skillDuration + 5000)
                {
                    switch (playerSkills.SelectedPlayerSkill)
                    {
                        case "Accelerate":
                            player.Speed = 20;
                            break;
                        case "Rapid Fire":
                            player.AddBulletInterval = 300;
                            break;
                        default:
                            break;
                    }
                }
            }

            //if (player.HP < 0)
            //{
            //    gameTimer.Stop(); // NEW DISCOVERY WITH THIS WE CAN CREATE A PAUSE MENU WHERE WE CAN JUST STOP AND START THIS WHEN ESC IS PRESSED
            //}

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
            // dummy

            if (gameStopWatch.ElapsedMilliseconds >= nextAddEnemy)
            {
                if (currentWave == 1)
                {
                    SpawnWave1();
                }

                else if (currentWave == 2)
                {
                    SpawnWave2();
                }
                else if (currentWave == 3)
                {
                    SpawnWave3();
                }
                else if (currentWave == 4)
                {
                    SpawnWave4();
                }
                else if (currentWave == 5)
                {
                    SpawnWave5();
                }
                else
                {
                    HandleAllWaveOver();
                }
                    nextAddEnemy = gameStopWatch.ElapsedMilliseconds + 500;
                
            }

            HandleGameOver();
           
        }
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            // Check the player's skill and perform the corresponding action
            switch (playerSkills.SelectedPlayerSkill)
            {
                case "Heal":
                    player.HP = playerSkills.HealPlayer(player.HP, player.MaxHP);
                    break;
                case "Accelerate":
                    player.Speed = playerSkills.ApplySpeedBoost(player.Speed);
                    break;
                case "Rapid Fire":
                    player.AddBulletInterval = playerSkills.StartRapidFire(player.AddBulletInterval);
                    break;
                default:
                    break;
            }
            Console.WriteLine(player.HP);
            this.Focus();
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

                    if (enemy.Type != "Boss" && enemy.Type != "Type3")
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
                    ObstacleList.Remove(obstacle);
                }
                if (obstacle.Bounds.IntersectsWith(player.Bounds))
                {
                    player.HP -= 5;
                    obstacle.Dispose();
                    ObstacleList.Remove(obstacle);
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

        public void StartWave(int wave)
        {
            if (gameOver == false)
            {
                switch (wave)
                {
                    case 1:
                        totalEnemiesToSpawn = 12; // Change to the total number of enemies you want in wave 1
                        break;
                    case 2:
                        totalEnemiesToSpawn = 11;
                        break;
                    case 3:
                        totalEnemiesToSpawn = 8;
                        break;
                    case 4:
                        totalEnemiesToSpawn = 16;
                        break;
                    case 5:
                        totalEnemiesToSpawn = 3;
                        break;
                    default:
                        totalEnemiesToSpawn = 0;
                        break;
                }

                type3Tag = 0;
                type1MovementChoice = -1;
                quarterPointChoiceMade = false;
                EnemiesSpawned = 0;
            }
        }
        private void SpawnWave1()
        {
            if (EnemiesSpawned < totalEnemiesToSpawn && gameOver == false)
            {
                if (gameStopWatch.ElapsedMilliseconds >= nextAddEnemy)
                {

                    if (EnemiesSpawned < 4)
                    {
                        enemyCreated = enemy.CreateEnemy(newX, newY, hp = 12, "Type1", 1, 7, goUp, mobSize);

                    }
                    else if (EnemiesSpawned < 8)
                    {
                        if (type3Tag == 0)
                        {
                            type3Tag++;
                            newY = 200;
                            goUp = false;
                        }
                        else if (type3Tag == 1)
                        {
                            type3Tag++;
                            newY = 550;
                            goUp = true;
                        }

                        else if (type3Tag == 2)
                        {
                            type3Tag++;
                            newY = 200;
                            goUp = false;
                        }
                        else if (type3Tag == 3)
                        {
                            type3Tag++;
                            newY = 550;
                            goUp = true;
                        }
                        else
                        {
                            type3Tag = 1;
                            newY = 200;
                        }
                        tag = type3Tag;
                        enemyCreated = enemy.CreateEnemy(newX, newY, hp = 15, "Type3", tag, 5, goUp, mobSize);
                    }
                    else if (EnemiesSpawned < 12)
                    {
                        enemyCreated = enemy.CreateEnemy(newX, 700, hp = 12, "Type1", 2, 7, goUp, mobSize);
                    }
                    EnemiesSpawned++;
                    nextAddEnemy = gameStopWatch.ElapsedMilliseconds + 400;
                    this.Controls.Add(enemyCreated.PictureBox);
                    enemiesList.Add(enemyCreated);
                }
            }
            else if (enemiesList.Count == 0)
            {
                currentWave++;
                if (currentWave <= maxWave)
                {
                    StartWave(currentWave);
                }
            }
        }
        private void SpawnWave2()
        {
            if (EnemiesSpawned < totalEnemiesToSpawn && gameOver == false)
            {
                if (EnemiesSpawned < 4)
                {
                    enemyCreated = enemy.CreateEnemy(newX, 0, hp = 15, "Type1", 1, 7, goUp, mobSize);
                }
                else if (EnemiesSpawned < 6)
                {
                    if (type3Tag == 0)
                    {
                        type3Tag++;
                        newY = 200;
                        goUp = false;
                    }
                    else if (type3Tag == 1)
                    {
                        type3Tag++;
                        newY = 550;
                        goUp = true;
                    }

                    else if (type3Tag == 2)
                    {
                        type3Tag++;
                        newY = 200;
                        goUp = false;
                    }
                    tag = type3Tag;
                    enemyCreated = enemy.CreateEnemy(newX, newY, hp = 15, "Type3", tag, 5, goUp, mobSize);
                }
                else if (EnemiesSpawned < 7)
                {
                    enemyCreated = enemy.CreateEnemy(newX, 100, hp = 20, "Type2", tag, 3, goUp, mobSize);
                }
                else if (EnemiesSpawned < 11)
                {
                    enemyCreated = enemy.CreateEnemy(newX, 700, hp = 15, "Type1", 2, 5, goUp, mobSize); ;
                }
                EnemiesSpawned++;
                nextAddEnemy = gameStopWatch.ElapsedMilliseconds + 400;
                this.Controls.Add(enemyCreated.PictureBox);
                enemiesList.Add(enemyCreated);
            }
            else if (enemiesList.Count == 0)
            {
                currentWave++;
                if (currentWave <= maxWave)
                {
                    StartWave(currentWave);
                }
            }
        }
        private void SpawnWave3()
        {
            if (EnemiesSpawned < totalEnemiesToSpawn && gameOver == false)
            {

                if (gameStopWatch.ElapsedMilliseconds >= nextAddEnemy)
                {

                    if (EnemiesSpawned < 2)
                    {
                        enemyCreated = enemy.CreateEnemy(newX, 0, hp = 12, "Type1", 1, 5, goUp, mobSize);

                    }
                    else if (EnemiesSpawned < 4)
                    {
                        if (type3Tag == 0)
                        {
                            type3Tag++;
                            newY = 200;
                            goUp = false;
                        }
                        else if (type3Tag == 1)
                        {
                            type3Tag++;
                            newY = 550;
                            goUp = true;
                        }
                        tag = type3Tag;
                        enemyCreated = enemy.CreateEnemy(600, newY, hp = 15, "Type3", tag, 7, goUp, mobSize);
                    }
                    else if (EnemiesSpawned < 5)
                    {
                        enemyCreated = enemy.CreateEnemy(newX, 100, hp = 12, "Type2", 1, 7, goUp, mobSize); ;
                    }
                    else if (EnemiesSpawned < 6)
                    {
                        enemyCreated = enemy.CreateEnemy(newX, 300, hp = 12, "Type2", 2, 7, goUp, mobSize); ;
                    }
                    else if (EnemiesSpawned < 7)
                    {
                        enemyCreated = enemy.CreateEnemy(newX, 500, hp = 12, "Type2", 1, 7, goUp, mobSize); ;
                    }
                    else if (EnemiesSpawned < 7)
                    {
                        enemyCreated = enemy.CreateEnemy(newX, 600, hp = 12, "Type2", 2, 7, goUp, mobSize); ;
                    }

                    EnemiesSpawned++;
                    nextAddEnemy = gameStopWatch.ElapsedMilliseconds + 400;
                    this.Controls.Add(enemyCreated.PictureBox);
                    enemiesList.Add(enemyCreated);
                }
            }
            else if (enemiesList.Count == 0)
            {
                currentWave++;
                if (currentWave <= maxWave)
                {
                    StartWave(currentWave);
                }
            }
        }
        private void SpawnWave4()
        {
            if (EnemiesSpawned < totalEnemiesToSpawn && gameOver == false)
            {

                if (gameStopWatch.ElapsedMilliseconds >= nextAddEnemy)
                {

                    if (EnemiesSpawned < 10)
                    {
                        enemyCreated = enemy.CreateEnemy(newX, 0, hp = 12, "Type1", 1, 7, goUp, mobSize);

                    }
                    else if (EnemiesSpawned < 11)
                    {
                        if (type3Tag == 0)
                        {
                            type3Tag++;
                            newY = 200;
                            goUp = false;
                        }
                        else if (type3Tag == 1)
                        {
                            type3Tag++;
                            newY = 550;
                            goUp = true;
                        }

                        else if (type3Tag == 2)
                        {
                            type3Tag++;
                            newY = 200;
                            goUp = false;
                        }
                        else if (type3Tag == 3)
                        {
                            type3Tag++;
                            newY = 550;
                            goUp = true;
                        }
                        else
                        {
                            type3Tag = 1;
                            newY = 200;
                        }
                        tag = type3Tag;
                        enemyCreated = enemy.CreateEnemy(newX, newY, hp = 12, "Type3", tag, 5, goUp, mobSize);
                    }
                    else if (EnemiesSpawned < 15)
                    {
                        enemyCreated = enemy.CreateEnemy(newX, 700, hp = 12, "Type1", 2, 7, goUp, mobSize);
                    }
                    else if (EnemiesSpawned < 16)
                    {
                        enemyCreated = enemy.CreateEnemy(newX, newY, hp = 15, "Type3", 1, 5, goUp, mobSize);
                    }
                    EnemiesSpawned++;
                    nextAddEnemy = gameStopWatch.ElapsedMilliseconds + 400;
                    this.Controls.Add(enemyCreated.PictureBox);
                    enemiesList.Add(enemyCreated);
                }
            }
            else if (enemiesList.Count == 0)
            {
                currentWave++;
                if (currentWave <= maxWave)
                {
                    StartWave(currentWave);
                }
            }
        }
        private void SpawnWave5()
        {
            if (EnemiesSpawned < totalEnemiesToSpawn && gameOver == false)
            {

                if (gameStopWatch.ElapsedMilliseconds >= nextAddEnemy)
                {

                    if (EnemiesSpawned < 1)
                    {
                        enemyCreated = enemy.CreateEnemy(newX, 300, hp = 150, "Boss", 2, 10, goUp, 180);

                    }
                    else if (EnemiesSpawned < 2)
                    {
                        enemyCreated = enemy.CreateEnemy(newX, 200, hp = 25, "Type3", 3, 5, goUp, mobSize);
                    }
                    else if (EnemiesSpawned < 3)
                    {
                        enemyCreated = enemy.CreateEnemy(newX, 550, hp = 25, "Type3", 2, 5, goUp, mobSize);
                    }

                    EnemiesSpawned++;
                    nextAddEnemy = gameStopWatch.ElapsedMilliseconds + 400;
                    this.Controls.Add(enemyCreated.PictureBox);
                    enemiesList.Add(enemyCreated);
                }

            }
            else if (enemiesList.Count == 0)
            {
                currentWave++;
                if (currentWave <= maxWave)
                {
                    StartWave(currentWave);
                }
                else if (currentWave > maxWave)
                {
                    HandleAllWaveOver();
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
                            BackColor = Color.Cyan,
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
                            BackColor = Color.Cyan,
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
                            BackColor = Color.Cyan,
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


                    //if (player.HP <= 0)
                    //{
                    //    gameOver = true;
                    //    //GameOver Remove everything
                    //    HandleGameOver();
                    //}
                    //break;
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
                    //HPBar();
                    this.Controls.Remove(enemyBullet.PictureBox);
                    bossBullets.Remove(enemyBullet);
                    player.HP -= enemyBullet.Damage;


                    //if (playerHP <= 0)
                    //{
                    //    gameOver = true;
                    //    //GameOver Remove everything
                    //    HandleGameOver();
                    //}
                    break;
                }

                if (newX < 0)
                {
                    this.Controls.Remove(enemyBullet.PictureBox);
                    bossBullets.Remove(enemyBullet);
                }
            }

            //HPBar();
            //Console.WriteLine(playerHP); // CHECK HP IN CONSOLE :)
        }
        public void HandleEnemyDeath(Enemy enemy)
        {

            // Check if the enemy HP is 0. Remove it enemies List + form
            if (enemy.HP <= 0)
            {
                this.Controls.Remove(enemy.PictureBox);
                enemiesList.Remove(enemy);

                //If all enemy killed add 1 to currentWave to start a new Wave
                if (enemiesList.Count == 0)
                {
                    currentWave++;
                    if (currentWave <= maxWave)
                    {
                        StartWave(currentWave);
                    }
                    //if (currentWave > maxWave)
                    //{
                    //    HandleAllWaveOver();
                    //}
                }
            }
        }

        public void StopAllTimers()
        {
            gameStopWatch.Stop();
            gameTimer.Stop();
        }

        public void HandleAllWaveOver()
        {
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

            gameOver = true;
            stage4SoundPlayer.Stop();
            MessageBox.Show("All Waves Completed");


            Hide();
            AnotherForm anotherForm = new AnotherForm();
            anotherForm.ShowDialog();
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
                stage4SoundPlayer.Stop();
                MessageBox.Show("Game Over");

                Hide();
                AnotherForm anotherForm = new AnotherForm();
                anotherForm.ShowDialog();


            }
        }

        private void SkillKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.J)
            {
                button1_Click(button1, EventArgs.Empty);
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
            gameTimer.Enabled = false;
            menuForm.Visible = true;
            menuForm.Enabled = true;
            menuForm.BringToFront();
            gameStopWatch.Stop();
        }
        public void CloseMenu()
        {
            GamePaused = false;
            gameTimer.Enabled = true;
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

            gameOver = true;
            stage4SoundPlayer.Stop();

            Hide();
            AnotherForm anotherForm = new AnotherForm();
            anotherForm.ShowDialog();
        }
    }
}


