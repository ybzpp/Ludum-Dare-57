using UnityEngine;
using UnityEngine.Rendering;

public class RenderSettingsSetter : MonoBehaviour
{
    [Header("Environment Lightingh")]
    public AmbientMode ambientMode;
    public Color ambientColor;
    public bool fog;
    public float fogDensity;
    public float fogStartDistance;
    public float fogEndDistance;
    public Color fogColor;
    public FogMode fogMode;
    public Color cameraBackgroundColor;
    public Camera currentCamera;


    void Start()
    {
        Load();
    }

    [ContextMenu("Bake")]
    void Bake()
    {
        ambientMode = RenderSettings.ambientMode;
        ambientColor = RenderSettings.ambientSkyColor;
        fog = RenderSettings.fog;
        fogDensity = RenderSettings.fogDensity;
        fogStartDistance = RenderSettings.fogStartDistance;
        fogEndDistance = RenderSettings.fogEndDistance;
        fogMode = RenderSettings.fogMode;
        fogColor = RenderSettings.fogColor;

        if (currentCamera)
        {
            cameraBackgroundColor = currentCamera.backgroundColor;
        }
        else
        {
            cameraBackgroundColor = Camera.main.backgroundColor;
        }
    }

    [ContextMenu("Load")]
    void Load()
    {
        RenderSettings.ambientMode = ambientMode;
        RenderSettings.ambientSkyColor = ambientColor;
        RenderSettings.fog = fog;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogMode = fogMode;
        RenderSettings.fogStartDistance = fogStartDistance;
        RenderSettings.fogEndDistance = fogEndDistance;

        if (currentCamera)
        {
            currentCamera.backgroundColor = cameraBackgroundColor;
        }
        else
        {
            Camera.main.backgroundColor = cameraBackgroundColor;
        }
    }
}
