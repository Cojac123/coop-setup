using UnityEngine;
using TMPro;

public class ButtonMashManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text timerText;
    public TMP_Text scoreText;

    [Header("Game Settings")]
    public float gameDuration = 60f;   // total seconds

    private float remainingTime;       // countdown timer
    private int score;                 // player score
    private bool gameRunning = false;  // track if timer is active

    void Start()
    {
        remainingTime = gameDuration;
        UpdateUI();  // show starting text
        Debug.Log("Press SPACE to start the mash test!");
    }

    void Update()
    {
        // Start the timer when player presses Space
        if (!gameRunning && Input.GetKeyDown(KeyCode.Space))
        {
            gameRunning = true;
            remainingTime = gameDuration;
            score = 0;
            Debug.Log("Timer started!");
        }

        // When timer is running, count down
        if (gameRunning)
        {
            remainingTime -= Time.deltaTime;

            // Mash the "A" key to gain score
            if (Input.GetKeyDown(KeyCode.A))
            {
                score++;
                Debug.Log("Mash! Current score = " + score);
            }

            // If timer runs out
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                gameRunning = false;
                Debug.Log("Time up! Final Score = " + score);
            }
        }

        // Always update UI
        UpdateUI();
    }

    // Update the on-screen text
    void UpdateUI()
    {
        timerText.text = "Time: " + Mathf.CeilToInt(remainingTime);
        scoreText.text = "Score: " + score;
    }
}
