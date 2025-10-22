using UnityEngine;

public class ButtonPressTracker : MonoBehaviour
{
    private int player1PressCount = 0;
    private int player2PressCount = 0;
    private float timeRemaining = 60.0f;
    private float timerDuration = 60.0f;
    private bool timerIsRunning = false;

    void Start()
    {
        timerIsRunning = true;
        Debug.Log("Multiplayer button press tracker initialized. A 60-second cycle is ready to start when the first button is pressed.");
    }

    void Update()
    {
        if (timerIsRunning)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerIsRunning = false;

                string results = "60 seconds elapsed. Final Scores:\n";
                results += "Player 1: " + player1PressCount + " presses\n";
                results += "Player 2: " + player2PressCount + " presses";
                Debug.Log(results);

                ResetTracker();
            }
        }
    }

    public void OnPlayer1ButtonPressed()
    {
        if (!timerIsRunning)
        {
            StartNewCycle();
        }
        player1PressCount++;
        Debug.Log("Player 1 pressed. P1 count: " + player1PressCount + " | P2 count: " + player2PressCount + " | Time left: " + Mathf.CeilToInt(timeRemaining) + "s");
    }

    public void OnPlayer2ButtonPressed()
    {
        if (!timerIsRunning)
        {
            StartNewCycle();
        }
        player2PressCount++;
        Debug.Log("Player 2 pressed. P1 count: " + player1PressCount + " | P2 count: " + player2PressCount + " | Time left: " + Mathf.CeilToInt(timeRemaining) + "s");
    }

    private void StartNewCycle()
    {
        player1PressCount = 0;
        player2PressCount = 0;
        timeRemaining = timerDuration;
        timerIsRunning = true;
        Debug.Log("New 60-second multiplayer cycle started!");
    }

    private void ResetTracker()
    {
        player1PressCount = 0;
        player2PressCount = 0;
        timeRemaining = timerDuration;
        timerIsRunning = false;
        Debug.Log("Tracker reset. Ready for a new game.");
    }
}