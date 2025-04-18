using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map_Tool_Application.src.View
{
    public class MapLabel
    {
        public Point Position { get; set; }  // Image coordinates
        public string Text { get; set; }
        public Font Font { get; set; } = new Font("Arial", 8);
        public Color Color { get; set; } = Color.Black;
        public float Angle { get; set; } = 0f;

        public bool Contains(Point p)
        {
            // 1. Translate point to label's local origin
            float dx = p.X - Position.X;
            float dy = p.Y - Position.Y;

            // 2. Reverse-rotate the point by -Angle
            float angleRad = -Angle * (float)Math.PI / 180f;
            float cos = (float)Math.Cos(angleRad);
            float sin = (float)Math.Sin(angleRad);

            float localX = dx * cos - dy * sin;
            float localY = dx * sin + dy * cos;

            // 3. Measure text size
            SizeF textSize;
            using (var bmp = new Bitmap(1, 1))
            using (var g = Graphics.FromImage(bmp))
                textSize = g.MeasureString(Text, Font);

            // 4. Check if the local point is inside the text rectangle
            return localX >= 0 && localY >= 0 && localX <= textSize.Width && localY <= textSize.Height;
        }
    }

}
