using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ButtonMashManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text timerText;
    public TMP_Text player1ScoreText;
    public TMP_Text player2ScoreText;

    [Header("Game Settings")]
    public float gameDuration = 60f;

    private float remainingTime;
    private bool gameRunning = false;

    private int p1Score = 0;
    private int p2Score = 0;

    public UIManager uiManager;

    private float p1Cooldown = 0f;
    private float p2Cooldown = 0f;

    public float baseCooldown = 0.5f; // seconds
    public float maxCooldown = 1.5f;
    public float minCooldown = 0.1f;

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
        inputActions.Player.ButtonMash.performed += OnButtonMash;  // listen for input
    }

    void OnDisable()
    {
        inputActions.Player.ButtonMash.performed -= OnButtonMash;
        inputActions.Disable();
    }

    void Start()
    {
        StartGame();
        remainingTime = gameDuration;
        UpdateUI();
    }

    void Update()
    {
        if (!gameRunning) return;

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
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                gameRunning = false;
                Debug.Log("Time’s up!");
            }
        }

        if (uiManager != null)
        {
            uiManager.UpdateCooldownDisplay(p1Cooldown, p2Cooldown);
        }

        UpdateUI();
    }

    public void StartGame()
    {
        gameRunning = true;
        remainingTime = gameDuration;
        p1Score = 0;
        p2Score = 0;
        Debug.Log("Game started!");
    }

    private void OnButtonMash(InputAction.CallbackContext ctx)
    {
        if (!gameRunning) return;

        // Get which key was pressed
        var control = ctx.control.displayName;
        float currentTime = Time.time;

        if (control == "Z" && currentTime - p1LastMashTime >= p1Cooldown)
        {
            p1Score++;
            p1LastMashTime = currentTime;
        }
        else if (control == "Left Button" && currentTime - p2LastMashTime >= p2Cooldown)
        {
            p2Score++;
            p2LastMashTime = currentTime;
        }

        Debug.Log($"Button pressed: {control} | P1: {p1Score} | P2: {p2Score}");
    }

    private void UpdateUI()
    {
        timerText.text = "Time: " + Mathf.CeilToInt(remainingTime);
        player1ScoreText.text = "P1 Score: " + p1Score;
        player2ScoreText.text = "P2 Score: " + p2Score;
    }
}
