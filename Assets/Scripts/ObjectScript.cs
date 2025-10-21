using UnityEngine;
using System.Collections.Generic;

public class ObjectScript : MonoBehaviour
{
    public GameObject[] vehicles;
    public GameObject[] parkingSpots;
    [HideInInspector]
    public Vector2[] startCoordinates;
    public AudioSource effects;
    public AudioClip[] audioCli;
    public static GameObject lastDragged = null;
    public static bool drag = false;

    public float minScale = 0.8f;
    public float maxScale = 2.9f;
    public bool randomRotation = true;
    public bool randomScale = true;
    public bool randomPosition = true;

    public RectTransform mapImage;
    public float spawnMargin = 50f;

    public float minDistanceBetweenObjects = 80f;
    public int maxSpawnAttempts = 50; 

    private List<Vector2> usedPositions = new List<Vector2>();

    void Awake()
    {
        if (vehicles == null || vehicles.Length == 0)
        {
            return;
        }

        startCoordinates = new Vector2[vehicles.Length];
        usedPositions.Clear();

        if (parkingSpots != null && parkingSpots.Length > 0)
        {
            RandomizeParkingSpots();
        }

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

        Debug.Log($"Spawned {vehicles.Length} vehicles and {parkingSpots?.Length ?? 0} parking spots without overlaps");
    }

    void ApplyRandomTransform(GameObject obj)
    {
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
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
            Vector2 randomPosition = GetRandomPositionWithoutOverlap();
            rectTransform.localPosition = randomPosition;

            vehicleData vData = obj.GetComponent<vehicleData>();
            if (vData != null)
            {
                vData.startCoordinate = randomPosition;
            }

            usedPositions.Add(randomPosition);
        }
    }

    void RandomizeParkingSpots()
    {
        foreach (GameObject spot in parkingSpots)
        {
            if (spot != null)
            {
                RectTransform rectTransform = spot.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    Vector2 randomPosition = GetRandomPositionWithoutOverlap();
                    rectTransform.localPosition = randomPosition;

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

                    usedPositions.Add(randomPosition);
                }
            }
        }
    }

    Vector2 GetRandomPositionWithoutOverlap()
    {
        Vector2 randomPosition;
        int attempts = 0;
        bool positionValid = false;

        do
        {
            randomPosition = GetRandomPositionOnMap();
            attempts++;
            positionValid = IsPositionValid(randomPosition);

            if (attempts >= maxSpawnAttempts)
            {
                
                break;
            }

        } while (!positionValid);

        return randomPosition;
    }

    bool IsPositionValid(Vector2 position)
    {
        foreach (Vector2 usedPosition in usedPositions)
        {
            float distance = Vector2.Distance(position, usedPosition);
            if (distance < minDistanceBetweenObjects)
            {
                return false; 
            }
        }
        return true; 
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

            if (minX >= maxX) minX = maxX - 10f;
            if (minY >= maxY) minY = maxY - 10f;

            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            return new Vector2(randomX, randomY);
        }
        else
        {
            return new Vector2(Random.Range(-250, 250), Random.Range(-120, 120));
        }
    }

    public void RandomizeAllVehicles()
    {
        usedPositions.Clear();

        if (parkingSpots != null && parkingSpots.Length > 0)
        {
            RandomizeParkingSpots();
        }

        foreach (GameObject vehicle in vehicles)
        {
            if (vehicle != null)
            {
                ApplyRandomTransform(vehicle);
            }
        }
    }
}
