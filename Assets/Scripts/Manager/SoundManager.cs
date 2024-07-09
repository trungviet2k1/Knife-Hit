using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Sound Effects")]
    public AudioClip hitTarget;
    public AudioClip hitKnives;
    public AudioClip hitFruits;

    [Header("Audio Sources")]
    public AudioSource hitTargetSource;
    public AudioSource hitKnivesSource;
    public AudioSource hitFruitsSource;

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

    public void PlayHitTargetSound()
    {
        hitTargetSource.PlayOneShot(hitTarget);
    }

    public void PlayKnivesTargetSound()
    {
        hitTargetSource.PlayOneShot(hitKnives);
    }

    public void PlayFruitsTargetSound()
    {
        hitTargetSource.PlayOneShot(hitFruits);
    }
}