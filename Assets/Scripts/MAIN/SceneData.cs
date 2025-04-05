using UnityEngine;

public class SceneData : MonoBehaviour
{
    private void Awake()
    {
        Game.SceneData = this;
    }
}
