using System.Collections;
using UnityEngine;

public class FlickerEffect : MonoBehaviour
{
    [Header("Light Settings")]
    public Light lightSource; // ������ �� �������� ����� (���� ����)
    public float minLightIntensity = 0.5f;
    public float maxLightIntensity = 1f;
    public float lightFlickerSpeed = 0.1f;

    [Header("Material Emission Settings")]
    public Renderer objectRenderer; // ������ �� �������� ������� (���� ����� �������� �������)
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
        // ��������� ������� ��������� �����
        if (lightSource != null)
        {
            hasLight = true;
        }

        // ��������� ������� ��������� � ���������
        if (objectRenderer != null && objectRenderer.material != null)
        {
            objectMaterial = objectRenderer.material;
            // ��������� ������������ ���� �������
            originalEmissionColor = objectMaterial.GetColor("_EmissionColor");
            hasEmission = true;
        }

        // ��������� �������� ��������
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
        // ��������������� ������������ ���� ������� ��� ���������� �������
        if (objectMaterial != null)
        {
            objectMaterial.SetColor("_EmissionColor", originalEmissionColor);
        }

        // ������������� ��������, ����� �������� ������ ��� ����������� �������
        StopAllCoroutines();
    }
}