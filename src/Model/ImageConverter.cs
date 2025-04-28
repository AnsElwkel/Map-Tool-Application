using System;
using System.Drawing.Imaging;

namespace Map_Creation_Tool.src.Model
{
    public class ImageConverter
    {
        public ImageConverter()
        {

        }

        public void convert(Image image)
        {
            Bitmap imgMap = new Bitmap(image);
            Cell[,] curGrid = new Cell[imgMap.Width, imgMap.Height];

            string colors = "";
            HashSet<string> uniqueColors = new HashSet<string>();
            for (int i = 0; i < imgMap.Width; i++)
            {
                for (int j = 0; j < imgMap.Height; j++)
                {
                    Color pixelColor = imgMap.GetPixel(i, j);

                    // Add the pixel color to the unique colors set
                    pixelColor = QuantizeColor(pixelColor);

                    CellType type = CellType.Walkable;
                    byte weight = 1;

                    // Check if the pixel color is one of the predefined colors
                    if (pixelColor == Color.FromArgb(255, PathFinder.VERY_BUSY_PATH_COLOR.R, PathFinder.VERY_BUSY_PATH_COLOR.G, PathFinder.VERY_BUSY_PATH_COLOR.B))
                    {
                        weight = 16;
                        type = CellType.Walkable;
                    }
                    else if (pixelColor == Color.FromArgb(255, PathFinder.BUSY_PATH_COLOR.R, PathFinder.BUSY_PATH_COLOR.G, PathFinder.BUSY_PATH_COLOR.B))
                    {
                        weight = 8;
                        type = CellType.Walkable;
                    }
                    else if (pixelColor == Color.FromArgb(255, PathFinder.REGULAR_PATH_COLOR.R, PathFinder.REGULAR_PATH_COLOR.G, PathFinder.REGULAR_PATH_COLOR.B))
                    {
                        weight = 1;
                        type = CellType.Walkable;
                    }
                    else if (pixelColor == Color.FromArgb(255, PathFinder.PLACE_COLOR.R, PathFinder.PLACE_COLOR.G, PathFinder.PLACE_COLOR.B))
                    {
                        weight = 255;
                        type = CellType.Place;
                    }
                    else if (pixelColor == Color.FromArgb(255, PathFinder.OBSTACLE_COLOR.R, PathFinder.OBSTACLE_COLOR.G, PathFinder.OBSTACLE_COLOR.B)
                        || pixelColor == Color.White)
                    {
                        type = CellType.Obstacle;
                        weight = 255;
                    }
                    else if (pixelColor == Color.Green)
                    {
                        weight = 1;
                        type = CellType.Exit;
                    }
                    else
                    {
                        throw new Exception($"Unknown color: {pixelColor} at ({i}, {j})");
                    }

                    curGrid[i, j] = new Cell(pixelColor, type, weight);
                }
            }

            Database.Instance.CurGrid = curGrid;

            PreprocessMap(Database.Instance.CurGrid); // Preprocess the map to find components and boundaries

            //Maybe not important
            Database.Instance.ImagePixels = imgMap;
            Database.Instance.CurMapImage = image;
        }


        public void PreprocessMap(Cell[,] grid)
        {
            int componentCounter = 0;
            int[,] componentIds = new int[grid.GetLength(0), grid.GetLength(1)]; // visited array

            for (int i = 0; i < componentIds.GetLength(0); i++)
                for (int j = 0; j < componentIds.GetLength(1); j++)
                    componentIds[i, j] = 0;


            List<(int x, int y)> tmp = new();
            //string gridInfo = "";
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {

                    if (grid[x, y].Type != CellType.Place || componentIds[x, y] != 0) continue;
                    //tmp.Add((x, y));
                    //gridInfo += $"[{x}, {y}]\n ";
                    componentCounter++;
                    FloodFillAndRecordComponent(grid, (x, y), componentCounter, ref componentIds);
                }
            }
        }

        private void FloodFillAndRecordComponent(Cell[,] grid, (int x, int y) start, int componentId, ref int[,] componentIds)
        {

            HashSet<(int x, int y)> ExitPoints = new HashSet<(int x, int y)>(); //To store the nearest walkable points

            Queue<(int x, int y)> queue = new Queue<(int x, int y)>();
            queue.Enqueue(start);

            componentIds[start.x, start.y] = componentId; // visited
            Database.Instance[start] = componentId; // map the point to the component id
            //s += $"Component {componentId}:  ";

            // Flood fill to find all cells in this place component
            while (queue.Count > 0)
            {
                (int x, int y) current = queue.Dequeue();
                //s += $"[{current.x}, {current.y}]";


                //Check if the boundary of this point is a Exit point
                for (int i = 0, nx, ny; i < PathFinder.dx.Length; i++)
                {
                    nx = current.x + PathFinder.dx[i];
                    ny = current.y + PathFinder.dy[i];
                    // Check if the neighbor is valid and not already visited
                    if (PathFinder.isValid(nx, ny, Database.Instance.GridWidth, Database.Instance.GridHeight) &&
                        grid[nx, ny].Type == CellType.Exit &&
                        !ExitPoints.Contains((nx, ny)))
                    {
                        ExitPoints.Add((nx, ny));
                    }
                }

                for (int i = 0, nx, ny; i < PathFinder.dx.Length; i++)
                {
                    nx = current.x + PathFinder.dx[i];
                    ny = current.y + PathFinder.dy[i];



                    if (PathFinder.isValid(nx, ny, Database.Instance.GridWidth, Database.Instance.GridHeight) &&
                        grid[nx, ny].Type == CellType.Place &&
                        componentIds[nx, ny] == 0)
                    {
                        componentIds[nx, ny] = componentId;
                        Database.Instance[(nx, ny)] = componentId;
                        queue.Enqueue((nx, ny));
                    }
                }
            }

            //Add all nearst walkable points to the component
            Database.Instance[componentId] = ExitPoints.ToList();
        }

        public static List<Color> MapColors = new List<Color>
        {
            Color.FromArgb(255, PathFinder.OBSTACLE_COLOR.R, PathFinder.OBSTACLE_COLOR.G, PathFinder.OBSTACLE_COLOR.B),     // OBSTACLE
            Color.FromArgb(255, PathFinder.PLACE_COLOR.R, PathFinder.PLACE_COLOR.G, PathFinder.PLACE_COLOR.B),    // PLACE
            Color.FromArgb(255, PathFinder.REGULAR_PATH_COLOR.R, PathFinder.REGULAR_PATH_COLOR.G, PathFinder.REGULAR_PATH_COLOR.B),    // REGULAR_PATH
            Color.FromArgb(255, PathFinder.BUSY_PATH_COLOR.R, PathFinder.BUSY_PATH_COLOR.G, PathFinder.BUSY_PATH_COLOR.B),    // BUSY_PATH
            Color.FromArgb(255, PathFinder.VERY_BUSY_PATH_COLOR.R, PathFinder.VERY_BUSY_PATH_COLOR.G, PathFinder.VERY_BUSY_PATH_COLOR.B),        // VERY_BUSY
            Color.Green, // Exit
        };

        // Quantize a pixel to the nearest predefined color
        private Color QuantizeColor(Color pixelColor)
        {
            return MapColors
                .OrderBy(c => ColorDistance(c, pixelColor))
                .First();
        }

        // Calculate Euclidean distance between two colors
        private double ColorDistance(Color a, Color b)
        {
            return Math.Sqrt(
                Math.Pow(a.R - b.R, 2) +
                Math.Pow(a.G - b.G, 2) +
                Math.Pow(a.B - b.B, 2)
            );
        }
    }
}

