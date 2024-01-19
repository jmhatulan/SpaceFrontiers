using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game5
{
    internal class ArmoryWinForm : Form
    {
        //Windows Form Elements Declaration
        PictureBox hpUpgrade = new PictureBox();
        PictureBox atkUpgrade = new PictureBox();
        PictureBox spdUpgrade = new PictureBox();

        Button upgradeHPButton = new Button();
        Button upgradeAtkButton = new Button();
        Button upgradeSpeedButton = new Button();

        Label upgradeInfoHP = new Label();
        Label upgradeInfoAtk = new Label();
        Label upgradeInfoSpeed = new Label();
        Label currencyText = new Label();
        Label currencyValue = new Label();

        Label costHPInfo = new Label();
        Label costAtkInfo = new Label();
        Label costSpdInfo = new Label();

        Button exitButton = new Button();

        //Other declarations
        PlayerUpgrades pu = new PlayerUpgrades();
        Currency currency = new Currency();
        private string[] upgradesArray;
        private int upgradeCost = 500;
        private int tempNum;

        public ArmoryWinForm()
        //Label Formatting & Placement of Windows Form Elements
        {
            this.Size = new Size(1280, 720);
            this.Text = "Armory";
            this.BackColor = Color.Gray;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;

            exitButton.Location = new Point(5, 5);
            exitButton.Size = new Size(100, 20);
            exitButton.Text = "Back to Menu";
            exitButton.Click += (ExitArmory);
            this.Controls.Add(exitButton);

            currencyText.Location = new Point(1080, 7);
            currencyText.Size = new Size(70, 20);
            currencyText.Font = new Font("Arial", 12, FontStyle.Bold);
            currencyText.Text = "Credits:";

            currencyValue.Location = new Point(1160, 7);
            currencyValue.Size = new Size(100, 20);
            currencyValue.Font = new Font("Arial", 12, FontStyle.Bold);

            hpUpgrade.Location = new Point(370, 100);
            atkUpgrade.Location = new Point(370, 200);
            spdUpgrade.Location = new Point(370, 300);

            costHPInfo.Location = new Point(900, 100);
            costAtkInfo.Location = new Point(900, 200);
            costSpdInfo.Location = new Point(900, 300);

            hpUpgrade.Size = new Size(400, 100);
            hpUpgrade.MouseHover += (DisplayInfoHP);
            hpUpgrade.MouseLeave += (HideInfo);
            atkUpgrade.Size = new Size(400, 100);
            atkUpgrade.MouseHover += (DisplayInfoAtk);
            atkUpgrade.MouseLeave += (HideInfo);
            spdUpgrade.Size = new Size(400, 100);
            spdUpgrade.MouseHover += (DisplayInfoSpd);
            spdUpgrade.MouseLeave += (HideInfo);

            upgradeHPButton.Location = new Point(820, 100);
            upgradeAtkButton.Location = new Point(820, 200);
            upgradeSpeedButton.Location = new Point(820, 300);

            upgradeHPButton.Size = new Size(75, 50);
            upgradeAtkButton.Size = new Size(75, 50);
            upgradeSpeedButton.Size = new Size(75, 50);

            upgradeHPButton.MouseHover += (DisplayCostHP);
            upgradeHPButton.MouseLeave += (HideInfo);
            upgradeAtkButton.MouseHover += (DisplayCostAtk);
            upgradeAtkButton.MouseLeave += (HideInfo);
            upgradeSpeedButton.MouseHover += (DisplayCostSpd);
            upgradeSpeedButton.MouseLeave += (HideInfo);

            upgradeHPButton.Text = "UPGRADE";
            upgradeHPButton.Click += (UpgradeHPStat);

            upgradeAtkButton.Text = "UPGRADE";
            upgradeAtkButton.Click += (UpgradeAtkStat);

            upgradeSpeedButton.Text = "UPGRADE";
            upgradeSpeedButton.Click += (UpgradeSpdStat);

            upgradeInfoHP.Text = "This will increase your spaceship's maximum HP by 10 each upgrade. Note: This is only applicable in Survival Mode.";
            upgradeInfoAtk.Text = "This will increase your spaceship's damage by 1 each upgrade. Note: This is only applicable in Survival Mode.";
            upgradeInfoSpeed.Text = "This will increase your spaceship's maneuverability. Note: This is only applicable in Survival Mode.";
            upgradeInfoHP.Hide();
            upgradeInfoAtk.Hide();
            upgradeInfoSpeed.Hide();

            costHPInfo.Text = "Cost: 500 Credits";
            costAtkInfo.Text = "Cost: 500 Credits";
            costSpdInfo.Text = "Cost: 500 Credits";
            costHPInfo.Hide();
            costAtkInfo.Hide();
            costSpdInfo.Hide();

            upgradeInfoHP.Size = new Size(175, 50);
            upgradeInfoAtk.Size = new Size(175, 50);
            upgradeInfoSpeed.Size = new Size(175, 50);

            upgradeInfoHP.Location = new Point(175, 100);
            upgradeInfoAtk.Location = new Point(175, 200);
            upgradeInfoSpeed.Location = new Point(175, 300);

            this.Controls.Add(hpUpgrade);
            this.Controls.Add(atkUpgrade);
            this.Controls.Add(spdUpgrade);
            this.Controls.Add(upgradeHPButton);
            this.Controls.Add(upgradeAtkButton);
            this.Controls.Add(upgradeSpeedButton);
            this.Controls.Add(upgradeInfoHP);
            this.Controls.Add(upgradeInfoAtk);
            this.Controls.Add(upgradeInfoSpeed);
            this.Controls.Add(currencyText);
            this.Controls.Add(currencyValue);
            this.Controls.Add(costHPInfo);
            this.Controls.Add(costAtkInfo);
            this.Controls.Add(costSpdInfo);

            pu.RetrieveUpgradeLevels();
            currency.GetCurrency();
            upgradesArray = pu.UpgradeLevels;
            UpdateArmoryForm();
        }
        public void UpdateArmoryForm()
        //This will update the windows form elements based on the change/s on the class properties of and/or Currency & PlayerUpgrades.
        {
            switch (upgradesArray[0])
            {
                case "0":
                    hpUpgrade.Image = Image.FromFile("hp-upgrade0.png");
                    break;
                case "1":
                    hpUpgrade.Image = Image.FromFile("hp-upgrade1.png");
                    break;
                case "2":
                    hpUpgrade.Image = Image.FromFile("hp-upgrade2.png");
                    break;
                case "3":
                    hpUpgrade.Image = Image.FromFile("hp-upgrade3.png");
                    break;
                case "4":
                    hpUpgrade.Image = Image.FromFile("hp-upgrade4.png");
                    break;
                case "5":
                    hpUpgrade.Image = Image.FromFile("hp-upgrade5.png");
                    break;
                case "6":
                    hpUpgrade.Image = Image.FromFile("hp-upgrade6.png");
                    break;
                case "7":
                    hpUpgrade.Image = Image.FromFile("hp-upgrade7.png");
                    break;
                case "8":
                    hpUpgrade.Image = Image.FromFile("hp-upgrade8.png");
                    break;
                case "9":
                    hpUpgrade.Image = Image.FromFile("hp-upgrade9.png");
                    break;
                case "10":
                    hpUpgrade.Image = Image.FromFile("hp-upgrade10.png");
                    upgradeHPButton.Enabled = false;
                    break;
            }
            switch (upgradesArray[1])
            {
                case "0":
                    atkUpgrade.Image = Image.FromFile("atk-upgrade0.png");
                    break;
                case "1":
                    atkUpgrade.Image = Image.FromFile("atk-upgrade1.png");
                    break;
                case "2":
                    atkUpgrade.Image = Image.FromFile("atk-upgrade2.png");
                    break;
                case "3":
                    atkUpgrade.Image = Image.FromFile("atk-upgrade3.png");
                    break;
                case "4":
                    atkUpgrade.Image = Image.FromFile("atk-upgrade4.png");
                    break;
                case "5":
                    atkUpgrade.Image = Image.FromFile("atk-upgrade5.png");
                    break;
                case "6":
                    atkUpgrade.Image = Image.FromFile("atk-upgrade6.png");
                    break;
                case "7":
                    atkUpgrade.Image = Image.FromFile("atk-upgrade7.png");
                    break;
                case "8":
                    atkUpgrade.Image = Image.FromFile("atk-upgrade8.png");
                    break;
                case "9":
                    atkUpgrade.Image = Image.FromFile("atk-upgrade9.png");
                    break;
                case "10":
                    atkUpgrade.Image = Image.FromFile("atk-upgrade10.png");
                    upgradeAtkButton.Enabled = false;
                    break;
            }
            switch (upgradesArray[2])
            {
                case "0":
                    spdUpgrade.Image = Image.FromFile("spd-upgrade0.png");
                    break;
                case "1":
                    spdUpgrade.Image = Image.FromFile("spd-upgrade1.png");
                    break;
                case "2":
                    spdUpgrade.Image = Image.FromFile("spd-upgrade2.png");
                    break;
                case "3":
                    spdUpgrade.Image = Image.FromFile("spd-upgrade3.png");
                    break;
                case "4":
                    spdUpgrade.Image = Image.FromFile("spd-upgrade4.png");
                    break;
                case "5":
                    spdUpgrade.Image = Image.FromFile("spd-upgrade5.png");
                    break;
                case "6":
                    spdUpgrade.Image = Image.FromFile("spd-upgrade6.png");
                    break;
                case "7":
                    spdUpgrade.Image = Image.FromFile("spd-upgrade7.png");
                    break;
                case "8":
                    spdUpgrade.Image = Image.FromFile("spd-upgrade8.png");
                    break;
                case "9":
                    spdUpgrade.Image = Image.FromFile("spd-upgrade9.png");
                    break;
                case "10":
                    spdUpgrade.Image = Image.FromFile("spd-upgrade10.png");
                    upgradeSpeedButton.Enabled = false;
                    break;
            }
            currencyValue.Text = currency.CurrencyAmount.ToString();

        }

        //The methods below are the function behind the upgrade buttons.
        //The method will check if the player has sufficient credits. It will call another method if the player has insufficient funds.
        public void UpgradeHPStat(object sender, EventArgs e)
        {
            if (currency.CurrencyAmount >= upgradeCost)
            {
                tempNum = int.Parse(upgradesArray[0]);
                tempNum += 1;
                upgradesArray[0] = tempNum.ToString();
                currency.SpendCurrency(upgradeCost);
                pu.UpdateUpgradeLevels(upgradesArray);
                UpdateArmoryForm();
            }
            else
            { InsufficientCreditMessage(); }
        }
        public void UpgradeAtkStat(object sender, EventArgs e)
        {
            if (currency.CurrencyAmount >= upgradeCost)
            {
                tempNum = int.Parse(upgradesArray[1]);
                tempNum += 1;
                upgradesArray[1] = tempNum.ToString();
                currency.SpendCurrency(upgradeCost);
                pu.UpdateUpgradeLevels(upgradesArray);
                UpdateArmoryForm();
            }
            else
            { InsufficientCreditMessage(); }
        }
        public void UpgradeSpdStat(object sender, EventArgs e)
        {
            if (currency.CurrencyAmount >= upgradeCost)
            {
                tempNum = int.Parse(upgradesArray[2]);
                tempNum += 1;
                upgradesArray[2] = tempNum.ToString();
                currency.SpendCurrency(upgradeCost);
                pu.UpdateTextFile(upgradesArray);
                UpdateArmoryForm();
            }
            else
            { InsufficientCreditMessage(); }
        }
        public void InsufficientCreditMessage()
        //This method will notify player that he/she has insufficient funds and cannot upgrade anything.
        {
            string message = "You have insufficient credits to purchase upgrades.";
            string caption = "Insufficient Credits";
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        //The following methods will display some information about the upgrades.
        //These methods will be called via mouse hover over the picture of upgrades.
        public void DisplayInfoHP(object sender, EventArgs e)
        {
            upgradeInfoHP.Show();
        }
        public void DisplayInfoAtk(object sender, EventArgs e)
        {
            upgradeInfoAtk.Show();
        }
        public void DisplayInfoSpd(object sender, EventArgs e)
        {
            upgradeInfoSpeed.Show();
        }
        public void DisplayCostHP(object sender, EventArgs e)
        {
            costHPInfo.Show();
        }
        public void DisplayCostAtk(object sender, EventArgs e)
        {
            costAtkInfo.Show();
        }
        public void DisplayCostSpd(object sender, EventArgs e)
        {
            costSpdInfo.Show();
        }
        public void HideInfo(object sender, EventArgs e)
        //This method will hide upgrade information once the mouse is not hovering over the pictures of upgrades anymore.
        {
            upgradeInfoHP.Hide();
            upgradeInfoAtk.Hide();
            upgradeInfoSpeed.Hide();
            costHPInfo.Hide();
            costAtkInfo.Hide();
            costSpdInfo.Hide();
        }
        public void ExitArmory(object sender, EventArgs e)
        //This will execute once the exit button is clicked and will close the armory form.
        {
            this.Hide();
            this.Close();
            MyForm mf = new MyForm();
            mf.Show();
        }
    }
}
