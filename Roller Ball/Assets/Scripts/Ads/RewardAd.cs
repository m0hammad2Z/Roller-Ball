using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;
using TMPro;
using System;

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

    private bool adLoaded = false;
    private bool isClicked = false;
    public float t;
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
        LoadAd();
    }

    void Update()
    {
        t += 1 * Time.unscaledDeltaTime;

        if (t >= 4f && !adLoaded && isClicked)
        {
            _showAdButton.interactable = true;
            _ShowAndroidToastMessage($"Ad failed to load, check you connection");
            t = 0;
            isClicked = false;
        }
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        adLoaded = false;
        if (_adUnitId != null)
        {
            try
            {
                Advertisement.Load(_adUnitId, this);
                _showAdButton.onClick.AddListener(ShowAd);
            }
            catch (Exception e)
            {
                _ShowAndroidToastMessage($"Ad failed to load., check you connection");
                return;
            }
        }
        else
        {
            _ShowAndroidToastMessage($"Can't load ad, check you connection");
        }
    }



    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        adLoaded = true;
        x = 0;
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {        
        _ShowAndroidToastMessage("Loading");
        _showAdButton.interactable = false;
        t = 0;
        isClicked = true;

        try
        {
            Advertisement.Show(_adUnitId, this);
        }
        catch (Exception e)
        {
            _ShowAndroidToastMessage($"failed to show ad, check you connection");
        }
        
    }


    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
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
            _showAdButton.interactable = true;
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
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }
}
