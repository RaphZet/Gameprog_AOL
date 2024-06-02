using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int score;
    public float currentHealth;
    public float[] playerPosition;
    public float[] respawnPosition;

    public float timeElapsed;

    public SaveData(GameManager gameManager, PlayerStats playerStats, Timer timer)
    {
        score = gameManager.score;
        currentHealth = playerStats.currentHealth;

        playerPosition = new float[3];
        playerPosition[0] = playerStats.gameObject.transform.position.x;
        playerPosition[1] = playerStats.gameObject.transform.position.y;
        playerPosition[2] = playerStats.gameObject.transform.position.z;

        timeElapsed = timer.timeRemaining;
    }
}
