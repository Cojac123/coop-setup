using UnityEngine;

public class Director_Manager : MonoBehaviour
{
    private int p1Score;
    private int p2Score;

    public float cooldownDuration = 5f;
    private float currentCooldown = 0f;
    public int leadingPlayerIndex = -1;

    public void UpdateDirectorState(int p1Score, int p2Socre)
    {
        leadingPlayerIndex = p1Score > p2Score ? 1 : (p2Score > p1Score ? 2 : -1);

        //Adjusts cooldown based on who's winning
        if (leadingPlayerIndex == 1)
            currentCooldown = Mathf.Min(currentCooldown + Time.deltaTime, cooldownDuration);
        else if (leadingPlayerIndex == 2)
            currentCooldown = Mathf.Max(currentCooldown - Time.deltaTime, 0);
    }

    public bool IsDirectorReady()
    {
        return currentCooldown <= 0f;
    }

    public float GetCooldownProgress()
    {
        return currentCooldown / cooldownDuration;
    }
}
