using System;
using Unity.VisualScripting;
using UnityEngine;

namespace LD
{
    public class PlayerInteractable : MonoBehaviour
    {
        public Camera Camera;
        public Action OnShow;
        public Action OnHide;

        public float Distance = 5f;
        public LayerMask InteractebleLayer;

        bool canUse = false;
        InteractableObject tempInteractableObject;

        private void Start()
        {
            if (Game.UI)
            {
                OnShow += Game.UI.ShowInteractable;
                OnHide += Game.UI.HideInteractable;
            }
        }

        private void OnDestroy()
        {
            if (Game.UI)
            {
                OnShow -= Game.UI.ShowInteractable;
                OnHide -= Game.UI.HideInteractable;
            }
        }

        void Update()
        {
            if (Game.RuntimeData.IsPause)
            {
                canUse = false;
                tempInteractableObject = null;
                OnHide?.Invoke();
                return;
            }

            Ray ray = new Ray(Camera.transform.position, Camera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, Distance, InteractebleLayer))
            {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * Distance, Color.green, 1f);

                canUse = true;
                OnShow?.Invoke();

                if (hit.rigidbody)
                {
                    if (hit.rigidbody.TryGetComponent(out InteractableObject interactable))
                    {
                        tempInteractableObject = interactable;
                    }
                }
                else
                {
                    if (hit.collider.TryGetComponent(out InteractableObject interactable))
                    {
                        tempInteractableObject = interactable;
                    }
                    else
                    {
                        if (hit.collider.transform.parent && 
                            hit.collider.transform.parent
                            .TryGetComponent(out InteractableObject interactableParent))
                        {
                            tempInteractableObject = interactableParent;
                        }
                    }
                }
            }
            else
            {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * Distance, Color.red, 1f);
                canUse = false;
                tempInteractableObject = null;
                OnHide?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
            {
                if (canUse && tempInteractableObject)
                {
                    tempInteractableObject.TryUse();
                }
            }
        }
    }
}
