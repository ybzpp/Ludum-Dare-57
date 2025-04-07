using System;
using System.Collections;
using UnityEngine;

public class ElectricalPanel : MonoBehaviour
{
    public AudioSource AudioSource;

    [Header("Toggle Settings")]
    public Vector3 DisableToggleRotation;
    public Vector3 EnableToggleRotation;
    public Transform ToggleTransform;
    public float AnimationTime = 1f;

    [Header("Fuse")]
    public TagTrigger FuseTrigger;
    public Transform FuseParent;
    public GameObject Fuse;

    [Header("Effects")]
    public ParticleSystem ElectricSparksFx;
    public ParticleSystem LightFx;
    public MeshRenderer EnableLightRenderer;
    public MeshRenderer DisableLightRenderer;
     
    [Header("Events")]
    public Action OnEnablePanel;
    public Action OnDisablePanel;

    public bool IsEnabled => _isEnable;
    private bool _isEnable;
    private bool _isReady = true;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _isEnable = false;
        RotateTo(DisableToggleRotation);

        FuseTrigger.OnColliderTrigger.AddListener(CheckFuse);
        UpdateVisuals(); 
    }

    private void OnDestroy()
    {
        FuseTrigger.OnColliderTrigger.RemoveListener(CheckFuse);
    }

    private void CheckFuse(Collider fuse)
    {
        if (Fuse != null)
            return;

        if (fuse.TryGetComponent(out Fuse f))
        {
            f.RbDisalbe();
            f.transform.SetParent(FuseParent);
            f.transform.localPosition = Vector3.zero;
            f.transform.localEulerAngles = Vector3.zero;
            f.gameObject.layer = 0;
            Fuse = f.gameObject;
        }
    }

    public void Switch()
    {
        UpdateState(!_isEnable);

    }

    public void UpdateState(bool enable)
    {
        if (!_isReady || _isEnable == enable)
            return;

        _isEnable = enable;
        StartCoroutine(AnimateToggle());
    }

    private IEnumerator AnimateToggle()
    {
        AudioHelper.PlaySound("SwitchToggle", AudioSource);

        _isReady = false;

        Vector3 startRotation = _isEnable ? DisableToggleRotation : EnableToggleRotation;
        Vector3 endRotation = _isEnable ? EnableToggleRotation : DisableToggleRotation;

        float elapsed = 0f;
        while (elapsed < AnimationTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / AnimationTime);
            RotateTo(Vector3.Lerp(startRotation, endRotation, t));
            yield return null;
        }
        RotateTo(endRotation);

        if (_isEnable)
        {
            if (!Fuse)
            {
                AudioHelper.PlaySound("SwitchToggleFail", AudioSource);

                while (elapsed > 0)
                {
                    elapsed -= Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / AnimationTime);
                    RotateTo(Vector3.Lerp(startRotation, endRotation, t));
                    yield return null;
                }
                RotateTo(startRotation);
                _isEnable = false;
            }
        }
            
        UpdateVisuals();

        if (_isEnable)
        {
            AudioHelper.PlaySound("SwitchToggleSuccess", AudioSource);
            OnEnablePanel?.Invoke();
        }
        else
            OnDisablePanel?.Invoke();

        _isReady = true;
    }

    private void RotateTo(Vector3 rotation)
    {
        if (ToggleTransform != null)
            ToggleTransform.localEulerAngles = rotation;
    }

    private void UpdateVisuals()
    {
        if (_isEnable)
            ElectricSparksFx?.Play();

        var color = _isEnable ? Color.green : Color.red;

        EnableLightRenderer.enabled = _isEnable;
        DisableLightRenderer.enabled = !_isEnable;

        if (LightFx != null)
        {
            var mainModule = LightFx.main;
            mainModule.startColor = color;
        }
    }
}
    