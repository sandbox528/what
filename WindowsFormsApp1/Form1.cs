using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Properties;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        bool onGround;
        bool goLeft = false;
        bool goRight = false;
        double xVel, yVel = 0;
        double angle = 0;

        RectangleF hitbox = new RectangleF(0, 0, 100, 100);

        int score = 0;

        Bitmap face = Resources.face;
        Bitmap gun = Resources.gun;
        public Form1()
        {

            InitializeComponent();
            

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            face = ResizeImage(face, (int) hitbox.Width, (int) hitbox.Height);
            gun = ResizeImage(gun, 150, 50);
  

            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            angle += xVel*1.5;
            angle %= 360;

            hitbox.X +=  (float) xVel;
            hitbox.Y +=  (float) yVel;


            
            if (onGround)
                xVel *= 0.9;
            yVel += 1;

            if (goLeft && xVel > -5)
                xVel -= 1;
            if (goRight && xVel < 5)
                xVel += 1;

            if (hitbox.IntersectsWith(platform.Bounds))
            {
                onGround = true;
                yVel = 0;
                hitbox.Y = platform.Top - hitbox.Height;
            }

            this.Invalidate();
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

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            Matrix rotation = new Matrix();
            PointF center = new PointF(hitbox.X + hitbox.Width/2,hitbox.Y + hitbox.Height/2);

            rotation.RotateAt((float)angle, center);
            gfx.Transform = rotation;
            gfx.DrawImage(face, hitbox.X, hitbox.Y);

            gfx.ResetTransform();

            
            Point cursorPoint = this.PointToClient(Cursor.Position);
            double gunAngle = Math.Atan2(cursorPoint.Y-center.Y, cursorPoint.X - center.X) * 180.0 / Math.PI;

            rotation = new Matrix();
            rotation.RotateAt((float)gunAngle, center);
            gfx.Transform = rotation;

            gfx.DrawImage(gun, center.X + 50, center.Y - gun.Height/2);

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

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }

}
