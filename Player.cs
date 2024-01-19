using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using static Game5.Stage3;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Game5
{    
    internal class Player : PictureBox
    {
        private int hp = 100;
        private int maxHP = 100;
        private int speed = 10;
        private int bulletDamage = 3;
        private int bulletSpeed = 30;
        private int addBulletInterval = 300;

        
        private Bitmap playerSpaceShipBitmap = new Bitmap(Path.Combine(Application.StartupPath, "SpaceShip.png"));
       

        public int AddBulletInterval { get { return addBulletInterval; } set { addBulletInterval = value; } }
        public int MaxHP
        {
            get { return maxHP; }
            set { maxHP = value; }
        }

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public int BulletSpeed
        {
            get { return bulletSpeed; }
            set { bulletSpeed = value; }
        }

        public int BulletDamage
        {
            get { return bulletDamage; }
            set { bulletDamage = value; }
        }

        public int HP
        {
            get { return hp; }
            set { hp = value; }
        }

        public Player()
        {
            this.BackColor = Color.Transparent;
            this.Image = playerSpaceShipBitmap;
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Size = new Size(60, 60);
            this.Location = new Point(1280 / 10, 720 / 2);
        }
       
    }

    internal class PlayerMovement:Form
    {
        private bool moveUp, moveDown, moveLeft, moveRight;

        public PlayerMovement(Form parentForm)
        {
            InitializeMovement(parentForm);
        }

        private void InitializeMovement(Form parentForm)
        {
            parentForm.KeyDown += KeyDownHandler;
            parentForm.KeyUp += KeyUpHandler;                 
        }
        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                moveLeft = true;
            }
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                moveRight = true;
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            {
                moveDown = true;
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                moveUp = true;
            }
            
        }

        private void KeyUpHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                moveLeft = false;
            }
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                moveRight = false;
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            {
                moveDown = false;
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                moveUp = false;
            }
        }

        public void MovePlayer(PictureBox player, int playerSpeed,Button button1)
        {
            int newX = player.Location.X;
            int newY = player.Location.Y;
            int bottomMostY = player.Location.Y + player.Height;
            int rightMost = player.Location.X + player.Width;

            if (moveLeft && newX > 0 && !player.Bounds.IntersectsWith(button1.Bounds))
            {
                newX -= playerSpeed;
            }
            if (moveRight && rightMost < 1280 - 20)
            {
                newX += playerSpeed;
            }

            if (moveDown && bottomMostY < 680)
            {
                newY += playerSpeed;
                
            }
            if (moveUp && newY > 0 && !player.Bounds.IntersectsWith(button1.Bounds))
            {
                newY -= playerSpeed;
            }

            player.Location = new Point(newX, newY);

        }

        public void MovePlayer(PictureBox player, int playerSpeed)
        {
            int newX = player.Location.X;
            int newY = player.Location.Y;
            int bottomMostY = player.Location.Y + player.Height;
            int rightMost = player.Location.X + player.Width;

            if (moveLeft && newX > 0)
            {
                newX -= playerSpeed;
            }
            if (moveRight && rightMost < 1280 - 20)
            {
                newX += playerSpeed;
            }

            if (moveDown && bottomMostY < 680)
            {
                newY += playerSpeed;

            }
            if (moveUp && newY > 0)
            {
                newY -= playerSpeed;
            }

            player.Location = new Point(newX, newY);

        }
    }

    internal class PlayerSKills:Form
    {
        private Bitmap medbutton = new Bitmap(Path.Combine(Application.StartupPath, "medkitbutton.png"));
        private Bitmap rapidbutton = new Bitmap(Path.Combine(Application.StartupPath, "rapidbutton.png"));
        private Bitmap boostSpeedbutton = new Bitmap(Path.Combine(Application.StartupPath, "boostspeedbutton.png"));

        //HP Bar
        public Bitmap hp10 = new Bitmap(Path.Combine(Application.StartupPath, "HP-stateA.png"));
        public Bitmap hp9 = new Bitmap(Path.Combine(Application.StartupPath, "HP-stateB.png"));
        public Bitmap hp8 = new Bitmap(Path.Combine(Application.StartupPath, "HP-stateC.png"));
        public Bitmap hp7 = new Bitmap(Path.Combine(Application.StartupPath, "HP-stateD.png"));
        public Bitmap hp6 = new Bitmap(Path.Combine(Application.StartupPath, "HP-stateE.png"));
        public Bitmap hp5 = new Bitmap(Path.Combine(Application.StartupPath, "HP-stateF.png"));
        public Bitmap hp4 = new Bitmap(Path.Combine(Application.StartupPath, "HP-stateG.png"));
        public Bitmap hp3 = new Bitmap(Path.Combine(Application.StartupPath, "HP-stateH.png"));
        public Bitmap hp2 = new Bitmap(Path.Combine(Application.StartupPath, "HP-stateI.png"));
        public Bitmap hp1 = new Bitmap(Path.Combine(Application.StartupPath, "HP-stateJ.png"));
        public Bitmap hp0 = new Bitmap(Path.Combine(Application.StartupPath, "HP-stateK.png"));

        //public Button button1;
        //public Button button2;


        private string selectedPlayerSkill;
        private string selectedPlayerSkill2;
        public string SelectedPlayerSkill { get { return selectedPlayerSkill; } set { selectedPlayerSkill = value; } }
        public string SelectedPlayerSkill2 { get { return selectedPlayerSkill2; } set { selectedPlayerSkill2 = value; } }
 
        public PlayerSKills()
        {
        }

        public Image UpdateHPBar(Form parentForm, int hp, int maxHP)
        {

            Image hpState = null;
            double currentHP;
            double hpPercentage = Convert.ToDouble(maxHP);
            currentHP = Convert.ToDouble(hp);
            if (currentHP >= (hpPercentage * .9))
            {
                hpState = hp10;
            }
            else if (currentHP < (hpPercentage * .9) && currentHP >= (hpPercentage * .8))
            {
                hpState = hp9;
            }
            else if (currentHP < (hpPercentage * .8) && currentHP >= (hpPercentage * .7))
            {
                hpState = hp8;
            }
            else if (currentHP < (hpPercentage * .7) && currentHP >= (hpPercentage * .6))
            {
                hpState = hp7;
            }
            else if (currentHP < (hpPercentage * .6) && currentHP >= (hpPercentage * .5))
            {
                hpState = hp6;
            }
            else if (currentHP < (hpPercentage * .5) && currentHP >= (hpPercentage * .4))
            {
                hpState = hp5;
            }
            else if (currentHP < (hpPercentage * .4) && currentHP >= (hpPercentage * .3))
            {
                hpState = hp4;
            }
            else if (currentHP < (hpPercentage * .3) && currentHP >= (hpPercentage * .2))
            {
                hpState = hp3;
            }
            else if (currentHP < (hpPercentage * .2) && currentHP >= (hpPercentage * .1))
            {
                hpState = hp2;
            }
            else if (currentHP < (hpPercentage * .1) && currentHP >= (hpPercentage * .0))
            {
                hpState = hp0;
            }
            else
            {
                hpState = hp0;
            }

            return hpState;
        }

        public string GetPlayerSelectedSkill()
        {
            // Create an instance of SelectSkillForm without showing it
            using (SelectSkillForm selectSkillForm = new SelectSkillForm())
            {
                // ShowDialog will block until the form is closed
                if (selectSkillForm.ShowDialog() == DialogResult.OK)
                {
                    // Return the selected skill
                    return selectSkillForm.SelectedSkill;
                }
            }
            return "";
        }

        public Button CreateSkill1Button(Form parentForm, Button button1)
        {
            Image skillButtonImage = null;
            switch (selectedPlayerSkill)
            {
                case "Heal":
                    skillButtonImage = medbutton;
                    break;
                case "Accelerate":
                    skillButtonImage = boostSpeedbutton;
                    break;
                case "Rapid Fire":
                    skillButtonImage = rapidbutton;
                    break;
            }

            button1 = new Button
            {
                Text = "J",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Image = skillButtonImage,
                Size = new Size(50, 50),
                Location = new Point(0, 31),
                BackColor = Color.White,
                TextAlign = ContentAlignment.TopRight,
            };
            return button1;
        }

        public Button CreateSkill2Button(Form parentForm, Button button2)
        {
            Image skillButtonImage = null;
            switch (selectedPlayerSkill2)
            {
                case "Heal":
                    skillButtonImage = medbutton;
                    break;
                case "Accelerate":
                    skillButtonImage = boostSpeedbutton;
                    break;
                case "Rapid Fire":
                    skillButtonImage = rapidbutton;
                    break;
            }

            button2 = new Button
            {
                Text = "K",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Image = skillButtonImage,
                Size = new Size(50, 50),
                Location = new Point(50, 31),
                BackColor = Color.White,
                TextAlign = ContentAlignment.TopRight,
            };
            return button2;
        }
        
        public int HealPlayer(int playerHP, int maxHP)
        {
            playerHP += 20;
            if (playerHP > maxHP)
            {
                playerHP = maxHP;
            }
            return playerHP;
        }

        public int ApplySpeedBoost(int playerSpeed)
        {
            playerSpeed = 50;
            return playerSpeed;
        }

        public int StartRapidFire(int bulletInterval)
        {
            bulletInterval = 100;

            return bulletInterval;

        }
    }
}


    

    
