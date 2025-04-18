using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Map_Creation_Tool.src.Model
{

    public partial class PathTypeDialog : Form
    {

        private void InitializeComponent()
        {
            components = new Container();
            btnFastest = new Button();
            btnShortest = new Button();
            label3 = new Label();
            imageList1 = new ImageList(components);
            imageList2 = new ImageList(components);
            pictureBox2 = new PictureBox();
            ((ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // btnFastest
            // 
            btnFastest.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnFastest.Location = new Point(12, 173);
            btnFastest.Name = "btnFastest";
            btnFastest.Size = new Size(94, 29);
            btnFastest.TabIndex = 0;
            btnFastest.Text = "fastest";
            btnFastest.UseVisualStyleBackColor = true;
            btnFastest.Click += button1_Click;
            // 
            // btnShortest
            // 
            btnShortest.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnShortest.Location = new Point(224, 173);
            btnShortest.Name = "btnShortest";
            btnShortest.Size = new Size(94, 29);
            btnShortest.TabIndex = 1;
            btnShortest.Text = "shortest";
            btnShortest.UseVisualStyleBackColor = true;
            btnShortest.Click += button2_Click;
            // 
            // label3
            // 
            label3.Location = new Point(111, 150);
            label3.Name = "label3";
            label3.Size = new Size(127, 20);
            label3.TabIndex = 4;
            label3.Text = "select your choice";
            label3.Click += label3_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(16, 16);
            imageList1.TransparentColor = Color.Transparent;
            // 
            // imageList2
            // 
            imageList2.ColorDepth = ColorDepth.Depth32Bit;
            imageList2.ImageSize = new Size(16, 16);
            imageList2.TransparentColor = Color.Transparent;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.logo2;
            pictureBox2.Location = new Point(26, 12);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(292, 115);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 6;
            pictureBox2.TabStop = false;
            // 
            // PathTypeDialog
            // 
            ClientSize = new Size(355, 226);
            Controls.Add(pictureBox2);
            Controls.Add(label3);
            Controls.Add(btnShortest);
            Controls.Add(btnFastest);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            Name = "PathTypeDialog";
            Load += PathTypeDialog_Load;
            ((ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);

        }

        public enum PathChoice
        {
            FASTEST,
            SHORTEST,
            CANCEL
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PathChoice SelectedChoice { get; private set; } = PathChoice.CANCEL;

        public PathTypeDialog()
        {
            InitializeComponent();
            btnFastest.Click += (s, e) => { SelectedChoice = PathChoice.FASTEST; this.DialogResult = DialogResult.OK; };
            btnShortest.Click += (s, e) => { SelectedChoice = PathChoice.SHORTEST; this.DialogResult = DialogResult.OK; };
        }

        private void PathTypeDialog_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private Button btnFastest;
        private Label label3;
        private ImageList imageList1;
        private IContainer components;
        private ImageList imageList2;
        private PictureBox pictureBox2;
        private Button btnShortest;

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }



}
