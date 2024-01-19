using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Game5.InitializeStage;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Game5
{
    internal class InitializeStage : Form
    {
        // Sets timer here since this will only be used during the game
        public Stopwatch gameStopWatch = new Stopwatch();

        protected Player player;
        protected PlayerMovement playerMovement;

        protected List<Bullet> bulletList = new List<Bullet>();
        protected List<Enemy> enemiesList = new List<Enemy>();
        protected List<EnemyBullet> enemyBulletList = new List<EnemyBullet>();
        public List<EnemyBullet> bossBullets = new List<EnemyBullet>();

        List<PictureBox> ObstacleList = new List<PictureBox>();

        protected Timer gameTimer;
        protected Enemy enemy = new Enemy();


        private bool quarterPointChoiceMade = false;
        private int tag = 1;
        private int totalEnemiesToSpawn = 0;
        private int type1MovementChoice = -1;
        private int type3Tag = 0;
        private Random rand = new Random();

        bool gameOver = false;

        long nextBulletAdd = 0;
        long nextBulletMove = 0;
        long nextAddEnemy = 0;
        long nextAddEnemyBullet = 0;
        long nextObstacleSpawn = 0;

        long nextFreeze = 10000;
        long nextUnfreeze = 11000;
        long nextMoveEnemy = 0;

        bool freezeEnabled = false;


        public int currentWave = 1;
        public int maxWave = 5;

        public int StageLevel { get; set; }
        public int EnemiesSpawned { get; set; }
        public int X { get; set; }

        public int newX = 1280 - 200;
        public int newY = 0;
        public int hp = 0;
        public bool goUp = true;
        public int mobSize = 60;
        public Button button1;

        Enemy enemyCreated = null;
        

        public InitializeStage()
        {
            Size = new Size(1280, 720);
            StartPosition = FormStartPosition.CenterScreen;
            player = new Player();
            this.Controls.Add(player);

            StartWave(currentWave);

            playerMovement = new PlayerMovement(this);
            enemy = new Enemy();

            gameStopWatch.Start();

            gameTimer = new Timer();
            gameTimer.Interval = 30; // edit to change speed of movement
            gameTimer.Tick += new EventHandler(MainGame_Tick);
            gameTimer.Start();
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
                        enemy.HP -= 5;
                    }

                }
                if (enemy.Type == "Type1")
                {
                    enemy.Type1Movement(enemy.PictureBox,enemiesList);
                }
                else if (enemy.Type == "Type2")
                {
                    enemy.Type2Movement(enemy.PictureBox, enemiesList);
                }
                else if (enemy.Type == "Type3")
                {
                    enemy.Type3Movement(enemy.PictureBox, enemiesList);
                }
                if (enemy.PictureBox.Bounds.IntersectsWith(player.Bounds))
                {
                    player.HP -= 5;
                    enemy.IsDead = true;
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
                obstacle.Left -= 5;
                if (obstacle.Left <= 0)
                {
                    obstacle.Dispose();
                }
                if (obstacle.Bounds.IntersectsWith(player.Bounds))
                {
                    player.HP -= 5;
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
                        totalEnemiesToSpawn = 20; // Change to the total number of enemies you want in wave 1
                        break;
                    case 2:
                        totalEnemiesToSpawn = 5;
                        break;
                    case 3:
                        totalEnemiesToSpawn = 5;
                        break;
                    case 4:
                        totalEnemiesToSpawn = 7;
                        break;
                    case 5:
                        totalEnemiesToSpawn = 6;
                        break;
                    default:
                        totalEnemiesToSpawn = 0;
                        break;
                }

                type1MovementChoice = -1;
                quarterPointChoiceMade = false;
                EnemiesSpawned = 0;
            }
        }

        private void MainGame_Tick(object sender, EventArgs e)
        {

            Console.WriteLine(player.HP);
            playerMovement.MovePlayer(player, player.Speed, button1);


            if (gameStopWatch.ElapsedMilliseconds >= nextObstacleSpawn)
            {
                AddObstacle();
                nextObstacleSpawn = gameStopWatch.ElapsedMilliseconds + 1000;
            }

            //if (player.HP < 0) 
            //{
            //    gameTimer.Stop(); // NEW DISCOVERY WITH THIS WE CAN CREATE A PAUSE MENU WHERE WE CAN JUST STOP AND START THIS WHEN ESC IS PRESSED
            //}

            if (gameStopWatch.ElapsedMilliseconds >= nextBulletAdd)
            {
                AddBullet();
                nextBulletAdd = gameStopWatch.ElapsedMilliseconds + 300;
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

            if (gameStopWatch.ElapsedMilliseconds >= nextFreeze && freezeEnabled)
            {
                // Stop the player for 1 second
                player.Speed = 0;
                //player.Image = playerSpaceShipFreezeBitmap;
                nextFreeze = gameStopWatch.ElapsedMilliseconds + 10000;   
            }

            if (gameStopWatch.ElapsedMilliseconds >= nextUnfreeze && freezeEnabled)
            {
                // Stop the player for 1 second
                player.Speed = 10;
                nextUnfreeze = gameStopWatch.ElapsedMilliseconds + 11000;
            }


            X = this.Size.Width;

            CheckCollision();
            // dummy

            if (currentWave == 1)
            {
                if (EnemiesSpawned < totalEnemiesToSpawn && gameOver == false)
                {
       
                    if (gameStopWatch.ElapsedMilliseconds >= nextAddEnemy)
                    {
                
                        if (EnemiesSpawned < 10)
                        {
                            enemyCreated = enemy.CreateEnemy(newX, newY, hp = 3, "Type1", 1, 1, goUp, mobSize);

                        }
                        else if (EnemiesSpawned < 20)
                        {
                            enemyCreated = enemy.CreateEnemy(newX, newY = 200 + (EnemiesSpawned - 10) * 100, hp = 5, "Type2", 1, 2, goUp, mobSize);
                        }
                        //else if (EnemiesSpawned < 16)
                        //{
                        //    enemyCreated = enemy.CreateEnemy(600, newY = 200, hp = 5, "Type3", 1, 2, false, mobSize);                         
                        //}
                        //else if (EnemiesSpawned < 20)
                        //{
                        //    if (type3Tag == 0)
                        //    {
                        //        type3Tag++;
                        //        newY = 200;
                        //        goUp = false;
                        //    }
                        //    else if (type3Tag == 1)
                        //    {
                        //        type3Tag++;
                        //        newY = 550;
                        //        goUp = true;
                        //    }

                        //    else if (type3Tag == 2)
                        //    {
                        //        type3Tag++;
                        //        newY = 200;
                        //        goUp = false;
                        //    }
                        //    else if (type3Tag == 3)
                        //    {
                        //        type3Tag++;
                        //        newY = 550;
                        //        goUp = true;
                        //    }
                        //    else
                        //    {
                        //        type3Tag = 1;
                        //        newY = 200;
                        //    }
                        //    tag = type3Tag;
                        //    enemyCreated = enemy.CreateEnemy(newX, newY, hp = 9, "Type3", tag, 3, goUp, mobSize);
                        //}

                        EnemiesSpawned++;
                        nextAddEnemy = gameStopWatch.ElapsedMilliseconds + 400;
                        this.Controls.Add(enemyCreated.PictureBox);
                        enemiesList.Add(enemyCreated);
                    }

                }
            }

            //if (gameOver == false)
            //{
            //    foreach (Enemy enemy in enemiesList.ToList())
            //    {
            //        if (enemy != null)
            //        {
            //            MoveEnemy(enemy.PictureBox);
            //        }
            //    }
            //}

        }


        private void AddObstacle()
        {
            PictureBox obstaclePictureBox = new PictureBox
            {
                Size = new Size(20, 20),
                BackColor = Color.Blue,
                Tag = "obstacle",
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
                        BackColor = System.Drawing.Color.Blue,
                        Tag = "enemyBullet",
                        Location = new Point(enemy.PictureBox.Location.X - 10, enemy.PictureBox.Location.Y + enemy.PictureBox.Height / 2)
                    };

                    //Add enemyBullet to list + form
                    EnemyBullet enemyBullet = new EnemyBullet(bulletPictureBox, enemy.damage,goUp); // enemyBullet NEEDS Work! Change depending on type of enemy
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
                            BackColor = System.Drawing.Color.Yellow,
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
                    newX -= 10;
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

        public class EnemyBullet:Bullet
        {
            public bool BulletGoUP { get; set; }
            public EnemyBullet(PictureBox pictureBox, int damage, bool bulletGoUP): base(pictureBox, damage)
            {
                PictureBox = pictureBox;
                Damage = damage;
                BulletGoUP = bulletGoUP;
            }
        }
    }


    

 

}
