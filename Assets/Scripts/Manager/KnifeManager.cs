using UnityEngine;

public class KnifeManager : MonoBehaviour
{
    public static KnifeManager Instance { get; private set; }

    public float throwSpeed;
    public GameObject knifePrefab;
    public Transform knifeSpawnPoint;

    private Rigidbody2D rb;
    private bool isThrown = false;

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

        InstantiateNewKnife();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isThrown)
        {
            isThrown = true;
            rb.velocity = new Vector2(0, throwSpeed);
        }
    }

    public void InstantiateNewKnifeAfterEmbed()
    {
        Invoke(nameof(InstantiateNewKnife), 0.05f);
    }

    void InstantiateNewKnife()
    {
        GameObject newKnife = Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity, knifeSpawnPoint);
        rb = newKnife.GetComponent<Rigidbody2D>();
        isThrown = false;
    }

    public void RemoveKnife()
    {
        foreach (Transform child in knifeSpawnPoint)
        {
            Destroy(child.gameObject);
        }
    }
}