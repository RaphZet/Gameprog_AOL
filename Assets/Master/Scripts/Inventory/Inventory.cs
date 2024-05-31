using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [SerializeField]
    private float ultCooldown = 2f;

    public GameObject[] slots;
    public bool[] isFull;

    private Animator playerAnimator;
    private float lastUltTime = Mathf.NegativeInfinity;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializeInventory(); // Initialize inventory when the game starts
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate inventory objects
        }
    }

    private void InitializeInventory()
    {
        // Initialize isFull array with the same length as slots array
        isFull = new bool[slots.Length];
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            DropItemFromInventory();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            Ult();
        }
    }

    public void SetPlayerAnimator(Animator animator)
    {
        playerAnimator = animator;
    }

    void Ult()
    {
        bool hasUltObject = CheckUltObject();

        if (hasUltObject && Time.time >= lastUltTime + ultCooldown) // Periksa apakah cooldown selesai
        {
            foreach (GameObject slot in slots)
            {
                if (slot.transform.childCount > 0)
                {
                    foreach (Transform child in slot.transform)
                    {
                        if (child.name == "Ults(Clone)")
                        {
                            Destroy(child.gameObject);
                        }
                    }
                }
            }
            playerAnimator.SetTrigger("Ult");

            // Set lastUltTime menjadi waktu sekarang ketika ultimate dipicu
            lastUltTime = Time.time;
        }
        else
        {
            Debug.Log("Unable to use Ultimate. Missing required conditions or items.");
        }
    }

    bool CheckUltObject()
    {
        foreach (GameObject slot in slots)
        {
            if (slot.transform.childCount > 0)
            {
                foreach (Transform child in slot.transform)
                {
                    if (child.name == "Ults(Clone)")
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    void DropItemFromInventory()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (isFull[i])
            {
                Slot slot = slots[i].GetComponent<Slot>();
                slot.DropItem();
                break;
            }
        }
    }

    public void ClearInventory()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].transform.childCount > 0)
            {
                foreach (Transform child in slots[i].transform)
                {
                    Destroy(child.gameObject);
                }
            }
            isFull[i] = false;
        }
    }
}
