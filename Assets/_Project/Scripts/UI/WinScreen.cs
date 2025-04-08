using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : ScreenUI
{
    public TMP_Text TimeText;
    public Button RestartButton;
    public Button MenuButton;

    private void OnEnable()
    {
        int totalSeconds = Mathf.FloorToInt(Game.RuntimeData.GameTime);

        // Вычисляем минуты и секунды
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        TimeText.text = $"Time: {string.Format("{0}:{1:00}", minutes, seconds)}";
        RestartButton.onClick.AddListener(Game.RestartGame);
    }

    private void OnDisable()
    {
        RestartButton.onClick.RemoveListener(Game.RestartGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Game.RestartGame();
        }
    }
}
