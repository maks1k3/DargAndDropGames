using Unity.VisualScripting;
using UnityEngine;

public class WinScript : MonoBehaviour
{
    public GameObject[] car;
    public GameObject win;


    

    // Update is called once per frame
    void Update()
    {
        if (carPlace())
        {
            if (win == null)
                win.SetActive(true);

            Time.timeScale = 0f;
        }
    }
    bool carPlace()
    {
        foreach (GameObject place in  car)
        {
            DropPlaceScript checker = place.GetComponent <DropPlaceScript>();
            if (checker == null || !checker.objScript.rightPlace)
                return false;
        }
        return true;
    }
}
