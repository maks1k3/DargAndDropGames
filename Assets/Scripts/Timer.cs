using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public float time = 0f;
    public bool isRunning = false;

    void UpdateUI()
    {
        int hour = Mathf.FloorToInt(time / 3600f);
        int min = Mathf.FloorToInt((time % 3600f) / 60f);
        int sec = Mathf.FloorToInt(time % 60f);
        timerText.text = string.Format("{0}:{1:00}:{2:00}", hour, min, sec);
    }

    void Start()
    {
        isRunning = true;
        Debug.Log("Timer started");
    }

    private void Update()
    {
        if (isRunning)
        {
            time += Time.deltaTime;
            UpdateUI();
        }
    }

    public void StopTimer()
    {
        if (isRunning)
        {
            isRunning = false;
            Debug.Log("Time: " + timerText.text);
        }
    }

    public void ResetTimer()
    {
        time = 0f;
        UpdateUI();
    }
}
