using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map_Creation_Tool.src.Model
{
    internal class Logo : PictureBox
    {
        public Logo()
        {
            this.Size = new Size(120, 120);
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.BackColor = Color.Transparent;
            this.Image = GenerateCompassLogo();
        }
        private Bitmap GenerateCompassLogo()
        {
            Bitmap logoImage = new Bitmap(120, 120);
            using (Graphics graphics = Graphics.FromImage(logoImage))
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.Clear(Color.Transparent);

                using (Pen pen = new Pen(Color.White, 3))
                {
                    graphics.DrawEllipse(pen, 10, 10, 100, 100);

                }

                string[] directions = { "N", "E", "S", "W" };
                Point[] positions = {
                new Point(60, 20),  // N
                new Point(100, 60), // E
                new Point(60, 100), // S
                new Point(20, 60)   // W
            };



                for (int i = 0; i < 4; i++)
                {
                    using (Font font = new Font("Arial", 14, FontStyle.Bold))
                    using (SolidBrush brush = new SolidBrush(Color.White))
                    {
                        SizeF size = graphics.MeasureString(directions[i], font);
                        graphics.DrawString(directions[i], font, brush,
                            positions[i].X - size.Width / 2,
                            positions[i].Y - size.Height / 2);
                    }
                }
                using (Pen redPen = new Pen(Color.Red, 2))
                using (Pen whitePen = new Pen(Color.White, 2))
                {
                    graphics.DrawLine(redPen, 60, 60, 60, 25); // North (red)
                    graphics.DrawLine(whitePen, 60, 60, 60, 95); // South (white)
                    graphics.DrawLine(whitePen, 60, 60, 25, 60); // West (white)
                    graphics.DrawLine(whitePen, 60, 60, 95, 60); // East (white)
                }
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    graphics.FillEllipse(brush, 55, 55, 10, 10);
                }

            }
            return logoImage;

        }
    }
}
