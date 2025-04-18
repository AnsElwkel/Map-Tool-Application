namespace Map_Creation_Tool.src.View
{
    partial class EntryScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // EntryScreen
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(0, 0, 192);
            BackgroundImage = Properties.Resources.firstScreen;
            ClientSize = new Size(1920, 1078);
            Font = new Font("Elephant", 19.9999981F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(10, 7, 10, 7);
            MaximumSize = new Size(1920, 1080);
            MinimumSize = new Size(1918, 1078);
            Name = "EntryScreen";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "EntryScreen";
            WindowState = FormWindowState.Maximized;
            Load += EntryScreen_Load;
            ResumeLayout(false);
        }

        #endregion
    }
}