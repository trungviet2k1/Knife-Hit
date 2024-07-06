using UnityEngine;

public class RotatingTarget : MonoBehaviour
{
    public bool reverseRotation = false;
    public float pauseDuration = 0f;
    private float currentPauseTime = 0f;
    private bool isPaused = false;

    void Update()
    {
        if (isPaused)
        {
            currentPauseTime += Time.deltaTime;
            if (currentPauseTime >= pauseDuration)
            {
                isPaused = false;
                currentPauseTime = 0f;
            }
        }
        else
        {
            RotateTarget();
        }
    }

    private void RotateTarget()
    {
        float rotationDirection = reverseRotation ? -1f : 1f;
        transform.Rotate(0, 0, rotationDirection * TargetManager.Instance.rotationSpeed * Time.deltaTime);
    }

    public void TogglePause(float duration)
    {
        pauseDuration = duration;
        isPaused = true;
    }

    public void ToggleReverse()
    {
        reverseRotation = !reverseRotation;
    }
}