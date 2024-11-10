using UnityEngine;
using System;
using TMPro;

public interface IDamageable
{
    void TakeDamage(float ammount);
}

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public static event Action OnPlayerDeath;
    [SerializeField] private float maxHealth;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private AudioSource hurtSound;

    private float health;
    private Animator animator;

    private void OnEnable()
    {
        OnPlayerDeath += Die;
    }

    private void OnDisable()
    {
        OnPlayerDeath -= Die;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        health = maxHealth;
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        healthText.text = health.ToString();
    }

    public void TakeDamage(float ammount)
    {
        if (health <= 0) return;

        if (health - ammount > 0)
        {
            health -= ammount;
        }
        else
        {
            health = 0;
            OnPlayerDeath?.Invoke();
        }

        hurtSound.PlayOneShot(hurtSound.clip);
        animator.SetTrigger("Damaged");
        UpdateHealthText();
    }

    public void Die()
    {
        // Drugi zvuk kad crkne
        transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
        animator.enabled = false;
    }
}
