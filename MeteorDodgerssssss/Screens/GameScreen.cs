/// Created by Declan Feore
/// September 2020
/// 

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Media;

namespace MeteorDodgerssssss
{
    public partial class GameScreen : UserControl
    {
        //player button control keys 
        Boolean leftArrowDown, rightArrowDown;

        int playerX, playerY, playerSize, playerSpeed, playerHealth, playerScore; //player variables
        int meteorSize, meteorSpeed, meteorDraw; //meteor variables
        int boostSize, boostSpeed, boostDraw; //boost variables
        List<int> meteorsX = new List<int>(); //list of meteor x locations
        List<int> meteorsY = new List<int>(); //list of meteor y locations
        List<int> boostsX = new List<int>(); //list of boost x locations
        List<int> boostsY = new List<int>(); //list of boost y locations
        SolidBrush playerBrush = new SolidBrush(Color.LightBlue); //player brush
        SolidBrush meteorBrush = new SolidBrush(Color.DarkRed); //meteor brush
        SolidBrush boostBrush = new SolidBrush(Color.Yellow); //boost brush
        Random randGen = new Random(); //random generator for all random values 
        SoundPlayer boostPlayer = new SoundPlayer(Properties.Resources.Boost); //sound that plays when player collides with a boost
        SoundPlayer hitPlayer = new SoundPlayer(Properties.Resources.Hit); //sound that plays when player collides with a meteor
        SoundPlayer deathPlayer = new SoundPlayer(Properties.Resources.Death); //sound that plays when player dies

        public GameScreen()
        {
            InitializeComponent();
            InitializeGameValues();
            ScoreboardValues();
        }

        public void InitializeGameValues()
        {
            //Initial player values 
            playerX = this.Width / 2;
            playerY = this.Height - 30;
            playerSize = 30;
            playerSpeed = 10;
            playerHealth = 3;
            playerScore = 1;

            //Initial meteor values 
            meteorsX.Add(randGen.Next(1, this.Width - meteorSize));
            meteorsY.Add(0);
            meteorSize = 30;
            meteorSpeed = 10;

            //Initial boost values
            boostsX.Add(randGen.Next(1, this.Width - meteorSize));
            boostsY.Add(0);
            boostSize = 30;
            boostSpeed = 10;
        }

        public void ScoreboardValues()
        {
            //Scoreboard values at the start of each level
            healthLabel.Text = "Health: " + playerHealth;
            scoreLabel.Text = "Score: " + playerScore;
        }

        public void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //Opens a pause screen if escape is pressed. 
            //Depending on what is pressed on pause screen the program will either continue or exit to main menu.
            if (e.KeyCode == Keys.Escape && gameTimer.Enabled)
            {
                gameTimer.Enabled = false;
                rightArrowDown = leftArrowDown = false;

                DialogResult result = PauseForm.Show();

                if (result == DialogResult.Cancel)
                {
                    gameTimer.Enabled = true;
                }
                else if (result == DialogResult.Abort)
                {
                    MainForm.ChangeScreen(this, "MenuScreen");
                }
            }

