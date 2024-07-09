using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager Instance { get; private set; }

    [Header("Gameplay UI")]
    public TextMeshProUGUI stageTitle;
    public TextMeshProUGUI score;
    public TextMeshProUGUI coin;

    [Header("Knife Icon")]
    public GameObject knifeIconContainer;
    public GameObject knifeIconPrefab;
    public Sprite usedKnifeIconSprite;
    public Sprite defaultKnifeIconSprite;

    private readonly List<Image> knifeIcons = new();

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

    public void InitializeKnifeIcons(int knifeCount)
    {
        foreach (var icon in knifeIcons)
        {
            Destroy(icon.gameObject);
        }
        knifeIcons.Clear();

        for (int i = 0; i < knifeCount; i++)
        {
            GameObject newIcon = Instantiate(knifeIconPrefab, knifeIconContainer.transform);
            knifeIcons.Add(newIcon.GetComponent<Image>());
        }
    }

    public void UpdateKnifeIcons(int knivesRemaining)
    {
        for (int i = 0; i < knifeIcons.Count; i++)
        {
            if (i < knifeIcons.Count - knivesRemaining)
            {
                knifeIcons[i].sprite = usedKnifeIconSprite;
            }
            else
            {
                knifeIcons[i].sprite = defaultKnifeIconSprite;
            }
        }
    }
}