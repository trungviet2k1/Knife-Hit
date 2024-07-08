using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    [Header("Game Over UI")]
    public GameObject gameOverPanel;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        ScoreManager.Instance.ResetScore();
    }

    public void Restart()
    {
        gameOverPanel.SetActive(false);
        LevelManager.Instance.RestartLevel();
    }

    public void BackToHome()
    {
        Debug.Log("Return Home Menu");
    }
}