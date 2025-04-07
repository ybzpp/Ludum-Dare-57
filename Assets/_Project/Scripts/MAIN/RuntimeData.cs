using UnityEngine;

public class RuntimeData : MonoBehaviour
{
    public GameState GameState;
    public bool IsPause;

    private void Awake()
    {
        Game.RuntimeData = this;
        IsPause = false;
    }
}
