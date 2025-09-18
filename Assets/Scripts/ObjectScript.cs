using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    public GameObject[] vehicles;
    [HideInInspector]
    public Vector2[] startCoordinates;
    public Canvas can;
    public AudioSource effects;
    public AudioClip[] audioCli;
    [HideInInspector]
    public bool rightPlace = false;
    public static GameObject lastDragged = null;
    public static bool drag = false;

    // Start is called before the first frame update
    void Awake()
    {
        startCoordinates = new Vector2[vehicles.Length];
        for(int i=0;i<vehicles.Length;i++)
        {
            startCoordinates[i] = vehicles[i].GetComponent<RectTransform>().localPosition;
        }
        

    }

    // Update is called once per frame
    void Update()
    {

    }
}