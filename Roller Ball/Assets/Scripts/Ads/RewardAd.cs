using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;
using TMPro;

public class RewardAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button _showAdButton;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null; // This will remain null for unsupported platforms

    public enum RewardType { RandomBall, ContinueAfterLose}
    public RewardType rewardType; 

    public GameManager gameManager;
    public ShopSystem shopSystem;

    void Awake()
    {
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        //Disable the button until the ad is ready to show:
        //_showAdButton.interactable = false;
    }

    void Start()
    {
        _showAdButton.onClick.AddListener(LoadAd);
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
        x = 0;
        if (adUnitId.Equals(_adUnitId))
        {
            // Configure the button to call the ShowAd() method when clicked:
            ShowAd();
            // Enable the button for users to click:
            _showAdButton.interactable = true;

            _ShowAndroidToastMessage("Loaded");
        }
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        // Disable the button:
        _showAdButton.interactable = false;
        // Then show the ad:
        Advertisement.Show(_adUnitId, this);
    }


    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        _ShowAndroidToastMessage($"Can't load ad, check you connection");
        // Use the error details to determine whether to try to load another ad.
    }


    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Failed To Show ad : {message}");
        _ShowAndroidToastMessage("Failed To Show ad");
    }

    public void OnUnityAdsShowStart(string placementId)
    { }

    public void OnUnityAdsShowClick(string placementId)
    { }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            x = x + 1;
            Debug.Log("Unity Ads Rewarded Ad Completed " + x);
            // Grant a reward.
            if (x <= 1)
            {
                switch (rewardType)
                {
                    case RewardType.RandomBall:
                        if(gameManager.activeBallsPanel == true)
                        {
                            shopSystem.UnlockRandom(shopSystem.balls);
                            _ShowAndroidToastMessage("New ball unlocked");
                        }
                        else if (gameManager.activeBallsPanel == false)
                        {
                            shopSystem.UnlockRandom(shopSystem.cylinders);
                            _ShowAndroidToastMessage("New stage unlocked");

                        }

                        // Load another ad:
                        Advertisement.Load(_adUnitId, this);
                        break;
                    case RewardType.ContinueAfterLose:
                        gameManager.OnContinue();
                        break;
                }
            }
        }
    }
    int x = 0;

    void OnDestroy()
    {
        // Clean up the button listeners:
        _showAdButton.onClick.RemoveAllListeners();
        
    }
    private void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }
}
