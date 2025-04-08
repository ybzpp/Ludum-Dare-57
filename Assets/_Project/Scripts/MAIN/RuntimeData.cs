using UnityEngine;

public class RuntimeData : MonoBehaviour
{
    public GameState GameState;
    public bool IsPause;
    public bool IsEnd;
    public float StartTime;
    public float EndTime;

    public float GameTime { get; internal set; }

    private void Awake()
    {
        Game.RuntimeData = this;
        IsPause = false;
        IsEnd = false;
    }
}
