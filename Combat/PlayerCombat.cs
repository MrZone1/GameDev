using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;


    public float attackRange = 0.5f;
    public int attackDamage = 50;

    public float attackRate = 2f;
    private float nextAttackTime = 0f;
    private Vector3 velocity = Vector3.zero;

    public static PlayerCombat instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerCombat dans la scène");
            return;
        }

        instance = this;
    }

    private void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
    }

    void Attack()
    {
        // Play an attack animation
        animator.SetTrigger("Attack");

        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage them
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
        {
            if (attackPoint == null)
                return;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
