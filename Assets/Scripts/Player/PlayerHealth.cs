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
    private float health;

    private void OnEnable()
    {
        OnPlayerDeath += Die;
    }

    private void OnDisable()
    {
        OnPlayerDeath -= Die;
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
            Debug.Log("Player health: " + health);
        }
        else
        {
            health = 0;
            OnPlayerDeath?.Invoke();
        }

        UpdateHealthText();
    }

    public void Die()
    {
        Debug.Log("Player is dead!");
    }
}
