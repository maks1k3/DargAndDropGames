using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlaceScript : MonoBehaviour, IDropHandler
{
    private float placeZRot, vehicleZRot, rotDiff;
    private Vector3 placeSiz, vehicleSiz;
    private float xSizeDiff, ySizeDiff;
    public ObjectScript objScript;

    public void OnDrop(PointerEventData eventData)
    {
        if ((eventData.pointerDrag != null) &&
            Input.GetMouseButtonUp(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
        {
            if (eventData.pointerDrag.tag.Equals(tag))
            {
                placeZRot = eventData.pointerDrag.GetComponent<RectTransform>().transform.eulerAngles.z;
                vehicleZRot = GetComponent<RectTransform>().transform.eulerAngles.z;

                rotDiff = Mathf.Abs(placeZRot - vehicleZRot);
                Debug.Log("Rotation difference: " + rotDiff);

                placeSiz = eventData.pointerDrag.GetComponent<RectTransform>().localScale;
                vehicleSiz = GetComponent<RectTransform>().localScale;
                xSizeDiff = Mathf.Abs(placeSiz.x - vehicleSiz.x);
                ySizeDiff = Mathf.Abs(placeSiz.y - vehicleSiz.y);
                Debug.Log("X size difference: " + xSizeDiff);
                Debug.Log("Y size difference: " + ySizeDiff);

                if ((rotDiff <= 5 || (rotDiff >= 350 && rotDiff <= 360)) &&
                    (xSizeDiff <= 0.30 && ySizeDiff <= 0.30))
                {
                    Debug.Log("Correct place: " + eventData.pointerDrag.name);

                    vehicleData vehicleData = eventData.pointerDrag.GetComponent<vehicleData>();
                    if (vehicleData != null)
                    {
                        vehicleData.rightPlace = true;
                        
                    }

                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                    eventData.pointerDrag.GetComponent<RectTransform>().localRotation = GetComponent<RectTransform>().localRotation;
                    eventData.pointerDrag.GetComponent<RectTransform>().localScale = GetComponent<RectTransform>().localScale;

                    switch (eventData.pointerDrag.tag)
                    {
                        case "Garbage":
                            objScript.effects.PlayOneShot(objScript.audioCli[2]);
                            Debug.Log("audio2");
                            break;
                        case "Medicine":
                            objScript.effects.PlayOneShot(objScript.audioCli[3]);
                            Debug.Log("audio3");
                            break;
                        case "Fire":
                            objScript.effects.PlayOneShot(objScript.audioCli[4]);
                            Debug.Log("audio4");
                            break;
                        case "Car":
                            objScript.effects.PlayOneShot(objScript.audioCli[5]);
                            Debug.Log("audio5");
                            break;
                        case "e46Car":
                            objScript.effects.PlayOneShot(objScript.audioCli[6]);
                            Debug.Log("audio6");
                            break;
                        case "e61Car":
                            objScript.effects.PlayOneShot(objScript.audioCli[7]);
                            Debug.Log("audio7");
                            break;
                        case "Traktor":
                            objScript.effects.PlayOneShot(objScript.audioCli[8]);
                            Debug.Log("audio8");
                            break;
                        case "YellowTraktor":
                            objScript.effects.PlayOneShot(objScript.audioCli[9]);
                            Debug.Log("audio9");
                            break;
                        case "Bus":
                            objScript.effects.PlayOneShot(objScript.audioCli[10]);
                            Debug.Log("audio10");
                            break;
                        case "Police":
                            objScript.effects.PlayOneShot(objScript.audioCli[11]);
                            Debug.Log("audio11");
                            break;
                        case "Cement":
                            objScript.effects.PlayOneShot(objScript.audioCli[12]);
                            Debug.Log("audio12");
                            break;
                        case "Eksakvators":
                            objScript.effects.PlayOneShot(objScript.audioCli[13]);
                            Debug.Log("audio13");
                            break;
                        default:
                            Debug.Log("Unknown tag detected");
                            break;
                    }
                }
                else
                {
                    
                    vehicleData vehicleData = eventData.pointerDrag.GetComponent<vehicleData>();
                    if (vehicleData != null)
                    {
                        vehicleData.rightPlace = false;
                    }
                    ReturnToStartPosition(eventData.pointerDrag);
                }
            }
            else
            {
                
                objScript.effects.PlayOneShot(objScript.audioCli[1]);
                ReturnToStartPosition(eventData.pointerDrag);
            }
        }
    }

    private void ReturnToStartPosition(GameObject draggedObject)
    {
        vehicleData vData = draggedObject.GetComponent<vehicleData>();
        if (vData != null)
        {
            vData.rightPlace = false;
        }

        if (vData != null)
        {
            draggedObject.GetComponent<RectTransform>().localPosition = vData.startCoordinate;
            
        }
        else
        {
            

            int vehicleIndex = FindVehicleIndex(draggedObject);
            if (vehicleIndex != -1 && vehicleIndex < objScript.startCoordinates.Length)
            {
                draggedObject.GetComponent<RectTransform>().localPosition = objScript.startCoordinates[vehicleIndex];
               
            }
            else
            {
                Debug.LogError("Not found " + draggedObject.name);
            }
        }
    }

    private int FindVehicleIndex(GameObject vehicle)
    {
        for (int i = 0; i < objScript.vehicles.Length; i++)
        {
            if (objScript.vehicles[i] == vehicle)
            {
                return i;
            }
        }
        return -1;
    }
}
