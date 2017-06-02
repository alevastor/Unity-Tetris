using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text hiScoreText;
    [SerializeField]
    Text levelText;
    [SerializeField]
    GameObject gameOverPanel;
    [SerializeField]
    GameObject pausePanel;
    [SerializeField]
    public float baseTimeToFall = 1f;

    public enum GameState { Playing, GameOver, Pause };
    [HideInInspector]
    public GameState gameState;

    public int linesToLevelUp = 30;

    ShapeSpawner spawner;
    int score;
    int hiScore;
    public int level
    {
        get;
        private set;
    }
    int linesBuilded;

    void Start()
    {
        spawner = FindObjectOfType<ShapeSpawner>();
        hiScore = PlayerPrefs.GetInt("hiScore");
        StartGame();
    }

    public void StartGame()
    {
        // clear after previous game
        foreach (Transform child in spawner.transform)
        {
            Destroy(child.gameObject);
        }
        Grid.Clear();
        gameOverPanel.SetActive(false);

        // prepare new game
        score = 0;
        level = 1;
        linesBuilded = 0;

        spawner.SpawnNext();
        gameState = GameState.Playing;
        UpdateVisuals();
    }

    public void GameOver()
    {
        // in case if player holded speed up button when lost
        Time.timeScale = 1f;
        gameState = GameState.GameOver;
        UpdateHiScore();
        gameOverPanel.SetActive(true);
    }

    private void UpdateHiScore()
    {
        if(score > hiScore)
        {
            hiScore = score;
        }
        PlayerPrefs.SetInt("hiScore", hiScore);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        gameState = GameState.Pause;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        gameState = GameState.Playing;
        pausePanel.SetActive(false);
    }

    public void RowsBuilded(int count)
    {
        linesBuilded += count;
        switch (count)
        {
            case 0:
                score += 5 * level; break;
            case 1:
                score += 20 * level; break;
            case 2:
                score += 50 * level; break;
            case 3:
                score += 125 * level; break;
            case 4:
                score += 400 * level; break;
            default:
                Debug.Log("GameManager::RowsBuilded - rows builded count: " + count); break;
        }
        level = linesBuilded / linesToLevelUp + 1;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        scoreText.text = score.ToString();
        hiScoreText.text = hiScore.ToString();
        levelText.text = level.ToString();
    }
}
