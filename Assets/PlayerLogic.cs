using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] int playerIndex = 1;

    [SerializeField] Material[] materials;

    private Vector2 input;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Assign player index automatically (1 = first player, 2 = second, etc.)
        playerIndex = FindObjectsByType(typeof(PlayerLogic), FindObjectsSortMode.InstanceID).Length;

        // Assign material color based on player
        GetComponent<MeshRenderer>().material = materials[playerIndex - 1];
    }

    void Update()
    {
        // You can add animation or particle updates here later
    }

    // 🔹 Handles jumping input
    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // 🔹 Handles movement input
    public void Move(InputAction.CallbackContext ctx)
    {
        input = ctx.ReadValue<Vector2>();
    }

    // 🔹 Apply movement physics
    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(input.x, 0, input.y) * moveSpeed, ForceMode.Acceleration);
    }
}
