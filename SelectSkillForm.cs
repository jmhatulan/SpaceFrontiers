using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game5
{
    internal class SelectSkillForm : Form
    {
        private Button skillButton1;
        private Button skillButton2;
        private Button skillButton3;
        private Button selectSkillButton;

        private Label welcomeLabel;

        private Label skillDescriptionLabel1;
        private Label skillDescriptionLabel2;
        private Label skillDescriptionLabel3;

        private Label skillNameLabel1;
        private Label skillNameLabel2;
        private Label skillNameLabel3;


        private Bitmap bg = new Bitmap(Path.Combine(Application.StartupPath, "selectskillBG.jpg"));
        private Bitmap med = new Bitmap(Path.Combine(Application.StartupPath, "medkit.png"));
        private Bitmap rapid = new Bitmap(Path.Combine(Application.StartupPath, "rapid.png"));
        private Bitmap boostSpeed = new Bitmap(Path.Combine(Application.StartupPath, "boostspeed.png"));

        private string hex = "#301934";
        private string hex2 = "#32CD32";

        public string SelectedSkill { get; private set; }

        private int formsizeW;

        public SelectSkillForm()
        {

            Color darkPurple = ColorTranslator.FromHtml(hex);
            Color lime = ColorTranslator.FromHtml(hex2);

            BackgroundImage = bg;
            Size = new Size(1280, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.MidnightBlue;

            formsizeW = this.Width;

            // Skill Button 1
            skillButton1 = new Button()
            {
                Image = med,
                Size = new Size(100, 100),
                BackColor = darkPurple,
                Location = new Point(386, 100),

            };

            skillButton1.Click += SkillButton1_Click;

            // Skill Button 2
            skillButton2 = new Button()
            {
                Image = boostSpeed,
                Size = new Size(100, 100),
                BackColor = darkPurple,
                Location = new Point(600, 100),
            };
            skillButton2.Click += SkillButton2_Click;

            // Skill Button 3
            skillButton3 = new Button()
            {
                Image = rapid,
                BackColor = darkPurple,
                Size = new Size(100, 100),
                Location = new Point(850, 100),
            };
            skillButton3.Click += SkillButton3_Click;

            selectSkillButton = new Button()
            {
                Text = "Select Skill",
                Size = new Size(120, 40),
                BackColor = darkPurple,
                ForeColor = Color.White,
                Location = new Point(formsizeW / 2 - 60, 500),
                Enabled = false,
            };
            selectSkillButton.Click += SelectSkillButton_Click;

            // Labels for skill descriptions
            skillNameLabel1 = new Label()
            {
                Text = "Heal",
                Location = new Point(formsizeW / 2 - 30, 250),
                Visible = false,

            };

            skillDescriptionLabel1 = new Label()
            {
                Text = "Instantly restores a portion of the player's\nhealth.\nCooldown: 5 seconds.",
                Visible = false,
            };

            skillNameLabel2 = new Label()
            {
                Text = "Accelerate",
                Visible = false,
                Location = new Point(formsizeW / 2 - 80, 250),
            };


            skillDescriptionLabel2 = new Label()
            {
                Text = "Temporarily boosts the player's speed,\nallowing swift movement for a short duration.\nCooldown: 10 seconds.",
                Visible = false,
            };

            skillNameLabel3 = new Label()
            {
                Text = "Rapid Fire",
                Visible = false,
                Location = new Point(formsizeW / 2 - 80, 250),
            };

            skillDescriptionLabel3 = new Label()
            {
                Text = "Temporarily supercharges your weapon,\naccelerating bullet interval for a brief period\nCooldown: 10 seconds.",
                Visible = false,
            };

            // Welcome Label
            welcomeLabel = new Label()
            {
                Text = "Select a skill",
                Location = new Point(560, 40),
                AutoSize = true,
                ForeColor = lime,
                Font = new Font("Arial", 20, FontStyle.Bold),
                BackColor = Color.Transparent,
            };

            // Add controls to the form
            Controls.Add(skillButton1);
            Controls.Add(skillButton2);
            Controls.Add(skillButton3);
            Controls.Add(selectSkillButton);

            Controls.Add(skillNameLabel1);
            Controls.Add(skillNameLabel2);
            Controls.Add(skillNameLabel3);


            Controls.Add(skillDescriptionLabel1);
            Controls.Add(skillDescriptionLabel2);
            Controls.Add(skillDescriptionLabel3);
            Controls.Add(welcomeLabel);
        }

        private void SkillButton1_Click(object sender, EventArgs e)
        {
            DisplaySkillDescription(skillDescriptionLabel1);
            DisplaySkillName(skillNameLabel1);
            selectSkillButton.Enabled = true;
        }

        private void SkillButton2_Click(object sender, EventArgs e)
        {
            DisplaySkillDescription(skillDescriptionLabel2);
            DisplaySkillName(skillNameLabel2);
            selectSkillButton.Enabled = true;
        }

        private void SkillButton3_Click(object sender, EventArgs e)
        {
            DisplaySkillDescription(skillDescriptionLabel3);
            DisplaySkillName(skillNameLabel3);
            selectSkillButton.Enabled = true;
        }

        private void DisplaySkillDescription(Label label)
        {
            Color lime = ColorTranslator.FromHtml(hex2);

            // Hide all skill description labels
            skillDescriptionLabel1.Visible = false;
            skillDescriptionLabel2.Visible = false;
            skillDescriptionLabel3.Visible = false;

            label.BackColor = Color.Transparent;
            label.Visible = true;
            label.ForeColor = lime;
            label.AutoSize = true;
            label.Font = new Font("Arial", 20, FontStyle.Bold);
            label.Location = new Point(formsizeW / 2 - 250, 320);
        }

        private void DisplaySkillName(Label label)
        {
            Color lime = ColorTranslator.FromHtml(hex2);
            // Hide all skill description labels
            skillNameLabel1.Visible = false;
            skillNameLabel2.Visible = false;
            skillNameLabel3.Visible = false;

            label.BackColor = Color.Transparent;
            label.Visible = true;
            label.ForeColor = lime;
            label.AutoSize = true;
            label.Font = new Font("Arial", 30, FontStyle.Bold);
        }

        private void SelectSkillButton_Click(object sender, EventArgs e)
        {
            // Update the selected skill variable based on the visible skill name label
            if (skillNameLabel1.Visible)
            {
                SelectedSkill = "Heal";
            }
            else if (skillNameLabel2.Visible)
            {
                SelectedSkill = "Accelerate";
            }
            else if (skillNameLabel3.Visible)
            {
                SelectedSkill = "Rapid Fire";
            }
            MessageBox.Show($"Selected Skill: {SelectedSkill}");

            DialogResult = DialogResult.OK;

            
        }

    }
}
