using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ToFirstScene()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void ToSecondScene()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("out");
    }
}
