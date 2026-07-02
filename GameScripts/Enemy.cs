using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Déplacement")]
    public float speed = 2f;
    public Transform target;

    [Header("Vie")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Tir")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    private float fireTimer = 0f;
    public float detectionRange = 8f;

    [Header("Score")]
    public int scoreValue = 10;

    private Transform player;

    void Start()
    {
        currentHealth = maxHealth;

        // Trouver le joueur automatiquement
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        // Si pas de joueur trouvé, chercher Cyborg
        if (player == null)
        {
            GameObject cyborg = GameObject.Find("Cyborg");
            if (cyborg != null) player = cyborg.transform;
        }

        // Marcher vers le joueur directement
        if (target == null)
            target = player;
    }

    void Update()
    {
        // --- Marcher vers la cible ---
        if (target != null)
        {
            Vector2 dir = (target.position - transform.position).normalized;
            transform.Translate(dir * speed * Time.deltaTime);

            // Retourner le sprite selon la direction
            if (dir.x < 0)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x),
                                                    transform.localScale.y, 1f);
            else
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),
                                                    transform.localScale.y, 1f);
        }

        // --- Tirer sur le joueur si proche ---
        if (player != null)
        {
            float dist = Vector2.Distance(transform.position, player.position);
            if (dist <= detectionRange)
            {
                fireTimer -= Time.deltaTime;
                if (fireTimer <= 0f)
                {
                    Shoot();
                    fireTimer = fireRate;
                }
            }
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        Vector2 dir = (player.position - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        EnemyBullet eb = bullet.GetComponent<EnemyBullet>();
        if (eb != null) eb.SetDirection(dir);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null) gm.AddScore(scoreValue);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            HeroHealth health = col.GetComponent<HeroHealth>();
            if (health != null) health.TakeDamage(10);
            Destroy(gameObject);
        }
    }
}