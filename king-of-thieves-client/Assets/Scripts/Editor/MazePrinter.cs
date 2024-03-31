using LevelMechanics.LevelMechanics.Cells;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public class MazePrinter
{
    [MenuItem("Thief/Print random maze %g")]
    private static void PrintRandomMaze()
    {
        var seed = new Random().Next();
        var maze = MazeGenerator.BuildMaze(9, 9, 1, 9 - 2, seed);
        var sb = new System.Text.StringBuilder();
        for (int y = 0; y < maze.GetLength(1); y++) {
            for (int x = 0; x < maze.GetLength(0); x++) {
                sb.Append(maze[x, y] ? "#" : " ");
            }

            sb.AppendLine();
        }

        sb.AppendLine($"seed: {seed}");
        Debug.Log(sb.ToString());
    }
}