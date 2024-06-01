using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform respawnPoint;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float respawnTime;

    [SerializeField]
    private TMP_Text scoreText; // UI score di canvas
    [SerializeField]
    private int score = 0;

    private float respawnTimeStart;

    private bool respawn;

    private CinemachineVirtualCamera CVC;

    private GameObject winPanelInstance; // Instance dari panel win
    private GameObject losePanelInstance; // Instance dari panel lose

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        CVC = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        respawnTimeStart = 0;
        respawn = false;

        // Mencari instance panel win dan lose
        winPanelInstance = GameObject.FindWithTag("WinPanel");
        losePanelInstance = GameObject.FindWithTag("LosePanel");
    }

    private void Update()
    {
        CheckRespawn();
    }

    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
    }

    public void RestartButton()
    {
        // Mengatur kembali Time.timeScale menjadi 1 sebelum memuat ulang scene
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitButton()
    {
        // Mengatur kembali Time.timeScale menjadi 1 sebelum keluar dari aplikasi
        Time.timeScale = 1;
#if UNITY_EDITOR
        // Jika dalam mode editor, log pesan keluar
        Debug.Log("Exit game");
#else
        // Jika dalam build yang sebenarnya, keluar dari aplikasi
        Application.Quit();
#endif
    }

    private void CheckRespawn()
    {
        if (Time.time >= respawnTimeStart + respawnTime && respawn)
        {
            var playerTemp = Instantiate(player, respawnPoint.position, respawnPoint.rotation);
            CVC.m_Follow = playerTemp.transform;
            respawn = false;
        }
    }

    public void DestroyPanels()
    {
        Destroy(winPanelInstance);
        Destroy(losePanelInstance);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score : " + score.ToString();
    }

}


