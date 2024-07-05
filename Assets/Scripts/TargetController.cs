using UnityEngine;

public class TargetController : MonoBehaviour
{
    public float rotationSpeed;
    public float knifeEmbedDepth;
    public float bounceForce = 10f;

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
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
                        Debug.Log("Game Over: Knife hit another knife.");
                        knifeRb.isKinematic = false;
                        Vector2 bounceDirection = (collision.transform.position - child.position).normalized;
                        knifeRb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
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
                KnifeController.Instance.InstantiateNewKnifeAfterEmbed();
            }
        }
    }
}