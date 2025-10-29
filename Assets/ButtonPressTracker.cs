using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class ButtonMashManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text timerText;
    public TMP_Text player1ScoreText;
    public TMP_Text player2ScoreText;
    public TMP_Text winnerText;       // 🆕 Create a TMP Text for “Winner” and drag it here
    public Image mashMeterP1;
    public Image mashMeterP2;
    public Image startOverlay;

    [Header("Game Settings")]
    public float gameDuration = 60f;
    public float fadeSpeed = 1.5f;
    public float resultDelay = 3f;     // 🆕 Time before resetting after showing winner

    private float remainingTime;
    private bool gameRunning = false;
    private bool fadingOut = false;
    private bool gameEnding = false;   // 🆕

    private int p1Score = 0;
    private int p2Score = 0;

    private IA_Player inputActions;

    void Awake()
    {
        inputActions = new IA_Player();
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        remainingTime = gameDuration;
        UpdateUI();

        if (startOverlay != null)
            startOverlay.color = new Color(0, 0, 0, 1);

        SetUIActive(false);

        if (winnerText != null)
            winnerText.gameObject.SetActive(false);
    }

    void Update()
    {
        // 🔹 Start Game on Space
        if (!gameRunning && !gameEnding && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            fadingOut = true;
            StartGame();
        }

        // 🔹 Fade-out intro
        if (fadingOut && startOverlay != null)
        {
            Color c = startOverlay.color;
            c.a -= Time.deltaTime * fadeSpeed;
            startOverlay.color = c;
            if (c.a <= 0)
            {
                c.a = 0;
                startOverlay.color = c;
                fadingOut = false;
                startOverlay.gameObject.SetActive(false);
                SetUIActive(true);
            }
        }

        // 🔹 Main timer & input
        if (gameRunning)
        {
            remainingTime -= Time.deltaTime;

            // Player 1 input
            if (Keyboard.current.zKey.wasPressedThisFrame)
                p1Score++;

            // Player 2 input (M key or Left Mouse)
            if (Keyboard.current.mKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
                p2Score++;

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                gameRunning = false;
                StartCoroutine(EndGame());
            }
        }

        UpdateMashMeters();
        UpdateUI();
    }

    public void StartGame()
    {
        gameRunning = true;
        remainingTime = gameDuration;
        p1Score = 0;
        p2Score = 0;

        if (winnerText != null)
            winnerText.gameObject.SetActive(false);

        Debug.Log("Game started!");
    }

    private IEnumerator EndGame()
    {
        gameEnding = true;
        SetUIActive(false);

        // 🏆 Determine winner
        string result;
        if (p1Score > p2Score)
            result = "🏆 Player 1 Wins!";
        else if (p2Score > p1Score)
            result = "🏆 Player 2 Wins!";
        else
            result = "🤝 It's a Draw!";

        if (winnerText != null)
        {
            winnerText.text = result + "\n\nPress SPACE to Restart";
            winnerText.gameObject.SetActive(true);
        }

        // 🕶️ Fade to black
        if (startOverlay != null)
        {
            startOverlay.gameObject.SetActive(true);
            Color c = startOverlay.color;
            c.a = 0;
            startOverlay.color = c;

            while (c.a < 1)
            {
                c.a += Time.deltaTime * fadeSpeed;
                startOverlay.color = c;
                yield return null;
            }
        }

        // 🕹️ Wait for space press to restart
        bool waiting = true;
        while (waiting)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                waiting = false;
            yield return null;
        }

        // Reset everything
        if (winnerText != null)
            winnerText.gameObject.SetActive(false);

        remainingTime = gameDuration;
        gameEnding = false;
        fadingOut = true;
        StartGame();
    }


    private void UpdateUI()
    {
        if (timerText != null)
            timerText.text = "Time: " + Mathf.CeilToInt(remainingTime);
        if (player1ScoreText != null)
            player1ScoreText.text = "P1 Score: " + p1Score;
        if (player2ScoreText != null)
            player2ScoreText.text = "P2 Score: " + p2Score;
    }

    private void UpdateMashMeters()
    {
        if (mashMeterP1 != null)
            mashMeterP1.fillAmount = Mathf.Clamp01(p1Score / 100f);
        if (mashMeterP2 != null)
            mashMeterP2.fillAmount = Mathf.Clamp01(p2Score / 100f);
    }

    private void SetUIActive(bool active)
    {
        if (timerText != null) timerText.gameObject.SetActive(active);
        if (player1ScoreText != null) player1ScoreText.gameObject.SetActive(active);
        if (player2ScoreText != null) player2ScoreText.gameObject.SetActive(active);
        if (mashMeterP1 != null) mashMeterP1.gameObject.SetActive(active);
        if (mashMeterP2 != null) mashMeterP2.gameObject.SetActive(active);
    }
}
