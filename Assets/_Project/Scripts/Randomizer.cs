using UnityEngine;

public class Randomizer : MonoBehaviour
{
    public GameObject[] EnableRandomObject;
    public bool StartRandomize;

    private void Start()
    {
        if (StartRandomize)
            Randomize();
    }

    [ContextMenu("Randomize")]
    public void Randomize()
    {
        var go = transform;
        if (EnableRandomObject.Length > 0)
        {
            var randomIndex = Random.Range(0, EnableRandomObject.Length);
            for (int i = 0; i < EnableRandomObject.Length; i++)
                EnableRandomObject[i].SetActive(i == randomIndex);

            go = EnableRandomObject[randomIndex].transform;
        }
    }
}
