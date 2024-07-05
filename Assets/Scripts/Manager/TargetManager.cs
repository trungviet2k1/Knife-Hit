using System.Collections;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }

    public float rotationSpeed;
    public float knifeEmbedDepth;
    public float bounceForce = 10f;
    public float gameOverDelay = 1.5f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

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

        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        transform.Rotate(initialRotation.x, initialRotation.y, rotationSpeed * Time.deltaTime);
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
                    if (Vector2.Distance(collision.transform.position, child.position) < 0.3f)
                    {
                        knifeRb.isKinematic = false;
                        Vector2 bounceDirection = (collision.transform.position - child.position).normalized;
                        knifeRb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
                        rotationSpeed = 0f;
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
                KnifeManager.Instance.InstantiateNewKnifeAfterEmbed();
            }
        }
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(gameOverDelay);
        GameOverManager.Instance.GameOver();
    }

    public void RestartTarget()
    {
        rotationSpeed = 150;
        transform.SetPositionAndRotation(initialPosition, initialRotation);
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}