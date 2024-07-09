using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

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

    [Header("Level Icon")]
    public GameObject stageIconContainer;
    public GameObject bossChallengeImage;
    public Image[] stageIcons;
    public Color defaultColor = new();
    public Color passedLevelColor = new();
    public Color bossLevelColor = new();
    private Animator bossChallengeAnimator;

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

    void Start()
    {
        stageIcons = stageIconContainer.GetComponentsInChildren<Image>();
        bossChallengeAnimator = bossChallengeImage.GetComponent<Animator>();
    }

    public void UpdateStageTitle(int stageNumber)
    {
        stageTitle.text = "Stage " + stageNumber.ToString();
    }

    public void UpdateBossTitle(string bossTitle)
    {
        stageTitle.text = "BOSS: " + bossTitle;
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

    public void UpdateStageIcons(int levelIndex, bool isBossLevel)
    {
        if (levelIndex < 0 || levelIndex >= stageIcons.Length)
        {
            return;
        }

        if (isBossLevel)
        {
            HideStageIcons(levelIndex);
            stageIcons[levelIndex].color = bossLevelColor;
            TriggerBossChallengeAnimation();
        }
        else
        {
            stageIcons[levelIndex].color = passedLevelColor;
            if (levelIndex > 0 && LevelManager.Instance.GetLevelData(levelIndex - 1).isBossLevel)
            {
                TriggerBossChallengeZoomIn();
            }
        }
    }

    private void HideStageIcons(int bossLevelIndex)
    {
        for (int i = 0; i < bossLevelIndex; i++)
        {
            stageIcons[i].gameObject.SetActive(false);
        }
    }

    private void TriggerBossChallengeAnimation()
    {
        if (bossChallengeAnimator != null)
        {
            bossChallengeAnimator.SetTrigger("ZoomOut");
        }
    }

    private void TriggerBossChallengeZoomIn()
    {
        if (bossChallengeAnimator != null)
        {
            bossChallengeAnimator.SetTrigger("ZoomIn");
        }
    }

    public void ResetStageIcons()
    {
        foreach (var icon in stageIcons)
        {
            icon.color = defaultColor;
            icon.gameObject.SetActive(true);
        }
    }
}