            //player button presses
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
            }
        }

        public void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //player button releases
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
            }
        }

        public void gameTimer_Tick(object sender, EventArgs e)
        {
            //TODO move main character 
            if (leftArrowDown == true && playerX > 0)
            {
                playerX = playerX - playerSpeed;
            }
            if (rightArrowDown == true && playerX < this.Width - playerSize)
            {
                playerX = playerX + playerSpeed;
            }

            //add new meteor x and y to meteor lists
            meteorDraw = randGen.Next(1, 10);
            if (meteorDraw == 1)
            {
                meteorsX.Add(randGen.Next(1, this.Width - meteorSize));
                meteorsY.Add(0);
            }

            //move meteors
            for (int i = 0; i < meteorsY.Count(); i++)
            {
                meteorsY[i] = meteorsY[i] + meteorSpeed;
            }

            //make meteors faster if player score surpases a multiple of 50
            if (playerScore % 50 == 0)
            {
                meteorSpeed++;
            }

            //add new health boost x and y to health boost lists
            boostDraw = randGen.Next(1, 100);
            if (boostDraw == 1)
            {
                boostsX.Add(randGen.Next(1, this.Width - meteorSize));
                boostsY.Add(0);
            }

            //move health boosts
            for (int i = 0; i < boostsY.Count(); i++)
            {
                boostsY[i] = boostsY[i] + boostSpeed;
            }

            //player rectangle to check collisions
            Rectangle playerRec = new Rectangle(playerX, playerY, playerSize, playerSize); 

            //checking to see if the player gets hit by a meteor
            for (int i = 0; i < meteorsX.Count; i++)
            {
                Rectangle meteorRec = new Rectangle(meteorsX[i], meteorsY[i], meteorSize, meteorSize);

                if (playerRec.IntersectsWith(meteorRec))
                {
                    hitPlayer.Play();
                    playerHealth = playerHealth - 1;
                    healthLabel.Text = "Health: " + playerHealth;
                    meteorsX.RemoveAt(i);
                    meteorsY.RemoveAt(i);
                    break;
                }
                else
                {
                    playerHealth = playerHealth * 1;
                }
            }

            //checking to see if the player gets hit by a health boost
            for (int i = 0; i < boostsX.Count; i++)
            {
                Rectangle boostRec = new Rectangle(boostsX[i], boostsY[i], boostSize, boostSize);

                if (playerRec.IntersectsWith(boostRec))
                {
                    boostPlayer.Play();
                    playerHealth = playerHealth + 1;
                    healthLabel.Text = "Health: " + playerHealth;
                    boostsX.RemoveAt(0);
                    boostsY.RemoveAt(0);
                    break;
                }
                else
                {
                    playerHealth = playerHealth * 1;
                }
            }

            //remove meteor from list if it goes off the screen
            if (meteorsY.Count() > 0)
            {
                if (meteorsY[0] > this.Height)
                {
                    meteorsX.RemoveAt(0);
                    meteorsY.RemoveAt(0);

                    //player gets a point because they have not been hit by that meteor
                    playerScore = playerScore + 1;
                    scoreLabel.Text = "Score: " + playerScore;
                }
            }               

            //remove boost from list if it goes off the screen
            if (boostsY.Count() > 0)
            {
                if (boostsY[0] > this.Height)
                {
                    boostsX.RemoveAt(0);
                    boostsY.RemoveAt(0);
                }
            }

            //stop the game if player health is zero
            if (playerHealth == 0)
            {
                deathPlayer.Play();
                gameTimer.Enabled = false;
                DeathOptions(); 
            }            

            //calls the GameScreen_Paint method to draw the screen.
            Refresh();
        }

        public void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //draw player to screen
            e.Graphics.FillRectangle(playerBrush, playerX, playerY, playerSize, playerSize);

            //draw boost to screen
            for (int i = 0; i < boostsX.Count; i++)
            {
                e.Graphics.FillRectangle(boostBrush, boostsX[i], boostsY[i], boostSize, boostSize);
            }

            //draw meteor to screen
            for (int i = 0; i < meteorsX.Count; i++)
            {
                e.Graphics.FillEllipse(meteorBrush, meteorsX[i], meteorsY[i], meteorSize, meteorSize);
            }
        }

        public void DeathOptions()
        {
            //shows the two options the player has
            retryButton.Enabled = true;
            retryButton.Visible = true;
            quitButton.Enabled = true;
            quitButton.Visible = true;
        }

        public void RetryButton_Click(object sender, EventArgs e)
        {
            //restart game 
            MainForm.ChangeScreen(this, "GameScreen");
        }

        public void QuitButton_Click(object sender, EventArgs e)
        {
            //go back to the starting menu
            MainForm.ChangeScreen(this, "MenuScreen");
        }

        private void button_Enter(object sender, EventArgs e)
        {
            //start by changing all the buttons to the default colour
            foreach (Control c in this.Controls)
            {
                if (c is Button)
                    c.BackColor = Color.DarkRed;
            }

            //change the current button to the active colour
            Button btn = (Button)sender;
            btn.BackColor = Color.Orange;
        }
    }

}
