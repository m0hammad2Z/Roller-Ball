using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

#if !UNITY_EDITOR && UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

public class GameManager : MonoBehaviour
{
#if !UNITY_EDITOR && UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern bool IsMobile();
#endif

    public bool isMobile()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
return IsMobile();
#endif
        return false;
    }

    //public int score;
    public bool isPlay;
    //public bool isVibrate = true;
    public LSystem lSystem;

    public Material[] skyMaterials;

    [Header("Ads")]
    [Space]
    [Header("Continue Reward Ad")]
    public BallController ballController;
    public GameObject counter;
    public ShopSystem shopSystem;
    public static bool isCounting;

    [Header("Audio")]
    [Space]
    public AudioClip completeLevel;
    public AudioClip passObstecale;


    [Header("UI")]
    [Space]
    [Header("Start Panel")]
    public GameObject StartPanel;
    public Slider slider;
    public TextMeshProUGUI lvlText, startText;

    [Header("Game Panel")]
    public GameObject gamePanel;

    [Header("Level Complete Panel")]
    public GameObject LCPanel;
    public TextMeshProUGUI text;


    [Header("Pause Game Panel")]
    public GameObject pauseGamePanel;

    [Header("Game Over Panel")]
    public GameObject gameOverPanel;

    [Header("Shop Panel")]
    public GameObject shopPanel;
    public GameObject ballsPanel, cylinderPanel;
    public bool activeBallsPanel;

    [Header("Voice UI")]
    public Sprite muteSprite;
    public Sprite unMuteSprite;
    public Button startVoiceButton;


    public GameObject wholeGameFnish;


    void Start()
    {

        activeBallsPanel = true;
        OnOpenScene();
        //isVibrate = PlayerPrefs.GetInt("vibrate") == 1 ? true : false;

        //Voice
        if (AudioManager.muteGameMusic == true)
        {
            startVoiceButton.image.sprite = muteSprite;
        }
        else
        {
            startVoiceButton.image.sprite = unMuteSprite;
        }


        RandomSky();

#if UNITY_WEBGL
        if (!isMobile())
        {
            startText.SetText("Press Enter");
        }
        else
        {
            startText.SetText("Tap To Start");
        }
#endif

#if UNITY_ANDROID
        startText.SetText("Tap To Start");
#endif

    }

    public void RandomSky()
    {
        int randSky = Random.Range(0, skyMaterials.Length);
        Debug.Log(randSky);
        if (randSky == PlayerPrefs.GetInt("randSky"))
        {
            RandomSky();
        }
        else
        {
            PlayerPrefs.SetInt("randSky", randSky);
            RenderSettings.skybox = skyMaterials[randSky];
        }

    }

    public void OnOpenScene()
    {
        Time.timeScale = 0;
        isPlay = false;
        //if(isPlay)
        //InvokeRepeating("ScoreUp", 0.2f, 0.1f);


        gamePanel.SetActive(false);
        StartPanel.SetActive(true);
        LCPanel.SetActive(false);
        pauseGamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        shopPanel.SetActive(false);
    }

    public void OnClickStart()
    {
        Time.timeScale = 1;
        StartPanel.SetActive(false);
        gamePanel.SetActive(true);
        LCPanel.SetActive(false);
        pauseGamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        shopPanel.SetActive(false);
        isPlay = true;
    }

    public void OnGameOver()
    {
        Time.timeScale = 0;
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        LCPanel.SetActive(false);
        pauseGamePanel.SetActive(false);
        StartPanel.SetActive(false);
        lvlText.gameObject.SetActive(false);

        isPlay = false;
    }

    public void OnPause()
    {
        Time.timeScale = 0;
        pauseGamePanel.SetActive(true);
        lvlText.gameObject.SetActive(false);

        isPlay = false;
    }
    public void OnContinue()
    {
        StartCoroutine(Counter());
    }

    IEnumerator Counter()
    {
        isCounting = true;
        Obstacle.moveSpeed = 0;
        counter.SetActive(true);
        OnClickStart();
        gamePanel.SetActive(false);
        ballController.childBall.gameObject.SetActive(true);

        Destroy(GameObject.FindGameObjectWithTag("clone"));

        counter.GetComponent<TextMeshProUGUI>().SetText("3");
        yield return new WaitForSeconds(1);
        counter.GetComponent<TextMeshProUGUI>().SetText("2");
        yield return new WaitForSeconds(1);
        counter.GetComponent<TextMeshProUGUI>().SetText("1");
        yield return new WaitForSeconds(1);

        counter.SetActive(false);
        lvlText.gameObject.SetActive(true);
        Obstacle.moveSpeed = 22;
        gamePanel.SetActive(true);
        isCounting = false;
    }

    public void OnResume()
    {
        StartCoroutine(ResumeCounter());
    }

    IEnumerator ResumeCounter()
    {
        isCounting = true;
        Obstacle.moveSpeed = 0;
        counter.SetActive(true);
        OnClickStart();
        ballController.childBall.gameObject.SetActive(true);
        gamePanel.SetActive(false);

        counter.GetComponent<TextMeshProUGUI>().SetText("3");
        yield return new WaitForSeconds(1);
        counter.GetComponent<TextMeshProUGUI>().SetText("2");
        yield return new WaitForSeconds(1);
        counter.GetComponent<TextMeshProUGUI>().SetText("1");
        yield return new WaitForSeconds(1);

        counter.SetActive(false);
        lvlText.gameObject.SetActive(true);
        Obstacle.moveSpeed = 22;
        gamePanel.SetActive(true);
        isCounting = false;
    }

    public void OnRestart()
    {
        SceneManager.LoadScene("Game");
        OnOpenScene();
    }

    public void OnLevelComplete()
    {
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        LCPanel.SetActive(true);
        pauseGamePanel.SetActive(false);
        StartPanel.SetActive(false);
        lvlText.gameObject.SetActive(false);

        text.SetText("Level " + lSystem.levels[lSystem.currentLevel].levelID + " Completed");

        Time.timeScale = 0;
        isPlay = false;
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        StartPanel.SetActive(false);
        gamePanel.SetActive(false);
        LCPanel.SetActive(false);
        pauseGamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void SwitchShopPanels()
    {
        if (activeBallsPanel)
        {
            activeBallsPanel = false;
        }
        else if (!activeBallsPanel)
        {
            activeBallsPanel = true;
        }
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        OnOpenScene();
    }

    public void NextLevel()
    {
        if (lSystem.currentLevel >= lSystem.levels.Count)
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("Game");

        }
        else
        {
            lSystem.currentLevel++;
            lSystem.SaveLevelNumber();
            SceneManager.LoadScene("Game");
            GetComponent<InterstitialAd>().ShowAd();
        }
    }


    public void ControlVoice()
    {
        if (AudioManager.muteGameMusic == true)
        {
            startVoiceButton.image.sprite = unMuteSprite;

            AudioManager.UnMute();
        }
        else
        {

            startVoiceButton.image.sprite = muteSprite;


            AudioManager.Mute();
        }
    }


    public void CloseGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (lSystem.currentLevel >= lSystem.levels.Count)
        {
            wholeGameFnish.SetActive(true);
        }
        else
        {
            lvlText.SetText("Level " + lSystem.levels[lSystem.currentLevel].levelID);
        }

        if (activeBallsPanel == true)
        {
            cylinderPanel.SetActive(false);
            ballsPanel.SetActive(true);
        }
        else
        {
            cylinderPanel.SetActive(true);
            ballsPanel.SetActive(false);

        }


        if (Keyboard.current.enterKey.isPressed == true)
        {
            OnClickStart();

        }
    }
}

