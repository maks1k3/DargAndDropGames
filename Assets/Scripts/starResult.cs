using UnityEngine;
using UnityEngine.UI;

public class starResult : MonoBehaviour
{
    public Image resultImage;

    public Sprite oneStar;
    public Sprite twoStars;
    public Sprite threeStars;

    // Настройка времени (в секундах)
    public float timeForThreeStars = 30f;
    public float timeForTwoStars = 60f;

    public void Show(float time)
    {
        // включаем изображение
        resultImage.gameObject.SetActive(true);

        if (time <= timeForThreeStars)
        {
            resultImage.sprite = threeStars;
        }
        else if (time <= timeForTwoStars)
        {
            resultImage.sprite = twoStars;
        }
        else
        {
            resultImage.sprite = oneStar;
        }
    }
}

