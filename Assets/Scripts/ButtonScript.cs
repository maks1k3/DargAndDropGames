using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    
    public void ToFirstScene()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void ToSecondScene()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    public void ToThirdScene()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("out");
    }
}
