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
    public partial class GuideForm : Form
    {
        int TotalSteps;
        private string BACKGROUND_PATH = "../../../res/Assets/firstScreen.png";
        private Image homeIcon = Image.FromFile("../../../res/Assets/home.png");
        private Image rightArrowIcon = Image.FromFile("../../../res/Assets/right arrow.png");
        private Image leftArrowIcon = Image.FromFile("../../../res/Assets/left arrow.png");

        private PictureBox stepImageBox;
        private PictureBox logoBox;
        private MenuButton backButton;
        private Label titleLabel;
        private Label stepNumberLabel;
        private Label stepDescriptionLabel;
        private int currentStep = 1;
        Panel contentPanel;
        private MenuButton nextSlideButton;
        private MenuButton previousSlideButton;
        Label stepTitleLabel;


        private Image[] images = {

                    Image.FromFile("../../../res/Assets/guide_steps/1.png"),
                    Image.FromFile("../../../res/Assets/guide_steps/2.png"),
                    Image.FromFile("../../../res/Assets/guide_steps/4.jpg"),
                    Image.FromFile("../../../res/Assets/guide_steps/12.jpg"),
                    Image.FromFile("../../../res/Assets/guide_steps/13.jpg"),
                    Image.FromFile("../../../res/Assets/guide_steps/5.jpg"),
                    Image.FromFile("../../../res/Assets/guide_steps/6.jpg"),
                    Image.FromFile("../../../res/Assets/guide_steps/14.png"),
                    Image.FromFile("../../../res/Assets/guide_steps/15.png"),
                    Image.FromFile("../../../res/Assets/guide_steps/16.png"),
                    Image.FromFile("../../../res/Assets/guide_steps/18.png"),
                    Image.FromFile("../../../res/Assets/guide_steps/19.png"),
                    Image.FromFile("../../../res/Assets/guide_steps/20.png"),

                };

        private string[] stepTitles = {
            "Start Creating Your Map",
            "Place Buildings and exits",
            "Connect Places with Paths",
            "Label Map Locations",
            "The final Results",
            "Save the Map as Image and text",
            "Load a Map to Navigate",
            "Use the Right Colors (RGB)",
            "Pick Start and End Points",
            "Choose Route Type",
            "Visualize the Path",
            "Shortest Path And Fastest Path",
            "Team of the project",
        };


        private string[] stepDescriptions = {
            "Open the Map Drawer tool to begin building your custom navigation map from scratch or using an existing layout.",

            "Add locations like rooms, or landmarks. These will be marked with special text labels on the map. but you must add exits (in green) to each buiding \r\n\r\n" +
            "Use the **Undo** and **Redo** buttons to correct any mistakes while placing locations or exits.",

            "Draw paths connecting places. You can choose the type of path: Normal, Busy, or Very Busy.",

            "Assign clear names or IDs to key locations—customize them with colors, sizes, or styles for better navigation and search results.",

            "Here is the final results of the map after drawing all possible paths and placing the texts",

            "Click on 'Save' to export your map as a .png and a .txt file. This saves the visual structure and colors in addition to the lables of each place.",

                    "In the main app, choose either:Your saved image + text file\r\n\rOr another RGB-colored map from your device",

                    "If you're creating your own map image, use these RGB values.",

                    "Click on the map to select your start and end points. Choose a starting place that begins with a green door, and avoid white places as they are obstacles.",

                    "Decide the kind of route:Fastest – Less time (ignores traffic)\nShortest – Less cost (avoids busy paths)",

                    "The system draws circles along the calculated path to guide you visually from start to finish.\n\r Here is the Shortest Path for example.",

                        "Here Is the Fastest path",

                        ""
        };

        public GuideForm()
        {
            TotalSteps = stepTitles.Length;
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            SuspendLayout();
            ClientSize = new Size(1920, 1080);
            Name = "GuideForm";
            Text = "GuideForm";
            Load += GuideForm_Load;
            
            logoBox = new Logo();

            this.Controls.Add(logoBox);
            this.FormBorderStyle = FormBorderStyle.None;
            this.KeyPreview = true;
            this.KeyDown += GuideForm_KeyDown;

            backButton = new MenuButton("Back To Main Menu", 0, homeIcon);
            backButton.Location = new Point(10, 20);
            backButton.Click += backButton_Click;
            this.Controls.Add(backButton);

            titleLabel = new Label
            {
                Text = "How To Use Map Navigator",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true

            };

            this.Controls.Add(titleLabel);


            contentPanel = new Panel
            {
                Size = new Size(900, 530),
                BackColor = Color.FromArgb(100, 20, 20, 60),
                Padding = new Padding(20)
            };
            this.Controls.Add(contentPanel);

            currentStep = 1;
            if (File.Exists(BACKGROUND_PATH))
            {
                this.BackgroundImage = Image.FromFile(BACKGROUND_PATH);
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                MessageBox.Show("Background image not found: " + BACKGROUND_PATH, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            stepNumberLabel = new Label
            {
                Text = $"STEP {currentStep} OF {TotalSteps}",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 180, 180),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(200, 40),
                Location = new Point(350, 20)
            };

            contentPanel.Controls.Add(stepNumberLabel);



            stepImageBox = new PictureBox
            {
                Size = new Size(400, 250),
                Location = new Point(250, 110),
                BackColor = Color.FromArgb(60, 30, 100),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = images[0]
            };
            contentPanel.Controls.Add(stepImageBox);




            nextSlideButton = new MenuButton("Next", 0, rightArrowIcon);

            previousSlideButton = new MenuButton("Previous", 0, leftArrowIcon);
            nextSlideButton.Location = new Point(600, 450);
            previousSlideButton.Location = new Point(100, 450);
            previousSlideButton.Height = nextSlideButton.Height = 55;
            previousSlideButton.Width = nextSlideButton.Width = 180;
            nextSlideButton.ImageAlign = ContentAlignment.MiddleRight;
            nextSlideButton.TextAlign = ContentAlignment.MiddleLeft;
            contentPanel.Controls.Add(nextSlideButton);

            contentPanel.Controls.Add(previousSlideButton);

            nextSlideButton.Click += NextSlideButton_Click;
            previousSlideButton.Click += PreviousSlideButton_Click;
            previousSlideButton.Enabled = false;




            stepTitleLabel = new Label
            {
                Text = stepTitles[0],
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(500, 40),
                Location = new Point(250, 60)
            };
            contentPanel.Controls.Add(stepTitleLabel);





            stepDescriptionLabel = new Label
            {
                Text = stepDescriptions[0],
                Font = new Font("Segoe UI", 14, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.TopLeft,
                Size = new Size(860, 100),
                Location = new Point(20, 370)
            };
            contentPanel.Controls.Add(stepDescriptionLabel);


            this.Resize += (s, e) => AdjustLayout();

            ResumeLayout(false);
        }

        private void PreviousSlideButton_Click(object? sender, EventArgs e)
        {
            if (currentStep > 1)
            {
                currentStep--;
                stepNumberLabel.Text = $"STEP {currentStep} OF {TotalSteps}";
                stepTitleLabel.Text = stepTitles[currentStep - 1];
                stepDescriptionLabel.Text = stepDescriptions[currentStep - 1];
                stepImageBox.Image = images[currentStep - 1];
                // disable 
                previousSlideButton.Enabled = currentStep > 1;
                nextSlideButton.Enabled = true; // Enable "Next" button
            }
        }
        private void NextSlideButton_Click(object? sender, EventArgs e)
        {
            if (currentStep < TotalSteps)
            {
                currentStep++;
                stepNumberLabel.Text = $"STEP {currentStep} OF {TotalSteps}";
                stepTitleLabel.Text = stepTitles[currentStep - 1];
                stepDescriptionLabel.Text = stepDescriptions[currentStep - 1];
                stepImageBox.Image = images[currentStep - 1];

                //disable button
                nextSlideButton.Enabled = currentStep < TotalSteps;
                previousSlideButton.Enabled = true;

            }

        }
        private void AdjustLayout()
        {
            // Center the content panel
            contentPanel.Size = new Size((int)(ClientSize.Width * 0.7), (int)(ClientSize.Height * 0.7));
            contentPanel.Location = new Point((ClientSize.Width - contentPanel.Width) / 2, (ClientSize.Height - contentPanel.Height) / 2 + 30);

            // Title Label
            titleLabel.Location = new Point((ClientSize.Width - titleLabel.Width) / 2, ClientSize.Height / 10);

            // Step Number Label
            stepNumberLabel.Size = new Size(contentPanel.Width / 3, contentPanel.Height / 10);
            stepNumberLabel.Location = new Point(contentPanel.Width / 3, contentPanel.Height / 25);

            // Step Image Box
            stepImageBox.Size = new Size(contentPanel.Width / 2, contentPanel.Height / 2);
            stepImageBox.Location = new Point((contentPanel.Width - stepImageBox.Width) / 2, contentPanel.Height / 5);

            // Step Title Label
            stepTitleLabel.Size = new Size(contentPanel.Width * 3 / 4, contentPanel.Height / 10);
            stepTitleLabel.Location = new Point((contentPanel.Width - stepTitleLabel.Width) / 2, stepNumberLabel.Bottom - 15);

            // Step Description Label
            stepDescriptionLabel.Size = new Size(contentPanel.Width - 40, contentPanel.Height / 4);
            stepDescriptionLabel.Location = new Point(20, stepImageBox.Bottom + 10);

            // Navigation Buttons
            int buttonWidth = contentPanel.Width / 4;
            int buttonHeight = contentPanel.Height / 10;

            previousSlideButton.Size = nextSlideButton.Size = new Size(buttonWidth, buttonHeight);
            previousSlideButton.Location = new Point(20, contentPanel.Height - buttonHeight - 20);
            nextSlideButton.Location = new Point(contentPanel.Width - buttonWidth - 20, contentPanel.Height - buttonHeight - 20);

            // Logo Position
            logoBox.Location = new Point(this.ClientSize.Width - logoBox.Width - 30, 10);
        }
        private void backButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void GuideForm_Load(object sender, EventArgs e)
        {
            AdjustLayout();
        }
        private void GuideForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Enter)
            {
                NextSlideButton_Click(null, null);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Left)
            {
                PreviousSlideButton_Click(null, null);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Back)
            {
                backButton.PerformClick();
                e.Handled = true;
            }
        }
    }
}
