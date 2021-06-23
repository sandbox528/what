using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        bool onGround;
        bool goLeft = false;
        bool goRight = false;
        double xVel, yVel = 0;
        double xPos, yPos = 0;

        int force = 8;
        int score = 0;
        public Form1()
        {
            InitializeComponent();
            xPos = player.Left;
            yPos = player.Top;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            yPos += yVel;
            xPos += xVel;
            player.Left = (int)xPos;
            player.Top = (int)yPos;
            if (onGround)
                xVel *= 0.9;
            yVel += 1;

            if (goLeft && xVel > -5)
                xVel -= 1;
            if (goRight && xVel < 5)
                xVel += 1;

            

            foreach (Control x in this.Controls)
            {
                System.Console.WriteLine(x.Tag);
                if (x is PictureBox && x.Tag == "platform")
                                    {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        onGround = true;
                        yVel = 0;
                        yPos = x.Top - player.Height;
                        player.Top = (int)yPos;
                    }
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Image imag = player.Image;
            e.Graphics.DrawImage(imag, new Point(0, 0));
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode) {
                case Keys.Left:
                case Keys.A:
                    goLeft = true;
                    break;
                case Keys.Right:
                case Keys.D:
                    goRight = true;
                    break;
                case Keys.Space:
                    if (onGround)
                        yVel = -15;
                        onGround = false;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.A:
                    goLeft = false;
                    break;
                case Keys.Right:
                case Keys.D:
                    goRight = false;
                    break;
            }
        }
    }
}
