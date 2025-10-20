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

    public float minScale = 0.5f;
    public float maxScale = 2.9f;
    public bool randomRotation = true;
    public bool randomScale = true;
    public bool randomPosition = true;

    public RectTransform mapImage;
    public float spawnMargin = 50f; 

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

                ApplyRandomTransform(vehicles[i]);
            }
        }
    }

    void ApplyRandomTransform(GameObject vehicle)
    {
        RectTransform rectTransform = vehicle.GetComponent<RectTransform>();
        if (rectTransform == null) return;

        if (randomScale)
        {
            float randomScaleValue = Random.Range(minScale, maxScale);
            rectTransform.localScale = new Vector3(randomScaleValue, randomScaleValue, 1f);
        }

        if (randomRotation)
        {
            float randomRotation = Random.Range(0f, 360f);
            rectTransform.rotation = Quaternion.Euler(0f, 0f, randomRotation);
        }

        if (randomPosition)
        {
            Vector2 randomPosition = GetRandomPositionOnMap();
            rectTransform.localPosition = randomPosition;

            vehicleData vData = vehicle.GetComponent<vehicleData>();
            if (vData != null)
            {
                vData.startCoordinate = randomPosition;
            }
        }
    }

    Vector2 GetRandomPositionOnMap()
    {
        if (mapImage != null)
        {
            Vector2 mapSize = mapImage.rect.size;
            Vector2 mapCenter = mapImage.localPosition;

            float minX = mapCenter.x - mapSize.x / 2 + spawnMargin;
            float maxX = mapCenter.x + mapSize.x / 2 - spawnMargin;
            float minY = mapCenter.y - mapSize.y / 2 + spawnMargin;
            float maxY = mapCenter.y + mapSize.y / 2 - spawnMargin;

            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            return new Vector2(randomX, randomY);
        }
        else
        {
           
            return new Vector2(Random.Range(-300, 300), Random.Range(-150, 150));
        }
    }

    public void RandomizeAllVehicles()
    {
        foreach (GameObject vehicle in vehicles)
        {
            if (vehicle != null)
            {
                ApplyRandomTransform(vehicle);
            }
        }
    }
}
