using System;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class InterstitialAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidId = "Interstitial_Android";
    private string _adUnit;

    private Button _button;

    public bool isLoaded = false;
    public event Action OnLoaded;

    private void Awake()
    {
        _adUnit = _androidId;
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

        OnLoaded?.Invoke();
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        LoadAd();
    }

    public void ShowAd()
    {
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

    public void OnUnityAdsShowStart(string placementId)
    {
        Time.timeScale = 0f;
    }

    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Time.timeScale = 1f;
        LoadAd();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        LoadAd();
    }

    public void SetButton(Button button)
    {
        _button = button;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ShowAd);

        button.interactable = isLoaded;
    }
}
