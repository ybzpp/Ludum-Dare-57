using UnityEngine;

public class RuntimeData : MonoBehaviour
{
    public GameState GameState;

    private void Awake()
    {
        Game.RuntimeData = this;
    }
}
