using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    public GameObject[] vehicles;
    [HideInInspector]
    public Vector2[] startCoordinates;
    public AudioSource effects;
    public AudioClip[] audioCli;
    public static GameObject lastDragged = null;
    public static bool drag = false;

    void Awake()
    {
        if (vehicles == null || vehicles.Length == 0)
        {
            
            return;
        }

        startCoordinates = new Vector2[vehicles.Length];

        for (int i = 0; i < vehicles.Length; i++)
        {
            if (vehicles[i] != null)
            {
                startCoordinates[i] = vehicles[i].GetComponent<RectTransform>().localPosition;

                if (vehicles[i].GetComponent<vehicleData>() == null)
                {
                    vehicles[i].AddComponent<vehicleData>();
                }
            }
        }

     
    }
}
