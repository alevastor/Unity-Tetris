using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    GameManager gm;

    // time since last fall
    float lastFall = 0;
    // time left to next fall
    float timeToFall;

    void Start()
    {
        // Disable shapes that are used in "NextShape"
        if (transform.parent.name == "NextShape")
        {
            enabled = false;
            return;
        }
        gm = FindObjectOfType<GameManager>();
        // move spawned shape in grid boundaries
        while (isValidBlockPosition(true) == false)
        {
            transform.position += new Vector3(0, -1, 0);
        }
        // check for block overlapping, if so, then game over
        if (isValidBlockPosition() == false)
        {
            GameOver();
        }
        else
        {
            updateGrid();
            // make this shape controlled by input manager
            InputManager.shape = this;
            timeToFall = gm.baseTimeToFall / gm.level;
        }
    }

    void GameOver(bool destroyObject = true)
    {
        gm.GameOver();
        if (destroyObject)
            Destroy(gameObject);
        else
            enabled = false;
    }

    // move shape if position is valid
    public bool Move(Vector3 dir)
    {
        transform.position += dir;
        if (isValidBlockPosition())
        {
            updateGrid();
            return true;
        }
        else
        {
            transform.position -= dir;
            return false;
        }
    }

    // rotate shape if position is valid
    public void Rotate(Vector3 rot)
    {
        transform.Rotate(rot);

        if (isValidBlockPosition())
            updateGrid();
        else
            transform.Rotate(-rot);
    }

    void Update()
    {
        // move shape down after timeToFall cooldown
        if (lastFall >= timeToFall)
        {
            // if shape position is invalid
            if (Move(new Vector3(0, -1, 0)) == false)
            {
                // check for builded rows
                int count = Grid.checkRows();
                // send rows count to the game manager
                gm.RowsBuilded(count);
                // check if not game over
                if (Grid.isGameOver())
                {
                    GameOver(false);
                }
                else
                {
                    // spawn next shape
                    FindObjectOfType<ShapeSpawner>().SpawnNext();
                    // make this shape disabled
                    enabled = false;
                }

            }
            lastFall = 0;
        }
        lastFall += Time.deltaTime;
    }

    bool isValidBlockPosition(bool invulnerable = false)
    {
        // check for every block in shape
        foreach (Transform child in transform)
        {
            Vector2 v = Utilities.roundVector2(child.position);

            // if some block is not in grid then position is invalid
            if (Grid.inGrid(v) == false)
                return false;

            if (invulnerable == false)
                // if position of some block already occupied by block of another shape then position is invalid
                if (Grid.grid[(int)v.x, (int)v.y] != null && Grid.grid[(int)v.x, (int)v.y].parent != transform)
                    return false;
        }
        return true;
    }

    void updateGrid()
    {
        // Remove old blocks of this shape from grid
        for (int x = 0; x < Grid.width; x++)
            for (int y = 0; y < Grid.height; y++)
                if (Grid.grid[x, y] != null)
                    if (Grid.grid[x, y].parent == transform)
                        Grid.grid[x, y] = null;

        // Add new blocks of this shape to the grid
        foreach (Transform child in transform)
        {
            Vector2 v = Utilities.roundVector2(child.position);
            Grid.grid[(int)v.x, (int)v.y] = child;
        }
    }
}
