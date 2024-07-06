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

        var rotatingTarget = newTarget.GetComponent<Target>();
        rotatingTarget.SetRotationSpeed(levelData.rotationSpeed);
        rotatingTarget.reverseRotation = levelData.reverseRotation;
        rotatingTarget.pauseDuration = levelData.pauseDuration;
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

    public void ToggleTargetPause(int index, float duration)
    {
        if (index >= 0 && index < targets.Count)
        {
            targets[index].GetComponent<Target>().PauseRotation(duration);
        }
    }

    public void ToggleTargetReverse(int index)
    {
        if (index >= 0 && index < targets.Count)
        {
            targets[index].GetComponent<Target>().ToggleReverseRotation();
        }
    }

    public void SetTargetRotationSpeed(int index, float speed)
    {
        if (index >= 0 && index < targets.Count)
        {
            targets[index].GetComponent<Target>().SetRotationSpeed(speed);
        }
    }
}