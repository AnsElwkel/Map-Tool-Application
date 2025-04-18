using Map_Creation_Tool.src.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Map_Creation_Tool.src.View
{
    public partial class EntryScreen : Form
    {
        public EntryScreen()
        {
            InitializeComponent();
            CustomizeUI();
        }
        private void CustomizeUI()
        {
            this.Text = "Navigator";
            this.BackColor = Color.FromArgb(26, 0, 102); // Dark Blue
            this.ClientSize = new Size(800, 900); // Adjust form size

            // Title Label
            Label lblTitle = new Label();
            lblTitle.Text = "NAVIGATOR";
            lblTitle.Font = new Font("Arial", 75, FontStyle.Bold );
            lblTitle.ForeColor = Color.White;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.AutoSize = true;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // Center Title Horizontally
            lblTitle.Location = new Point((this.ClientSize.Width - lblTitle.Width) / 2, 150);

            this.Controls.Add(lblTitle);

            // Get Started Button
            RoundedButton btn = new RoundedButton();
            btn.Text = "Get started!";
            btn.Size = new Size(200, 50);
            btn.Left = (this.ClientSize.Width - btn.Width) / 2;

            btn.Top = (this.ClientSize.Height - btn.Height) / 2 + 200;
            btn.Click += BtnNavigate_Click;
            this.Controls.Add(btn);
            this.PerformLayout();
        }
        private void EntryScreen_Load(object sender, EventArgs e)
        {
          

        }
        private void BtnNavigate_Click(object sender, EventArgs e)
        {
           
         
        }
    }
}
