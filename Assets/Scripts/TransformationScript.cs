using UnityEngine;
using UnityEngine.EventSystems;

// CHANGES FOR ANDROID
public class TransformationScript : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public float scaleSpeed = 2.0f; // Значение за кадр
    public float minScale = 0.35f;
    public float maxScale = 3.0f;
    public static bool isTransforming = false;
    private bool rotateCW, rotateCCW, scaleUpY, scaleDownY, scaleUpX, scaleDownX;

    void Update()
    {
        if (ObjectScript.lastDragged == null) return;

        GameObject draggedObject = ObjectScript.lastDragged;
        RectTransform rt = draggedObject.GetComponent<RectTransform>();
        if (rt == null) return;

        // ПРОВЕРКА НА ПРАВИЛЬНОЕ МЕСТО
        vehicleData vData = draggedObject.GetComponent<vehicleData>();
        if (vData != null && vData.rightPlace) return;

        // ВРАЩЕНИЕ (оставляем с Time.deltaTime)
        if (rotateCW) rt.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        if (rotateCCW) rt.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // МАСШТАБИРОВАНИЕ (без Time.deltaTime для быстрого отклика)
        if (scaleUpY)
        {
            Vector3 newScale = rt.localScale;
            newScale.y += scaleSpeed * Time.deltaTime * 50; // Умножаем для компенсации
            newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
            rt.localScale = newScale;
            rt.ForceUpdateRectTransforms();
        }

        if (scaleDownY)
        {
            Vector3 newScale = rt.localScale;
            newScale.y -= scaleSpeed * Time.deltaTime * 50;
            newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
            rt.localScale = newScale;
            rt.ForceUpdateRectTransforms();
        }

        if (scaleUpX)
        {
            Vector3 newScale = rt.localScale;
            newScale.x += scaleSpeed * Time.deltaTime * 50;
            newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
            rt.localScale = newScale;
            rt.ForceUpdateRectTransforms();
        }

        if (scaleDownX)
        {
            Vector3 newScale = rt.localScale;
            newScale.x -= scaleSpeed * Time.deltaTime * 50;
            newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
            rt.localScale = newScale;
            rt.ForceUpdateRectTransforms();
        }

        isTransforming = rotateCW || rotateCCW || scaleUpY || scaleDownY || scaleUpX || scaleDownX;
    }

    public void StartRotateCW(BaseEventData data)
    {
        Debug.Log("StartRotateCW");
        rotateCW = true;
    }
    public void StopRotateCW(BaseEventData data)
    {
        Debug.Log("StopRotateCW");
        rotateCW = false;
    }

    public void StartRotateCCW(BaseEventData data)
    {
        Debug.Log("StartRotateCCW");
        rotateCCW = true;
    }
    public void StopRotateCCW(BaseEventData data)
    {
        Debug.Log("StopRotateCCW");
        rotateCCW = false;
    }

    public void StartScaleUpY(BaseEventData data)
    {
        Debug.Log("StartScaleUpY");
        scaleUpY = true;
    }
    public void StopScaleUpY(BaseEventData data)
    {
        Debug.Log("StopScaleUpY");
        scaleUpY = false;
    }

    public void StartScaleDownY(BaseEventData data)
    {
        Debug.Log("StartScaleDownY");
        scaleDownY = true;
    }
    public void StopScaleDownY(BaseEventData data)
    {
        Debug.Log("StopScaleDownY");
        scaleDownY = false;
    }

    public void StartScaleUpX(BaseEventData data)
    {
        Debug.Log("StartScaleUpX");
        scaleUpX = true;
    }
    public void StopScaleUpX(BaseEventData data)
    {
        Debug.Log("StopScaleUpX");
        scaleUpX = false;
    }

    public void StartScaleDownX(BaseEventData data)
    {
        Debug.Log("StartScaleDownX");
        scaleDownX = true;
    }
    public void StopScaleDownX(BaseEventData data)
    {
        Debug.Log("StopScaleDownX");
        scaleDownX = false;
    }
}
