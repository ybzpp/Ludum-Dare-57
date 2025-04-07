using UnityEngine;

public class RuntimeData : MonoBehaviour
{
    public GameState GameState;
    public bool IsPause;
    public float StartTime;
    public float EndTime;

    public float GameTime { get; internal set; }

    private void Awake()
    {
        Game.RuntimeData = this;
        IsPause = false;
    }
}
