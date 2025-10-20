using UnityEngine;

public class TransformationScript : MonoBehaviour
{
    private float minScale = 0.5f;
    private float maxScale = 2.9f;
    private float scaleSpeed = 0.05f; 
    private float rotationSpeed = 60f;

    void Update()
    {
        if (ObjectScript.lastDragged != null)
        {
            Transform draggedTransform = ObjectScript.lastDragged.GetComponent<RectTransform>().transform;
            Vector3 currentScale = draggedTransform.localScale;

            // ВРАЩЕНИЕ
            if (Input.GetKey(KeyCode.Z))
            {
                draggedTransform.Rotate(0, 0, Time.deltaTime * rotationSpeed);
            }

            if (Input.GetKey(KeyCode.X))
            {
                draggedTransform.Rotate(0, 0, -Time.deltaTime * rotationSpeed);
            }

            // ИЗМЕНЕНИЕ РАЗМЕРА
            // СТРЕЛКА ВВЕРХ - УВЕЛИЧИТЬ ВЫСОТУ (Y)
            if (Input.GetKey(KeyCode.UpArrow) && currentScale.y < maxScale)
            {
                draggedTransform.localScale = new Vector3(
                    currentScale.x,
                    currentScale.y + scaleSpeed,
                    1f
                );
            }

            // СТРЕЛКА ВНИЗ - УМЕНЬШИТЬ ВЫСОТУ (Y)
            if (Input.GetKey(KeyCode.DownArrow) && currentScale.y > minScale)
            {
                draggedTransform.localScale = new Vector3(
                    currentScale.x,
                    currentScale.y - scaleSpeed,
                    1f
                );
            }

            // СТРЕЛКА ВЛЕВО - УМЕНЬШИТЬ ШИРИНУ (X)
            if (Input.GetKey(KeyCode.LeftArrow) && currentScale.x > minScale)
            {
                draggedTransform.localScale = new Vector3(
                    currentScale.x - scaleSpeed,
                    currentScale.y,
                    1f
                );
            }

            // СТРЕЛКА ВПРАВО - УВЕЛИЧИТЬ ШИРИНУ (X)
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
