using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

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
    }

    public void Restart()
    {
        gameOverPanel.SetActive(false);
        TargetManager.Instance.RestartTarget();
        KnifeManager.Instance.RemoveKnife();
        KnifeManager.Instance.InstantiateNewKnifeAfterEmbed();
    }

    public void BackToHome()
    {
        Debug.Log("Return Home Menu");
    }
}