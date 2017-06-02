using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameManager gm;
    // shape that is under control right now
    public static Shape shape { get; set; }

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (gm.gameState == GameManager.GameState.Playing)
        {
            // Move Left
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                shape.Move(new Vector3(-1, 0, 0));
            }
            // Move Right
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                shape.Move(new Vector3(1, 0, 0));
            }
            // Counter Clockwise Rotation
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                shape.Rotate(new Vector3(0, 0, -90));
            }
            // Clockwise Rotation
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                shape.Rotate(new Vector3(0, 0, 90));
            }
            // Speed up things
            else if (Input.GetKey(KeyCode.Space))
            {
                Time.timeScale = 10f / gm.level;
            }
            // Return to normal speed
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                Time.timeScale = 1f;
            }
            // Pause game
            else if (Input.GetKeyDown(KeyCode.P))
            {
                gm.PauseGame();
            }
        }
        else if (gm.gameState == GameManager.GameState.Pause)
        {
            // Unpause game
            if (Input.GetKeyDown(KeyCode.P))
                gm.ResumeGame();
        }
        else if (gm.gameState == GameManager.GameState.GameOver)
        {

        }
        // Restart if not on pause
        if (gm.gameState != GameManager.GameState.Pause)
        {
            if (Input.GetKeyDown(KeyCode.R))
                gm.StartGame();
        }
    }
}
