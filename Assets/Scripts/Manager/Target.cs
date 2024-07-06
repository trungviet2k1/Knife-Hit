using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Target Value Settings")]
    public float knifeEmbedDepth;
    public float bounceForce = 10f;
    public float gameOverDelay = 1.5f;

    void Update()
    {
        transform.Rotate(0, 0, TargetManager.Instance.rotationSpeed * Time.deltaTime);
    }

    public void SetRotationSpeed(float speed)
    {
        TargetManager.Instance.rotationSpeed = speed;
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
                        TargetManager.Instance.rotationSpeed = 0f;
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