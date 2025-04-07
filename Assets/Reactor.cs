using UnityEngine;
using UnityEngine.Events;

public class Reactor : MonoBehaviour
{
    public UnityEvent OnStartGame;
    public UnityEvent OnEndGame;

    private void Awake()
    {
        Game.Reactor = this;
    }
}
