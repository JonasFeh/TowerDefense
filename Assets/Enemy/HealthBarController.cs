using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    [SerializeField]
    private HealthBar healthBarPrefab;

    private Dictionary<EnemyHealth, HealthBar> healthBars = new Dictionary<EnemyHealth, HealthBar>();

    private void Awake()
    {
        EnemyHealth.OnHealthAdded += AddHealthBar;
        EnemyHealth.OnHealthRemoved += RemoveHealthBar;
    }

    private void AddHealthBar(EnemyHealth Health)
    {
        if(!healthBars.ContainsKey(Health))
        {
            var HealthBar = Instantiate(healthBarPrefab, transform);
            healthBars.Add(Health, HealthBar);
            HealthBar.SetHealth(Health);
        }
    }

    private void RemoveHealthBar(EnemyHealth Health)
    {
        if (healthBars.ContainsKey(Health))
        {
            Destroy(healthBars[Health].gameObject);
            healthBars.Remove(Health);
        }
    }
}
