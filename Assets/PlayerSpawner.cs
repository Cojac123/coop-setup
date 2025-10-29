using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject player1Prefab;
    public GameObject player2Prefab;

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
        if (player1 != null) Destroy(player1);
        if (player2 != null) Destroy(player2);

        player1 = Instantiate(player1Prefab, spawnPoint1.position, Quaternion.identity);
        player1.name = "Player1";

        player2 = Instantiate(player2Prefab, spawnPoint2.position, Quaternion.identity);
        player2.name = "Player2";
    }
}
