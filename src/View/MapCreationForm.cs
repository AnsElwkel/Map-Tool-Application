using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Security.Cryptography;
using System.ComponentModel;
using Map_Tool_Application.src.View;

namespace Map_Creation_Tool.src.View
{
    public enum ToolType : byte
    {
        Brush,
        Eraser,
        Rectangle,
        Line,
        Fill,
        Text
    }

    public class MapCreationForm : Form
    {

        #region Attributes
        // Colors
        private static readonly Color REGULAR_PATH_COLOR = Color.FromArgb(189, 198, 197);
        private static readonly Color BUSY_PATH_COLOR = Color.FromArgb(162, 193, 221);
        private static readonly Color VERY_BUSY_PATH_COLOR = Color.FromArgb(255, 0, 0);
        private static readonly Color PLACE_COLOR = Color.FromArgb(238, 232, 232);
        private static readonly Color OBSTACLE_COLOR = Color.FromArgb(255, 255, 255);

        private readonly List<Color> exactColors = new List<Color>
        {
            REGULAR_PATH_COLOR,
            BUSY_PATH_COLOR,
            VERY_BUSY_PATH_COLOR,
            PLACE_COLOR,
            OBSTACLE_COLOR,
            Color.Green
        };

        private const short WIDTH = 1280;
        private const short HEIGHT = 720;

        // Labels
        private List<MapLabel> labels;
        private MapLabel selectedLabel;
        private Point dragOffset;
        private bool draggingLabel;
        private ContextMenuStrip labelContextMenu;

        

        private Bitmap canvas;
        private PictureBox pictureBox;
        private Color currentColor;
        private ToolType currentTool;
        private int brushSize; //or eraser size
        private Pen previewPen;
        private Bitmap tempDrawing;
        private int gridSize;
        private Point? startPoint; //for drawing shapes
        private bool gridSnap; //gridSnapping mode

        // Undo and Redo stacks
        private Stack<Bitmap> undoStack = new Stack<Bitmap>(); 
        private Stack<Bitmap> redoStack = new Stack<Bitmap>();

        #endregion

        #region Initialization
        public MapCreationForm()
        {
            labels = new List<MapLabel>();
            selectedLabel = null;
            dragOffset = Point.Empty;
            draggingLabel = false;



            currentColor = PLACE_COLOR;
            currentTool = ToolType.Brush;
            startPoint = null;
            brushSize = 4;
            tempDrawing = null;
            previewPen = new Pen(Color.Black, 1) { DashStyle = DashStyle.Dash };
            gridSnap = false;
            gridSize = 20;
            undoStack = new Stack<Bitmap>();
            redoStack = new Stack<Bitmap>();
            labelContextMenu = new ContextMenuStrip();
            labelContextMenu.Items.Add("Edit", null, (s, e) => EditSelectedLabel());
            labelContextMenu.Items.Add("Delete", null, (s, e) => DeleteSelectedLabel());

            InitializeComponent();
            InitializeCanvas(WIDTH, HEIGHT);
        }

        private void InitializeComponent()
        {
            this.Text = "Map Drawer";
            this.Size = new Size(WIDTH + 20, HEIGHT + 90);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.KeyPreview = true;

            this.KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.Z) Undo();
                if (e.Control && e.KeyCode == Keys.Y) Redo();
            };

