using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void RestartGame()
    {
        Time.timeScale = 1f; // ОБЯЗАТЕЛЬНО восстановить скорость игры
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
