namespace MazeMechanics.Cells
{
    public static class MazeGenerator
    {
        public static bool[,] BuildMaze(int width, int height, int entryX, int entryY, int seed)
        {
            // Initialize maze with walls
            bool[,] maze = new bool[width, height];
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    maze[x, y] = true;
                }
            }

            var random = new System.Random(seed);
            
            GenerateMaze(maze, entryX, entryY, random);
            
            return maze;
        }

        private static void GenerateMaze(bool[,] maze, int x, int y, System.Random random)
        {
            int[] directions = { 1, 2, 3, 4 };
            Shuffle(directions, random);

            foreach (int direction in directions) {
                int newX = x;
                int newY = y;

                switch (direction) {
                    case 1:
                        newY -= 2;
                        break; // Up
                    case 2:
                        newX += 2;
                        break; // Right
                    case 3:
                        newY += 2;
                        break; // Down
                    case 4:
                        newX -= 2;
                        break; // Left
                }
                
                if (CheckIsInsideWalls(maze, newX, newY) && CheckIsWall(maze, newX, newY)) {
                    maze[newX, newY] = false;
                    var pointBetweenXAndNewX = newX + (x - newX) / 2;
                    var pointBetweenYAndNewY = newY + (y - newY) / 2;
                    maze[pointBetweenXAndNewX, pointBetweenYAndNewY] = false;
                    GenerateMaze(maze, newX, newY, random);
                }
            }
        }

        private static bool CheckIsInsideWalls(bool[,] maze, int newX, int newY)
        {
            var width = maze.GetLength(0);
            var height = maze.GetLength(1);
            var isInsideWallsNewX = newX > 0 && newX < width;
            var isInsideWallsNewY = newY > 0 && newY < height;
            var isInsideWalls = isInsideWallsNewX && isInsideWallsNewY;
            return isInsideWalls;
        }

        private static bool CheckIsWall(bool[,] maze, int newX, int newY)
        {
            return maze[newX, newY];
        }

        private static void Shuffle(int[] array, System.Random random)
        {
            int n = array.Length;
            for (int i = 0; i < n; i++) {
                int r = i + random.Next(n - i);
                (array[r], array[i]) = (array[i], array[r]);
            }
        }
    }
}