            // Toolbar Panel
            var toolbar = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.LightGray
            };

            // Color Buttons
            AddColorButton(toolbar, "Path", Color.FromArgb(255, Model.PathFinder.REGULAR_PATH_COLOR.R, Model.PathFinder.REGULAR_PATH_COLOR.G, Model.PathFinder.REGULAR_PATH_COLOR.B));
            AddColorButton(toolbar, "Busy", Color.FromArgb(255, Model.PathFinder.BUSY_PATH_COLOR.R, Model.PathFinder.BUSY_PATH_COLOR.G, Model.PathFinder.BUSY_PATH_COLOR.B));
            AddColorButton(toolbar, "Very Busy", Color.FromArgb(255, Model.PathFinder.VERY_BUSY_PATH_COLOR.R, Model.PathFinder.VERY_BUSY_PATH_COLOR.G, Model.PathFinder.VERY_BUSY_PATH_COLOR.B));
            AddColorButton(toolbar, "Place", Color.FromArgb(255, Model.PathFinder.PLACE_COLOR.R, Model.PathFinder.PLACE_COLOR.G, Model.PathFinder.PLACE_COLOR.B));
            AddColorButton(toolbar, "Obstacle", Color.FromArgb(255, Model.PathFinder.OBSTACLE_COLOR.R, Model.PathFinder.OBSTACLE_COLOR.G, Model.PathFinder.OBSTACLE_COLOR.B));
            AddColorButton(toolbar, "Exit", Color.Green);


            // Tool Buttons
            AddToolButton(toolbar, "Brush", ToolType.Brush);
            AddToolButton(toolbar, "Eraser", ToolType.Eraser);
            AddToolButton(toolbar, "Rectangle", ToolType.Rectangle);
            AddToolButton(toolbar, "Line", ToolType.Line);
            AddToolButton(toolbar, "Fill", ToolType.Fill);
            AddToolButton(toolbar, "Text", ToolType.Text);

            var btnNew = new Button { Text = "New", Width = 60 };
            btnNew.Click += (s, e) => InitializeCanvas(WIDTH, HEIGHT);

            var btnLoad = new Button { Text = "Load", Width = 60 };
            btnLoad.Click += LoadMap;

            var btnSave = new Button { Text = "Save", Width = 60 };
            btnSave.Click += SaveMap;

            var btnUndo = new Button { Text = "Undo", Width = 60 };
            btnUndo.Click += (s, e) => Undo();

            var btnRedo = new Button { Text = "Redo", Width = 60 };
            btnRedo.Click += (s, e) => Redo();

            var gridCheck = new CheckBox { Text = "Snap to Grid", AutoSize = true };
            gridCheck.CheckedChanged += (s, e) => gridSnap = gridCheck.Checked;

            toolbar.Controls.AddRange(new Control[] { btnNew, btnLoad, btnSave, btnUndo, btnRedo, gridCheck });

            // Drawing Area
            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Cursor = Cursors.Cross
            };

            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseUp += PictureBox_MouseUp;
            pictureBox.Paint += PictureBox_Paint;

            this.Controls.Add(pictureBox);
            this.Controls.Add(toolbar);
        }

        private void InitializeCanvas(int width, int height)
        {
            canvas = new Bitmap(width, height);
            using (var g = Graphics.FromImage(canvas))
                g.Clear(Color.White);
            pictureBox.Image = canvas;
            undoStack.Clear();
            redoStack.Clear();
        }
        #endregion

        #region Mouse Events
        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            SaveState();

            if (currentTool == ToolType.Text)
            {
                // Hit-test existing label
                selectedLabel = labels.LastOrDefault(lbl => lbl.Contains(e.Location));
                if (selectedLabel != null)
                {
                    dragOffset = new Point(e.Location.X - selectedLabel.Position.X, e.Location.Y - selectedLabel.Position.Y);
                    draggingLabel = true;
                    if (e.Button == MouseButtons.Right)
                    {
                        labelContextMenu.Show(pictureBox, e.Location);
                    }
                    else if (e.Clicks == 2)
                    {
                        EditSelectedLabel();
                    }
                }
                else if (e.Button == MouseButtons.Left)
                {
                    // Add new label
                    var dlg = new LabelEditDialog();
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        labels.Add(new MapLabel
                        {
                            Position = e.Location,
                            Text = dlg.LabelText,
                            Font = new Font(dlg.LabelFontFamily, dlg.LabelFontSize, FontStyle.Regular),
                            Color = dlg.LabelColor,
                            Angle = dlg.LabelAngle
                        });
                        pictureBox.Invalidate();
                    }
                }
                return;
            }
            else
            {
                selectedLabel = null;
                draggingLabel = false;
            }

            startPoint = SnapPoint(e.Location);

            if (currentTool == ToolType.Fill)
            {
                FloodFill(SnapPoint(e.Location));
                return;
            }

            if (currentTool == ToolType.Brush || currentTool == ToolType.Eraser)
            {
                DrawFreehand(SnapPoint(e.Location));
            }
            else
            {
                tempDrawing = (Bitmap)canvas.Clone();
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentTool == ToolType.Text && draggingLabel && selectedLabel != null && e.Button == MouseButtons.Left)
            {
                selectedLabel.Position = new Point(e.Location.X - dragOffset.X, e.Location.Y - dragOffset.Y);
                pictureBox.Invalidate();
            }
            if (startPoint.HasValue)
            {
                if (currentTool == ToolType.Brush || currentTool == ToolType.Eraser)
                {
                    DrawFreehand(SnapPoint(e.Location));
                }
                else
                {
                    ShowShapePreview(SnapPoint(e.Location));
                }
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            draggingLabel = false;
            if (startPoint.HasValue && currentTool != ToolType.Brush &&
                currentTool != ToolType.Eraser && currentTool != ToolType.Fill)
            {
                CommitShape(SnapPoint(e.Location));
            }
            startPoint = null;
            tempDrawing = null;
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (gridSnap)
            {
                using (var gridPen = new Pen(Color.FromArgb(50, Color.Black)))
                {
                    for (int x = 0; x < pictureBox.Width; x += gridSize)
                        e.Graphics.DrawLine(gridPen, x, 0, x, pictureBox.Height);
                    for (int y = 0; y < pictureBox.Height; y += gridSize)
                        e.Graphics.DrawLine(gridPen, 0, y, pictureBox.Width, y);
                }
            }
            foreach (var label in labels)
            {
                var viewPos = label.Position;
                using (var brush = new SolidBrush(label.Color))
                {
                    var gstate = e.Graphics.Save();
                    e.Graphics.TranslateTransform(viewPos.X, viewPos.Y);
                    e.Graphics.RotateTransform(label.Angle);
                    e.Graphics.DrawString(label.Text, label.Font, brush, 0, 0);
                    e.Graphics.Restore(gstate);
                }

                // Draw rotated selection rectangle if this is the selected label
                if (label == selectedLabel)
                {
                    SizeF textSize;
                    using (var tmpg = Graphics.FromImage(new Bitmap(1, 1)))
                        textSize = tmpg.MeasureString(label.Text, label.Font);

                    // The four corners of the rectangle before rotation, relative to (0,0)
                    PointF[] rectCorners = new PointF[]
                    {
            new PointF(0, 0),
            new PointF(textSize.Width, 0),
            new PointF(textSize.Width, textSize.Height),
            new PointF(0, textSize.Height)
                    };

                    // Calculate rotation in radians
                    float angleRad = label.Angle * (float)Math.PI / 180f;
                    float cos = (float)Math.Cos(angleRad);
                    float sin = (float)Math.Sin(angleRad);

                    // Rotate and translate each corner to screen coordinates
                    for (int i = 0; i < rectCorners.Length; i++)
                    {
                        float x = rectCorners[i].X;
                        float y = rectCorners[i].Y;
                        // Rotate
                        float xr = x * cos - y * sin;
                        float yr = x * sin + y * cos;
                        // Translate to label position
                        rectCorners[i] = new PointF(label.Position.X + xr, label.Position.Y + yr);
                    }

                    using (var pen = new Pen(Color.Red, 1) { DashStyle = DashStyle.Dash })
                    {
                        e.Graphics.DrawPolygon(pen, rectCorners);
                    }
                }
            }
        }
        #endregion

        #region Drawing Methods
        private void DrawFreehand(Point point)
        {
            // Use exact color drawing with no anti-aliasing
            using (var g = Graphics.FromImage(canvas))
            {
                g.SmoothingMode = SmoothingMode.None;
                Color drawColor = GetDrawingColor();

                if (currentTool == ToolType.Brush)
                {
                    using (var brush = new SolidBrush(drawColor))
                    {
                        int halfSize = brushSize / 2;
                        g.FillRectangle(brush,
                            point.X - halfSize,
                            point.Y - halfSize,
                            brushSize,
                            brushSize);
                    }
                }
                else if (currentTool == ToolType.Eraser)
                {
                    using (var brush = new SolidBrush(OBSTACLE_COLOR))
                    {
                        int halfSize = brushSize / 2;
                        g.FillRectangle(brush,
                            point.X - halfSize,
                            point.Y - halfSize,
                            brushSize,
                            brushSize);
                    }
                }
            }
            pictureBox.Invalidate();
        }

        private void ShowShapePreview(Point endPoint)
        {
            if (tempDrawing == null) return;

            pictureBox.Image = tempDrawing;
            using (var g = Graphics.FromImage(pictureBox.Image))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = GetRectangle(startPoint.Value, endPoint);

                switch (currentTool)
                {
                    case ToolType.Rectangle:
                        g.DrawRectangle(previewPen, rect);
                        break;
                    case ToolType.Line:
                        g.DrawLine(previewPen, startPoint.Value, endPoint);
                        break;
                }
            }
            pictureBox.Refresh();
        }

        private void CommitShape(Point endPoint)
        {
            using (var g = Graphics.FromImage(canvas))
            {
                g.SmoothingMode = SmoothingMode.None;
                var rect = GetRectangle(startPoint.Value, endPoint);
                Color drawColor = GetDrawingColor();

                using (var brush = new SolidBrush(drawColor))
                {
                    switch (currentTool)
                    {
                        case ToolType.Rectangle:
                            g.FillRectangle(brush, rect);
                            break;
                        case ToolType.Line:
                            // For lines, use precise pixel drawing
                            DrawPreciseLine(g, brush, startPoint.Value, endPoint);
                            break;
                    }
                }
            }
            pictureBox.Image = canvas;
        }

        private void FloodFill(Point point)
        {
            Color targetColor = canvas.GetPixel(point.X, point.Y);
            Color replaceColor = GetDrawingColor();

            if (targetColor.ToArgb() == replaceColor.ToArgb()) return;

            var stack = new Stack<Point>();
            stack.Push(point);

            while (stack.Count > 0)
            {
                Point p = stack.Pop();
                if (!IsInCanvas(p) || canvas.GetPixel(p.X, p.Y) != targetColor)
                    continue;

                canvas.SetPixel(p.X, p.Y, replaceColor);
                stack.Push(new Point(p.X - 1, p.Y));
                stack.Push(new Point(p.X + 1, p.Y));
                stack.Push(new Point(p.X, p.Y - 1));
                stack.Push(new Point(p.X, p.Y + 1));
            }
            pictureBox.Invalidate();
        }

        private void DrawPreciseLine(Graphics g, Brush brush, Point start, Point end)
        {
            // Bresenham's line algorithm for exact pixel placement
            int dx = Math.Abs(end.X - start.X);
            int dy = Math.Abs(end.Y - start.Y);
            int sx = start.X < end.X ? 1 : -1;
            int sy = start.Y < end.Y ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                g.FillRectangle(brush, start.X - brushSize / 2, start.Y - brushSize / 2, brushSize, brushSize);
                if (start.X == end.X && start.Y == end.Y) break;
                int e2 = 2 * err;
                if (e2 > -dy) { err -= dy; start.X += sx; }
                if (e2 < dx) { err += dx; start.Y += sy; }
            }
        }
        #endregion

        #region Stack Operations
        private void SaveState()
        {
            undoStack.Push((Bitmap)canvas.Clone());
            redoStack.Clear();
        }

        private void Undo()
        {
            if (undoStack.Count > 0)
            {
                redoStack.Push((Bitmap)canvas.Clone());
                canvas = (Bitmap)undoStack.Pop().Clone();
                pictureBox.Image = canvas;
            }
        }

        private void Redo()
        {
            if (redoStack.Count > 0)
            {
                undoStack.Push((Bitmap)canvas.Clone());
                canvas = (Bitmap)redoStack.Pop().Clone();
                pictureBox.Image = canvas;
            }
        }
        #endregion

        #region Another Methods
        private void AddColorButton(Control parent, string text, Color color)
        {
            var btn = new Button
            {
                BackColor = color,
                Text = text,
                Width = 70,
                FlatStyle = FlatStyle.Flat,
                ForeColor = GetContrastColor(color)
            };
            btn.Font = new Font(btn.Font, FontStyle.Bold);
            btn.Click += (s, e) => currentColor = color;
            parent.Controls.Add(btn);
        }

        private void AddToolButton(Control parent, string text, ToolType tool)
        {
            var btn = new Button
            {
                Text = text,
                Width = 70,
                Tag = tool,
                FlatStyle = FlatStyle.Flat
            };
            btn.Click += (s, e) =>
            {
                currentTool = tool;
                UpdateButtonStates();
            };
            parent.Controls.Add(btn);
        }

        private void UpdateButtonStates()
        {
            foreach (Control c in this.Controls)
            {
                if (c is Button btn && btn.Tag is ToolType)
                {
                    btn.BackColor = (ToolType)btn.Tag == currentTool ?
                        Color.SteelBlue : SystemColors.Control;
                }
            }
        }

        private Point SnapPoint(Point p) => gridSnap ?
            new Point(p.X / gridSize * gridSize, p.Y / gridSize * gridSize) : p;

        private Color GetDrawingColor() =>
            currentTool == ToolType.Eraser ? Color.White : currentColor;

        private Rectangle GetRectangle(Point p1, Point p2) => new Rectangle(
            Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y),
            Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));

        private bool IsInCanvas(Point p)
        {
            return p.X >= 0 && p.X < canvas.Width && p.Y >= 0 && p.Y < canvas.Height;
        }

        private Color GetContrastColor(Color color)
        {
            double luminance = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;
            return luminance > 0.5 ? Color.Black : Color.White;
        }

        private Bitmap QuantizeImage(Bitmap source)
        {
            Bitmap quantized = new Bitmap(source.Width, source.Height);
            for (int x = 0; x < source.Width; x++)
            {
                for (int y = 0; y < source.Height; y++)
                {
                    Color original = source.GetPixel(x, y);
                    quantized.SetPixel(x, y, GetNearestExactColor(original));
                }
            }
            return quantized;
        }

        private Color GetNearestExactColor(Color input)
        {
            Color closest = OBSTACLE_COLOR;
            double minDistance = double.MaxValue;

            foreach (Color exactColor in exactColors)
            {
                double distance = CalculateColorDistance(input, exactColor);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = exactColor;
                }
            }
            return closest;
        }

        private double CalculateColorDistance(Color a, Color b)
        {
            return Math.Sqrt(
                Math.Pow(a.R - b.R, 2) +
                Math.Pow(a.G - b.G, 2) +
                Math.Pow(a.B - b.B, 2));
        }
        #endregion

        #region Load and Save
        private void LoadMap(object sender, EventArgs e)
        {
            using (var openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "Image Files|*.png;*.jpg";
                openDialog.Title = "Load Map Image";
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var loaded = new Bitmap(openDialog.FileName))
                    {
                        canvas = QuantizeImage(loaded);
                        pictureBox.Image = canvas;
                    }
                }
            }


            using (var openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "Labels File|*.txt";
                openDialog.Title = "Load Map Labels";
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    // Load labels
                    labels.Clear();
                    var txtFile = openDialog.FileName;
                    if (File.Exists(txtFile))
                    {
                        foreach (var line in File.ReadAllLines(txtFile))
                        {
                            var parts = line.Split('|');
                            var posParts = parts[0].Split(',');
                            var pos = new Point(int.Parse(posParts[0]), int.Parse(posParts[1]));
                            var text = parts[1].Replace("/", "|"); // restore | if any
                            var fontFamily = parts[2];
                            var fontSize = float.Parse(parts[3]);
                            var color = Color.FromArgb(int.Parse(parts[4]));
                            var angle = float.Parse(parts[5]);
                            labels.Add(new MapLabel
                            {
                                Position = pos,
                                Text = text,
                                Font = new Font(fontFamily, fontSize),
                                Color = color,
                                Angle = angle
                            });
                            
                        }
                    }

                }
            }

            pictureBox.Invalidate();

        }
       
        private void SaveMap(object sender, EventArgs e)
        {
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg";
                saveDialog.Title = "Save Map Image";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ImageFormat format = saveDialog.FileName.EndsWith(".jpg") ?
                        ImageFormat.Jpeg : ImageFormat.Png;

                    canvas.Save(saveDialog.FileName, format);

                    MessageBox.Show("Image Map saved successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            var dlg = new SaveFileDialog { Filter = "Text Files|*.txt" };
            dlg.Title = "Save Labels Map";
            if (dlg.ShowDialog() != DialogResult.OK) return;

            canvas.Save(dlg.FileName + ".png", ImageFormat.Png);

            // Save labels in a parallel .txt file
            using (var sw = new StreamWriter(dlg.FileName + ".txt"))
            {
                foreach (var label in labels)
                {
                    sw.WriteLine($"{label.Position.X},{label.Position.Y}|{label.Text.Replace("|", "/")}|{label.Font.FontFamily.Name}|{label.Font.Size}|{label.Color.ToArgb()}|{label.Angle}");
                }
            }

            MessageBox.Show("Labels Map saved successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region Label Edit/Delete

        private void EditSelectedLabel()
        {
            if (selectedLabel == null) return;

            var dlg = new LabelEditDialog();
            dlg.LabelText = selectedLabel.Text;
            dlg.LabelFontFamily = selectedLabel.Font.FontFamily.Name;
            dlg.LabelFontSize = selectedLabel.Font.Size;
            dlg.LabelColor = selectedLabel.Color;
            dlg.LabelAngle = selectedLabel.Angle;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedLabel.Text = dlg.LabelText;
                selectedLabel.Font = new Font(dlg.LabelFontFamily, dlg.LabelFontSize, FontStyle.Regular);
                selectedLabel.Color = dlg.LabelColor;
                selectedLabel.Angle = dlg.LabelAngle;
                pictureBox.Invalidate();
            }
        }

        private void DeleteSelectedLabel()
        {
            if (selectedLabel == null) return;
            labels.Remove(selectedLabel);
            selectedLabel = null;
            pictureBox.Invalidate();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (selectedLabel != null && keyData == Keys.Delete)
            {
                DeleteSelectedLabel();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

    }
}