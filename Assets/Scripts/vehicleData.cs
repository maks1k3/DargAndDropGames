
using UnityEngine;

public class vehicleData : MonoBehaviour
{
    public bool rightPlace = false;
    public Vector2 startCoordinate;

    void Start()
    {
        startCoordinate = GetComponent<RectTransform>().localPosition;
    }
}
