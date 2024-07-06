using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "LevelData")]
public class LevelData : ScriptableObject
{
    public int stageNumber;
    public float rotationSpeed;
    public int numberOfKnives;
    public GameObject Targets;
}