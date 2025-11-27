using UnityEngine;

public class FlyingObjectManager : MonoBehaviour
{
    public static FlyingObjectManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void DestroyAllFlyingObjects()
    {
        FlyingObjectsControllerScript[] flyingObjects =
            Object.FindObjectsByType<FlyingObjectsControllerScript>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None);

        foreach (var obj in flyingObjects)
        {
            if (obj == null)
                continue;

            if (obj.CompareTag("Bomb"))
                obj.TriggerExplosion();
            else
                obj.StartToDestroy();
        }
    }
}
