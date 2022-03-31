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
    [SerializeField]
    private float positionOffset;

    [SerializeField]
    private float minHealthBarHeight = 1.0f;
    [SerializeField]
    private float maxHealthBarHeight = 2.0f;
    [SerializeField]
    private float minHealthBarWidth = 1.0f;
    [SerializeField]
    private float maxHealthBarWidth = 2.0f;

    private EnemyHealth health;

    CameraController cameraController;

    private void HandleHealthChanged(float pct)
    {
        StartCoroutine(ChangeToPct(pct));
    }

    private void Awake()
    {
        cameraController = FindObjectOfType<CameraController>();
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

    internal void SetHealth(EnemyHealth Health)
    {
        foregroundImage.fillAmount = 1.0f;
        health = Health;
        Health.OnHealthPctChanged += HandleHealthChanged;

    }

    private void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(health.transform.position + Vector3.up * positionOffset);
        transform.localScale = new Vector3(mapScaleFactor(minHealthBarWidth, maxHealthBarWidth), mapScaleFactor(minHealthBarHeight, maxHealthBarHeight), 1.0f);
    }

    /// <summary>
    /// Remapping the value to a scale factor for the healthbar.
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <param name="currentValue"></param>
    private float mapScaleFactor(float minValue, float maxValue)
    {
        var CameraRangeMin = cameraController.maxScrollDistance;
        var CameraRangeMax = cameraController.minScrollDistance;
        var CameraCurrentDistance = cameraController.transform.position.y;
        return minValue + (CameraCurrentDistance - CameraRangeMin) * (maxValue - minValue) / (CameraRangeMax - CameraRangeMin);
    }

    private void OnDestroy()
    {
        health.OnHealthPctChanged -= HandleHealthChanged;
    }
}