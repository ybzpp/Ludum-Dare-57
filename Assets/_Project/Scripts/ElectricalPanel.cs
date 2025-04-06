using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ElectricalPanel : MonoBehaviour
{
    [Header("Toggle Settings")]
    public Vector3 DisableToggleRotation;
    public Vector3 EnableToggleRotation;
    public Transform ToggleTransform;
    public float AnimationTime = 1f;

    [Header("Fuse")]
    public Transform FuseParent;
    public GameObject Fuse;

    [Header("Effects")]
    public ParticleSystem ElectricSparksFx;
    public ParticleSystem LightFx;
    public MeshRenderer LightRenderer;
     
    [Header("Events")]
    public Action OnEnablePanel;
    public Action OnDisablePanel;
    public TagTrigger FuseTrigger;

    private bool _isEnable;
    private bool _isReady = true;

    public bool IsEnabled => _isEnable;

    private void Start()
    {
        _isEnable = false;
        RotateTo(DisableToggleRotation);

        FuseTrigger.OnColliderTrigger.AddListener(CheckFuse);
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
            OnEnablePanel?.Invoke();
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

        if (LightRenderer != null)
            LightRenderer.material.color = _isEnable ? Color.green : Color.red;

        if (LightFx != null)
        {
            var mainModule = LightFx.main;
            mainModule.startColor = _isEnable ? Color.green : Color.red;
        }
    }
}
    