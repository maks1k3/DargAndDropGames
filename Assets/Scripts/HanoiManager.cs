using UnityEngine;

public class HanoiManager : MonoBehaviour
{
    public Tower[] towers;
    public Timer timer;
    public WonScript wonScript;

    void Update()
    {
        foreach (var tower in towers)
        {
            if (tower.DiskCount == 5 && tower.IsCorrectlySorted())
            {
                Debug.Log("Победа! Все диски отсортированы.");

                timer.StopTimer();     
                Time.timeScale = 0f;  

                wonScript.ShowWin(timer.time); 
                enabled = false; 
                return;
            }
        }
    }
}
