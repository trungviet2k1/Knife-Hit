using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }

    [Header("Target Settings")]
    public Transform targetSpawnPoint;
    public float rotationSpeed;

    private readonly List<GameObject> targets = new();
    private int knivesEmbedded = 0;
    private int requiredKnives = 0;
    private bool levelCompleted = false;

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

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    public void SetLevelData(LevelData levelData)
    {
        rotationSpeed = levelData.rotationSpeed;
        requiredKnives = levelData.numberOfKnives;
        knivesEmbedded = 0;
        levelCompleted = false;

        ClearTargets();

        GameObject newTarget = Instantiate(levelData.Targets, targetSpawnPoint.position, Quaternion.identity, transform);
        targets.Add(newTarget);
    }

    public void KnifeEmbedded()
    {
        knivesEmbedded++;
        if (knivesEmbedded == requiredKnives && !levelCompleted)
        {
            levelCompleted = true;

            //Updated UI, Animation and effects for target when winning

            ClearTargets();
            LevelManager.Instance.NextLevel();
        }
    }

    private void ClearTargets()
    {
        foreach (var target in targets)
        {
            Destroy(target);
        }
        targets.Clear();
    }
}