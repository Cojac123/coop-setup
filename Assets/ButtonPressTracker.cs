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
    public TMP_Text winnerText; // Winner announcement text
    public Image mashMeterP1;
    public Image mashMeterP2;
    public Image startOverlay;
    public UIManager uiManager; // Optional cooldown UI display

    [Header("Spawner Reference")]
    public PlayerSpawner playerSpawner; // 🆕 reference to PlayerSpawner

    [Header("Game Settings")]
    public float gameDuration = 60f;
    public float fadeSpeed = 1.5f;
    public float resultDelay = 3f;

    [Header("Cooldown Settings")]
    public float baseCooldown = 0.5f; // seconds
    public float maxCooldown = 1.5f;
    public float minCooldown = 0.1f;

    private float remainingTime;
    private bool gameRunning = false;
    private bool fadingOut = false;
    private bool gameEnding = false;

    private int p1Score = 0;
    private int p2Score = 0;

    private float p1Cooldown = 0f;
    private float p2Cooldown = 0f;
    private float p1LastMashTime = -999f;
    private float p2LastMashTime = -999f;

    private IA_Player inputActions;

    void Awake()
    {
        inputActions = new IA_Player();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.ButtonMash.performed += OnButtonMash; // listen for inputs
    }

    void OnDisable()
    {
        inputActions.Player.ButtonMash.performed -= OnButtonMash;
        inputActions.Disable();
    }

    void Start()
    {
        remainingTime = gameDuration;
        UpdateUI();

        // Start with black overlay
        if (startOverlay != null)
            startOverlay.color = new Color(0, 0, 0, 1);

        SetUIActive(false);

        if (winnerText != null)
            winnerText.gameObject.SetActive(false);

        // initialize cooldowns
        p1Cooldown = baseCooldown;
        p2Cooldown = baseCooldown;
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

        // 🔹 Main game timer & cooldown balancing
        if (gameRunning)
        {
            remainingTime -= Time.deltaTime;

            int scoreDiff = Mathf.Abs(p1Score - p2Score);
            if (p1Score > p2Score)
            {
                p1Cooldown = Mathf.Clamp(baseCooldown + scoreDiff * 0.1f, baseCooldown, maxCooldown);
                p2Cooldown = Mathf.Clamp(baseCooldown - scoreDiff * 0.1f, minCooldown, baseCooldown);
            }
            else if (p2Score > p1Score)
            {
                p2Cooldown = Mathf.Clamp(baseCooldown + scoreDiff * 0.1f, baseCooldown, maxCooldown);
                p1Cooldown = Mathf.Clamp(baseCooldown - scoreDiff * 0.1f, minCooldown, baseCooldown);
            }
            else
            {
                p1Cooldown = baseCooldown;
                p2Cooldown = baseCooldown;
            }

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                gameRunning = false;
                StartCoroutine(EndGame());
            }
        }

        // update visuals
        UpdateMashMeters();
        if (uiManager != null)
            uiManager.UpdateCooldownDisplay(p1Cooldown, p2Cooldown);

        UpdateUI();
    }

    public void StartGame()
    {
        gameRunning = true;
        remainingTime = gameDuration;
        p1Score = 0;
        p2Score = 0;

        // 🆕 move Player 2 back to spawn
        if (playerSpawner != null)
            playerSpawner.ResetPlayer2Position();

        if (winnerText != null)
            winnerText.gameObject.SetActive(false);

        Debug.Log("Game started!");
    }

    private void OnButtonMash(InputAction.CallbackContext ctx)
    {
        if (!gameRunning) return;

        var control = ctx.control.displayName;
        float currentTime = Time.time;

        // 🟦 Player 1 (Z)
        if (control == "Z" && currentTime - p1LastMashTime >= p1Cooldown)
        {
            p1Score++;
            p1LastMashTime = currentTime;
        }

        // 🟥 Player 2 (M or Left Mouse)
        else if ((control == "M" || control == "Left Button" || control == "Mouse Left")
                 && currentTime - p2LastMashTime >= p2Cooldown)
        {
            p2Score++;
            p2LastMashTime = currentTime;
        }

        UpdateMashMeters();
        Debug.Log($"Button pressed: {control} | P1: {p1Score} | P2: {p2Score}");
    }

    private IEnumerator EndGame()
    {
        gameEnding = true;
        SetUIActive(false);

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

        // Fade to black
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

        // Wait for space to restart
        bool waiting = true;
        while (waiting)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                waiting = false;
            yield return null;
        }

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
