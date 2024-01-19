using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Game5.InitializeStage;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Game5
{
    internal abstract class Entity:PictureBox
    {
        public Bitmap type1Bitmap = new Bitmap(Path.Combine(Application.StartupPath, "Type1.png"));
        public Bitmap type2Bitmap = new Bitmap(Path.Combine(Application.StartupPath, "Type2.png"));
        public Bitmap type3Bitmap = new Bitmap(Path.Combine(Application.StartupPath, "Type3.png"));
        public Bitmap type4Bitmap = new Bitmap(Path.Combine(Application.StartupPath, "Type4.png"));
        public Bitmap type5Bitmap = new Bitmap(Path.Combine(Application.StartupPath, "Type5.png"));
        public Bitmap finalBossBitmap = new Bitmap(Path.Combine(Application.StartupPath, "dataStructureBoss.gif"));
        public Bitmap miniBossBitmap = new Bitmap(Path.Combine(Application.StartupPath, "miniBoss.png"));
        public Bitmap backGroundBitmap = new Bitmap(Path.Combine(Application.StartupPath, "Spacebackground.png"));

        public PictureBox pictureBox;
        public int hp;
        public string type;
        public int tag;
        public bool goUp;
        public int damage;
        public bool isDead;
        public bool isDisposed;
        public bool isShooting;

        public PictureBox PictureBox { get { return pictureBox; } set { pictureBox = value; } }
        public int HP { get { return hp; } set { hp = value; } }
        public string Type { get { return type; } set { type = value; } }
        public int Tag { get { return tag; } set { tag = value; } }
        public bool GoUp { get { return goUp; } set { goUp = value; } }
        public int Damage { get { return damage; } set { damage = value; } }
        public bool IsDead { get { return isDead; } set { isDead = value; } }
        public bool IsDisposed { get { return isDisposed; } set { isDisposed = value; } }
        public bool IsShooting { get { return isShooting; } set { isShooting = value; } }
    }


    internal class Enemy:Entity
    {
        private bool gameOver = false;
        private bool quarterPointChoiceMade = false;
        private int tag = 1;
        private int totalEnemiesToSpawn = 0;
        private int enemiesSpawned = 0;
        private int type1MovementChoice = -1;
        private int type3Tag = 0;
        private Random rand = new Random();

        public int currentWave = 1;
        public int maxWave = 5;

        public Enemy()
        {

        }

        public Enemy(PictureBox pictureBox, string type, int hp, int tag, int damage, bool goUp)
        {
            PictureBox = pictureBox;
            HP = hp;
            GoUp = goUp;
            Tag = tag;
            Damage = damage;
            Type = type;
            IsDead = false;
            IsDisposed = false;
        }

        public Enemy CreateEnemy(int newX, int newY, int hp, string enemyType, int tag, int eDamage, bool goUp, int size)
        {
            PictureBox enemyPictureBox = new PictureBox()
            {
                Image = GetEnemyBitmap(enemyType),
                Size = new Size(size, size),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
                Location = new Point(newX, newY),
                Tag = "enemy",
            };

            Enemy enemy = new Enemy(enemyPictureBox, enemyType, hp, tag, eDamage, goUp);
            return enemy;
        }
        private Bitmap GetEnemyBitmap(string enemyType)
        {
            switch (enemyType)
            {
                case "Type1":
                    return type1Bitmap;
                case "Type2":
                    return type2Bitmap;
                case "Type3":
                    return type3Bitmap;
                case "Type4":
                    return type4Bitmap;
                case "Type5":
                    return type5Bitmap;
                case "Boss":
                    return miniBossBitmap;
                case "FinalBoss":
                    return finalBossBitmap;
                // Add cases for other enemy types if needed
                default:
                    return null; // Return a default bitmap or handle this case as needed
            }
        }
        public void RemoveEnemy(Enemy enemy, List<Enemy> enemiesList)
        {
            //Remove Specific Enemy from form + list
            this.Controls.Remove(enemy.PictureBox);
            enemy.PictureBox.Dispose();
            enemiesList.Remove(enemy);
        }

        public void Type1Movement(PictureBox enemyPictureBox, List<Enemy> enemiesList)
        {
            Enemy enemyC = GetEnemyFromPictureBox(enemyPictureBox, enemiesList);
            if (enemyC != null)
            {
                if (enemyC.Tag == 1)
                {
                    int newX = enemyPictureBox.Location.X - 5;
                    int newY = enemyPictureBox.Location.Y + 5;

                    if (type1MovementChoice == -1 && newX <= 1280 / 2)
                    {
                        type1MovementChoice = rand.Next(0, 3); // Make the choice only once, Randomly Go up, down, straight left
                    }

                    if (quarterPointChoiceMade == false && newX <= 1280 / 4)
                    {
                        type1MovementChoice = rand.Next(0, 3);
                        quarterPointChoiceMade = true; // Mark that type1 reach quarter of form. Randomly Go up, down, straight left
                    }

                    if (type1MovementChoice == 0)
                    {
                        newY += -5;  // Move straight
                    }
                    else if (type1MovementChoice == 1)
                    {
                        newY -= 10; //Move up
                    }

                    enemyPictureBox.Location = new Point(newX, newY);
                    if (enemyPictureBox.Left < 0 || enemyPictureBox.Top < 0)
                    {
                        RemoveEnemy(GetEnemyFromPictureBox(enemyPictureBox, enemiesList), enemiesList);
                    }
                }
                if (enemyC.Tag == 2)
                {
                    int newX = enemyPictureBox.Location.X - 5;
                    int newY = enemyPictureBox.Location.Y - 5;

                    if (type1MovementChoice == -1 && newX <= 1280 / 2)
                    {
                        type1MovementChoice = rand.Next(0, 3); // Make the choice only once, Randomly Go up, down, straight left
                        //type1MovementChoice = 2;
                    }

                    if (quarterPointChoiceMade == false && newX <= 1280 / 4)
                    {
                        type1MovementChoice = rand.Next(0, 3);

                        quarterPointChoiceMade = true; // Mark that type1 reach quarter of form. Randomly Go up, down, straight left
                    }

                    if (type1MovementChoice == 0)
                    {
                        newY += -5;  // Move straight
                    }
                    else if (type1MovementChoice == 1)
                    {
                        newY -= 10; //Move up
                    }
                    else if (type1MovementChoice == 2)
                    {
                        newY += 10; //Move down
                    }

                    enemyPictureBox.Location = new Point(newX, newY);
                    if (enemyPictureBox.Left < 0 || enemyPictureBox.Top < 0)
                    {
                        RemoveEnemy(GetEnemyFromPictureBox(enemyPictureBox, enemiesList), enemiesList);
                    }
                }
            }
        }

        public void Type2Movement(PictureBox enemyPictureBox, List<Enemy> enemiesList)
        {
            Enemy enemyC = GetEnemyFromPictureBox(enemyPictureBox, enemiesList);
            int newX = enemyC.PictureBox.Location.X - 3;
            int newY = enemyC.PictureBox.Location.Y;

            // Your existing movement logic for Type2

            if (enemyC.Tag == 1)
            {
                if (enemyPictureBox.Left <= 1280 - 200) // Maybe NEEDS Change (200).. Make variable?
                {
                    newX = enemyPictureBox.Location.X;
                }
            }
            else if (enemyC.Tag == 2)
            {
                // IF enemy reaches point X=700 Stop
                if (enemyPictureBox.Left <= 1280 - 300) // Maybe NEEDS Change (300).. Make variable?
                {
                    newX = enemyPictureBox.Location.X;
                }


            }

            enemyC.PictureBox.Location = new Point(newX, newY);
        }

        public void Type3Movement(PictureBox enemyPictureBox, List<Enemy> enemiesList)
        {
            Enemy enemyC = GetEnemyFromPictureBox(enemyPictureBox, enemiesList);
            if (enemyC != null)
            {
                int newX = enemyC.PictureBox.Location.X - 4;
                int newY = enemyC.PictureBox.Location.Y;


                // Check if the enemy reaches a certain X position to change movement to Y-axis
                if (enemyC.Tag == 1)
                {
                    // Use the tag to determine the movement pattern
                    if (newX <= 1280 - 200)
                    {
                        newX = enemyC.PictureBox.Location.X;

                        if (enemyC.GoUp == false)
                        {
                            newY = enemyC.PictureBox.Location.Y + 5;

                            if (newY >= (720/ 2) - 12 && enemyC.GoUp == false)
                            {
                                enemyC.GoUp = true;
                            }


                        }
                        else if (enemyC.GoUp == true)
                        {
                            newY = enemyC.PictureBox.Location.Y - 5;

                            if (newY <= 50)
                            {
                                enemyC.GoUp = false;
                            }
                        }

                    }

                }

                else if (enemyC.Tag == 3)
                {
                    // Use the tag to determine the movement pattern
                    if (newX <= 1280 - 400)
                    {
                        newX = enemyC.PictureBox.Location.X;

                        if (enemyC.GoUp == false)
                        {
                            newY = enemyC.PictureBox.Location.Y + 5;

                            if (newY >= (720 / 2) - 120 && enemyC.GoUp == false)
                            {
                                enemyC.GoUp = true;
                            }


                        }
                        else if (enemyC.GoUp == true)
                        {
                            newY = enemyC.PictureBox.Location.Y - 5;

                            if (newY <= 50)
                            {
                                enemyC.GoUp = false;
                            }
                        }

                    }

                }

                else if (enemyC.Tag == 2)
                {
                    if (newX <= 1280 - 400)
                    {
                        newX = enemyC.PictureBox.Location.X;

                        if (enemyC.GoUp == true)
                        {
                            newY = enemyC.PictureBox.Location.Y - 5;

                            if (newY <= (720 / 2) - 80 && enemyC.GoUp == true)
                            {
                                enemyC.GoUp = false;
                            }


                        }
                        else if (enemyC.GoUp == false)
                        {
                            newY = enemyC.PictureBox.Location.Y + 5;

                            if (newY >= 600)
                            {
                                enemyC.GoUp = true;
                            }
                        }

                    }
                }

                else if (enemyC.Tag == 4)
                {
                    if (newX <= 1280 - 200)
                    {
                        newX = enemyC.PictureBox.Location.X;

                        if (enemyC.GoUp == true)
                        {
                            newY = enemyC.PictureBox.Location.Y - 5;

                            if (newY <= (720 / 2) - 80 && enemyC.GoUp == true)
                            {
                                enemyC.GoUp = false;
                            }
                        }
                        else if (enemyC.GoUp == false)
                        {
                            newY = enemyC.PictureBox.Location.Y + 5;

                            if (newY >= 600)
                            {
                                enemyC.GoUp = true;
                            }
                        }

                    }
                }
                enemyC.PictureBox.Location = new Point(newX, newY);
            }
        }

        public void Type4Movement(PictureBox enemyPictureBox, List<Enemy> enemiesList)
        {
            Enemy enemyC = GetEnemyFromPictureBox(enemyPictureBox, enemiesList);
            if (enemyC != null)
            {

                int newX = enemyPictureBox.Location.X;
                int newY = enemyPictureBox.Location.Y;

                newX += -50;

                enemyPictureBox.Location = new Point(newX, newY);


                //if (newX + 60 < 0 || enemyPictureBox.Top < 0)
                //{
                //    RemoveEnemy(GetEnemyFromPictureBox(enemyPictureBox, enemiesList), enemiesList);
                //}


            }
        }

        public void Type5Movement(PictureBox enemyPictureBox, List<Enemy> enemiesList)
        {
            Enemy enemyC = GetEnemyFromPictureBox(enemyPictureBox, enemiesList);
            if (enemyC != null)
            {

                int newX = enemyPictureBox.Location.X;
                int newY = enemyPictureBox.Location.Y;

                newY += 10;

                enemyPictureBox.Location = new Point(newX, newY);


                //if (newX + 60 <= 0 || enemyPictureBox.Top < 0)
                //{
                //    RemoveEnemy(GetEnemyFromPictureBox(enemyPictureBox, enemiesList), enemiesList);
                //}


            }
        }


        public void BossMovement(PictureBox enemyPictureBox, List<Enemy> enemiesList)
        {
            Enemy enemy = GetEnemyFromPictureBox(enemyPictureBox, enemiesList);
            int newX = enemy.PictureBox.Location.X - 3;
            int newY = enemy.PictureBox.Location.Y;

            // Your existing movement logic for Type2

            if (enemy.Tag == 1)
            {
                if (enemyPictureBox.Left <= 1280 - 100) // Maybe NEEDS Change (200).. Make variable?
                {
                    newX = enemyPictureBox.Location.X;
                }
            }
            else if (enemy.Tag == 2)
            {
                // IF enemy reaches point X=700 Stop
                if (enemyPictureBox.Left <= 1280 - 300) // Maybe NEEDS Change (300).. Make variable?
                {
                    newX = enemyPictureBox.Location.X;
                }
            }

            enemy.PictureBox.Location = new Point(newX, newY);
        }

        public void FinalBossMovement(PictureBox enemyPictureBox, List<Enemy> enemiesList)
        {
            Enemy enemy = GetEnemyFromPictureBox(enemyPictureBox, enemiesList);
            
            int newX = enemy.PictureBox.Location.X - 5;
            int newY = enemy.PictureBox.Location.Y;

            // Check if the enemy reaches a certain X position to change movement to Y-axis
            if (enemy.Tag == 1)
            {
                // Use the tag to determine the movement pattern
                if (newX <= 1280 - 500)
                {
                    newX = enemy.PictureBox.Location.X;

                    if (enemy.GoUp == false)
                    {
                        newY = enemy.PictureBox.Location.Y + 5;

                        if (newY >= (720 - enemy.PictureBox.Height - 50))
                        {
                            enemy.GoUp = true;
                        }


                    }
                    else if (enemy.GoUp == true)
                    {
                        newY = enemy.PictureBox.Location.Y - 5;

                        if (newY <= 50)
                        {
                            enemy.GoUp = false;
                        }
                    }

                }

            }

            enemy.PictureBox.Location = new Point(newX, newY);
        }

        public Enemy GetEnemyFromPictureBox(PictureBox pictureBox, List<Enemy> enemiesList)
        {
            //Find Specific enemy in enemies list based on enemyPicture
            return enemiesList.Find(enemy => enemy.PictureBox == pictureBox);
        }

    }
}
