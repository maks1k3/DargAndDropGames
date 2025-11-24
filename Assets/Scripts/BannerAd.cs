using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class BannerAd : MonoBehaviour
{
    [SerializeField] string _androidId = "Banner_Android";
    private string _adUnit;

    private Button _button;

    public bool isLoaded = false;
    public bool isVisible = false;

    [SerializeField] BannerPosition _position = BannerPosition.BOTTOM_CENTER;

    private void Awake()
    {
        _adUnit = _androidId;
        Advertisement.Banner.SetPosition(_position);
    }

    public void LoadBanner()
    {
        BannerLoadOptions opts = new BannerLoadOptions
        {
            loadCallback = OnLoaded,
            errorCallback = OnError
        };

        Advertisement.Banner.Load(_adUnit, opts);
    }

    private void OnLoaded()
    {
        isLoaded = true;

        if (_button != null)
            _button.interactable = true;
    }

    private void OnError(string msg)
    {
        LoadBanner();
    }

    public void ToggleBanner()
    {
        if (!isVisible)
        {
            Advertisement.Banner.Show(_adUnit);
            isVisible = true;
        }
        else
        {
            Advertisement.Banner.Hide();
            isVisible = false;
        }
    }

    public void SetButton(Button button)
    {
        _button = button;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ToggleBanner);

        button.interactable = isLoaded;
    }
}
