using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map_Creation_Tool.src.Model
{
    internal class MenuButton:Button
    {
        public MenuButton(string text,int index=0, Image? icon = null) {
            this.Text = text;
            this.Size = new Size(300, 60);
            this.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            this.FlatStyle = FlatStyle.Flat;
            this.BackColor = Color.FromArgb(80, 40, 120);
            this.ForeColor = Color.White;
            this.Cursor = Cursors.Hand;
            this.TabIndex = index;
            this.Tag = index;


            this.FlatAppearance.BorderColor = Color.FromArgb(150, 100, 200);
            this.FlatAppearance.BorderSize = 2;
            //this.Location = new Point(50, 40 + index * 80);



            if (icon != null)
            {
                this.Image = icon;
                this.ImageAlign = ContentAlignment.MiddleLeft; // Adjust alignment if needed
                this.TextAlign = ContentAlignment.MiddleRight; // Keep text readable
                this.Padding = new Padding(5, 0, 5, 0); // Add padding for spacing
                this.ForeColor=Color.White;
            }


            this.MouseEnter += (s, e) =>
            {
                this.BackColor = Color.FromArgb(120, 60, 180);
                this.FlatAppearance.BorderColor = Color.FromArgb(200, 180, 255);
            };

            this.MouseLeave += (s, e) =>
            {
                this.BackColor = Color.FromArgb(80, 40, 120);
                this.FlatAppearance.BorderColor = Color.FromArgb(150, 100, 200);
            };

          

        }
        


    }
}
