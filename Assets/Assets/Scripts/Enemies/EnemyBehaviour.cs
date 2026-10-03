using System;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            if (currentHealth <= 0) return;

            currentHealth -= 50;

            if (currentHealth <= 0)
            {
                Die();
            }

        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
