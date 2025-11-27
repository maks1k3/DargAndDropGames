using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidId = "Rewarded_Android";
    private string _adUnit;

    private Button _button;

    public bool isLoaded = false;

    private bool isCooldown = false;
    private float cooldownTime = 10f;  
    private float freezeTime = 3f;     

    private void Awake()
    {
        _adUnit = _androidId;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadAd()
    {
        if (!Advertisement.isInitialized) return;
        Advertisement.Load(_adUnit, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        isLoaded = true;

        if (_button != null && !isCooldown)
            _button.interactable = true;
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(1f);
        LoadAd();
    }

    public void ShowAd()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (isCooldown) return;

            StartCoroutine(FreezeTimeFor3Sec());
            StartCoroutine(CooldownTimer());
        }

        if (!isLoaded)
        {
            LoadAd();
            return;
        }

        Advertisement.Show(_adUnit, this);

        isLoaded = false;

        if (_button != null)
            _button.interactable = false;
    }

    private IEnumerator FreezeTimeFor3Sec()
    {
        float oldScale = Time.timeScale;

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(freezeTime);
        Time.timeScale = oldScale;
    }

    private IEnumerator CooldownTimer()
    {
        isCooldown = true;

        if (_button != null)
            _button.interactable = false;

        yield return new WaitForSecondsRealtime(cooldownTime);

        isCooldown = false;

        if (isLoaded && _button != null)
            _button.interactable = true;
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        
    }

    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (FlyingObjectManager.Instance != null)
            FlyingObjectManager.Instance.DestroyAllFlyingObjects();

        StartCoroutine(Reload());
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        StartCoroutine(Reload());
    }

    public void SetButton(Button button)
    {
        _button = button;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ShowAd);

        button.interactable = isLoaded;
    }
}
