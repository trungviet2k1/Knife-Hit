using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Level Settings")]
    public int stageNumber;
    public string bossName;
    public int numberOfKnives;
    public GameObject Targets;
    public bool isBossLevel;

    [Header("Rotation Settings")]
    public float rotationSpeed;
    public bool reverseRotation;

    [Header("Pause Duration")]
    public float pauseDuration;
    public bool completeStop;
    public bool speedGraduallyDecreases;
}