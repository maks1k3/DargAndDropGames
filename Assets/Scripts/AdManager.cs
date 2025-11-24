using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    public AdsInitializer initializer;
    public InterstitialAd interstitial;
    public RewardedAds rewarded;
    public BannerAd banner;

    private bool visitedOtherScenes = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (initializer == null)
            initializer = FindFirstObjectByType<AdsInitializer>();

        if (initializer != null)
            initializer.OnAdsInitialized += OnInitialized;
    }

    private void OnInitialized()
    {
        FindAds();
        LoadAds();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        FindAds();
        BindButtons();
        LoadAds();

        if (s.buildIndex != 0)
            visitedOtherScenes = true;

        if (s.buildIndex == 1 && interstitial != null)
            interstitial.ShowAd();

        if (s.buildIndex == 0 && visitedOtherScenes && interstitial != null)
            interstitial.ShowAd();
    }

    private void FindAds()
    {
        if (interstitial == null) interstitial = FindFirstObjectByType<InterstitialAd>();
        if (rewarded == null) rewarded = FindFirstObjectByType<RewardedAds>();
        if (banner == null) banner = FindFirstObjectByType<BannerAd>();
    }

    private void LoadAds()
    {
        interstitial?.LoadAd();
        rewarded?.LoadAd();
        banner?.LoadBanner();
    }

    private void BindButtons()
    {
        var i = GameObject.FindGameObjectWithTag("IntertitialButton");
        if (i) interstitial?.SetButton(i.GetComponent<Button>());

        var r = GameObject.FindGameObjectWithTag("RewardedButton");
        if (r) rewarded?.SetButton(r.GetComponent<Button>());

        var b = GameObject.FindGameObjectWithTag("BannerButton");
        if (b) banner?.SetButton(b.GetComponent<Button>());
    }
}
