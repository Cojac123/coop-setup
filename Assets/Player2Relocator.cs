using UnityEngine;

public class Player2Relocator : MonoBehaviour
{
    [Header("Player 2 Settings")]
    public GameObject player2Object;     // Drag Player 2 here
    public Vector3 newPosition = new Vector3(4, 1, 0);  // adjust as needed

    void Start()
    {
        if (player2Object != null)
        {
            player2Object.transform.position = newPosition;
            Debug.Log($" Player 2 moved to {newPosition}");
        }
        else
        {
            Debug.LogWarning(" No Player 2 object assigned in Player2Relocator!");
        }
    }
}
