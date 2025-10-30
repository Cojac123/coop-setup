using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Player Prefabs")]
    public GameObject player1Prefab;
    public GameObject player2Prefab;

    [Header("Spawn Points")]
    public Transform spawnPoint1;
    public Transform spawnPoint2;

    private GameObject player1;
    private GameObject player2;

    void Start()
    {
        SpawnPlayers();
    }

    public void SpawnPlayers()
    {
        // Clean up any existing players
        if (player1 != null) Destroy(player1);
        if (player2 != null) Destroy(player2);

        // Spawn Player 1 at SpawnPoint1
        player1 = Instantiate(player1Prefab, spawnPoint1.position, Quaternion.identity);
        player1.name = "Player1";

        // Spawn Player 2 at SpawnPoint2
        player2 = Instantiate(player2Prefab, spawnPoint2.position, Quaternion.identity);
        player2.name = "Player2";

        Debug.Log(" Players spawned at separate positions!");
    }

    // 🔹 This ONLY repositions Player 2 when a new round starts
    public void ResetPlayer2Position()
    {
        if (player2 != null && spawnPoint2 != null)
        {
            player2.transform.position = spawnPoint2.position;
            player2.transform.rotation = Quaternion.identity;
            Debug.Log(" Player 2 repositioned for new round!");
        }
    }
}
