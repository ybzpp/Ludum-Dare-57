using UnityEngine;

public class Ending : MonoBehaviour
{
    public GameObject PreEndView;
    public GameObject EndView;

    public void ShowEnd()
    {
        PreEndView.SetActive(false);
        EndView.SetActive(true);
    }
}
