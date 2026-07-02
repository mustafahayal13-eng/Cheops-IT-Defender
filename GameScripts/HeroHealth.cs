using UnityEngine;
using UnityEngine.UI;

public class HeroHealth : MonoBehaviour
{
    [Header("Vie")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public Slider healthBar;
    public Image healthBarFill;

    [Header("Invincibilité après dégâts")]
    public float invincibleDuration = 1f;
    private float invincibleTimer = 0f;
    private bool isInvincible = false;

    private SpriteRenderer sr;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;

            if (sr != null)
                sr.enabled = Mathf.FloorToInt(invincibleTimer * 10f) % 2 == 0;

            if (invincibleTimer <= 0f)
            {
                isInvincible = false;
                if (sr != null) sr.enabled = true;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (animator != null)
            animator.SetTrigger("Hurt");

        if (healthBar != null)
            healthBar.value = currentHealth;

        if (healthBarFill != null)
        {
            float ratio = (float)currentHealth / maxHealth;
            if (ratio > 0.5f)
                healthBarFill.color = Color.green;
            else if (ratio > 0.25f)
                healthBarFill.color = Color.yellow;
            else
                healthBarFill.color = Color.red;
        }

        isInvincible = true;
        invincibleTimer = invincibleDuration;

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (healthBar != null)
            healthBar.value = currentHealth;
    }

    void Die()
    {
        // Arrêter le clignotement
        isInvincible = false;
        if (sr != null) sr.enabled = true;

        // Animation de mort
        if (animator != null)
            animator.SetTrigger("Death");

        // Désactiver les mouvements
        HeroMovement movement = GetComponent<HeroMovement>();
        if (movement != null) movement.enabled = false;

        HeroAttack attack = GetComponent<HeroAttack>();
        if (attack != null) attack.enabled = false;

        // Afficher le Game Over après 2 secondes
        Invoke("ShowGameOver", 2f);
    }

    void ShowGameOver()
    {
        // Récupérer le score depuis le GameManager
        int score = 0;
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null) score = gm.score;

        // Afficher l'écran Game Over
        GameOverManager gom = FindFirstObjectByType<GameOverManager>();
        if (gom != null) gom.ShowGameOver(score);
    }
}