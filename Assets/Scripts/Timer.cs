using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;    
    public Text timerText2;    

    public float time = 0f;
    public bool isRunning = false;

    public WonScript wonScript;

    void Start()
    {
        Time.timeScale = 1f; 
        time = 0f;
        isRunning = true;
        Debug.Log("Timer started");
    }

    void Update()
    {
        if (!isRunning) return;

        time += Time.deltaTime;
        UpdateUI();
        Debug.Log("UPDATE WORKS!");
    }

    void UpdateUI()
    {
        int hour = Mathf.FloorToInt(time / 3600f);
        int min = Mathf.FloorToInt((time % 3600f) / 60f);
        int sec = Mathf.FloorToInt(time % 60f);

        string text = $"{hour}:{min:00}:{sec:00}";

        if (timerText != null) timerText.text = text;
        if (timerText2 != null) timerText2.text = text;
    }

    public void StopTimer()
    {
        isRunning = false;
        Debug.Log("Timer stopped at: " + time);
    }
}
