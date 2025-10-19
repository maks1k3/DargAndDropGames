using UnityEngine;

public class TransformationScript : MonoBehaviour
{
    public float maxScale = 4.0f; 
    public float minScale = 0.3f; 

    void Update()
    {
        if (ObjectScript.lastDragged != null)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                ObjectScript.lastDragged.GetComponent<RectTransform>().transform.Rotate(
                    0, 0, Time.deltaTime * 15f);
            }

            if (Input.GetKey(KeyCode.X))
            {
                ObjectScript.lastDragged.GetComponent<RectTransform>().transform.Rotate(
                    0, 0, -Time.deltaTime * 15f);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.y < maxScale)
                {
                    ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale =
                        new Vector3(
                        ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.x,
                        ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.y + 0.005f,
                        1f);
                }
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.y > minScale)
                {
                    ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale =
                    new Vector3(
                    ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.x,
                    ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.y - 0.005f,
                    1f);
                }
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.x > minScale)
                {
                    ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale =
                        new Vector3(
                        ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.x - 0.005f,
                        ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.y,
                        1f);
                }
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.x < maxScale)
                {
                    ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale =
                        new Vector3(
                        ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.x + 0.005f,
                       ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.y,
                        1f);
                }
            }
        }
    }
}
