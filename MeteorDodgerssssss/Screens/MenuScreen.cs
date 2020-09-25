using System;
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
            //f is the form this control is on - ("this" is the current User Control
            Form f = this.FindForm();
            f.Controls.Remove(this);

            // create an instance of the SecondScreen
            GameScreen gs = new GameScreen();
            gs.Location = new Point((f.Width - gs.Width) / 2, (f.Height - gs.Height / 2));

            // Add the user control to the form
            f.Controls.Add(gs);
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
