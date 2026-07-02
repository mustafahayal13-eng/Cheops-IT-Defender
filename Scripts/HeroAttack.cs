using UnityEngine;
 
public class HeroAttack : MonoBehaviour
{
    [Header("Attaque")]
    public float attackDamage = 1f;
    public float attackRange = 1.5f;
    public float attackCooldown = 0.4f;
    public LayerMask enemyLayer;
 
    [Header("Visuel")]
    public Transform attackPoint; // point devant le personnage
    public GameObject hitEffect;  // optionnel : effet visuel au point d'impact
 
    private float cooldownTimer = 0f;
    private bool facingRight = true;
 
    // Référence au HeroMovement pour savoir la direction
    private HeroMovement heroMovement;
 
    void Start()
    {
        heroMovement = GetComponent<HeroMovement>();
    }
 
    void Update()
    {
        cooldownTimer -= Time.deltaTime;
 
        // --- Attaque avec clic gauche ou touche J ---
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J))
            && cooldownTimer <= 0f)
        {
            Attack();
            cooldownTimer = attackCooldown;
        }
    }
 
    void Attack()
    {
        // Positionner le point d'attaque devant le personnage
        if (attackPoint != null)
        {
            float dir = transform.localScale.x > 0 ? 1f : -1f;
            attackPoint.localPosition = new Vector3(dir * Mathf.Abs(attackPoint.localPosition.x),
                                                    attackPoint.localPosition.y, 0);
        }
 
        // Détecter tous les ennemis dans la zone d'attaque
        Vector2 origin = attackPoint != null
            ? (Vector2)attackPoint.position
            : (Vector2)transform.position;
 
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, attackRange, enemyLayer);
 
        foreach (Collider2D hit in hits)
        {
            // Essayer d'appeler TakeDamage sur l'ennemi
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage((int)attackDamage);
                Debug.Log("Ennemi touché : " + hit.name);
            }
        }
 
        // Effet visuel optionnel
        if (hitEffect != null && attackPoint != null)
            Instantiate(hitEffect, attackPoint.position, Quaternion.identity);
    }
 
    // Visualiser la zone d'attaque en rouge dans l'éditeur
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 origin = attackPoint != null
            ? (Vector2)attackPoint.position
            : (Vector2)transform.position;
        Gizmos.DrawWireSphere(origin, attackRange);
    }
}
