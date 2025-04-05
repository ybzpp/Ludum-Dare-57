using System.Collections;
using UnityEngine;

public class FlickerEffect : MonoBehaviour
{
    [Header("Light Settings")]
    public Light lightSource; // Ссылка на источник света (если есть)
    public float minLightIntensity = 0.5f;
    public float maxLightIntensity = 1f;
    public float lightFlickerSpeed = 0.1f;

    [Header("Material Emission Settings")]
    public Renderer objectRenderer; // Ссылка на рендерер объекта (если нужно мерцание эмиссии)
    public float minEmissionIntensity = 0.5f;
    public float maxEmissionIntensity = 1f;
    public float emissionFlickerSpeed = 0.1f;
    public Color emissionColor = Color.white;

    private Material objectMaterial;
    private Color originalEmissionColor;
    private bool hasLight = false;
    private bool hasEmission = false;

    void Start()
    {
        // Проверяем наличие источника света
        if (lightSource != null)
        {
            hasLight = true;
        }

        // Проверяем наличие рендерера и материала
        if (objectRenderer != null && objectRenderer.material != null)
        {
            objectMaterial = objectRenderer.material;
            // Сохраняем оригинальный цвет эмиссии
            originalEmissionColor = objectMaterial.GetColor("_EmissionColor");
            hasEmission = true;
        }

        // Запускаем корутины мерцания
        if (hasLight) StartCoroutine(FlickerLight());
        if (hasEmission) StartCoroutine(FlickerEmission());
    }

    private IEnumerator FlickerLight()
    {
        while (true)
        {
            float randomIntensity = Random.Range(minLightIntensity, maxLightIntensity);
            lightSource.intensity = randomIntensity;
            yield return new WaitForSeconds(lightFlickerSpeed);
        }
    }

    private IEnumerator FlickerEmission()
    {
        while (true)
        {
            float randomIntensity = Random.Range(minEmissionIntensity, maxEmissionIntensity);
            Color newEmissionColor = emissionColor * randomIntensity;
            objectMaterial.SetColor("_EmissionColor", newEmissionColor);
            yield return new WaitForSeconds(emissionFlickerSpeed);
        }
    }

    private void OnDisable()
    {
        // Восстанавливаем оригинальный цвет эмиссии при отключении скрипта
        if (objectMaterial != null)
        {
            objectMaterial.SetColor("_EmissionColor", originalEmissionColor);
        }

        // Останавливаем корутины, чтобы избежать ошибок при уничтожении объекта
        StopAllCoroutines();
    }
}