using UnityEngine;

public class TransformationScript : MonoBehaviour
{
    private float minScale = 0.8f;
    private float maxScale = 2.9f;
    private float scaleSpeed = 0.010f; 
    private float rotationSpeed = 60f;

    void Update()
    {
        if (ObjectScript.lastDragged != null)
        {
            Transform draggedTransform = ObjectScript.lastDragged.GetComponent<RectTransform>().transform;
            Vector3 currentScale = draggedTransform.localScale;

            if (Input.GetKey(KeyCode.Z))
            {
                draggedTransform.Rotate(0, 0, Time.deltaTime * rotationSpeed);
            }

            if (Input.GetKey(KeyCode.X))
            {
                draggedTransform.Rotate(0, 0, -Time.deltaTime * rotationSpeed);
            }

            if (Input.GetKey(KeyCode.UpArrow) && currentScale.y < maxScale)
            {
                draggedTransform.localScale = new Vector3(
                    currentScale.x,
                    currentScale.y + scaleSpeed,
                    1f
                );
            }

            if (Input.GetKey(KeyCode.DownArrow) && currentScale.y > minScale)
            {
                draggedTransform.localScale = new Vector3(
                    currentScale.x,
                    currentScale.y - scaleSpeed,
                    1f
                );
            }

            if (Input.GetKey(KeyCode.LeftArrow) && currentScale.x > minScale)
            {
                draggedTransform.localScale = new Vector3(
                    currentScale.x - scaleSpeed,
                    currentScale.y,
                    1f
                );
            }

            if (Input.GetKey(KeyCode.RightArrow) && currentScale.x < maxScale)
            {
                draggedTransform.localScale = new Vector3(
                    currentScale.x + scaleSpeed,
                    currentScale.y,
                    1f
                );
            }
        }
    }
}
