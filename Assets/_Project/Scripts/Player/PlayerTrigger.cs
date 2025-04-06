using UnityEngine;
using UnityEngine.Events;

namespace LD
{
    public class PlayerTrigger : MonoBehaviour 
    {
        public UnityEvent OnTrigger;
        public Collider Collider;
        public bool Disposable = true;

        private void Awake()
        {
            if (!Collider) Collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Trigger();
            }
        }

        public void Trigger()
        {
            OnTrigger?.Invoke();
            if (Disposable) Collider.enabled = false;
        }
    } 
}
