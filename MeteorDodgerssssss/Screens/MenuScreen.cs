﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace MeteorDodgerssssss
{
    public partial class MenuScreen : UserControl
    {
        public MenuScreen()
        {
            InitializeComponent();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            MainForm.ChangeScreen(this, "GameScreen");
        }

        private void HowToPlayButton_Click_1(object sender, EventArgs e)
        {
            MainForm.ChangeScreen(this, "HowToPlayScreen");
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
