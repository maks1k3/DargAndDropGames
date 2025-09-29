using UnityEngine;
using UnityEngine.UI;

public class WinScript : MonoBehaviour
{
    public GameObject[] car;
    public GameObject winPanel;
    public Timer timer;
    public Text finalTimeText;

    private bool gameFinished = false;

    void Update()
    {
        if (!gameFinished && carPlace())
        {
            gameFinished = true;


            if (timer != null)
                timer.StopTimer();
            if (winPanel != null)
                winPanel.SetActive(true);


            if (finalTimeText != null && timer != null)
                finalTimeText.text = "Laiks:  " + timer.timerText.text;
        }
    }

    bool carPlace()
    {
        foreach (GameObject place in car)
        {
            DropPlaceScript checker = place.GetComponent<DropPlaceScript>();
            if (checker == null || !checker.objScript.rightPlace)
                return false;
        }
        return true;
    }
}