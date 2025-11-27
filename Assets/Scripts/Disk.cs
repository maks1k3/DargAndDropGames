using UnityEngine;
using UnityEngine.EventSystems;

public class Disk : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rect;
    private CanvasGroup group;

    private Tower currentTower;
    public int size;
    private void Start()
    {
        rect = GetComponent<RectTransform>();

        group = GetComponent<CanvasGroup>();
        if (group == null)
            group = gameObject.AddComponent<CanvasGroup>();

        canvas = FindObjectOfType<Canvas>();
    }

    public void SetTower(Tower t)
    {
        currentTower = t;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentTower == null)
        {
            Debug.LogError(name + " — НЕТ currentTower!");
            eventData.pointerDrag = null;
            return;
        }

        if (currentTower.GetTopDisk() != this)
        {
            eventData.pointerDrag = null;
            return;
        }

        group.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        group.blocksRaycasts = true;

        Tower tower = FindNearestTower();
        if (tower != null)
        {
            currentTower.RemoveDisk(this);
            tower.AddDisk(this);
            currentTower = tower;
        }
        else
        {
            currentTower.UpdateStack();
        }
    }

    Tower FindNearestTower()
    {
        Tower[] towers = FindObjectsOfType<Tower>();

        float minDist = float.MaxValue;
        Tower best = null;

        Vector2 diskWorldPos = rect.position; 

        foreach (Tower t in towers)
        {
            Vector2 towerWorldPos = t.GetComponent<RectTransform>().position;

            float dist = Vector2.Distance(diskWorldPos, towerWorldPos);

            if (dist < minDist)
            {
                minDist = dist;
                best = t;
            }
        }

        if (minDist < 300f)   
            return best;

        return null;
    }

}
