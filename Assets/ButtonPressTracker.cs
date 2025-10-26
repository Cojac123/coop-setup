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
        remainingTime = gameDuration;
        UpdateUI();
    }

    void Update()
    {
        if (gameRunning)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                gameRunning = false;
                Debug.Log("Time’s up!");
            }
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
        if (control == "Z") p1Score++;
        else if (control == "M") p2Score++;

        Debug.Log($"Button pressed: {control} | P1: {p1Score} | P2: {p2Score}");
    }

    private void UpdateUI()
    {
        timerText.text = "Time: " + Mathf.CeilToInt(remainingTime);
        player1ScoreText.text = "P1 Score: " + p1Score;
        player2ScoreText.text = "P2 Score: " + p2Score;
    }
}
