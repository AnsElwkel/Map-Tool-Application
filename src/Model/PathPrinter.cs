using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map_Creation_Tool.src.Model
{
    internal class PathPrinter
    {
        private List<(int x, int y)> path;

        public PathPrinter(List<(int x, int y)> path)
        {
            this.path = path;
        }

        //Print the path
        public void printPath()
        {
            if (path == null || path.Count == 0)
            {
                MessageBox.Show("No path");
                return;
            }
        }
    }
}