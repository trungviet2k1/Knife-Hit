using TMPro;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager Instance {  get; private set; }

    [Header("Gameplay UI")]
    public TextMeshProUGUI stageTitle;
    public TextMeshProUGUI score;
    public TextMeshProUGUI coin;

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

    public void UpdateStageTitle(int stageNumber)
    {
        stageTitle.text = "Stage " + stageNumber.ToString();
    }

    public void UpdateScore(int score)
    {
        this.score.text = score.ToString();
    }

    public void UpdateCoin(int coinNumber)
    {
        coin.text = coinNumber.ToString();
    }
}