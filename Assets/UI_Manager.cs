﻿using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text timerText;           // Drag your TimerText here
    public TMP_Text player1ScoreText;    // Drag your Player1ScoreText here
    public TMP_Text player2ScoreText;    // Drag your Player2ScoreText here

    [Header("Optional Mash Meters")]
    public Image mashMeterP1;            // (optional) can leave empty
    public Image mashMeterP2;            // (optional) can leave empty

    public TMP_Text p1CooldownText;
    public TMP_Text p2CooldownText;

    public void UpdateCooldownDisplay(float p1CD, float p2CD)
    {
        if (p1CooldownText != null) p1CooldownText.text = $"P1 CD: {p1CD:F2}s";
        if (p2CooldownText != null) p2CooldownText.text = $"P2 CD: {p2CD:F2}s";
    }

    //  Update the timer display
    public void UpdateTimerDisplay(float timeRemaining)
    {
        if (timerText != null)
            timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining);
    }

    //  Update both player scores
    public void UpdateScores(int p1, int p2)
    {
        if (player1ScoreText != null)
            player1ScoreText.text = "P1 Score: " + p1;
        if (player2ScoreText != null)
            player2ScoreText.text = "P2 Score: " + p2;
    }

    //  Show who won at the end
    public void DisplayWinner(string message)
    {
        if (timerText != null)
            timerText.text = message;
    }
}
