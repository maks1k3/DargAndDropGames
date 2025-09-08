using UnityEngine;
using UnityEngine.EventSystems;
public class DragAndDropScript : MonoBehaviour , IPointerDownHandler, 
    IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private CanvasGroup canvasGro;
    private RectTransform rectTra;
    public ObjectScript objectScr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGro=GetComponent<CanvasGroup>();
        rectTra=GetComponent<RectTransform>();

    }

    // Update is called once per frame
    public void OnPointerDown(PointerEventData eventData)
    {
        if(Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButton(2)) {
            Debug.Log("OnPointerDown");
            objectScr.effects.PlayOneShot(objectScr.audioCli[0]);
        }
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {

    }


    public void OnEndDrag(PointerEventData eventData)
    {

    }


}
