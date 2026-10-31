using UnityEngine;

public class Ball : MonoBehaviour
{
    public float killBelowY = -7.5f;

    void Update()
    {
        if (transform.position.y < killBelowY)
            Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponentInParent<Cather>() == null)
            return;

        if (GameManager.Instance != null)
            GameManager.Instance.AddScore();

        Destroy(gameObject);
    }
}