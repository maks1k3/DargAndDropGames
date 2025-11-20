using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    public AdsInitializer adsInitializer;
    public InterstitialAd interstitialAd;
    [SerializeField] bool turnOffInterstitialAd = false;
    private bool firstAdShown = false;

    public RewardedAds rewardedAds;
    [SerializeField] bool turnOffRewardedAds = false;

    public static AdManager Instance { get; private set; }

    private int currentSceneIndex = -1;
    private int previousSceneIndex = -1;
    private bool isFirstLaunch = true;
    private int[] gameSceneIndices = new int[] { 1, 2 };

    private void Awake()
    {
        if (adsInitializer == null)
            adsInitializer = FindFirstObjectByType<AdsInitializer>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        adsInitializer.OnAdsInitialized += HandleAdsInitialized;
    }

    private void HandleAdsInitialized()
    {
        if (!turnOffInterstitialAd)
        {
            interstitialAd.OnInterstitialAdReady += HandleInterstitialReady;
            interstitialAd.LoadAd();
        }

        if (!turnOffRewardedAds)
        {
            rewardedAds.LoadAd();
        }
    }



    private void HandleInterstitialReady()
    {
        if (!firstAdShown)
        {
            if (IsGameScene(currentSceneIndex))
            {
                Debug.Log($"First interstitial ad ready - showing in game scene ({currentSceneIndex})!");
                interstitialAd.ShowAd();
                firstAdShown = true;
            }
            else
            {
                Debug.Log("First interstitial ad ready, but we are not in game scene - not showing");
                firstAdShown = true;
            }
        }
        else
        {
            Debug.Log("Next interstitial ad is ready for manual show!");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (interstitialAd == null)
            interstitialAd = FindFirstObjectByType<InterstitialAd>();

        GameObject interstitialButtonObj = GameObject.FindGameObjectWithTag("IntertitialButton");

        if (rewardedAds == null)
            rewardedAds = FindFirstObjectByType<RewardedAds>();

        Button rewardedAdButton = GameObject.FindGameObjectWithTag("RewardedButton").GetComponent<Button>();

        if (rewardedAds != null && rewardedAdButton != null)
            rewardedAds.SetButton(rewardedAdButton);
        if (interstitialButtonObj != null)
        {
            Button interstitialButton = interstitialButtonObj.GetComponent<Button>();
            if (interstitialAd != null && interstitialButton != null)
            {
                interstitialAd.SetButton(interstitialButton);
            }
        }

        

        previousSceneIndex = currentSceneIndex;
        currentSceneIndex = scene.buildIndex;

        Debug.Log($"Scene loaded: {scene.name} (Index: {currentSceneIndex}), Previous Index: {previousSceneIndex}, First Launch: {isFirstLaunch}");

        if (IsGameScene(currentSceneIndex) && !turnOffInterstitialAd)
        {
            Debug.Log($"Game scene ({currentSceneIndex}) loaded - showing ad!");
            ShowAdOnGameScene();
        }

        if (currentSceneIndex == 0 && IsGameScene(previousSceneIndex) && !turnOffInterstitialAd)
        {
            Debug.Log($"Transition from game scene ({previousSceneIndex}) to menu (0) - showing ad!");
            ShowAdOnMenuScene();
        }

        if (isFirstLaunch)
        {
            isFirstLaunch = false;
        }

        
    }

    private bool IsGameScene(int sceneIndex)
    {
        foreach (int gameIndex in gameSceneIndices)
        {
            if (sceneIndex == gameIndex)
            {
                return true;
            }
        }
        return false;
    }

    private void ShowAdOnGameScene()
    {
        if (interstitialAd == null) return;

        Debug.Log("Attempting to show ad on game scene...");

        if (interstitialAd.isReady)
        {
            Debug.Log("Showing interstitial ad on game scene!");
            interstitialAd.ShowAd();
        }
        else
        {
            Debug.Log("Interstitial ad not ready for game scene, loading...");
            interstitialAd.LoadAd();

            interstitialAd.OnInterstitialAdReady += OnAdReadyForGameScene;
        }
    }

    private void ShowAdOnMenuScene()
    {
        if (interstitialAd == null) return;

        Debug.Log("Attempting to show ad on menu scene...");

        if (interstitialAd.isReady)
        {
            Debug.Log("Showing interstitial ad on menu scene!");
            interstitialAd.ShowAd();
        }
        else
        {
            Debug.Log("Interstitial ad not ready for menu scene, loading...");
            interstitialAd.LoadAd();

            interstitialAd.OnInterstitialAdReady += OnAdReadyForMenuScene;
        }
    }

    private void OnAdReadyForGameScene()
    {
        if (interstitialAd != null && interstitialAd.isReady)
        {
            Debug.Log("Ad ready for game scene, showing now!");
            interstitialAd.ShowAd();
            interstitialAd.OnInterstitialAdReady -= OnAdReadyForGameScene;
        }
    }

    private void OnAdReadyForMenuScene()
    {
        if (interstitialAd != null && interstitialAd.isReady)
        {
            Debug.Log("Ad ready for menu scene, showing now!");
            interstitialAd.ShowAd();
            interstitialAd.OnInterstitialAdReady -= OnAdReadyForMenuScene;
        }
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.isReady)
        {
            interstitialAd.ShowAd();
        }
        else
        {
            Debug.Log("Interstitial ad not ready, loading...");
            interstitialAd?.LoadAd();
        }
    }
}
