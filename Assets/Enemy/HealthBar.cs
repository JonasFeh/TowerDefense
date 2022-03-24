using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image foregroundImage;

    [SerializeField]
    private float updateSpeedSeconds = 0.5f;

    private void Awake()
    {
        var EnemyHealth = GetComponentInParent<EnemyHealth>();
        EnemyHealth.OnHealthPctChanged += HandleHealthChanged;
        EnemyHealth.ResetHealthPctOnSpawn += ResetHealthBar;
    }

    private void HandleHealthChanged(float pct)
    {
        StartCoroutine(ChangeToPct(pct));
    }

    private void ResetHealthBar()
    {
        foregroundImage.fillAmount = 1.0f;
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = foregroundImage.fillAmount;
        float elapsed = 0f;
        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }

        foregroundImage.fillAmount = pct;

    }
}