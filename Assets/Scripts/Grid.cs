using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static int width = 10;
    public static int height = 20;

    public static Transform[,] grid = new Transform[width, height];

    // checks if block is in grid boundaries
    public static bool inGrid(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0 && (int)pos.y < height);
    }

    static void deleteRow(int y)
    {
        // destroy every block object in row
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    static void buildRows(int i)
    {
        // check every block from row y and above
        for (int x = 0; x < width; x++)
        {
            for (int y = i; y < height; y++)
            {
                // move all blocks at one row down
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;

                    grid[x, y - 1].position += new Vector3(0, -1, 0);
                }
            }
        }
    }

    static bool isRowFull(int y)
    {
        for (int x = 0; x < width; x++)
            if (grid[x, y] == null)
                return false;
        return true;
    }

    public static int checkRows()
    {
        // builded rows counter
        int i = 0;
        for (int y = 0; y < height; y++)
        {
            if (isRowFull(y))
            {
                deleteRow(y);
                buildRows(y + 1);
                y--;
                i++;
            }
        }
        return i;
    }

    public static void Clear()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] != null)
                {
                    Destroy(grid[x, y].gameObject);
                    grid[x, y] = null;
                }
            }
        }
    }

    public static bool isGameOver()
    {
        // game over when some block of top line is occupied 
        for (int x = 0; x < width; x++)
        {
            if (grid[x, height - 1] != null)
            {
                return true;
            }
        }
        return false;
    }
}