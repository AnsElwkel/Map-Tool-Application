using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map_Tool_Application.src.View
{
    public class LabelEditDialog : Form
    {
        private TextBox txtText;
        private ComboBox cmbFontFamily;
        private NumericUpDown nudFontSize;
        private Button btnColor;
        private NumericUpDown nudAngle;
        private ColorDialog colorDialog;
        private Button btnOK, btnCancel;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string LabelText
        {
            get => txtText.Text;
            set => txtText.Text = value;
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string LabelFontFamily
        {
            get => cmbFontFamily.SelectedItem?.ToString() ?? "Arial";
            set
            {
                int idx = cmbFontFamily.Items.IndexOf(value);
                cmbFontFamily.SelectedIndex = idx >= 0 ? idx : 0;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float LabelFontSize
        {
            get => (float)nudFontSize.Value;
            set => nudFontSize.Value = (decimal)Math.Max(nudFontSize.Minimum, Math.Min(nudFontSize.Maximum, (decimal)value));
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color LabelColor { get; set; } = Color.Black;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float LabelAngle
        {
            get => (float)nudAngle.Value;
            set => nudAngle.Value = (decimal)value;
        }

        public LabelEditDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Edit Text Label";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(350, 260);

            Label lblText = new Label { Text = "Text:", Left = 10, Top = 18, Width = 60 };
            txtText = new TextBox { Left = 80, Top = 15, Width = 240 };

            Label lblFont = new Label { Text = "Font:", Left = 10, Top = 53, Width = 60 };
            cmbFontFamily = new ComboBox { Left = 80, Top = 50, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            foreach (FontFamily family in new InstalledFontCollection().Families)
            {
                cmbFontFamily.Items.Add(family.Name);
            }
            cmbFontFamily.SelectedItem = "Arial";

            Label lblSize = new Label { Text = "Size:", Left = 10, Top = 88, Width = 60 };
            nudFontSize = new NumericUpDown { Left = 80, Top = 85, Width = 80, Minimum = 6, Maximum = 72, Value = 12 };

            Label lblColor = new Label { Text = "Color:", Left = 10, Top = 123, Width = 60 };
            btnColor = new Button { Left = 80, Top = 120, Width = 80, Text = "Pick...", BackColor = LabelColor };
            btnColor.Click += (s, e) =>
            {
                colorDialog.Color = btnColor.BackColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    LabelColor = colorDialog.Color;
                    btnColor.BackColor = LabelColor;
                }
            };

            Label lblAngle = new Label { Text = "Angle:", Left = 10, Top = 158, Width = 60 };
            nudAngle = new NumericUpDown { Left = 80, Top = 155, Width = 80, Minimum = -180, Maximum = 180, Value = 0 };

            btnOK = new Button { Text = "OK", DialogResult = DialogResult.OK, Left = 80, Top = 200, Width = 80 };
            btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Left = 180, Top = 200, Width = 80 };

            // Events to update color if set externally
            this.Load += (s, e) => btnColor.BackColor = LabelColor;

            // Accept on Enter in text box
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;

            colorDialog = new ColorDialog();

            this.Controls.AddRange(new Control[]
            {
                lblText, txtText,
                lblFont, cmbFontFamily,
                lblSize, nudFontSize,
                lblColor, btnColor,
                lblAngle, nudAngle,
                btnOK, btnCancel
            });
        }
    }

}
