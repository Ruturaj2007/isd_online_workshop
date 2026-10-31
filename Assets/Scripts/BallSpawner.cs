using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject ballPrefab;
    public GameObject bombPrefab;

    [Header("Cadence")]
    public float interval = 1.1f;
    public float minInterval = 0.5f;
    public float intervalRampPerSecond = 0.02f;
    public float startDelay = 0.6f;

    [Header("Placement")]
    public float halfWidth = 7f;
    public float spawnY = 6.5f;

    [Header("Difficulty")]
    public float bombChance = 0.25f;
    public float speedRampPerSecond = 0.03f;
    public float maxSpeedFactor = 2f;

    float timer;
    float elapsed;

    public float Elapsed { get { return elapsed; } }
    public float CurrentInterval { get { return Mathf.Max(minInterval, interval - elapsed * intervalRampPerSecond); } }
    public float CurrentSpeedFactor { get { return Mathf.Min(maxSpeedFactor, 1f + elapsed * speedRampPerSecond); } }

    void Start()
    {
        timer = Mathf.Max(0f, interval - startDelay);
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        elapsed += Time.deltaTime;
        timer += Time.deltaTime;

        if (timer < CurrentInterval)
            return;

        timer = 0f;
        Spawn();
    }

    public void Spawn()
    {
        bool spawnBomb = bombPrefab != null && ballPrefab != null && Random.value < bombChance;
        GameObject prefab = spawnBomb ? bombPrefab : ballPrefab;

        if (prefab == null)
            return;

        Vector3 pos = new Vector3(Random.Range(-halfWidth, halfWidth), spawnY, 0f);
        GameObject spawned = Instantiate(prefab, pos, Quaternion.identity);

        Rigidbody2D rb = spawned.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.gravityScale = CurrentSpeedFactor;
    }
}