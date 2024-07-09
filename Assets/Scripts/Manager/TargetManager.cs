using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }

    [Header("Target Settings")]
    public Transform targetSpawnPoint;

    private readonly List<GameObject> targets = new();
    private int knivesEmbedded = 0;
    private int requiredKnives = 0;
    private bool levelCompleted = false;

    [HideInInspector] public float rotationSpeed = 0;

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

    public void SetLevelData(LevelData levelData)
    {
        rotationSpeed = levelData.rotationSpeed;
        requiredKnives = levelData.numberOfKnives;
        knivesEmbedded = 0;
        levelCompleted = false;

        ClearTargets();

        GameObject newTarget = Instantiate(levelData.Targets, targetSpawnPoint.position, Quaternion.identity, transform);
        targets.Add(newTarget);

        if (newTarget.TryGetComponent<Animator>(out var targetAnimator))
        {
            targetAnimator.SetTrigger("Appear");
        }

        var rotatingTarget = newTarget.GetComponent<Target>();
        rotatingTarget.SetRotationSpeed(levelData.rotationSpeed);
        rotatingTarget.reverseRotation = levelData.reverseRotation;
        rotatingTarget.pauseDuration = levelData.pauseDuration;
        rotatingTarget.completeStop = levelData.completeStop;
        rotatingTarget.speedGraduallyDecreases = levelData.speedGraduallyDecreases;

        GamePlayManager.Instance.InitializeKnifeIcons(levelData.numberOfKnives);
    }

    public void KnifeEmbedded()
    {
        knivesEmbedded++;
        GamePlayManager.Instance.UpdateKnifeIcons(requiredKnives - knivesEmbedded);
        if (knivesEmbedded == requiredKnives && !levelCompleted)
        {
            levelCompleted = true;

            // Updated UI, Animation and effects for target when winning

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