using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float jumpForce = 10;
    [SerializeField] int playerIndex = 1;

    [SerializeField]
    Material[] materials;

    Vector2 input;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerIndex = FindObjectsByType(typeof(PlayerLogic), FindObjectsSortMode.InstanceID).Length;

        GetComponent<MeshRenderer>().material = materials[playerIndex - 1];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        input = ctx.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(input.x, 0, input.y) * moveSpeed, ForceMode.Acceleration);
    }
}
