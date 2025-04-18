using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace Map_Creation_Tool.src.View
{
    public partial class MainMenu : Form


    {

        private Button btnHowToUse;
        private Button btnUseYourMap;
        private Button btnDrawYourMap;
        private Button btnExit;
        private Panel headerPanel;
        private Label titleLabel;




        private IconButton currentBtn;
        private Panel leftBorderBtn;
        public MainMenu()
        {
            InitializeComponent();
            iconButton1.AutoSize = false;
            iconButton1.Padding = new Padding(0);
            iconButton1.Margin = new Padding(0);

            iconButton2.AutoSize = false;
            iconButton2.Padding = new Padding(0);
            iconButton2.Margin = new Padding(0);

            iconButton3.AutoSize = false;
            iconButton3.Padding = new Padding(0);
            iconButton3.Margin = new Padding(0);

            iconButton4.AutoSize = false;
            iconButton4.Padding = new Padding(0);
            iconButton4.Margin = new Padding(0);

            iconButton5.AutoSize = false;
            iconButton5.Padding = new Padding(0);
            iconButton5.Margin = new Padding(0);





            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(7, 60);
            panel1.Controls.Add(leftBorderBtn);
        }


        private struct RGBColors

        {

            public static Color color1 = Color.FromArgb(172, 126, 241);

            public static Color color2 = Color.FromArgb(249, 118, 176);

            public static Color color3 = Color.FromArgb(253, 138, 114);

            public static Color color4 = Color.FromArgb(95, 77, 221);

            public static Color color5 = Color.FromArgb(249, 88, 155);

            public static Color color6 = Color.FromArgb(24, 161, 251);

        }

        private void ActivateButton(object sender, Color c)
        {

            if (sender != null)
            {
                DisableButton();
                currentBtn = (IconButton)sender;
                currentBtn.BackColor = Color.FromArgb(37, 36, 81);
                currentBtn.ForeColor = c;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.IconColor = c;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;
                //left border
                leftBorderBtn.BackColor = c;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();

            }
        }


        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.FromArgb(31, 30, 68);
                currentBtn.ForeColor = Color.Gainsboro;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = Color.Gainsboro;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }

        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color2);
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color3);
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color4);
        }

        private void iconButton5_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color5);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void homeBtn_Click(object sender, EventArgs e)
        {
            reset();
        }
        private  void reset () {
            
            DisableButton();
            leftBorderBtn.Visible = false;
            }
    }
}
