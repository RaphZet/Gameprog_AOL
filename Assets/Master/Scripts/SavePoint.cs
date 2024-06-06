using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] SpriteRenderer activeSprite, inactiveSprite;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Activate(this);
        }
    }

    private void Activate(SavePoint item)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if(gameManager.respawnPoint != null) 
        { 
            Deactivate(gameManager.respawnStatue);
            if(gameManager.respawnStatue != item)
            {
                FindObjectOfType<AudioManager>().Play("Statue activate");
            }
        }
        gameManager.respawnPoint.position = gameObject.transform.position;
        gameManager.respawnStatue = this;

        activeSprite.enabled = true;
        inactiveSprite.enabled = false;

        gameManager.SaveData();
    }

    public void Deactivate(SavePoint item)
    {
        if(item == null)
        {
            return;
        }
        item.activeSprite.enabled = false;
        item.inactiveSprite.enabled = true;
    }
}
