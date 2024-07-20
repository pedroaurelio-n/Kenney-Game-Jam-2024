using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] PlayerDeathbox playerDeathbox;
    
    [SerializeField] float startTime;
    [SerializeField] TextMeshProUGUI timerText;
    
    float timeLeft;
    bool timeRunning;

    void Update ()
    {
        if (!timeRunning)
            return;
        
        timerText.text = timeLeft.ToString("F1");
        timeLeft = Mathf.Max(0f, timeLeft - Time.deltaTime);
        
        if (timeLeft <= 0f)
            playerDeathbox.KillPlayer(false);
    }

    public void StartTimer ()
    {
        timeRunning = true;
        timeLeft = startTime;
    }

    public void AddTime (float value) => timeLeft += value;
}
