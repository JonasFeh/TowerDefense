using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 5;

    [Tooltip("Adds amount to maxHealth when enemy dies.")]
    [SerializeField] int difficultyRamp = 1;

    public event Action<float> OnHealthPctChanged = delegate { };
    public event Action ResetHealthPctOnSpawn = delegate { };

    int currentHealth = 0;
    Enemy enemy;


    void OnEnable()
    {
        currentHealth = maxHealth;
        ResetHealthPctOnSpawn.Invoke();
    }

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    void ProcessHit()
    {
        ModifyHealth(-1);
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            maxHealth += difficultyRamp;
            enemy.RewardGold();
        }
    }

    public void ModifyHealth(int Amount)
    {
        currentHealth += Amount;

        var currentHealthPercentage = (float)currentHealth / (float)maxHealth;
        OnHealthPctChanged.Invoke(currentHealthPercentage);
    }
}
