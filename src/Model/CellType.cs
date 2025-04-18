using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map_Creation_Tool.src.Model
{
    [Flags]
    public enum CellType : byte
    {
        Exit = 0,
        Place = 1,
        Walkable = 2,
        Obstacle = 4
    }

    public struct Cell
    {
        public Color CellColor { set; get; }
        public CellType Type { set; get; }
        public byte Weight { set; get; }

        public Cell(Color cellColor, CellType type, byte weight)
        {
            this.CellColor = cellColor;
            this.Type = type;
            this.Weight = weight;
        }

    }
}
