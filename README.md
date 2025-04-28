# Map Creation Application

## Overview

The **Map Creation Application** is a C# WinForms project designed to let users create custom maps with labeled places and streets. Users can design maps either with the built-in map drawer or by importing images created in programs like Photoshop (following the user guide). Once a map is created, users can leverage powerful pathfinding algorithms to find the shortest or fastest routes between walkable points.

---

## Features

- **Custom Map Drawer:**  
  Draw and label streets and places directly within the application.
- **Import Maps:**  
  Optionally, design your map in Photoshop and import it into the application.
- **Pathfinding Algorithms:**  
  - Find the shortest or fastest path between two points using the efficient A* algorithm.
  - Map graph structure supports robust pathfinding.
- **Map Editing Tools:**  
  - Draw lines, rectangles, and snap to grid for precise map creation.
  - Undo and redo capabilities for flexible editing.
- **MVC Architecture:**  
  Clean separation of concerns using the Model-View-Controller design pattern.
- **Singleton Pattern:**  
  Ensures a single instance of key classes.
- **SOLID Principles:**  
  Applied to improve maintainability and extensibility.
- **Simulated Database:**  
  A database class is used to mimic persistent storage.

---

## Architecture

- **Language & Framework:**  
  C# with Windows Forms (WinForms) for the GUI.
- **Design Patterns:**  
  - **MVC:** For separation of UI, business logic, and data.
  - **Singleton:** To ensure single instances where necessary.
- **Algorithms Used:**  
  - **Flood Fill (BFS):** For filling areas in the map drawer.
  - **A* Algorithm:** For efficient pathfinding.
  - **Geometry Utilities:** For drawing and snapping shapes.

---

## Getting Started

### Prerequisites

- Windows OS
- [.NET Framework](https://dotnet.microsoft.com/)
- Visual Studio (recommended)

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/AnsElwkel/Map-Tool-Application
   ```
2. **Open the solution file** (`.sln`) in Visual Studio.
3. **Build and Run** the solution.

---

## Usage

1. **Create a Map:**
   - Use the integrated map drawer to design your map.
   - Label streets and places as needed.
   - Or import a map image (see user guide for details).
2. **Edit Map:**
   - Use draw tools (line, rectangle, snap to grid).
   - Use Undo/Redo to modify your map easily.
3. **Pathfinding:**
   - Load your map.
   - Select two walkable points.
   - Find the shortest or fastest path.

---

## Algorithms

- **Flood Fill (BFS):**  
  Used for filling regions in the map drawer.
- **A* Pathfinding:**  
  Calculates the most efficient route between two points.
- **Geometry & Math:**  
  Used for drawing operations and snapping elements to grid.

---

## Design Patterns

- **MVC:**  
  Ensures a structured and maintainable codebase.
- **Singleton:**  
  Guarantees only one instance for core management classes.
- **SOLID Principles:**  
  We try to follow some SOLID principles for better extensibility.

---

## Simulated Database

A dedicated `Database` class mimics persistent storage for maps and user data.

---

## Acknowledgements

- Algorithms reference: [Wikipedia - A* search algorithm](https://en.wikipedia.org/wiki/A*_search_algorithm)
- C# and WinForms documentation by Microsoft

---

**Enjoy creating and navigating your own maps!**

---
