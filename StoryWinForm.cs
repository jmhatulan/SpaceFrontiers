using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Game5
{
    internal class StoryWinForm : Form
    {
        //Windows Form Elements Declaration
        Label dialogue = new Label();
        Label charName = new Label();
        Label instruction = new Label();
        PictureBox characterAvatar = new PictureBox();
        Button skipButton = new Button();

        //Class Variable Declaration
        private int iteration = 0;
        private string[] storyDataArray;
        private string[] tempArray;
                    

        public StoryWinForm(string textfile)
        //Label Formatting & Placement of Windows Form Elements
        {
            this.Size = new Size(1280, 720);
            this.Text = "Space Frontier";
            this.BackColor = Color.Gray;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Click += (PlayStory);

            dialogue.Location = new Point(50, 520);

            dialogue.Font = new Font("Arial", 12);
            charName.Font = new Font("Arial", 12, FontStyle.Bold);
            charName.TextAlign = ContentAlignment.MiddleCenter;

            instruction.Font = new Font("Arial", 10);
            instruction.Text = "<Click Anywhere to Continue>";
            instruction.Size = new Size(200, 15);
            instruction.Location = new Point(540, 5);

            dialogue.Size = new Size(1175, 200);
            charName.Size = new Size(150, 25);
            characterAvatar.Size = new Size(200, 200);

            skipButton.Location = new Point(1210, 5);
            skipButton.Size = new Size(50, 25);
            skipButton.Text = "Skip";
            skipButton.Click += (SkipStory);
            this.Controls.Add(skipButton);

            this.Controls.Add(dialogue);
            this.Controls.Add(charName);
            this.Controls.Add(characterAvatar);
            this.Controls.Add(instruction);

            storyDataArray = File.ReadAllLines(textfile);
        }
        public void PlayStory(object sender, EventArgs e)
        //This method will display the story in a visual novel style format.
        {
            try
            {
                if (iteration < storyDataArray.Length)
                {
                    tempArray = storyDataArray[iteration].Split(';');
                    charName.Text = tempArray[0];
                    characterAvatar.Image = Image.FromFile(tempArray[1]);
                    dialogue.Text = tempArray[3];

                    switch (tempArray[2])
                    {
                        //This will change the position of the Character's Name and Position to simulate a 'conversation'.
                        case "A":
                            charName.Location = new Point(55, 495);
                            characterAvatar.Location = new Point(75, 380);
                            break;
                        case "B":
                            charName.Location = new Point(1050, 495);
                            characterAvatar.Location = new Point(1075, 380);
                            break;
                    }
                    iteration += 1;
                }
                else
                { this.Hide(); this.Close(); }
            }
            catch (Exception)
            {
                { this.Hide(); this.Close(); }
            }
        }
        public void SkipStory(object sender, EventArgs e)
        // This will close the Story Win Forms upon clicking the skip button.
        {
            this.Hide();
            this.Close();
        }
    }
}
