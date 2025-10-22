using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text timerText;           // drag TimerText here
    public TMP_Text player1ScoreText;    // drag Player1ScoreText here
    public TMP_Text player2ScoreText;    // drag Player2ScoreText here
    public Image mashMeterP1;
    public Image mashMeterP2;

    // --- Update the timer on screen ---
    public void UpdateTimerDisplay(float timeRemaining)
    {
        timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining);
    }

    // --- Update both player scores ---
    public void UpdateScores(int p1, int p2)
    {
        player1ScoreText.text = "P1 Score: " + p1;
        player2ScoreText.text = "P2 Score: " + p2;

        if (mashMeterP1 != null)
            mashMeterP1.fillAmount = Mathf.Clamp01(p1 / 100f);
        if (mashMeterP2 != null)
            mashMeterP2.fillAmount = Mathf.Clamp01(p2 / 100f);
    }

    // --- Display final winner message ---
    public void DisplayWinner(string message)
    {
        timerText.text = message;
    }
}
