using Random = System.Random;

namespace LevelMechanics.LevelMechanics.Cells
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

            var random = new Random(seed);
            
            GenerateMaze(maze, entryX, entryY, random);
            
            return maze;
        }

        private static void GenerateMaze(bool[,] maze, int x, int y, Random random)
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

                var couldRemoveWallInNewCoords = CheckIsWall(maze, newX, newY);
                if (couldRemoveWallInNewCoords) {
                    maze[newX, newY] = false;
                    var pointBetweenXAndNewX = newX + (x - newX) / 2;
                    var pointBetweenYAndNewY = newY + (y - newY) / 2;
                    maze[pointBetweenXAndNewX, pointBetweenYAndNewY] = false;
                    GenerateMaze(maze, newX, newY, random);
                }
            }
        }

        private static bool CheckIsWall(bool[,] maze, int newX, int newY)
        {
            var width = maze.GetLength(0);
            var height = maze.GetLength(1);
            var isInRangeNewX = newX > 0 && newX < width;
            var isInRangeNewY = newY > 0 && newY < height;
            var areNewCoordsValid = isInRangeNewX && isInRangeNewY;
            var isWallInNewCoors = maze[newX, newY];
            var couldRemoveWall = areNewCoordsValid && isWallInNewCoors;
            return couldRemoveWall;
        }

        private static void Shuffle(int[] array, Random random)
        {
            int n = array.Length;
            for (int i = 0; i < n; i++) {
                int r = i + random.Next(n - i);
                (array[r], array[i]) = (array[i], array[r]);
            }
        }
    }
}