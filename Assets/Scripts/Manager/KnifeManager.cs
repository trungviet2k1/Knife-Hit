using UnityEngine;

public class KnifeManager : MonoBehaviour
{
    public static KnifeManager Instance { get; private set; }

    [Header("Knife Settings")]
    public float throwSpeed;
    public GameObject knifePrefab;
    public Transform knifeSpawnPoint;

    private Rigidbody2D rb;
    private bool isThrown = false;
    private int knivesRemaining;

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

    void Start()
    {
        InstantiateNewKnife();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isThrown)
        {
            isThrown = true;
            rb.velocity = new Vector2(0, throwSpeed);
            knivesRemaining--;
        }
    }

    public void InstantiateNewKnife()
    {
        if (knivesRemaining > 0)
        {
            RemoveAllKnives();
            GameObject newKnife = Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity, knifeSpawnPoint);
            rb = newKnife.GetComponent<Rigidbody2D>();
            isThrown = false;
        }
    }

    public void SetKnivesRemaining(int knives)
    {
        knivesRemaining = knives;
    }

    public void ResetKnives()
    {
        RemoveAllKnives();
        InstantiateNewKnife();
    }

    public void RemoveAllKnives()
    {
        foreach (Transform child in knifeSpawnPoint)
        {
            Destroy(child.gameObject);
        }
    }
}