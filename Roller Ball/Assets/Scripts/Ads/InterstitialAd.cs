using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class InterstitialAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] string _iOsAdUnitId = "Interstitial_iOS";
    string _adUnitId;

    public bool isLoaded;

    public Button restartButton;
    public int maxRestart;
    public static int restartNumber;

    void Awake()
    {
        // Get the Ad Unit ID for the current platform:
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitId
            : _androidAdUnitId;
    }

    void Start()
    {
        isLoaded = false;
        LoadAd();

        restartButton.onClick.AddListener(() => 
        { 
            restartNumber++;
            Debug.Log(restartNumber);
            if(restartNumber >= maxRestart && isLoaded)
            {
                Advertisement.Show(_adUnitId, this);
                restartNumber = 0;
            }
        });
    }

    void Update()
    {

    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // Show the loaded content in the Ad Unit:
    public void ShowAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        if (GetComponent<LSystem>().currentLevel != 0 && isLoaded)
        {
            if ((GetComponent<LSystem>().currentLevel % 4 == 0 && GetComponent<LSystem>().currentLevel <= 12) ||
                (GetComponent<LSystem>().currentLevel % 3 == 0 && GetComponent<LSystem>().currentLevel > 12 && GetComponent<LSystem>().currentLevel <= 27) ||
                (GetComponent<LSystem>().currentLevel % 2 == 0 && GetComponent<LSystem>().currentLevel > 27 && GetComponent<LSystem>().currentLevel <= 40) ||
                (GetComponent<LSystem>().currentLevel % 1 == 0 && GetComponent<LSystem>().currentLevel > 40 && GetComponent<LSystem>().currentLevel <= GetComponent<LSystem>().levels.Count
                ))
            {
                Debug.Log("Showing Ad: " + _adUnitId);
                Advertisement.Show(_adUnitId, this);
            }
        }
    }

    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        isLoaded = true;
        //ShowAd();
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        isLoaded = false;
        Advertisement.Load(_adUnitId, this);
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        Advertisement.Show(_adUnitId, this);
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) { restartNumber = 0; isLoaded = false; }
}