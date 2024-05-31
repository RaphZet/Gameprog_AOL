using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject item;
    private Transform player;
    private PlayerController playerController;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
    }

    public void SpawnDroppedItem()
    {
        // Jarak dari player ke posisi item
        float dropOffset = 3f;
        // Tentukan arah hadapan pemain
        int direction = playerController.GetFacingDir();

        // Tentukan posisi item berdasarkan arah hadapan pemain
        Vector2 playerPos = new Vector2(player.position.x + (dropOffset * direction), player.position.y);

        // Instantiate item
        Instantiate(item, playerPos, Quaternion.identity);
    }
}
