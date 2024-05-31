using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private Inventory inventory;
    public int i;

    private void Start()
    {
        // Menggunakan Inventory.instance untuk mendapatkan referensi inventori
        inventory = Inventory.instance;

        // Pastikan inventory tidak null
        if (inventory == null)
        {
            Debug.LogError("Inventory instance is null. Make sure InventoryManager is in the scene.");
        }
    }

    private void Update()
    {
        if (transform.childCount <= 0)
        {
            inventory.isFull[i] = false;
        }
    }

    public void DropItem()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Spawn>().SpawnDroppedItem();
            GameObject.Destroy(child.gameObject);
        }
    }
}
