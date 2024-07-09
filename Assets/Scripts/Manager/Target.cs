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

    [Header("Pause Duration")]
    public float pauseDuration;
    public bool completeStop;
    public bool speedGraduallyDecreases;

    private bool isPaused = false;
    private float initialRotationSpeed;
    private float currentRotation = 0f;
    private bool shouldReverseRotation = false;

    private void Start()
    {
        initialRotationSpeed = rotationSpeed;

        if (reverseRotation && pauseDuration == 0)
        {
            shouldReverseRotation = true;
        }

        if (pauseDuration > 0)
        {
            if (completeStop)
            {
                StartCoroutine(Type1RotationCycle());
            }
            else if (speedGraduallyDecreases)
            {
                StartCoroutine(Type2RotationCycle());
            }
        }
    }

    public void SetRotationSpeed(float speed)
    {
        if (!isPaused)
        {
            initialRotationSpeed = speed;
        }
        rotationSpeed = speed;
    }

    public void ToggleReverseRotation()
    {
        reverseRotation = !reverseRotation;
    }

    private void Update()
    {
        ReverseRotation();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Knife"))
        {
            HandleKnifeCollision(collision.gameObject);
            HandleCoinCollision(collision.gameObject);
        }
    }

    private void HandleKnifeCollision(GameObject knife)
    {
        Rigidbody2D knifeRb = knife.GetComponent<Rigidbody2D>();

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Knife") && child != knife.transform)
            {
                if (Vector2.Distance(knife.transform.position, child.position) < 0.5f)
                {
                    SoundManager.Instance.PlayKnivesTargetSound();
                    knifeRb.isKinematic = false;
                    Vector2 bounceDirection = (knife.transform.position - child.position).normalized;
                    knifeRb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
                    SetRotationSpeed(initialRotationSpeed = 0);
                    SetRotationSpeed(rotationSpeed = 0);
                    StartCoroutine(ShowGameOverPanel());
                    return;
                }
            }
        }

        if (knife.transform.parent != transform)
        {
            SoundManager.Instance.PlayHitTargetSound();
            knifeRb.velocity = Vector2.zero;
            knifeRb.isKinematic = true;
            knife.transform.position += knife.transform.up * knifeEmbedDepth;
            knife.transform.SetParent(transform);
            KnifeManager.Instance.InstantiateNewKnife();
            ScoreManager.Instance.IncrementScore();
            TargetManager.Instance.KnifeEmbedded();
        }
    }

    private void HandleCoinCollision(GameObject coin)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(coin.transform.position, 0.1f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Coin") && collider.transform.IsChildOf(transform))
            {
                SoundManager.Instance.PlayFruitsTargetSound();
                CoinManager.Instance.IncrementCoin();
                Destroy(collider.gameObject);
            }
        }
    }

    //Reverse the rotation direction when rotating through 360 degrees
    void ReverseRotation()
    {
        if (!isPaused)
        {
            float direction = reverseRotation ? -1 : 1;
            float rotationStep = rotationSpeed * direction * Time.deltaTime;
            transform.Rotate(0, 0, rotationStep);
            currentRotation += Mathf.Abs(rotationStep);

            if (pauseDuration == 0 && shouldReverseRotation && currentRotation >= 360f)
            {
                ToggleReverseRotation();
                currentRotation = 0f;
            }
        }
    }

    //Stop completely after "Pause Duration"
    private IEnumerator Type1RotationCycle()
    {
        while (true)
        {
            isPaused = false;
            rotationSpeed = initialRotationSpeed;
            yield return new WaitForSeconds(pauseDuration);

            isPaused = true;
            rotationSpeed = 0;
            yield return new WaitForSeconds(pauseDuration);
        }
    }

    //Gradually reduce rotation speed after "Pause Duration"
    private IEnumerator Type2RotationCycle()
    {
        while (true)
        {
            isPaused = false;
            float currentSpeed = initialRotationSpeed;
            while (currentSpeed > 0)
            {
                currentSpeed -= initialRotationSpeed * Time.deltaTime / pauseDuration;
                rotationSpeed = currentSpeed;
                yield return null;
            }

            rotationSpeed = 0;
            yield return new WaitForSeconds(pauseDuration);

            currentSpeed = 0;
            while (currentSpeed < initialRotationSpeed)
            {
                currentSpeed += initialRotationSpeed * Time.deltaTime / pauseDuration;
                rotationSpeed = currentSpeed;
                yield return null;
            }

            rotationSpeed = initialRotationSpeed;
            yield return new WaitForSeconds(pauseDuration);
        }
    }

    private IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(gameOverDelay);
        GameOverManager.Instance.GameOver();
    }
}