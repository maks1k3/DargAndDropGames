using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

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

        AutoCreateAds();
    }

    private void AutoCreateAds()
    {
        if (interstitial == null)
        {
            var obj = new GameObject("InterstitialAd_Auto");
            obj.transform.SetParent(transform);
            interstitial = obj.AddComponent<InterstitialAd>();
        }

        if (rewarded == null)
        {
            var obj = new GameObject("RewardedAd_Auto");
            obj.transform.SetParent(transform);
            rewarded = obj.AddComponent<RewardedAds>();
        }

        if (banner == null)
        {
            var obj = new GameObject("BannerAd_Auto");
            obj.transform.SetParent(transform);
            banner = obj.AddComponent<BannerAd>();
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

    private void OnInitialized()
    {
        LoadAds();
    }

    private void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        StartCoroutine(DelayedSceneSetup(s));
    }

    private IEnumerator DelayedSceneSetup(Scene s)
    {
        yield return null; 

        BindButtons();
        LoadAds();

        if (s.buildIndex != 0)
            visitedOtherScenes = true;

        if (s.buildIndex == 1)
            interstitial?.ShowAd();

        if (s.buildIndex == 0 && visitedOtherScenes)
            interstitial?.ShowAd();

        if (s.buildIndex == 2)
        {
            banner?.LoadBanner();
            banner?.ToggleBanner();
        }
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
        if (i && interstitial) interstitial.SetButton(i.GetComponent<Button>());

        var r = GameObject.FindGameObjectWithTag("RewardedButton");
        if (r && rewarded) rewarded.SetButton(r.GetComponent<Button>());

        var b = GameObject.FindGameObjectWithTag("BannerButton");
        if (b && banner) banner.SetButton(b.GetComponent<Button>());
    }
}
