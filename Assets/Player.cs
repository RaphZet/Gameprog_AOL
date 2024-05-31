using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject winPanelPrefab; // Prefab untuk panel win
    public GameObject losePanelPrefab; // Prefab untuk panel lose

    private GameObject winPanelInstance; // Instance dari panel win
    private GameObject losePanelInstance; // Instance dari panel lose

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate panel win dan panel lose dari prefab
        winPanelInstance = Instantiate(winPanelPrefab);
        losePanelInstance = Instantiate(losePanelPrefab);

        // Pastikan panel win dan lose tidak aktif pada awalnya
        winPanelInstance.SetActive(false);
        losePanelInstance.SetActive(false);

        // Menambahkan panel ke canvas
        winPanelInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);
        losePanelInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "WIN")
        {
            winPanelInstance.SetActive(true); // Aktifkan panel win
            Time.timeScale = 0;
        }
        else if (collision.tag == "LOSE")
        {
            losePanelInstance.SetActive(true); // Aktifkan panel lose
            Time.timeScale = 0;
        }
    }
}
