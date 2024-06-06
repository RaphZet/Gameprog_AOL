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
    public Transform respawnPoint;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float respawnTime;

    [SerializeField]
    private TMP_Text scoreText; // UI score di canvas
    public int score = 0;
    [SerializeField]
    private GameObject ingameUI, titlePanel, pausePanel;

    private float respawnTimeStart;

    private bool respawn;

    [HideInInspector] public SavePoint respawnStatue;

    private CinemachineVirtualCamera CVC;

    private GameObject winPanelInstance; // Instance dari panel win
    private GameObject losePanelInstance; // Instance dari panel lose

    private AudioManager audioManager;

    public enum GameState
    {
        Title, Paused, Playing
    }

    private GameState gameState;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {

        CVC = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        respawnTimeStart = 0;
        respawn = false;

        audioManager = FindObjectOfType<AudioManager>();

        // Mencari instance panel win dan lose
        winPanelInstance = GameObject.FindWithTag("WinPanel");
        losePanelInstance = GameObject.FindWithTag("LosePanel");

        UpdateScoreText();
        SetGameState(GameState.Title);
    }

    public void SetGameState(GameState state)
    {
        gameState = state;
        if(state == GameState.Title)
        {
            titlePanel.SetActive(true);
            pausePanel.SetActive(false);
            ingameUI.SetActive(false);
            Time.timeScale = 0;
        }
        else if(state == GameState.Playing)
        {
            titlePanel.SetActive(false);
            pausePanel.SetActive(false);
            ingameUI.SetActive(true);
            Time.timeScale = 1;
        }
        else if (state == GameState.Paused)
        {
            titlePanel.SetActive(false);
            pausePanel.SetActive(true);
            ingameUI.SetActive(false);
            Time.timeScale = 0;
        }
    }

    public void StartButton() //kalo player teken button start
    {
        audioManager.Play("Button click");
        SetGameState(GameState.Playing);
    }

    public void LoadButton()
    {
        audioManager.Play("Button click");
        SetGameState(GameState.Playing);
        LoadData();
    }

    public void ResumeButton()
    {
        audioManager.Play("Button click");
        SetGameState(GameState.Playing);
    }

    public void PauseButton()
    {
        audioManager.Play("Button click");
        SetGameState(GameState.Paused);
    }

    private void Update()
    {
        CheckInput();
        CheckRespawn();
    }

    private void CheckInput()
    {
        if(gameState == GameState.Playing)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                SetGameState(GameState.Paused);
            }
        }
        else if (gameState == GameState.Paused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetGameState(GameState.Playing);
            }
        }
        else if (gameState == GameState.Title)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SetGameState(GameState.Playing);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                LoadButton();
            }
        }
    }

    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
    }

    public void NewGameButton()
    {
        ResetData();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        Application.Quit();
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
        if(score < 10)
        {
            scoreText.text = "0" + score.ToString();
            return;
        }
        scoreText.text = score.ToString();
    }

    public void SaveData()
    {
        SaveSystem.SavePlayer(this, FindObjectOfType<PlayerStats>(), FindObjectOfType<Timer>());
    }

    public void ResetData() //buat delete save data
    {
        SaveSystem.ResetData();
    }
        
    public void LoadData()
    {
        SaveData data = SaveSystem.LoadPlayer();

        score = data.score;
        UpdateScoreText();

        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.currentHealth = data.currentHealth;
            playerStats.gameObject.transform.position = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);
        }
        respawnPoint.position = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);

        FindObjectOfType<Timer>().timeRemaining = data.timeElapsed;
    }
}


