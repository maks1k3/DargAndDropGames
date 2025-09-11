using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public float time = 0f;
    public bool isRunning = false;

    void updateUI()
    {
        int min=Mathf.FloorToInt(time/60);
        int sec=Mathf.FloorToInt(time%60);
        int milSec=Mathf.FloorToInt((time * 100)%100);
        timerText.text = $"{min:00}:{sec:00}.{milSec:00}";   

    }

    void Start()
    {
        isRunning = true;

    }
    private void Update()
    {
        if (isRunning)
        {
            time += Time.deltaTime;
            updateUI();
        }
    }
   
    public void StopTimer()
    {
        isRunning = false;
    }
    public void ResetTimer()
    {
        time = 0f;
       
    }


}
