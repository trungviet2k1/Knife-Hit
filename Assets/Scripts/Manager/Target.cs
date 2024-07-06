using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Target Value Settings")]
    public float knifeEmbedDepth;
    public float bounceForce = 10f;
    public float gameOverDelay = 1.5f;

    [Header("Rotation Settings")]
    public float rotationSpeed;
    public bool reverseRotation;
    public float pauseDuration;

    private bool isPaused = false;
    private Coroutine rotationCoroutine;

    private void Start()
    {
        rotationSpeed = TargetManager.Instance.rotationSpeed;
        rotationCoroutine = StartCoroutine(RotationRoutine());
    }

    public void SetRotationSpeed(float speed)
    {
        TargetManager.Instance.rotationSpeed = speed;
    }

    public void ToggleReverseRotation()
    {
        reverseRotation = !reverseRotation;
    }

    public void PauseRotation(float duration)
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }
        StartCoroutine(PauseRoutine(duration));
    }

    private IEnumerator PauseRoutine(float duration)
    {
        isPaused = true;
        yield return new WaitForSeconds(duration);
        isPaused = false;
        rotationCoroutine = StartCoroutine(RotationRoutine());
    }

    private IEnumerator RotationRoutine()
    {
        while (true)
        {
            if (!isPaused)
            {
                float direction = reverseRotation ? -1 : 1;
                transform.Rotate(0, 0, rotationSpeed * direction * Time.deltaTime);
            }
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Knife"))
        {
            Rigidbody2D knifeRb = collision.gameObject.GetComponent<Rigidbody2D>();

            foreach (Transform child in transform)
            {
                if (child.CompareTag("Knife") && child != collision.transform)
                {
                    if (Vector2.Distance(collision.transform.position, child.position) < 0.4f)
                    {
                        knifeRb.isKinematic = false;
                        Vector2 bounceDirection = (collision.transform.position - child.position).normalized;
                        knifeRb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
                        PauseRotation(rotationSpeed = 0);
                        StartCoroutine(ShowGameOverPanel());
                        return;
                    }
                }
            }

            if (collision.transform.parent != transform)
            {
                knifeRb.velocity = Vector2.zero;
                knifeRb.isKinematic = true;
                collision.transform.position += collision.transform.up * knifeEmbedDepth;
                collision.transform.SetParent(transform);
                KnifeManager.Instance.InstantiateNewKnife();
                KnifeManager.Instance.IncrementScore();
                TargetManager.Instance.KnifeEmbedded();
            }
        }
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(gameOverDelay);
        GameOverManager.Instance.GameOver();
    }
}