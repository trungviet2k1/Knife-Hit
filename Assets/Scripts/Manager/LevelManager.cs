using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("List Stages")]
    public List<LevelData> levels;

    [Header("Stage Transition Delay")]
    public float levelTransitionDelay = 1.5f;

    private int currentLevelIndex = 0;

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
        LoadLevel(currentLevelIndex);
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Count)
            return;

        currentLevelIndex = levelIndex;
        LevelData levelData = levels[levelIndex];
        TargetManager.Instance.SetLevelData(levelData);
        KnifeManager.Instance.SetKnivesRemaining(levelData.numberOfKnives);
        KnifeManager.Instance.ResetKnives();
        GamePlayManager.Instance.UpdateStageTitle(levelData.stageNumber);
    }

    public void NextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < levels.Count)
        {
            StartCoroutine(WaitAndLoadNextLevel(levelTransitionDelay));
        }
        else
        {
            Debug.Log("Game Completed!");
        }
    }

    IEnumerator WaitAndLoadNextLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadLevel(currentLevelIndex);
    }

    public void RestartLevel()
    {
        currentLevelIndex = 0;
        LoadLevel(currentLevelIndex);
    }
}