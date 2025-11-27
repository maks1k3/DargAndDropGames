using UnityEngine;
using UnityEngine.UI;

public class WonScript : MonoBehaviour
{
    public GameObject winPanel;
    public Image starsImage;
    public Text timeText;

    public Sprite[] starSprites;

    public float threeStarTime = 30f;
    public float twoStarTime = 60f;

    public void ShowWin(float time)
    {
        winPanel.SetActive(true);

        int min = Mathf.FloorToInt(time / 60f);
        int sec = Mathf.FloorToInt(time % 60f);

        timeText.text = $"{min:00}:{sec:00}";

        int index = 0;
        if (time <= threeStarTime) index = 2;
        else if (time <= twoStarTime) index = 1;
        else index = 0;

        starsImage.sprite = starSprites[index];
    }
}
