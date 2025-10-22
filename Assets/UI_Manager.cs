using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text timerText;
    public TMP_Text player1ScoreText;
    public TMP_Text player2ScoreText;
    public Image mashMeterP1;
    public Image mashMeterP2;

    [Header("Gameplay Settings")]
    public float gameDuration = 60f;
    private float remainingTime;
    private int player1Score;
    private int player2Score;

    void Start()
    {
        remainingTime = gameDuration;
        UpdateUI();
    }

    void Update()
    {
        // Timer countdown
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                EndGame();
            }
        }

        UpdateUI();
    }

    public void AddScore(int playerIndex)
    {
        if (playerIndex == 1) player1Score++;
        else player2Score++;
    }

    void UpdateUI()
    {
        timerText.text = $"Time: {remainingTime:F0}";
        player1ScoreText.text = $"P1 Score: {player1Score}";
        player2ScoreText.text = $"P2 Score: {player2Score}";

        // Optional mash meter logic (if using)
        if (mashMeterP1) mashMeterP1.fillAmount = Mathf.Clamp01(player1Score / 100f);
        if (mashMeterP2) mashMeterP2.fillAmount = Mathf.Clamp01(player2Score / 100f);
    }

    void EndGame()
    {
        string winner = player1Score > player2Score ? "Player 1 Wins!" :
                        player2Score > player1Score ? "Player 2 Wins!" : "Draw!";
        timerText.text = winner;
    }
}
