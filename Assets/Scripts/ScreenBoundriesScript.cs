using UnityEngine;

public class ScreenBoundriesScript : MonoBehaviour
{
    [HideInInspector]
    public Vector3 screenPoint, offset;

    [HideInInspector]
    public float minX, maxX, minY, maxY;

    public Camera targetCamera;

    public float mapMinX = -1000f;
    public float mapMaxX = 1000f;
    public float mapMinY = -600f;
    public float mapMaxY = 600f;

    [Range(0f, 0.1f)]
    public float padding = 0.02f;

    public float minCamX { get; private set; }
    public float maxCamX { get; private set; }
    public float minCamY { get; private set; }
    public float maxCamY { get; private set; }

    float lastOrthoSize;
    float lastAspect;
    Vector3 lastCamPos;

    void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        AutoDetectMapBounds();
        RecalculateBounds();
        UpdatePublicFields();
    }

    void Update()
    {
        if (targetCamera == null) return;

        bool changed = false;

        if (targetCamera.orthographic)
        {
            if (!Mathf.Approximately(targetCamera.orthographicSize, lastOrthoSize))
                changed = true;
        }

        if (!Mathf.Approximately(targetCamera.aspect, lastAspect))
            changed = true;

        if (targetCamera.transform.position != lastCamPos)
            changed = true;

        if (changed)
        {
            RecalculateBounds();
            UpdatePublicFields();

            lastOrthoSize = targetCamera.orthographicSize;
            lastAspect = targetCamera.aspect;
            lastCamPos = targetCamera.transform.position;
        }
    }

    void AutoDetectMapBounds()
    {
        GameObject mapObject = GameObject.Find("Map");
        if (mapObject != null)
        {
            RectTransform mapRect = mapObject.GetComponent<RectTransform>();
            if (mapRect != null)
            {
                Vector3[] corners = new Vector3[4];
                mapRect.GetWorldCorners(corners);

                mapMinX = corners[0].x;
                mapMaxX = corners[2].x;
                mapMinY = corners[0].y;
                mapMaxY = corners[2].y;
            }
        }
    }

    public void RecalculateBounds()
    {
        if (targetCamera == null) return;

        float cameraHeight = targetCamera.orthographicSize;
        float cameraWidth = cameraHeight * targetCamera.aspect;

        if (cameraWidth * 2f >= (mapMaxX - mapMinX))
        {
            minCamX = maxCamX = (mapMinX + mapMaxX) * 0.5f;
        }
        else
        {
            minCamX = mapMinX + cameraWidth;
            maxCamX = mapMaxX - cameraWidth;
        }

        if (cameraHeight * 2f >= (mapMaxY - mapMinY))
        {
            minCamY = maxCamY = (mapMinY + mapMaxY) * 0.5f;
        }
        else
        {
            minCamY = mapMinY + cameraHeight;
            maxCamY = mapMaxY - cameraHeight;
        }
    }

    void UpdatePublicFields()
    {
        minX = mapMinX;
        maxX = mapMaxX;
        minY = mapMinY;
        maxY = mapMaxY;
    }

    public Vector2 GetClampedPosition(Vector3 curPosition)
    {
        float paddingX = (mapMaxX - mapMinX) * padding;
        float paddingY = (mapMaxY - mapMinY) * padding;

        float wbMinX = mapMinX + paddingX;
        float wbMaxX = mapMaxX - paddingX;
        float wbMinY = mapMinY + paddingY;
        float wbMaxY = mapMaxY - paddingY;

        return new Vector2(
            Mathf.Clamp(curPosition.x, wbMinX, wbMaxX),
            Mathf.Clamp(curPosition.y, wbMinY, wbMaxY)
        );
    }

    public Vector3 GetClampedCameraPosition(Vector3 desiredCamCenter)
    {
        return new Vector3(
            Mathf.Clamp(desiredCamCenter.x, minCamX, maxCamX),
            Mathf.Clamp(desiredCamCenter.y, minCamY, maxCamY),
            desiredCamCenter.z
        );
    }

    public Rect worldBounds
    {
        get
        {
            return new Rect(mapMinX, mapMinY, mapMaxX - mapMinX, mapMaxY - mapMinY);
        }
    }
}