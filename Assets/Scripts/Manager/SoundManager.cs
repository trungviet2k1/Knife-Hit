using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Sound Effects")]
    public AudioClip targetAppear;
    public AudioClip hitTarget;
    public AudioClip hitKnives;
    public AudioClip hitFruits;
    public AudioClip targetBreaking;

    [Header("Audio Sources")]
    public AudioSource targetAppearSource;
    public AudioSource hitTargetSource;
    public AudioSource hitKnivesSource;
    public AudioSource hitFruitsSource;
    public AudioSource targetBreakingSource;

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

    public void PlayTargetAppearSound()
    {
        targetAppearSource.PlayOneShot(targetAppear);
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
    
    public void PlayTargetBreakingSound()
    {
        targetBreakingSource.PlayOneShot(targetBreaking);
    }
}