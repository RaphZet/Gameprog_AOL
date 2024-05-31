using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 0;
    public bool timeIsRunning = true;
    public TMP_Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        timeIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeIsRunning)
        {
            timeRemaining += Time.deltaTime;
            DisplayTime(timeRemaining);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1; // Menambahkan satu detik ke waktu yang akan ditampilkan

        int hours = Mathf.FloorToInt(timeToDisplay / 3600); // Menghitung jumlah jam
        int minutes = Mathf.FloorToInt((timeToDisplay % 3600) / 60); // Menghitung sisa menit setelah jam
        int seconds = Mathf.FloorToInt(timeToDisplay % 60); // Menghitung sisa detik setelah menit
        int milliseconds = Mathf.FloorToInt((timeToDisplay % 1) * 1000); // Menghitung milidetik dari sisa detik

        timeText.text = string.Format("{0:00}:{1:00}:{2:00}:{3:000}", hours, minutes, seconds, milliseconds); // Menampilkan waktu dalam format 00:00:00:000
    }
}
