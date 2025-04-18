using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography.Xml;
using System.Windows.Controls;
using Map_Creation_Tool.src.Model;

namespace Map_Creation_Tool.src.Controller
{

    public enum PathType
    {
        SHORTEST_PATH,
        FASTEST_PATH
    }

    public class PathFinderController
    {
        private int fromX, fromY, toX, toY;
        PathType pathType;

        public PathFinderController()
        {
        }

        public bool validatePoints(int fromX, int fromY, int toX, int toY, PathType pathType)
        {
            if ((Database.Instance[fromX, fromY].Type == CellType.Walkable ||
                   Database.Instance[fromX, fromY].Type == CellType.Exit ||
                   Database.Instance[fromX, fromY].Type == CellType.Place)
                   &&
                   (Database.Instance[toX, toY].Type == CellType.Walkable ||
                Database.Instance[toX, toY].Type == CellType.Exit ||
                Database.Instance[toX, toY].Type == CellType.Place))
            {
                this.fromX = fromX;
                this.fromY = fromY;
                this.toX = toX;
                this.toY = toY;
                this.pathType = pathType;
                return true;
            }
            return false;
        }

        public List<(int x, int y)> pathfinder()
        {
            bool isFromPointPath = Database.Instance[fromX, fromY].Type == CellType.Walkable || Database.Instance[fromX, fromY].Type == CellType.Exit;
            bool isToPointPath = Database.Instance[toX, toY].Type == CellType.Walkable || Database.Instance[toX, toY].Type == CellType.Exit;

            PathFinder finder = new(isFromPointPath ? new List<(int x, int y)>() { (fromX, fromY) } : Database.Instance[Database.Instance[(fromX, fromY)]],
                isToPointPath ? new List<(int x, int y)>() { (toX, toY) } : Database.Instance[Database.Instance[(toX, toY)]], pathType);

            return finder.findPath();
        }

    }

}
