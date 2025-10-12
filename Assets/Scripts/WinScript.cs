using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{
    public ObjectScript objectScript;
    public GameObject winPanel;
    public Image restartImage;
    public Button exitButton;
    public Timer timer;
    public Text finalTimeText;

    // Система звезд
    public Image star1;
    public Image star2;
    public Image star3;
    public Color starFullColor = Color.yellow;
    public Color starEmptyColor = Color.gray;
    public float timeFor3Stars = 60f;
    public float timeFor2Stars = 120f;
    public float timeFor1Star = 180f;

    private bool gameFinished = false;

    void Start()
    {
        if (objectScript == null)
        {
            Debug.LogError("ObjectScript reference is null!");
            return;
        }

        if (restartImage != null)
        {
            SetupRestartImage();
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(GoToMainMenu);
        }
    }

    void Update()
    {
        if (!gameFinished && objectScript != null && AllVehiclesPlaced())
        {
            WinGame();
        }
    }

    void SetupRestartImage()
    {
        EventTrigger trigger = restartImage.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = restartImage.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry clickEntry = new EventTrigger.Entry();
        clickEntry.eventID = EventTriggerType.PointerClick;
        clickEntry.callback.AddListener((data) => { RestartGame(); });
        trigger.triggers.Add(clickEntry);

        restartImage.raycastTarget = true;
    }

    void WinGame()
    {
        gameFinished = true;

        if (timer != null)
        {
            timer.StopTimer();
        }

        StopAllGameActivity();

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        if (finalTimeText != null && timer != null)
        {
            finalTimeText.text = "Laiks: " + timer.timerText.text;
        }

        CalculateStars();
    }

    void StopAllGameActivity()
    {
       

        StopAllSpawning();

        StopAllFlyingObjects();

        BlockAllVehicleMovement();

        StopAllCoroutines();

       
    }

    void StopAllSpawning()
    {
        FlyingObjectSpawnScript[] spawners = FindObjectsOfType<FlyingObjectSpawnScript>();
        foreach (FlyingObjectSpawnScript spawner in spawners)
        {
            if (spawner != null)
            {
                spawner.CancelInvoke();
                spawner.enabled = false;
            }
        }
        
    }

    void StopAllFlyingObjects()
    {
        FlyingObjectsControllerScript[] flyingObjects = FindObjectsOfType<FlyingObjectsControllerScript>();
        foreach (FlyingObjectsControllerScript flyingObj in flyingObjects)
        {
            if (flyingObj != null)
            {
                flyingObj.enabled = false;

                flyingObj.StopAllCoroutines();
            }
        }
       
    }

    void BlockAllVehicleMovement()
    {
        if (objectScript != null)
        {
            ObjectScript.drag = false;
            ObjectScript.lastDragged = null;
        }

        DragAndDropScript[] dragScripts = FindObjectsOfType<DragAndDropScript>();
        foreach (DragAndDropScript dragScript in dragScripts)
        {
            if (dragScript != null)
            {
                dragScript.enabled = false;
            }
        }
       
    }

    bool AllVehiclesPlaced()
    {
        if (objectScript == null || objectScript.vehicles == null)
            return false;

        foreach (GameObject vehicle in objectScript.vehicles)
        {
            if (vehicle == null) continue;

            vehicleData vehicleData = vehicle.GetComponent<vehicleData>();
            if (vehicleData == null || !vehicleData.rightPlace)
                return false;
        }

        return true;
    }

    void CalculateStars()
    {
        if (timer == null) return;

        float finishTime = timer.time;
        int starsEarned = GetStarsCount(finishTime);
        ShowStars(starsEarned);
    }

    int GetStarsCount(float time)
    {
        if (time <= timeFor3Stars) return 3;
        if (time <= timeFor2Stars) return 2;
        if (time <= timeFor1Star) return 1;
        return 0;
    }

    void ShowStars(int starsCount)
    {
        ResetStars();

        switch (starsCount)
        {
            case 3:
                SetStarColor(star3, starFullColor);
                goto case 2;
            case 2:
                SetStarColor(star2, starFullColor);
                goto case 1;
            case 1:
                SetStarColor(star1, starFullColor);
                break;
        }
    }

    void ResetStars()
    {
        SetStarColor(star1, starEmptyColor);
        SetStarColor(star2, starEmptyColor);
        SetStarColor(star3, starEmptyColor);
    }

    void SetStarColor(Image star, Color color)
    {
        if (star != null)
        {
            star.color = color;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
