using UnityEngine.Events;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public AudioSource AudioSource;
    public bool AudioSourceTakeOutFromParent = false;

    public UnityEvent OnStart;
    public UnityEvent OnUse;
    public bool Disposable = false;

    bool isUses = false;

    [Header("Outline Settings")]
    public float outlineWidth = 0.05f;

    private Material[] originalMaterials;
    private Material[] outlinedMaterials;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        isSelect = false;
        isUses = false;
        OnStart?.Invoke();
        CustomAwake();
    }

    //private void Start()
    //{
    //    // Инициализация обводки
    //    meshRenderer = GetComponent<MeshRenderer>();
    //    if (meshRenderer != null)
    //    {
    //        originalMaterials = meshRenderer.materials;
    //        outlinedMaterials = new Material[originalMaterials.Length + 1];
    //        for (int i = 0; i < originalMaterials.Length; i++)
    //        {
    //            outlinedMaterials[i] = originalMaterials[i];
    //        }

    //        // Создаем копию материала обводки для этого объекта
    //        Material outlineMatInstance = new Material(Game.SceneData.OutlineMaterial);
    //        outlineMatInstance.SetFloat("_OutlineWidth", outlineWidth);
    //        outlinedMaterials[outlinedMaterials.Length - 1] = outlineMatInstance;
    //    }
    //}

    public virtual void CustomAwake() { }

    public void TryUse()
    {
        if (Disposable && isUses) return;
        Use();
    }

    public virtual void Use()
    {
        if (AudioSource)
        {
            AudioSource.PlayOneShot(AudioSource.clip);
            if (AudioSourceTakeOutFromParent)
            {
                AudioSource.transform.parent = null;
            }
        }

        OnUse?.Invoke();
        if (Disposable)
        {
            isUses = true;
        }
    }

    private bool isSelect;
    public void Select()
    {
        //if (isSelect)
        //    return;

        //isSelect = true;

        //if (meshRenderer != null)
        //{
        //    meshRenderer.materials = outlinedMaterials;
        //}
    }

    public void Unselect()
    {
        //isSelect = false;

        //if (meshRenderer != null)
        //{
        //    meshRenderer.materials = originalMaterials;
        //}
    }

    // Очистка при уничтожении объекта
    private void OnDestroy()
    {
        if (outlinedMaterials != null)
        {
            foreach (var mat in outlinedMaterials)
            {
                if (mat != null && mat != Game.SceneData.OutlineMaterial)
                {
                    Destroy(mat);
                }
            }
        }
    }
}


