using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : ScreenUI
{
    public Button StartButton;
    public Button SettingsButton;
    public AudioSource AudioSource;

    private void Awake()
    {
        if (!AudioSource)
            AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartButton.onClick.AddListener(StartGame);
    }

    private void OnDestroy()
    {
        StartButton.onClick.RemoveListener(StartGame);
    }

    public void StartGame()
    {
        Game.StartGame();
        AudioHelper.PlaySound("Play", AudioSource);
    }
} 
