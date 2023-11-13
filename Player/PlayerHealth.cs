using UnityEngine;
using System.Collections;
using UnityEditorInternal;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public float invicibilityFlashDelay = 0.2f;
    public bool isInvicible = false;
    public float invicibilityTimeAfterHit = 3f;

    public SpriteRenderer graphics;

    public HealthBar healthBar;

    public static PlayerHealth instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerHealth dans la scène");
            return;
        }

        instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if(!isInvicible)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);

            // vérifier si le joueur est toujours vivant
            if(currentHealth <= 0)
            {
                Die();
                return;
            }

            isInvicible=true;
            StartCoroutine(InvisibilityFlash());
            StartCoroutine(HandleInvicibilityDelay());
        }
    }

    public void Die()
    {
        // bloquer les mouvements du personnage
        PlayerMovement.instance.enabled = false;
        // jouer l'animation d'élimination
        PlayerMovement.instance.animator.SetTrigger("Die");
        // bloquer les attaque du personnage
        PlayerCombat.instance.enabled = false;
        // empêcher les interaction physique avec les autres élément de la scène
        PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Kinematic;
        PlayerMovement.instance.playerCollider.enabled = false;
        GameOverManager.instance.OnPlayerDeath();
    }

    public void Respawn()
    {
        PlayerMovement.instance.enabled = true;
        PlayerMovement.instance.animator.SetTrigger("Respawn");
        PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Dynamic;
        PlayerMovement.instance.playerCollider.enabled = true;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public IEnumerator InvisibilityFlash()
    {
        while (isInvicible)
        {
            graphics.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(invicibilityFlashDelay);
            graphics.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invicibilityFlashDelay);
        }

    }

    public IEnumerator HandleInvicibilityDelay()
    {
        yield return new WaitForSeconds(invicibilityTimeAfterHit);
        isInvicible = false;
    }
}
