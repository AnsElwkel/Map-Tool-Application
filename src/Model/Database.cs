using System;
using System.Runtime.CompilerServices;
namespace Map_Creation_Tool.src.Model
{
    /*Contain any data that needs to be stored
     */

    public class Database
    {

        public static (int R, int G, int B) PLACE_COLOR = (238, 232, 232);       //Light Gray
        public static (int R, int G, int B) REGULAR_PATH_COLOR = (189, 198, 197);//Light grayish cyan
        public static (int R, int G, int B) BUSY_PATH_COLOR = (162, 193, 221);   //Light Blue
        public static (int R, int G, int B) VERY_BUSY_PATH_COLOR = (255, 0, 0);  //Red
        public static (int R, int G, int B) OBSTACLE_COLOR = (255, 255, 255);    //White

        private static Cell[,] curGrid;

        private static Dictionary<(int x, int y), int> placeComponentMap = new Dictionary<(int x, int y), int>();

        // Maps each component ID to its boundary walkable points
        private static Dictionary<int, List<(int x, int y)>> componentBoundaryMap = new Dictionary<int, List<(int x, int y)>>();

        private static Bitmap imagePixels;
        private static Image curMapImage;
        private static string labelsContent;

        public string LabelsContent
        {
            get { return labelsContent; }
            set { labelsContent = value; }
        }


        public List<(int x, int y)> this[int componentId]
        {
            get
            {
                if (componentBoundaryMap.ContainsKey(componentId))
                {
                    return componentBoundaryMap[componentId];
                }
                else
                {
                    return new List<(int x, int y)>();
                }
            }

            set
            {
                if (componentBoundaryMap.ContainsKey(componentId))
                {
                    componentBoundaryMap[componentId] = value;
                }
                else
                {
                    componentBoundaryMap.Add(componentId, value);
                }
            }
        }

        public int this[(int x, int y) point]
        {
            get
            {
                if (placeComponentMap.ContainsKey(point))
                {
                    return placeComponentMap[point];
                }
                else
                {
                    return -1; // or some other default value
                }
            }

            set
            {
                if (placeComponentMap.ContainsKey(point))
                {
                    placeComponentMap[point] = value;
                }
                else
                {
                    placeComponentMap.Add(point, value);
                }
            }
        }





        public Cell this[int x, int y]
        {
            get { return curGrid[x, y]; }
        }

        public int GridWidth
        {
            get
            {
                return curGrid.GetLength(0);
            }
        }

        public int GridHeight
        {
            get
            {
                return curGrid.GetLength(1);
            }
        }

        public Cell[,] CurGrid
        {
            get { return curGrid; }
            set { curGrid = value; }
        }





        public Bitmap ImagePixels
        {
            set { imagePixels = value; }
        }

        public Image CurMapImage
        {
            set { curMapImage = value; }
            get { return curMapImage; }
        }

        public static Database Instance { get; } = new Database();
        private Database()
        {
        }
    }
}