using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidId = "Rewarded_Android";
    private string _adUnit;

    private Button _button;

    public bool isLoaded = false;
    public FlyingObjectManager flyingManager;

    private void Awake()
    {
        _adUnit = _androidId;

        if (flyingManager == null)
            flyingManager = FindFirstObjectByType<FlyingObjectManager>();
    }

    public void LoadAd()
    {
        if (!Advertisement.isInitialized) return;
        Advertisement.Load(_adUnit, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        isLoaded = true;

        if (_button != null)
            _button.interactable = true;
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(2);
        LoadAd();
    }

    public void ShowAd()
    {
        if (!isLoaded)
            return;

        Advertisement.Show(_adUnit, this);

        isLoaded = false;

        if (_button != null)
            _button.interactable = false;
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Time.timeScale = 0f;
    }

    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        flyingManager.DestroyAllFlyingObjects();
        Time.timeScale = 1f;

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
