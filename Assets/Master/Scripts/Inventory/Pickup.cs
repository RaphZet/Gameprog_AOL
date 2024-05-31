using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemPickup;

    private void Start()
    {
        GameObject inventoryManagerObject = GameObject.FindGameObjectWithTag("InventoryManager");
        if (inventoryManagerObject != null)
        {
            inventory = inventoryManagerObject.GetComponent<Inventory>();
        }
        else
        {
            Debug.LogWarning("InventoryManager object not found in the scene.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Masuk gan");
            for(int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.isFull[i] == false)
                {
                    inventory.isFull[i] = true;
                    Instantiate(itemPickup, inventory.slots[i].transform, false);
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
