using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 6f;
    public int damage = 10;
    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Touche le joueur
        if (col.CompareTag("Player"))
        {
            HeroHealth health = col.GetComponent<HeroHealth>();
            if (health != null) health.TakeDamage(damage);
            Destroy(gameObject);
        }

        // Touche le sol ou un mur
        if (col.CompareTag("Ground"))
            Destroy(gameObject);
    }

    void Start()
    {
        // Détruire le projectile après 5 secondes
        Destroy(gameObject, 5f);
    }
}
