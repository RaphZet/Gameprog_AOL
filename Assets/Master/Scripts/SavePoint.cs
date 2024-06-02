using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.respawnPoint.position = gameObject.transform.position;
            gameManager.SaveData();

        }
    }
}
