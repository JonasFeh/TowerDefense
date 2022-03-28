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

    public static event Action<EnemyHealth> OnHealthAdded = delegate { };
    public static event Action<EnemyHealth> OnHealthRemoved = delegate { };

    public int CurrentHealth { get; private set; }

    public event Action<float> OnHealthPctChanged = delegate { };
    Enemy enemy;


    void OnEnable()
    {
        CurrentHealth = maxHealth;
        OnHealthAdded?.Invoke(this);
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
        if (CurrentHealth <= 0)
        {
            gameObject.SetActive(false);
            enemy.RewardGold();
            maxHealth += difficultyRamp;
        }
    }

    public void ModifyHealth(int Amount)
    {
        CurrentHealth += Amount;

        var currentHealthPercentage = (float)CurrentHealth / (float)maxHealth;
        OnHealthPctChanged?.Invoke(currentHealthPercentage);
    }

    private void OnDisable()
    {
        OnHealthRemoved?.Invoke(this);
    }
}
