using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// CHANGES NOT NEEDED FOR ANDROID
public class FlyingObjectsControllerScript : MonoBehaviour
{
    [HideInInspector]
    public float speed = 1f;
    public float fadeDuration = 1.5f;
    public float waveAmplitude = 25f;
    public float waveFrequency = 1f;
    private ObjectScript objectScript;
    private ScreenBoundriesScript scrreenBoundriesScript;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private bool isFadingOut = false;
    private bool isExploading = false;
    private Image image;
    private Color originalColor;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        rectTransform = GetComponent<RectTransform>();

        image = GetComponent<Image>();
        originalColor = image.color;
        objectScript = FindFirstObjectByType<ObjectScript>();
        scrreenBoundriesScript = FindFirstObjectByType<ScreenBoundriesScript>();
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        float waveOffset = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
        rectTransform.anchoredPosition += new Vector2(-speed * Time.deltaTime, waveOffset * Time.deltaTime);

        if (speed > 0 && transform.position.x < (scrreenBoundriesScript.minX + 80) && !isFadingOut)
        {
            StartCoroutine(FadeOutAndDestroy());
            isFadingOut = true;
        }

        if (speed < 0 && transform.position.x > (scrreenBoundriesScript.maxX - 80) && !isFadingOut)
        {
            StartCoroutine(FadeOutAndDestroy());
            isFadingOut = true;
        }
        Vector2 inputPosition;
        if (!TryGetInputPosition(out inputPosition))
            return;
        if (CompareTag("Bomb") && !isExploading && !ObjectScript.drag &&
            RectTransformUtility.RectangleContainsScreenPoint(rectTransform, inputPosition, Camera.main))
        {
            Debug.Log("The cursor collided with a bomb! (without car)");
            TriggerExplosion();
        }

        if (ObjectScript.drag && !isFadingOut && !isExploading &&
            RectTransformUtility.RectangleContainsScreenPoint(rectTransform, inputPosition, Camera.main))
        {
            Debug.Log("The cursor collided with a flying object!");

            if (ObjectScript.lastDragged != null)
            {
                if (CompareTag("Bomb"))
                {
                    StartCoroutine(HandleBombWithCar(ObjectScript.lastDragged));
                }
                else
                {
                    StartCoroutine(HandleCarCollision(ObjectScript.lastDragged));
                }
                return;
            }
            StartToDestroy();
        }
    }
    bool TryGetInputPosition(out Vector2 position)
    {
    #if UNITY_EDITOR || UNITY_STANDALONE
            position = Input.mousePosition;
            return true;

    #elif UNITY_ANDROID
      if(Input.touchCount>0){
          position=Input.GetTouch(0).position;
          return true;
        }
        else
        {
        position=Vector2.zero;
        return false;
        }
    
    #endif
    }
    IEnumerator HandleBombWithCar(GameObject car)
    {
        isExploading = true;

        ObjectScript.lastDragged = null;
        ObjectScript.drag = false;

        if (TryGetComponent<Animator>(out Animator animator))
        {
            animator.SetBool("explode", true);
        }

        if (objectScript != null && objectScript.audioCli.Length > 15)
        {
            objectScript.effects.PlayOneShot(objectScript.audioCli[15], 5f);
        }

        image.color = Color.red;
        StartCoroutine(RecoverColor(0.4f));
        StartCoroutine(Vibrate());

        yield return StartCoroutine(ShrinkAndDestroy(car, 0.8f));

        float radius = 0f;
        if (TryGetComponent<CircleCollider2D>(out CircleCollider2D circleCollider))
        {
            radius = circleCollider.radius * transform.lossyScale.x;
            ExploadAndDestroy(radius);
        }

       

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator HandleCarCollision(GameObject car)
    {
        ObjectScript.lastDragged = null;
        ObjectScript.drag = false;

        if (objectScript != null && objectScript.audioCli.Length > 14)
        {
            objectScript.effects.PlayOneShot(objectScript.audioCli[14]); 
        }

        StartToDestroy();

        yield return StartCoroutine(ShrinkAndDestroy(car, 0.8f));

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TriggerExplosion()
    {
        isExploading = true;
        objectScript.effects.PlayOneShot(objectScript.audioCli[15], 5f);

        if (TryGetComponent<Animator>(out Animator animator))
        {
            animator.SetBool("explode", true);
        }
        image.color = Color.red;
        StartCoroutine(RecoverColor(0.4f));

        StartCoroutine(Vibrate());
        StartCoroutine(WaitBeforeExpload());
    }

    IEnumerator WaitBeforeExpload()
    {
        float radius = 0f;
        if (TryGetComponent<CircleCollider2D>(out CircleCollider2D circleCollider))
        {
            radius = circleCollider.radius * transform.lossyScale.x;
        }
        ExploadAndDestroy(radius);
        yield return new WaitForSeconds(1f);
        ExploadAndDestroy(radius);
        Destroy(gameObject);
    }

    void ExploadAndDestroy(float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider != null && hitCollider.gameObject != gameObject)
            {
                FlyingObjectsControllerScript obj = hitCollider.gameObject.GetComponent<FlyingObjectsControllerScript>();
                if (obj != null && !obj.isExploading)
                {
                    obj.StartToDestroy();
                }
            }
        }
    }

    public void StartToDestroy()
    {
        if (!isFadingOut)
        {
            StartCoroutine(FadeOutAndDestroy());
            isFadingOut = true;

            if (CompareTag("Bomb"))
                image.color = Color.red;
            else
                image.color = Color.cyan;

            StartCoroutine(RecoverColor(0.5f));

            if (objectScript != null && objectScript.audioCli.Length > 5)
            {
                objectScript.effects.PlayOneShot(objectScript.audioCli[14]);
            }

            StartCoroutine(Vibrate());
        }
    }

    IEnumerator Vibrate()
    {

#if UNITY_ANDROID
        Handheld.Vibrate();
#endif
        Vector2 originalPosition = rectTransform.anchoredPosition;
        float duration = 0.3f;
        float elapsed = 0f;
        float intensity = 5f;

        while (elapsed < duration)
        {
            rectTransform.anchoredPosition = originalPosition + Random.insideUnitCircle * intensity;
            elapsed += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = originalPosition;
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    IEnumerator FadeOutAndDestroy()
    {
        float t = 0f;
        float startAlpha = canvasGroup.alpha;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        Destroy(gameObject);
    }

    IEnumerator ShrinkAndDestroy(GameObject target, float duration)
    {
        Vector3 originalScale = target.transform.localScale;
        Quaternion originalRotation = target.transform.rotation;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            target.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t / duration);
            float angle = Mathf.Lerp(0f, 360f, t / duration);
            target.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            yield return null;
        }

        if (target != null)
        {
            Destroy(target);
        }
    }

    IEnumerator RecoverColor(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        image.color = originalColor;
    }
}
