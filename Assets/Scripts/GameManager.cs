using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text scoreText;
    public Text gameOverText;
    public List<Image> hearts = new List<Image>();
    public int maxLives = 3;

    public Color heartColor = new Color(0.92f, 0.20f, 0.28f, 1f);
    public Color lostHeartColor = new Color(1f, 1f, 1f, 0.16f);

    int score;
    int lives;
    bool gameOver;

    public int Score { get { return score; } }
    public int Lives { get { return lives; } }
    public bool IsGameOver { get { return gameOver; } }

    void Awake()
    {
        Instance = this;
        score = 0;
        lives = maxLives;
        gameOver = false;
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);
        RefreshScore();
        RefreshHearts();
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore()
    {
        if (gameOver)
            return;

        score++;
        RefreshScore();
    }

    public void LoseLife()
    {
        if (gameOver)
            return;

        lives--;
        if (lives < 0)
            lives = 0;

        RefreshHearts();

        if (lives <= 0)
            EndGame();
    }

    void EndGame()
    {
        gameOver = true;

        BallSpawner[] spawners = FindObjectsByType<BallSpawner>(FindObjectsSortMode.None);
        for (int i = 0; i < spawners.Length; i++)
            spawners[i].enabled = false;

        Ball[] balls = FindObjectsByType<Ball>(FindObjectsSortMode.None);
        for (int i = 0; i < balls.Length; i++)
            Destroy(balls[i].gameObject);

        Bomb[] bombs = FindObjectsByType<Bomb>(FindObjectsSortMode.None);
        for (int i = 0; i < bombs.Length; i++)
            Destroy(bombs[i].gameObject);

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(true);
    }

    void RefreshScore()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void RefreshHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (hearts[i] == null)
                continue;

            hearts[i].color = i < lives ? heartColor : lostHeartColor;
        }
    }
}